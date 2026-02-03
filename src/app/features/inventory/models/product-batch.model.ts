export interface ProductBatch {
  batch_id: number;
  product_id: number;
  warehouse_id: number;
  batch_no: string;
  expiry_date: Date;
  quantity: number;
}

export interface ProductSerial {
  serial_id: number;
  product_id: number;
  serial_no: string;
  status: 'available' | 'sold' | 'damaged' | 'reserved';
}
