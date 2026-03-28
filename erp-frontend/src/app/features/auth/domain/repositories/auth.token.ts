import { InjectionToken } from '@angular/core';
import { IAuthRepository } from './auth.repository.interface';

export const AUTH_REPOSITORY = new InjectionToken<IAuthRepository>('AUTH_REPOSITORY');
