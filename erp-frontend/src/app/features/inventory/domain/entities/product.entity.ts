import { BaseEntity } from '../../../../core/models/base.model';

export interface Product extends BaseEntity {
  id: string;
  product_id: number;
  name: string;
  code?: string;
  category_id: number;
  category_name?: string;
  base_unit_id: number;
  tracking_type: 'none' | 'batch' | 'serial';
  base_cost_price: number;
  base_sale_price: number;
  description?: string;
  status?: 'active' | 'inactive';
}

export interface CreateProductDto {
  name: string;
  code?: string;
  category_id: number;
  base_unit_id: number;
  tracking_type: 'none' | 'batch' | 'serial';
  base_cost_price: number;
  base_sale_price: number;
  description?: string;
}

export interface UpdateProductDto extends Partial<CreateProductDto> {
  product_id: number;
  status?: 'active' | 'inactive';
}

export interface ProductFilter {
  search?: string;
  category_id?: number;
  status?: 'active' | 'inactive';
}