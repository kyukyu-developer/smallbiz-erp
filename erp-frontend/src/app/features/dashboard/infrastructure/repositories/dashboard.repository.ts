import { Injectable, inject } from '@angular/core';
import { Observable, of, delay } from 'rxjs';
import { DashboardStats, SalesData, TopProduct, RecentTransaction } from '../../domain/entities/dashboard.entity';
import { IDashboardRepository } from '../../domain/repositories/dashboard.repository.interface';

@Injectable({ providedIn: 'root' })
export class DashboardRepository implements IDashboardRepository {
  getStats(): Observable<DashboardStats> {
    return of({
      totalSales: '$45,678',
      totalPurchases: '$28,450',
      inventoryItems: '1,234',
      activeEmployees: '45',
      salesTrend: '+12.5%',
      purchasesTrend: '+8.2%',
      inventoryTrend: '+5.7%',
      employeesTrend: '+2'
    }).pipe(delay(300));
  }

  getSalesData(): Observable<SalesData[]> {
    return of([
      { month: 'Jan', amount: 15000 },
      { month: 'Feb', amount: 18000 },
      { month: 'Mar', amount: 22000 },
      { month: 'Apr', amount: 19000 },
      { month: 'May', amount: 25000 },
      { month: 'Jun', amount: 28000 }
    ]).pipe(delay(300));
  }

  getTopProducts(): Observable<TopProduct[]> {
    return of([
      { id: 1, name: 'Product A', sales: 1500 },
      { id: 2, name: 'Product B', sales: 1200 },
      { id: 3, name: 'Product C', sales: 900 },
      { id: 4, name: 'Product D', sales: 750 },
      { id: 5, name: 'Product E', sales: 600 }
    ]).pipe(delay(300));
  }

  getRecentTransactions(): Observable<RecentTransaction[]> {
    const transactions: RecentTransaction[] = [
      { id: '1', type: 'sale', description: 'Sale to Customer A', amount: 1500, date: new Date() },
      { id: '2', type: 'purchase', description: 'Purchase from Supplier B', amount: 800, date: new Date() },
      { id: '3', type: 'sale', description: 'Sale to Customer C', amount: 2200, date: new Date() }
    ];
    return of(transactions).pipe(delay(300));
  }
}