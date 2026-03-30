import { Observable } from 'rxjs';
import {
  Category,
  CreateCategoryDto,
  UpdateCategoryDto,
  CategoryFilter,
} from '../entities/category.entity';

export interface ICategoryRepository {
  getAll(): Observable<Category[]>;
  getById(id: string): Observable<Category | null>;
  getByFilter(filter: CategoryFilter): Observable<Category[]>;
  create(category: CreateCategoryDto): Observable<Category>;
  update(id: string, category: UpdateCategoryDto): Observable<Category>;
  delete(id: string): Observable<void>;
}
