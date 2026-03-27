import {
  HttpInterceptorFn,
  HttpErrorResponse,
  HttpRequest,
  HttpEvent,
  HttpHandlerFn,
} from '@angular/common/http';
import { inject } from '@angular/core';
import {
  catchError,
  switchMap,
  tap,
  throwError,
  BehaviorSubject,
  filter,
  take,
  Observable,
} from 'rxjs';
import { AuthRepository } from '../../features/auth/infrastructure/repositories/auth.repository';
import { AuthResponse } from '../../features/auth/domain/entities/user.entity';

let isRefreshing = false;
const refreshTokenSubject = new BehaviorSubject<string | null>(null);

export const authInterceptor: HttpInterceptorFn = (
  req,
  next,
): Observable<HttpEvent<unknown>> => {
  const authRepository = inject(AuthRepository);
  const token = authRepository.getToken();

  let authReq = req;
  if (token) {
    authReq = addToken(req, token);
  }

  return next(authReq).pipe(
    catchError((error: HttpErrorResponse) => {
      if (error.status === 401) {
        return handle401Error(req, authReq, next, authRepository);
      }
      return throwError(() => error);
    }),
  );
};

function addToken(
  request: HttpRequest<unknown>,
  token: string,
): HttpRequest<unknown> {
  return request.clone({
    setHeaders: {
      Authorization: `Bearer ${token}`,
    },
  });
}

function handle401Error(
  originalReq: HttpRequest<unknown>,
  modifiedReq: HttpRequest<unknown>,
  next: HttpHandlerFn,
  authRepository: AuthRepository,
): Observable<HttpEvent<unknown>> {
  if (!isRefreshing) {
    isRefreshing = true;
    refreshTokenSubject.next(null);

    const refreshToken = authRepository.getRefreshToken();
    if (!refreshToken) {
      isRefreshing = false;
      authRepository.logout();
      return throwError(() => new Error('No refresh token'));
    }

    return authRepository.refreshToken().pipe(
      tap((response: AuthResponse) => {
        isRefreshing = false;
        refreshTokenSubject.next(response.accessToken);
      }),
      catchError((error: unknown) => {
        isRefreshing = false;
        authRepository.logout();
        return throwError(() => error);
      }),
      switchMap((response: AuthResponse) => {
        refreshTokenSubject.next(response.accessToken);
        const newReq = addToken(originalReq, response.accessToken);
        return next(newReq);
      }),
    );
  }

  return refreshTokenSubject.pipe(
    filter((token): token is string => token !== null),
    take(1),
    switchMap((token: string) => {
      const newReq = addToken(originalReq, token);
      return next(newReq);
    }),
  );
}
