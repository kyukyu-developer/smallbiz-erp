export interface BaseEntity {
  id: string;
  createdOn?: Date;
  modifiedOn?: Date;
  createdBy?: string;
  lastAction?: string;
}

export interface PaginationParams {
  page: number;
  pageSize: number;
  sortBy?: string;
  sortOrder?: 'asc' | 'desc';
}

export interface PaginatedResult<T> {
  data: T[];
  total: number;
  page: number;
  pageSize: number;
  totalPages: number;
}

export interface ApiResponse<T> {
  success: boolean;
  data?: T;
  message?: string;
  errors?: string[];
}

export interface FilterParams {
  search?: string;
  [key: string]: any;
}