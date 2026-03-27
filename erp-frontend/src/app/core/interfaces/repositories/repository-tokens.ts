import { InjectionToken } from '@angular/core';

export interface IAuthRepository {
  login(credentials: any): any;
  logout(): any;
  getCurrentUser(): any;
  isAuthenticated(): boolean;
  getToken(): string | null;
}

export interface IDashboardRepository {
  getStats(): any;
  getSalesData(): any;
  getTopProducts(): any;
  getRecentTransactions(): any;
}

export const AUTH_REPOSITORY = new InjectionToken<IAuthRepository>('AUTH_REPOSITORY');
export const DASHBOARD_REPOSITORY = new InjectionToken<IDashboardRepository>('DASHBOARD_REPOSITORY');

export const CORE_REPOSITORIES = {
  AUTH: AUTH_REPOSITORY,
  DASHBOARD: DASHBOARD_REPOSITORY
};