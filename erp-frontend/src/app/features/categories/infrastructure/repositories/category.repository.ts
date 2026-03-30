import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import {
  Category,
  CreateCategoryDto,
  CategoryFilter,
} from '../../domain/entities/category.entity';
import { ICategoryRepository } from '../../domain/repositories/category.repository.interface';
import { environment } from '../../../../../environments/environment';

@Injectable({ providedIn: 'root' })
export class CategoryRepository implements ICategoryRepository {
  private http = inject(HttpClient);
  private apiUrl = `${environment.apiUrl}/categories`;

  getAll(): Observable<Category[]> {
    return this.http.get<Category[]>(this.apiUrl);
  }

  getById(id: string): Observable<Category | null> {
    return this.http.get<Category>(`${this.apiUrl}/${id}`);
  }

  getByFilter(filter: CategoryFilter): Observable<Category[]> {
    let params = new HttpParams();
    if (filter.search) params = params.set('search', filter.search);
    if (filter.parentCategoryId !== undefined && filter.parentCategoryId !== null) 
      params = params.set('parentCategoryId', filter.parentCategoryId);
    if (filter.includeInactive !== undefined) 
      params = params.set('includeInactive', filter.includeInactive.toString());
    if (filter.active !== null && filter.active !== undefined)
      params = params.set('active', filter.active.toString());
    return this.http.get<Category[]>(this.apiUrl, { params });
  }

  create(category: CreateCategoryDto): Observable<Category> {
    return this.http.post<Category>(this.apiUrl, category);
  }

  update(id: string, category: Partial<Category>): Observable<Category> {
    return this.http.put<Category>(`${this.apiUrl}/${id}`, category);
  }

  delete(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
