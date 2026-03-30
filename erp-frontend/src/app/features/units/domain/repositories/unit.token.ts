import { InjectionToken } from '@angular/core';
import { IUnitRepository } from './unit.repository.interface';

export const UNIT_REPOSITORY = new InjectionToken<IUnitRepository>('UNIT_REPOSITORY');
