export interface Sale {
  sale_id: number;
  customer_id: number;
  warehouse_id: number;
  sale_date: Date;

  // Additional fields for UI/display
  sale_no?: string;
  total_amount?: number;
  payment_status?: 'pending' | 'partial' | 'paid';
  status?: 'draft' | 'confirmed' | 'completed' | 'cancelled';
}

export interface SalesItem {
  sales_item_id: number;
  sale_id: number;
  product_id: number;
  unit_id: number;
  quantity: number;
  unit_price: number;
  base_quantity: number;
  batch_id?: number;
  serial_id?: number;

  // Additional fields for UI/display
  discount?: number;
  tax?: number;
  total?: number;
}
