import { InjectionToken } from '@angular/core';
import { IBrandRepository } from './brand.repository.interface';

export const BRAND_REPOSITORY = new InjectionToken<IBrandRepository>('BRAND_REPOSITORY');
