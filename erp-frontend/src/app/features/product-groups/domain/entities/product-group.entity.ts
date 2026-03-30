export interface ProductGroup {
  id: string;
  name: string;
  description?: string;
  active: boolean;
  createdAt?: Date;
  updatedAt?: Date;
  createdBy?: string;
  updatedBy?: string;
  lastAction?: string;
}

export interface CreateProductGroupDto {
  name: string;
  description?: string;
  active?: boolean;
}

export interface UpdateProductGroupDto extends Partial<CreateProductGroupDto> {
  id: string;
}

export interface ProductGroupFilter {
  search?: string;
  active?: boolean | null;
}
