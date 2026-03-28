import { InjectionToken } from '@angular/core';
import { IProductRepository } from './product.repository.interface';

export const PRODUCT_REPOSITORY = new InjectionToken<IProductRepository>('PRODUCT_REPOSITORY');
