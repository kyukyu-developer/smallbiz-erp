import { Component } from '@angular/core';

interface StatCard {
  title: string;
  value: string;
  icon: string;
  color: string;
  trend?: string;
}

@Component({
  selector: 'app-dashboard',
  standalone: false,
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss'
})
export class DashboardComponent {
  stats: StatCard[] = [
    {
      title: 'Total Sales',
      value: '$45,678',
      icon: 'trending_up',
      color: '#4caf50',
      trend: '+12.5%'
    },
    {
      title: 'Total Purchases',
      value: '$28,450',
      icon: 'shopping_cart',
      color: '#ff9800',
      trend: '+8.2%'
    },
    {
      title: 'Inventory Items',
      value: '1,234',
      icon: 'inventory_2',
      color: '#2196f3',
      trend: '+5.7%'
    },
    {
      title: 'Active Employees',
      value: '45',
      icon: 'people',
      color: '#9c27b0',
      trend: '+2'
    }
  ];
}
