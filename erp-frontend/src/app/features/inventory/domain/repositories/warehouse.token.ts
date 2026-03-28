import { InjectionToken } from '@angular/core';
import { IWarehouseRepository } from './warehouse.repository.interface';

export const WAREHOUSE_REPOSITORY = new InjectionToken<IWarehouseRepository>('WAREHOUSE_REPOSITORY');
