import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { AuthResponse } from '../../domain/entities/user.entity';
import { IAuthRepository } from '../../domain/repositories/auth.repository.interface';
import { AUTH_REPOSITORY } from '../../../../core/interfaces/repositories/repository-tokens';

@Injectable({ providedIn: 'root' })
export class RefreshTokenUseCase {
  private repository = inject(AUTH_REPOSITORY);

  execute(): Observable<AuthResponse> {
    return this.repository.refreshToken();
  }
}
