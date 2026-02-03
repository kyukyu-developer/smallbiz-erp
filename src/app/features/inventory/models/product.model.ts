export interface Product {
  product_id: number;
  name: string;
  category_id: number;
  base_unit_id: number;
  tracking_type: 'none' | 'batch' | 'serial';
  base_cost_price: number;
  base_sale_price: number;

  // Additional fields for UI/display (not in DB schema)
  code?: string;
  description?: string;
  status?: 'active' | 'inactive';
  created_at?: Date;
  updated_at?: Date;
}
