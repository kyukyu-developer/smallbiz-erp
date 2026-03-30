import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import {
  ProductGroup,
  CreateProductGroupDto,
  ProductGroupFilter,
} from '../../domain/entities/product-group.entity';
import { IProductGroupRepository } from '../../domain/repositories/product-group.repository.interface';
import { environment } from '../../../../../environments/environment';

@Injectable({ providedIn: 'root' })
export class ProductGroupRepository implements IProductGroupRepository {
  private http = inject(HttpClient);
  private apiUrl = `${environment.apiUrl}/productgroup`;

  getAll(): Observable<ProductGroup[]> {
    return this.http.get<ProductGroup[]>(this.apiUrl);
  }

  getById(id: string): Observable<ProductGroup | null> {
    return this.http.get<ProductGroup>(`${this.apiUrl}/${id}`);
  }

  getByFilter(filter: ProductGroupFilter): Observable<ProductGroup[]> {
    let params = new HttpParams();
    if (filter.search) params = params.set('search', filter.search);
    if (filter.active !== null && filter.active !== undefined)
      params = params.set('active', filter.active.toString());
    return this.http.get<ProductGroup[]>(this.apiUrl, { params });
  }

  create(productGroup: CreateProductGroupDto): Observable<ProductGroup> {
    return this.http.post<ProductGroup>(this.apiUrl, productGroup);
  }

  update(id: string, productGroup: Partial<ProductGroup>): Observable<ProductGroup> {
    return this.http.put<ProductGroup>(`${this.apiUrl}/${id}`, productGroup);
  }

  delete(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
