import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Warehouse, CreateWarehouseDto, WarehouseFilter } from '../../domain/entities/warehouse.entity';
import { IWarehouseRepository } from '../../domain/repositories/warehouse.repository.interface';
import { environment } from '../../../../../environments/environment';

@Injectable({ providedIn: 'root' })
export class WarehouseRepository implements IWarehouseRepository {
  private http = inject(HttpClient);
  private apiUrl = `${environment.apiUrl}/warehouses`;

  getAll(): Observable<Warehouse[]> {
    return this.http.get<Warehouse[]>(this.apiUrl);
  }

  getById(id: string): Observable<Warehouse> {
    return this.http.get<Warehouse>(`${this.apiUrl}/${id}`);
  }

  getByFilter(filter: WarehouseFilter): Observable<Warehouse[]> {
    let params = new HttpParams();
    if (filter.branchTypes?.length) params = params.set('branchType', filter.branchTypes[0]);
    return this.http.get<Warehouse[]>(this.apiUrl, { params });
  }

  create(warehouse: CreateWarehouseDto): Observable<Warehouse> {
    return this.http.post<Warehouse>(this.apiUrl, warehouse);
  }

  update(id: string, warehouse: Partial<Warehouse>): Observable<Warehouse> {
    return this.http.put<Warehouse>(`${this.apiUrl}/${id}`, warehouse);
  }

  delete(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }

  getMainWarehouses(): Observable<Warehouse[]> {
    return this.http.get<Warehouse[]>(this.apiUrl, { 
      params: new HttpParams().set('mainWarehousesOnly', 'true') 
    });
  }
}
