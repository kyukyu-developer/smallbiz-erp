export interface UnitConversion {
  id: string;
  productId: string;
  productName?: string;
  fromUnitId: string;
  fromUnitName?: string;
  toUnitId: string;
  toUnitName?: string;
  factor: number;
  active: boolean;
}

export interface CreateUnitConversionDto {
  productId: string;
  fromUnitId: string;
  toUnitId: string;
  factor: number;
  active?: boolean;
}
