import { Observable } from 'rxjs';
import { DashboardStats, SalesData, TopProduct, RecentTransaction } from '../entities/dashboard.entity';

export interface IDashboardRepository {
  getStats(): Observable<DashboardStats>;
  getSalesData(): Observable<SalesData[]>;
  getTopProducts(): Observable<TopProduct[]>;
  getRecentTransactions(): Observable<RecentTransaction[]>;
}
