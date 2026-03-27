import { Observable } from 'rxjs';
import {
  Brand,
  CreateBrandDto,
  UpdateBrandDto,
  BrandFilter,
} from '../entities/brand.entity';

export interface IBrandRepository {
  getAll(): Observable<Brand[]>;
  getById(id: string): Observable<Brand | null>;
  getByFilter(filter: BrandFilter): Observable<Brand[]>;
  create(brand: CreateBrandDto): Observable<Brand>;
  update(id: string, brand: UpdateBrandDto): Observable<Brand>;
  delete(id: string): Observable<void>;
}
