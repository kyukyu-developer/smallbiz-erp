import { InjectionToken } from '@angular/core';
import { IUnitConversionRepository } from './unit-conversion.repository.interface';

export const UNIT_CONVERSION_REPOSITORY = new InjectionToken<IUnitConversionRepository>('UNIT_CONVERSION_REPOSITORY');
