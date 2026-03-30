import { Observable } from 'rxjs';
import {
  ProductGroup,
  CreateProductGroupDto,
  UpdateProductGroupDto,
  ProductGroupFilter,
} from '../entities/product-group.entity';

export interface IProductGroupRepository {
  getAll(): Observable<ProductGroup[]>;
  getById(id: string): Observable<ProductGroup | null>;
  getByFilter(filter: ProductGroupFilter): Observable<ProductGroup[]>;
  create(productGroup: CreateProductGroupDto): Observable<ProductGroup>;
  update(id: string, productGroup: UpdateProductGroupDto): Observable<ProductGroup>;
  delete(id: string): Observable<void>;
}
