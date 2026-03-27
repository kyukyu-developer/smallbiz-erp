import { Injectable, inject } from '@angular/core';
import { Observable, map, tap } from 'rxjs';
import { LoginCredentials, AuthResponse, User } from '../../domain/entities/user.entity';
import { IAuthRepository } from '../../domain/repositories/auth.repository.interface';
import { AUTH_REPOSITORY } from '../../../../core/interfaces/repositories/repository-tokens';

export interface LoginResult {
  success: boolean;
  user?: User;
  token?: string;
  error?: string;
}

@Injectable({ providedIn: 'root' })
export class LoginUseCase {
  private repository = inject(AUTH_REPOSITORY);

  execute(credentials: LoginCredentials): Observable<LoginResult> {
    return this.repository.login(credentials).pipe(
      map((response: AuthResponse) => ({
        success: true,
        user: response.user,
        token: response.accessToken
      }))
    );
  }
}

@Injectable({ providedIn: 'root' })
export class LogoutUseCase {
  private repository = inject(AUTH_REPOSITORY);

  execute(): Observable<void> {
    localStorage.removeItem('token');
    return this.repository.logout();
  }
}

@Injectable({ providedIn: 'root' })
export class GetCurrentUserUseCase {
  private repository = inject(AUTH_REPOSITORY);

  execute(): Observable<User | null> {
    return this.repository.getCurrentUser();
  }
}

@Injectable({ providedIn: 'root' })
export class IsAuthenticatedUseCase {
  private repository = inject(AUTH_REPOSITORY);

  execute(): boolean {
    return this.repository.isAuthenticated();
  }
}