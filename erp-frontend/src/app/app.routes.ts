import { Routes } from '@angular/router';
import { authGuard } from './auth/guards/auth.guard';

export const routes: Routes = [
  { path: '', redirectTo: '/login', pathMatch: 'full' },
  {
    path: 'login',
    loadComponent: () => import('./auth/login/login.component').then(m => m.LoginComponent)
  },
  {
    path: 'dashboard',
    loadComponent: () => import('./features/dashboard/dashboard.component').then(m => m.DashboardComponent),
    canActivate: [authGuard]
  },
  {
    path: 'inventory',
    canActivate: [authGuard],
    children: [
      {
        path: '',
        loadComponent: () => import('./features/inventory/inventory.component').then(m => m.InventoryComponent)
      },
      {
        path: 'products',
        loadComponent: () => import('./features/inventory/product-list/product-list.component').then(m => m.ProductListComponent)
      },
      {
        path: 'products/:id',
        loadComponent: () => import('./features/inventory/product-detail/product-detail.component').then(m => m.ProductDetailComponent)
      },
      {
        path: 'warehouses',
        loadComponent: () => import('./features/inventory/warehouse-list/warehouse-list.component').then(m => m.WarehouseListComponent)
      },
      {
        path: 'warehouses/:id',
        loadComponent: () => import('./features/inventory/warehouse-detail/warehouse-detail.component').then(m => m.WarehouseDetailComponent)
      },
      {
        path: 'brands',
        loadComponent: () => import('./features/brands/presentation/brand-list/brand-list.component').then(m => m.BrandListComponent)
      },
      {
        path: 'brands/:id',
        loadComponent: () => import('./features/brands/presentation/brand-detail/brand-detail.component').then(m => m.BrandDetailComponent)
      }
    ]
  },
  {
    path: 'sales',
    loadComponent: () => import('./features/sales/sales.component').then(m => m.SalesComponent),
    canActivate: [authGuard]
  },
  {
    path: 'purchases',
    loadComponent: () => import('./features/purchases/purchases.component').then(m => m.PurchasesComponent),
    canActivate: [authGuard]
  },
  {
    path: 'accounting',
    loadComponent: () => import('./features/accounting/accounting.component').then(m => m.AccountingComponent),
    canActivate: [authGuard]
  },
  {
    path: 'hr-payroll',
    loadComponent: () => import('./features/hr-payroll/hr-payroll.component').then(m => m.HrPayrollComponent),
    canActivate: [authGuard]
  },
  {
    path: 'reports',
    loadComponent: () => import('./features/reports/reports.component').then(m => m.ReportsComponent),
    canActivate: [authGuard]
  }
];