import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of, delay } from 'rxjs';
import {
  Brand,
  CreateBrandDto,
  BrandFilter,
} from '../../domain/entities/brand.entity';
import { IBrandRepository } from '../../domain/repositories/brand.repository.interface';
import { environment } from '../../../../../environments/environment';

@Injectable({ providedIn: 'root' })
export class BrandRepository implements IBrandRepository {
  private http = inject(HttpClient);
  private apiUrl = `${environment.apiUrl}/brands`;

  getAll(): Observable<Brand[]> {
    return this.http.get<Brand[]>(this.apiUrl);
  }

  getById(id: string): Observable<Brand | null> {
    return this.http.get<Brand>(`${this.apiUrl}/${id}`);
  }

  getByFilter(filter: BrandFilter): Observable<Brand[]> {
    const params: any = {};
    if (filter.search) params.search = filter.search;
    if (filter.active !== null && filter.active !== undefined)
      params.active = filter.active;
    return this.http.get<Brand[]>(this.apiUrl, { params });
  }

  create(brand: CreateBrandDto): Observable<Brand> {
    return this.http.post<Brand>(this.apiUrl, brand);
  }

  update(id: string, brand: Partial<Brand>): Observable<Brand> {
    return this.http.put<Brand>(`${this.apiUrl}/${id}`, brand);
  }

  delete(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
