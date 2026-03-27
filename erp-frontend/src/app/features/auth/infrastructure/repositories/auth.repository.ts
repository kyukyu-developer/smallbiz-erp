import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, tap, catchError, of } from 'rxjs';
import {
  User,
  LoginCredentials,
  AuthResponse,
} from '../../domain/entities/user.entity';
import { IAuthRepository } from '../../domain/repositories/auth.repository.interface';
import { environment } from '../../../../../environments/environment';

@Injectable({ providedIn: 'root' })
export class AuthRepository implements IAuthRepository {
  private http = inject(HttpClient);
  private apiUrl = `${environment.apiUrl}/Auth`;

  private currentUserSubject: BehaviorSubject<User | null>;
  public currentUser: Observable<User | null>;
  private _isRefreshing = false;
  private refreshTokenSubject = new BehaviorSubject<string | null>(null);

  constructor() {
    const storedUser = localStorage.getItem('currentUser');
    this.currentUserSubject = new BehaviorSubject<User | null>(
      storedUser ? JSON.parse(storedUser) : null,
    );
    this.currentUser = this.currentUserSubject.asObservable();
  }

  login(credentials: LoginCredentials): Observable<AuthResponse> {
    return this.http
      .post<AuthResponse>(`${this.apiUrl}/login`, {
        username: credentials.username,
        password: credentials.password,
      })
      .pipe(
        tap((response) => this.handleAuthSuccess(response)),
        catchError((error) => {
          console.error('Login error:', error);
          throw error;
        }),
      );
  }

  refreshToken(): Observable<AuthResponse> {
    const refreshToken = localStorage.getItem('refreshToken');

    if (!refreshToken) {
      return new Observable((observer) => {
        observer.error(new Error('No refresh token available'));
      });
    }

    return this.http
      .post<AuthResponse>(`${this.apiUrl}/refresh-token`, {
        refreshToken: refreshToken,
      })
      .pipe(
        tap((response) => this.handleAuthSuccess(response)),
        catchError((error) => {
          this.logout();
          throw error;
        }),
      );
  }

  private handleAuthSuccess(response: AuthResponse): void {
    localStorage.setItem('token', response.accessToken);
    localStorage.setItem('refreshToken', response.refreshToken);
    localStorage.setItem('tokenExpiry', response.expiresAt.toString());
    localStorage.setItem('currentUser', JSON.stringify(response.user));
    this.currentUserSubject.next(response.user);
    this.refreshTokenSubject.next(response.refreshToken);
  }

  logout(): Observable<void> {
    localStorage.removeItem('currentUser');
    localStorage.removeItem('token');
    localStorage.removeItem('refreshToken');
    localStorage.removeItem('tokenExpiry');
    this.currentUserSubject.next(null);
    this.refreshTokenSubject.next(null);
    return of(void 0);
  }

  getCurrentUser(): Observable<User | null> {
    return this.currentUser;
  }

  isAuthenticated(): boolean {
    return !!this.currentUserSubject.value || !!localStorage.getItem('token');
  }

  getToken(): string | null {
    return localStorage.getItem('token');
  }

  getRefreshToken(): string | null {
    return localStorage.getItem('refreshToken');
  }

  isRefreshing(): boolean {
    return this._isRefreshing;
  }

  getRefreshTokenSubject(): BehaviorSubject<string | null> {
    return this.refreshTokenSubject;
  }
}
