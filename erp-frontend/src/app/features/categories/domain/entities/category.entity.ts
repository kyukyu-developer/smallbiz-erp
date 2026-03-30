export interface Category {
  id: string;
  code: string;
  name: string;
  description?: string;
  parentCategoryId?: string;
  active: boolean;
  createdAt?: Date;
  modifiedAt?: Date;
}

export interface CreateCategoryDto {
  code: string;
  name: string;
  description?: string;
  parentCategoryId?: string;
  active?: boolean;
}

export interface UpdateCategoryDto extends Partial<CreateCategoryDto> {
  id: string;
}

export interface CategoryFilter {
  search?: string;
  parentCategoryId?: string | null;
  active?: boolean | null;
  includeInactive?: boolean;
}
