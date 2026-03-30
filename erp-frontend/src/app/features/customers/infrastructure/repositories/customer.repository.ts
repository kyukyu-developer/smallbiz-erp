import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import {
  Customer,
  CreateCustomerDto,
  CustomerFilter,
} from '../../domain/entities/customer.entity';
import { ICustomerRepository } from '../../domain/repositories/customer.repository.interface';
import { environment } from '../../../../../environments/environment';

@Injectable({ providedIn: 'root' })
export class CustomerRepository implements ICustomerRepository {
  private http = inject(HttpClient);
  private apiUrl = `${environment.apiUrl}/customers`;

  getAll(): Observable<Customer[]> {
    return this.http.get<Customer[]>(this.apiUrl);
  }

  getById(id: string): Observable<Customer | null> {
    return this.http.get<Customer>(`${this.apiUrl}/${id}`);
  }

  getByFilter(filter: CustomerFilter): Observable<Customer[]> {
    let params = new HttpParams();
    if (filter.search) params = params.set('search', filter.search);
    if (filter.includeInactive !== undefined) 
      params = params.set('includeInactive', filter.includeInactive.toString());
    if (filter.active !== null && filter.active !== undefined)
      params = params.set('active', filter.active.toString());
    return this.http.get<Customer[]>(this.apiUrl, { params });
  }

  create(customer: CreateCustomerDto): Observable<Customer> {
    return this.http.post<Customer>(this.apiUrl, customer);
  }

  update(id: string, customer: Partial<Customer>): Observable<Customer> {
    return this.http.put<Customer>(`${this.apiUrl}/${id}`, customer);
  }

  delete(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
