import { Component } from '@angular/core';

interface MenuItem {
  label: string;
  icon: string;
  route?: string;
  children?: MenuItem[];
  expanded?: boolean;
}

interface MenuSection {
  title?: string;
  items: MenuItem[];
}

@Component({
  selector: 'app-sidebar',
  standalone: false,
  templateUrl: './sidebar.component.html',
  styleUrl: './sidebar.component.scss'
})
export class SidebarComponent {
  companyName = 'ERP System';
  companyAddress = 'Business Management';

  menuSections: MenuSection[] = [
    {
      items: [
        { label: 'Dashboard', icon: 'dashboard', route: '/dashboard' }
      ]
    },
    {
      title: 'OPERATIONS',
      items: [
        {
          label: 'Inventory',
          icon: 'inventory_2',
          route: '/inventory',
          expanded: false,
          children: [
            { label: 'Products', icon: 'shopping_bag', route: '/inventory/products' },
            { label: 'Categories', icon: 'category', route: '/inventory/categories' },
            { label: 'Stock Management', icon: 'inventory', route: '/inventory/stock' },
            { label: 'Warehouse', icon: 'warehouse', route: '/inventory/warehouses' },
            { label: 'Suppliers', icon: 'local_shipping', route: '/inventory/suppliers' }
          ]
        },
        {
          label: 'Sales',
          icon: 'shopping_cart',
          route: '/sales',
          expanded: false,
          children: [
            { label: 'Orders', icon: 'receipt', route: '/sales/orders' },
            { label: 'Customers', icon: 'person', route: '/sales/customers' },
            { label: 'Invoices', icon: 'description', route: '/sales/invoices' },
            { label: 'Quotations', icon: 'request_quote', route: '/sales/quotations' }
          ]
        },
        {
          label: 'Purchases',
          icon: 'shopping_bag',
          route: '/purchases',
          expanded: false,
          children: [
            { label: 'Purchase Orders', icon: 'shopping_cart', route: '/purchases/orders' },
            { label: 'Vendors', icon: 'store', route: '/purchases/vendors' },
            { label: 'Bills', icon: 'receipt_long', route: '/purchases/bills' }
          ]
        }
      ]
    },
    {
      title: 'FINANCE',
      items: [
        {
          label: 'Accounting',
          icon: 'account_balance',
          route: '/accounting',
          expanded: false,
          children: [
            { label: 'Accounts', icon: 'account_balance_wallet', route: '/accounting/accounts' },
            { label: 'Journal Entries', icon: 'book', route: '/accounting/journal' },
            { label: 'Transactions', icon: 'swap_horiz', route: '/accounting/transactions' }
          ]
        },
        { label: 'Payment Method', icon: 'credit_card', route: '/payment-method' }
      ]
    },
    {
      title: 'HUMAN RESOURCES',
      items: [
        {
          label: 'HR & Payroll',
          icon: 'people',
          route: '/hr-payroll',
          expanded: false,
          children: [
            { label: 'Employees', icon: 'badge', route: '/hr-payroll/employees' },
            { label: 'Attendance', icon: 'event_available', route: '/hr-payroll/attendance' },
            { label: 'Payroll', icon: 'payments', route: '/hr-payroll/payroll' },
            { label: 'Leaves', icon: 'event_busy', route: '/hr-payroll/leaves' }
          ]
        }
      ]
    },
    {
      title: 'REPORTS & SUPPORT',
      items: [
        {
          label: 'Reports',
          icon: 'assessment',
          route: '/reports',
          expanded: false,
          children: [
            { label: 'Sales Reports', icon: 'trending_up', route: '/reports/sales' },
            { label: 'Inventory Reports', icon: 'inventory', route: '/reports/inventory' },
            { label: 'Financial Reports', icon: 'attach_money', route: '/reports/financial' }
          ]
        },
        { label: 'Customer Support', icon: 'support_agent', route: '/support' }
      ]
    }
  ];

  toggleSubmenu(item: MenuItem): void {
    if (item.children && item.children.length > 0) {
      item.expanded = !item.expanded;
    }
  }

  hasChildren(item: MenuItem): boolean {
    return !!item.children && item.children.length > 0;
  }
}
