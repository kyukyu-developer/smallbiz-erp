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
      },
      {
        path: 'product-groups',
        loadComponent: () => import('./features/product-groups/presentation/product-group-list/product-group-list.component').then(m => m.ProductGroupListComponent)
      },
      {
        path: 'product-groups/:id',
        loadComponent: () => import('./features/product-groups/presentation/product-group-detail/product-group-detail.component').then(m => m.ProductGroupDetailComponent)
      },
      {
        path: 'categories',
        loadComponent: () => import('./features/categories/presentation/category-list/category-list.component').then(m => m.CategoryListComponent)
      },
      {
        path: 'categories/:id',
        loadComponent: () => import('./features/categories/presentation/category-detail/category-detail.component').then(m => m.CategoryDetailComponent)
      },
      {
        path: 'units',
        loadComponent: () => import('./features/units/presentation/unit-list/unit-list.component').then(m => m.UnitListComponent)
      },
      {
        path: 'units/:id',
        loadComponent: () => import('./features/units/presentation/unit-detail/unit-detail.component').then(m => m.UnitDetailComponent)
      },
      {
        path: 'unit-conversions',
        loadComponent: () => import('./features/unit-conversions/presentation/unit-conversion-list/unit-conversion-list.component').then(m => m.UnitConversionListComponent)
      }
    ]
  },
  {
    path: 'sales',
    canActivate: [authGuard],
    children: [
      {
        path: '',
        loadComponent: () => import('./features/sales/sales.component').then(m => m.SalesComponent)
      },
      {
        path: 'customers',
        loadComponent: () => import('./features/customers/presentation/customer-list/customer-list.component').then(m => m.CustomerListComponent)
      },
      {
        path: 'customers/:id',
        loadComponent: () => import('./features/customers/presentation/customer-detail/customer-detail.component').then(m => m.CustomerDetailComponent)
      }
    ]
  },
  {
    path: 'purchases',
    canActivate: [authGuard],
    children: [
      {
        path: '',
        loadComponent: () => import('./features/purchases/purchases.component').then(m => m.PurchasesComponent)
      },
      {
        path: 'suppliers',
        loadComponent: () => import('./features/suppliers/presentation/supplier-list/supplier-list.component').then(m => m.SupplierListComponent)
      },
      {
        path: 'suppliers/:id',
        loadComponent: () => import('./features/suppliers/presentation/supplier-detail/supplier-detail.component').then(m => m.SupplierDetailComponent)
      }
    ]
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