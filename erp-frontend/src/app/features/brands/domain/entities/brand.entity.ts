export interface Brand {
  id: string;
  name: string;
  description?: string;
  active: boolean;
  createdOn?: Date;
  modifiedOn?: Date;
}

export interface CreateBrandDto {
  name: string;
  description?: string;
  active?: boolean;
}

export interface UpdateBrandDto extends Partial<CreateBrandDto> {
  id: string;
}

export interface BrandFilter {
  search?: string;
  active?: boolean | null;
}
