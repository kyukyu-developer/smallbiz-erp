export interface Unit {
  unit_id: number;
  unit_name: string;
}

export interface UnitConversion {
  conversion_id: number;
  product_id: number;
  from_unit_id: number;
  to_unit_id: number;
  factor: number;
}
