import { InjectionToken } from '@angular/core';
import { IDashboardRepository } from './dashboard.repository.interface';

export const DASHBOARD_REPOSITORY = new InjectionToken<IDashboardRepository>('DASHBOARD_REPOSITORY');
