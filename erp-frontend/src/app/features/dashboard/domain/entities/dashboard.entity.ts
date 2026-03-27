export interface DashboardStats {
  totalSales: string;
  totalPurchases: string;
  inventoryItems: string;
  activeEmployees: string;
  salesTrend: string;
  purchasesTrend: string;
  inventoryTrend: string;
  employeesTrend: string;
}

export interface SalesData {
  month: string;
  amount: number;
}

export interface TopProduct {
  id: number;
  name: string;
  sales: number;
}

export interface RecentTransaction {
  id: string;
  type: 'sale' | 'purchase';
  description: string;
  amount: number;
  date: Date;
}