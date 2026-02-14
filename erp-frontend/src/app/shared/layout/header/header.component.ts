import { Component, EventEmitter, Input, Output, OnInit } from '@angular/core';
import { AuthService, User } from '../../../auth/services/auth.service';
import { Observable } from 'rxjs';
import { Router, NavigationEnd } from '@angular/router';
import { filter } from 'rxjs/operators';

interface SubMenuItem {
  label: string;
  route: string;
}

interface MenuItem {
  label: string;
  icon: string;
  route: string;
  subItems?: SubMenuItem[];
}

@Component({
  selector: 'app-header',
  standalone: false,
  templateUrl: './header.component.html',
  styleUrl: './header.component.scss'
})
export class HeaderComponent implements OnInit {
  @Input() sidebarOpen: boolean = true;
  @Output() toggleSidebar = new EventEmitter<void>();
  currentUser$: Observable<User | null>;
  pageTitle: string = 'Dashboard';

  // Breadcrumb properties for detail pages
  hasDetailParam: boolean = false;
  baseTitle: string = '';
  detailParam: string = '';
  baseRoute: string = '';

  menuItems: MenuItem[] = [
    {
      label: 'Dashboard',
      icon: 'dashboard',
      route: '/dashboard'
    },
    {
      label: 'Inventory',
      icon: 'inventory_2',
      route: '/inventory',
      subItems: [
        { label: 'Stock Receive', route: '/inventory/stock-receive' },
        { label: 'Stock Delivery', route: '/inventory/stock-delivery' },
        { label: 'Stock Adjustment', route: '/inventory/stock-adjustment' },
        { label: 'Product List', route: '/inventory/products' },
        { label: 'Warehouses', route: '/inventory/warehouses' },
        { label: 'Categories', route: '/inventory/categories' }
      ]
    },
    {
      label: 'Sales',
      icon: 'shopping_cart',
      route: '/sales',
      subItems: [
        { label: 'New Sale', route: '/sales/new' },
        { label: 'Sales Orders', route: '/sales/orders' },
        { label: 'Invoices', route: '/sales/invoices' },
        { label: 'Customers', route: '/sales/customers' },
        { label: 'Sales Reports', route: '/sales/reports' }
      ]
    },
    {
      label: 'Purchases',
      icon: 'shopping_bag',
      route: '/purchases',
      subItems: [
        { label: 'New Purchase', route: '/purchases/new' },
        { label: 'Purchase Orders', route: '/purchases/orders' },
        { label: 'Bills', route: '/purchases/bills' },
        { label: 'Suppliers', route: '/purchases/suppliers' },
        { label: 'Purchase Reports', route: '/purchases/reports' }
      ]
    },
    {
      label: 'Accounting',
      icon: 'account_balance',
      route: '/accounting',
      subItems: [
        { label: 'Chart of Accounts', route: '/accounting/chart-of-accounts' },
        { label: 'Journal Entries', route: '/accounting/journal-entries' },
        { label: 'General Ledger', route: '/accounting/general-ledger' },
        { label: 'Trial Balance', route: '/accounting/trial-balance' },
        { label: 'Financial Reports', route: '/accounting/reports' }
      ]
    },
    {
      label: 'HR & Payroll',
      icon: 'people',
      route: '/hr-payroll',
      subItems: [
        { label: 'Employees', route: '/hr-payroll/employees' },
        { label: 'Attendance', route: '/hr-payroll/attendance' },
        { label: 'Payroll', route: '/hr-payroll/payroll' },
        { label: 'Leave Management', route: '/hr-payroll/leave' },
        { label: 'HR Reports', route: '/hr-payroll/reports' }
      ]
    },
    {
      label: 'Reports',
      icon: 'assessment',
      route: '/reports',
      subItems: [
        { label: 'Sales Reports', route: '/reports/sales' },
        { label: 'Inventory Reports', route: '/reports/inventory' },
        { label: 'Financial Reports', route: '/reports/financial' },
        { label: 'Custom Reports', route: '/reports/custom' }
      ]
    }
  ];

  constructor(
    private authService: AuthService,
    private router: Router
  ) {
    this.currentUser$ = this.authService.currentUser;
  }

  ngOnInit() {
    // Set initial title
    this.updatePageTitle(this.router.url);

    // Listen to route changes
    this.router.events.pipe(
      filter(event => event instanceof NavigationEnd)
    ).subscribe((event: any) => {
      this.updatePageTitle(event.urlAfterRedirects);
    });
  }

  updatePageTitle(url: string) {
    // Handle warehouse detail pages with parameters (e.g., /inventory/warehouses/WH001 or /inventory/warehouses/new)
    if (url.includes('/inventory/warehouses/')) {
      const segments = url.split('/');
      const warehouseParam = segments[segments.length - 1].split('?')[0]; // Remove query params if any
      if (warehouseParam) {
        const formattedParam = warehouseParam === 'new' ? 'New' : warehouseParam.toUpperCase();
        this.pageTitle = `Warehouses / ${formattedParam}`;

        // Set breadcrumb properties
        this.hasDetailParam = true;
        this.baseTitle = 'Warehouses';
        this.detailParam = formattedParam;
        this.baseRoute = '/inventory/warehouses';
        return;
      }
    }

    // Reset breadcrumb for other pages
    this.hasDetailParam = false;
    this.baseTitle = '';
    this.detailParam = '';
    this.baseRoute = '';

    // Map routes to titles
    const routeTitleMap: { [key: string]: string } = {
      '/dashboard': 'Dashboard',
      '/inventory': 'Inventory',
      '/inventory/products': 'Products',
      '/inventory/warehouses': 'Warehouse',
      '/inventory/categories': 'Categories',
      '/inventory/stock-receive': 'Stock Receive',
      '/inventory/stock-delivery': 'Stock Delivery',
      '/inventory/stock-adjustment': 'Stock Adjustment',
      '/sales': 'Sales',
      '/sales/new': 'New Sale',
      '/sales/orders': 'Sales Orders',
      '/sales/invoices': 'Invoices',
      '/sales/customers': 'Customers',
      '/sales/reports': 'Sales Reports',
      '/purchases': 'Purchases',
      '/purchases/new': 'New Purchase',
      '/purchases/orders': 'Purchase Orders',
      '/purchases/bills': 'Bills',
      '/purchases/suppliers': 'Suppliers',
      '/purchases/reports': 'Purchase Reports',
      '/accounting': 'Accounting',
      '/accounting/chart-of-accounts': 'Chart of Accounts',
      '/accounting/journal-entries': 'Journal Entries',
      '/accounting/general-ledger': 'General Ledger',
      '/accounting/trial-balance': 'Trial Balance',
      '/accounting/reports': 'Financial Reports',
      '/hr-payroll': 'HR & Payroll',
      '/hr-payroll/employees': 'Employees',
      '/hr-payroll/attendance': 'Attendance',
      '/hr-payroll/payroll': 'Payroll',
      '/hr-payroll/leave': 'Leave Management',
      '/hr-payroll/reports': 'HR Reports',
      '/reports': 'Reports',
      '/reports/sales': 'Sales Reports',
      '/reports/inventory': 'Inventory Reports',
      '/reports/financial': 'Financial Reports',
      '/reports/custom': 'Custom Reports'
    };

    // Sort routes by length (longest first) to match most specific route first
    const sortedRoutes = Object.keys(routeTitleMap).sort((a, b) => b.length - a.length);

    // Find matching route
    for (const route of sortedRoutes) {
      if (url.startsWith(route)) {
        this.pageTitle = routeTitleMap[route];
        return;
      }
    }

    // Default title
    this.pageTitle = 'Dashboard';
  }

  onToggleSidebar() {
    this.toggleSidebar.emit();
  }

  logout() {
    this.authService.logout();
  }
}
