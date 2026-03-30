export interface Supplier {
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
  paymentTermDays?: number;
  active: boolean;
}

export interface CreateSupplierDto {
  code: string;
  name: string;
  contactPerson?: string;
  phone?: string;
  email?: string;
  address?: string;
  city?: string;
  country?: string;
  taxNumber?: string;
  paymentTermDays?: number;
  active?: boolean;
}
