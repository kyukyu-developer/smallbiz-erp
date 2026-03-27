import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AUTH_REPOSITORY } from '../../core/interfaces/repositories/repository-tokens';

export const authGuard: CanActivateFn = (route, state) => {
  const authRepository = inject(AUTH_REPOSITORY);
  const router = inject(Router);

  if (authRepository.isAuthenticated()) {
    return true;
  }

  router.navigate(['/login'], { queryParams: { returnUrl: state.url } });
  return false;
};