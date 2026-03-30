import { InjectionToken } from '@angular/core';
import { ICategoryRepository } from './category.repository.interface';

export const CATEGORY_REPOSITORY = new InjectionToken<ICategoryRepository>('CATEGORY_REPOSITORY');
