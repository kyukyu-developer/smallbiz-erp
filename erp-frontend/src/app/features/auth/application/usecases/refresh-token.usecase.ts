import { Injectable, inject } from '@angular/core';
import { Router } from '@angular/router';
import { HttpClient, HttpInterceptor, HttpRequest, HttpHandler, HttpEvent, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError, BehaviorSubject, catchError, filter, take, switchMap, tap } from 'rxjs';
import { AuthResponse, User } from '../../domain/entities/user.entity';
import { IAuthRepository } from '../../domain/repositories/auth.repository.interface';
import { AUTH_REPOSITORY } from '../../../../core/interfaces/repositories/repository-tokens';

@Injectable({ providedIn: 'root' })
export class RefreshTokenUseCase {
  private repository = inject(AUTH_REPOSITORY);
  private router = inject(Router);

  execute(): Observable<AuthResponse> {
    return this.repository.refreshToken();
  }
}

@Injectable({ providedIn: 'root' })
export class AuthInterceptor implements HttpInterceptor {
  private repository = inject(AUTH_REPOSITORY);
  private isRefreshing = false;
  private refreshTokenSubject = new BehaviorSubject<string | null>(null);

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const token = this.repository.getToken();

    if (token) {
      request = this.addToken(request, token);
    }

    return next.handle(request).pipe(
      catchError(error => {
        if (error instanceof HttpErrorResponse && error.status === 401) {
          return this.handle401Error(request, next);
        }
        return throwError(() => error);
      })
    );
  }

  private addToken(request: HttpRequest<any>, token: string): HttpRequest<any> {
    return request.clone({
      setHeaders: {
        Authorization: `Bearer ${token}`
      }
    });
  }

  private handle401Error(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    if (!this.isRefreshing) {
      this.isRefreshing = true;
      this.refreshTokenSubject.next(null);

      const refreshToken = this.repository.getRefreshToken();
      if (!refreshToken) {
        this.isRefreshing = false;
        this.repository.logout();
        return throwError(() => new Error('No refresh token'));
      }

      return this.repository.refreshToken().pipe(
        tap(response => {
          this.isRefreshing = false;
          this.refreshTokenSubject.next(response.accessToken);
        }),
        catchError(error => {
          this.isRefreshing = false;
          this.repository.logout();
          return throwError(() => error);
        }),
        switchMap(response => {
          this.refreshTokenSubject.next(response.accessToken);
          return next.handle(this.addToken(request, response.accessToken));
        })
      );
    }

    return this.refreshTokenSubject.pipe(
      filter(token => token !== null),
      take(1),
      switchMap(token => next.handle(this.addToken(request, token!)))
    );
  }
}