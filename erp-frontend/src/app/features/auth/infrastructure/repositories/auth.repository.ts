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

  private accessToken: string | null = null;

  constructor() {
    const storedUser = localStorage.getItem('currentUser');
    this.currentUserSubject = new BehaviorSubject<User | null>(
      storedUser ? JSON.parse(storedUser) : null,
    );
    this.currentUser = this.currentUserSubject.asObservable();
  }
  getRefreshToken(): string | null {
    // If you store the refresh token in localStorage or another secure place, retrieve it here.
    // For security, it's best not to store refresh tokens in localStorage. Adjust as needed.
    const stored = localStorage.getItem('refreshToken');
    return stored ? stored : null;
  }

  login(credentials: LoginCredentials): Observable<AuthResponse> {
    return this.http
      .post<AuthResponse>(
        `${this.apiUrl}/login`,
        {
          username: credentials.username,
          password: credentials.password,
        },
        { withCredentials: true },
      )
      .pipe(
        tap((response) => this.handleAuthSuccess(response)),
        catchError((error) => {
          console.error('Login error:', error);
          throw error;
        }),
      );
  }

  refreshToken(): Observable<AuthResponse> {
    return this.http
      .post<AuthResponse>(
        `${this.apiUrl}/refresh-token`,
        {},
        { withCredentials: true },
      )
      .pipe(
        tap((response) => this.handleAuthSuccess(response)),
        catchError((error) => {
          this.logout();
          throw error;
        }),
      );
  }

  private handleAuthSuccess(response: AuthResponse): void {
    this.accessToken = response.accessToken;
    localStorage.setItem('currentUser', JSON.stringify(response.user));
    this.currentUserSubject.next(response.user);
  }

  logout(): Observable<void> {
    this.accessToken = null;
    localStorage.removeItem('currentUser');
    this.currentUserSubject.next(null);
    return this.http
      .post<void>(`${this.apiUrl}/logout`, {}, { withCredentials: true })
      .pipe(catchError(() => of(void 0)));
  }

  getCurrentUser(): Observable<User | null> {
    return this.currentUser;
  }

  isAuthenticated(): boolean {
    return !!this.accessToken || !!this.currentUserSubject.value;
  }

  getToken(): string | null {
    return this.accessToken;
  }
}
