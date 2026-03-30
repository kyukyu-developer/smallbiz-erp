import { InjectionToken } from '@angular/core';
import { ISupplierRepository } from './supplier.repository.interface';

export const SUPPLIER_REPOSITORY = new InjectionToken<ISupplierRepository>('SUPPLIER_REPOSITORY');
