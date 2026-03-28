import { Observable } from 'rxjs';
import { User, LoginCredentials, AuthResponse } from '../entities/user.entity';

export interface IAuthRepository {
  login(credentials: LoginCredentials): Observable<AuthResponse>;
  refreshToken(): Observable<AuthResponse>;
  logout(): Observable<void>;
  getCurrentUser(): Observable<User | null>;
  isAuthenticated(): boolean;
  getToken(): string | null;
}