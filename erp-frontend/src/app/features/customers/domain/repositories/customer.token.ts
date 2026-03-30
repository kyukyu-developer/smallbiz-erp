import { InjectionToken } from '@angular/core';
import { ICustomerRepository } from './customer.repository.interface';

export const CUSTOMER_REPOSITORY = new InjectionToken<ICustomerRepository>('CUSTOMER_REPOSITORY');
