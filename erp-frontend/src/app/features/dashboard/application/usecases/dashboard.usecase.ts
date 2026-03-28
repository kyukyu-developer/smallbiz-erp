import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { DashboardStats, SalesData, TopProduct, RecentTransaction } from '../../domain/entities/dashboard.entity';
import { IDashboardRepository } from '../../domain/repositories/dashboard.repository.interface';
import { DASHBOARD_REPOSITORY } from '../../domain/repositories/dashboard.token';

@Injectable({ providedIn: 'root' })
export class GetDashboardStatsUseCase {
  private repository = inject<IDashboardRepository>(DASHBOARD_REPOSITORY);

  execute(): Observable<DashboardStats> {
    return this.repository.getStats();
  }
}

@Injectable({ providedIn: 'root' })
export class GetSalesDataUseCase {
  private repository = inject<IDashboardRepository>(DASHBOARD_REPOSITORY);

  execute(): Observable<SalesData[]> {
    return this.repository.getSalesData();
  }
}

@Injectable({ providedIn: 'root' })
export class GetTopProductsUseCase {
  private repository = inject<IDashboardRepository>(DASHBOARD_REPOSITORY);

  execute(): Observable<TopProduct[]> {
    return this.repository.getTopProducts();
  }
}

@Injectable({ providedIn: 'root' })
export class GetRecentTransactionsUseCase {
  private repository = inject<IDashboardRepository>(DASHBOARD_REPOSITORY);

  execute(): Observable<RecentTransaction[]> {
    return this.repository.getRecentTransactions();
  }
}
