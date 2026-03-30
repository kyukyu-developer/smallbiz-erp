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
import { CategoryRepository } from './features/categories/infrastructure/repositories/category.repository';
import { CustomerRepository } from './features/customers/infrastructure/repositories/customer.repository';
import { ProductGroupRepository } from './features/product-groups/infrastructure/repositories/product-group.repository';
import { UnitRepository } from './features/units/infrastructure/repositories/unit.repository';
import { SupplierRepository } from './features/suppliers/infrastructure/repositories/supplier.repository';
import { UnitConversionRepository } from './features/unit-conversions/infrastructure/repositories/unit-conversion.repository';
import {
  AUTH_REPOSITORY,
  DASHBOARD_REPOSITORY,
  PRODUCT_REPOSITORY,
  WAREHOUSE_REPOSITORY,
  BRAND_REPOSITORY,
  CATEGORY_REPOSITORY,
  CUSTOMER_REPOSITORY,
  PRODUCT_GROUP_REPOSITORY,
  UNIT_REPOSITORY,
  SUPPLIER_REPOSITORY,
  UNIT_CONVERSION_REPOSITORY,
} from './core/interfaces/repositories/repository-tokens';
import { authInterceptor } from './core/interceptors/auth.interceptor';

const repositoryProviders: Provider[] = [
  { provide: AUTH_REPOSITORY, useClass: AuthRepository },
  { provide: DASHBOARD_REPOSITORY, useClass: DashboardRepository },
  { provide: PRODUCT_REPOSITORY, useClass: ProductRepository },
  { provide: WAREHOUSE_REPOSITORY, useClass: WarehouseRepository },
  { provide: BRAND_REPOSITORY, useClass: BrandRepository },
  { provide: CATEGORY_REPOSITORY, useClass: CategoryRepository },
  { provide: CUSTOMER_REPOSITORY, useClass: CustomerRepository },
  { provide: PRODUCT_GROUP_REPOSITORY, useClass: ProductGroupRepository },
  { provide: UNIT_REPOSITORY, useClass: UnitRepository },
  { provide: SUPPLIER_REPOSITORY, useClass: SupplierRepository },
  { provide: UNIT_CONVERSION_REPOSITORY, useClass: UnitConversionRepository },
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
