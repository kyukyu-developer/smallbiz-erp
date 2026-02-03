export interface Category {
  category_id: number;
  category_name: string;
  parent_category_id?: number;
  description?: string;
  status?: 'active' | 'inactive';
  created_at?: Date;
  updated_at?: Date;
}
