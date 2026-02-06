export interface StockMovement {
  movement_id: number;
  product_id: number;
  warehouse_id: number;
  movement_type: 'in' | 'out' | 'adjustment' | 'transfer';
  reference_type: 'purchase' | 'sale' | 'adjustment' | 'transfer' | 'return';
  reference_id: number;
  base_quantity: number;
  batch_id?: number;
  movement_date: Date;
}
