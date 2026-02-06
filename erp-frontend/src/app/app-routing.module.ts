import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { authGuard } from './auth/guards/auth.guard';

const routes: Routes = [
  { path: '', redirectTo: '/login', pathMatch: 'full' },
  {
    path: 'login',
    loadChildren: () => import('./auth/auth.module').then(m => m.AuthModule)
  },
  {
    path: 'dashboard',
    loadChildren: () => import('./features/dashboard/dashboard.module').then(m => m.DashboardModule),
    canActivate: [authGuard]
  },
  {
    path: 'inventory',
    loadChildren: () => import('./features/inventory/inventory.module').then(m => m.InventoryModule),
    canActivate: [authGuard]
  },
  {
    path: 'sales',
    loadChildren: () => import('./features/sales/sales.module').then(m => m.SalesModule),
    canActivate: [authGuard]
  },
  {
    path: 'purchases',
    loadChildren: () => import('./features/purchases/purchases.module').then(m => m.PurchasesModule),
    canActivate: [authGuard]
  },
  {
    path: 'accounting',
    loadChildren: () => import('./features/accounting/accounting.module').then(m => m.AccountingModule),
    canActivate: [authGuard]
  },
  {
    path: 'hr-payroll',
    loadChildren: () => import('./features/hr-payroll/hr-payroll.module').then(m => m.HrPayrollModule),
    canActivate: [authGuard]
  },
  {
    path: 'reports',
    loadChildren: () => import('./features/reports/reports.module').then(m => m.ReportsModule),
    canActivate: [authGuard]
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
