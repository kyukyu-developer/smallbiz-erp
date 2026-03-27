import { BaseEntity } from '../../../../core/models/base.model';

export interface Warehouse extends BaseEntity {
  id: string;
  name: string;
  city: string;
  branch_type: 'Main' | 'Branch' | 'Sub';
  is_main_warehouse: boolean;
  parent_warehouse_id?: string | null;
  parent_warehouse_name?: string;
  is_used_warehouse?: boolean;
  active: boolean;
  phone?: string;
  email?: string;
  address?: string;
  description?: string;
  created_on?: Date;
  modified_on?: Date;
  created_by?: string;
  modified_by?: string;
  last_action?: string;
}

export interface CreateWarehouseDto {
  name: string;
  city: string;
  branch_type: 'Main' | 'Branch' | 'Sub';
  is_main_warehouse: boolean;
  parent_warehouse_id?: string | null;
  is_used_warehouse?: boolean;
  active?: boolean;
  description?: string;
  address?: string;
  phone?: string;
  email?: string;
}

export interface UpdateWarehouseDto extends Partial<CreateWarehouseDto> {
  id: string;
}

export interface WarehouseFilter {
  search?: string;
  branchTypes?: string[];
  cities?: string[];
  isUsed?: boolean | null;
  isActive?: boolean | null;
}