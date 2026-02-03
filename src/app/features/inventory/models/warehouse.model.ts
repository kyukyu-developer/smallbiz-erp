export interface Warehouse {
  warehouse_id: number;
  warehouse_name: string;
  location: string;

  // Additional fields for UI/display (not in DB schema)
  code?: string;
  description?: string;
  address?: string;
  city?: string;
  state?: string;
  zipCode?: string;
  country?: string;
  phone?: string;
  email?: string;
  manager?: string;
  capacity?: number;
  currentOccupancy?: number;
  status?: 'active' | 'inactive' | 'maintenance';
  created_at?: Date;
  updated_at?: Date;
}

export interface WarehouseStock {
  warehouse_stock_id: number;
  warehouse_id: number;
  quantity: number;
}
