import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Supplier, CreateSupplierDto } from '../../domain/entities/supplier.entity';
import { ISupplierRepository } from '../../domain/repositories/supplier.repository.interface';
import { environment } from '../../../../../environments/environment';

@Injectable({ providedIn: 'root' })
export class SupplierRepository implements ISupplierRepository {
  private http = inject(HttpClient);
  private apiUrl = `${environment.apiUrl}/suppliers`;

  getAll(): Observable<Supplier[]> { return this.http.get<Supplier[]>(this.apiUrl); }
  getById(id: string): Observable<Supplier | null> { return this.http.get<Supplier>(`${this.apiUrl}/${id}`); }
  create(supplier: CreateSupplierDto): Observable<Supplier> { return this.http.post<Supplier>(this.apiUrl, supplier); }
  update(id: string, supplier: Partial<Supplier>): Observable<Supplier> { return this.http.put<Supplier>(`${this.apiUrl}/${id}`, supplier); }
  delete(id: string): Observable<void> { return this.http.delete<void>(`${this.apiUrl}/${id}`); }
}
