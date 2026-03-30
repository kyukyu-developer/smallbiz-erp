import { Observable } from 'rxjs';
import {
  Customer,
  CreateCustomerDto,
  UpdateCustomerDto,
  CustomerFilter,
} from '../entities/customer.entity';

export interface ICustomerRepository {
  getAll(): Observable<Customer[]>;
  getById(id: string): Observable<Customer | null>;
  getByFilter(filter: CustomerFilter): Observable<Customer[]>;
  create(customer: CreateCustomerDto): Observable<Customer>;
  update(id: string, customer: UpdateCustomerDto): Observable<Customer>;
  delete(id: string): Observable<void>;
}
