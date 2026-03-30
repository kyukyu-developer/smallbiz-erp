import { InjectionToken } from '@angular/core';
import { IProductGroupRepository } from './product-group.repository.interface';

export const PRODUCT_GROUP_REPOSITORY = new InjectionToken<IProductGroupRepository>('PRODUCT_GROUP_REPOSITORY');
