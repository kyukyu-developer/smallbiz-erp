import { Observable } from 'rxjs';
import { Product, CreateProductDto, UpdateProductDto, ProductFilter } from '../entities/product.entity';

export interface IProductRepository {
  getAll(): Observable<Product[]>;
  getById(id: number): Observable<Product | null>;
  getByFilter(filter: ProductFilter): Observable<Product[]>;
  create(product: CreateProductDto): Observable<Product>;
  update(id: number, product: UpdateProductDto): Observable<Product>;
  delete(id: number): Observable<void>;
}