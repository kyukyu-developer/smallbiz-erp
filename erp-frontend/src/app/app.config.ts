import { ApplicationConfig, provideZoneChangeDetection } from '@angular/core';
import { provideRouter, withComponentInputBinding } from '@angular/router';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { provideHttpClient, withFetch, withInterceptors } from '@angular/common/http';
import { routes } from './app.routes';
import { AuthRepository } from './features/auth/infrastructure/repositories/auth.repository';
import { DashboardRepository } from './features/dashboard/infrastructure/repositories/dashboard.repository';
import { AUTH_REPOSITORY, DASHBOARD_REPOSITORY } from './core/interfaces/repositories/repository-tokens';
import { authInterceptor } from './core/interceptors/auth.interceptor';

export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes, withComponentInputBinding()),
    provideAnimationsAsync(),
    provideHttpClient(
      withFetch(),
      withInterceptors([authInterceptor])
    ),
    { provide: AUTH_REPOSITORY, useClass: AuthRepository },
    { provide: DASHBOARD_REPOSITORY, useClass: DashboardRepository }
  ]
};