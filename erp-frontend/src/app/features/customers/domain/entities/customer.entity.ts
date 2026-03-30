export interface Customer {
  id: string;
  code: string;
  name: string;
  contactPerson?: string;
  phone?: string;
  email?: string;
  address?: string;
  city?: string;
  country?: string;
  taxNumber?: string;
  creditLimit?: number;
  active: boolean;
  createdAt?: Date;
  modifiedAt?: Date;
}

export interface CreateCustomerDto {
  code: string;
  name: string;
  contactPerson?: string;
  phone?: string;
  email?: string;
  address?: string;
  city?: string;
  country?: string;
  taxNumber?: string;
  creditLimit?: number;
  active?: boolean;
}

export interface UpdateCustomerDto extends Partial<CreateCustomerDto> {
  id: string;
}

export interface CustomerFilter {
  search?: string;
  active?: boolean | null;
  includeInactive?: boolean;
}
