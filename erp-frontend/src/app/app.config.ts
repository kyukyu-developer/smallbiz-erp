import {
  ApplicationConfig,
  provideZoneChangeDetection,
  Provider,
} from '@angular/core';
import { provideRouter, withComponentInputBinding } from '@angular/router';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import {
  provideHttpClient,
  withFetch,
  withInterceptors,
} from '@angular/common/http';
import { routes } from './app.routes';
import { AuthRepository } from './features/auth/infrastructure/repositories/auth.repository';
import { DashboardRepository } from './features/dashboard/infrastructure/repositories/dashboard.repository';
import { ProductRepository } from './features/inventory/infrastructure/repositories/product.repository';
import { WarehouseRepository } from './features/inventory/infrastructure/repositories/warehouse.repository';
import { BrandRepository } from './features/brands/infrastructure/repositories/brand.repository';
import {
  AUTH_REPOSITORY,
  DASHBOARD_REPOSITORY,
  PRODUCT_REPOSITORY,
  WAREHOUSE_REPOSITORY,
  BRAND_REPOSITORY,
} from './core/interfaces/repositories/repository-tokens';
import { authInterceptor } from './core/interceptors/auth.interceptor';

const repositoryProviders: Provider[] = [
  { provide: AUTH_REPOSITORY, useClass: AuthRepository },
  { provide: DASHBOARD_REPOSITORY, useClass: DashboardRepository },
  { provide: PRODUCT_REPOSITORY, useClass: ProductRepository },
  { provide: WAREHOUSE_REPOSITORY, useClass: WarehouseRepository },
  { provide: BRAND_REPOSITORY, useClass: BrandRepository },
];

export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes, withComponentInputBinding()),
    provideAnimationsAsync(),
    provideHttpClient(withFetch(), withInterceptors([authInterceptor])),
    ...repositoryProviders,
  ],
};
