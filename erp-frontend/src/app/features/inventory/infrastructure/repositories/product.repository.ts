import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Product, CreateProductDto, ProductFilter } from '../../domain/entities/product.entity';
import { IProductRepository } from '../../domain/repositories/product.repository.interface';
import { environment } from '../../../../../environments/environment';

@Injectable({ providedIn: 'root' })
export class ProductRepository implements IProductRepository {
  private http = inject(HttpClient);
  private apiUrl = `${environment.apiUrl}/products`;

  getAll(): Observable<Product[]> {
    return this.http.get<Product[]>(this.apiUrl);
  }

  getById(id: number): Observable<Product> {
    return this.http.get<Product>(`${this.apiUrl}/${id}`);
  }

  getByFilter(filter: ProductFilter): Observable<Product[]> {
    let params = new HttpParams();
    if (filter.category_id) params = params.set('categoryId', filter.category_id.toString());
    if (filter.search) params = params.set('search', filter.search);
    return this.http.get<Product[]>(this.apiUrl, { params });
  }

  create(product: CreateProductDto): Observable<Product> {
    return this.http.post<Product>(this.apiUrl, product);
  }

  update(id: number, product: Partial<Product>): Observable<Product> {
    return this.http.put<Product>(`${this.apiUrl}/${id}`, product);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}