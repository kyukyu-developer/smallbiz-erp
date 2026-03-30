import { Observable } from 'rxjs';
import { Supplier, CreateSupplierDto } from '../entities/supplier.entity';

export interface ISupplierRepository {
  getAll(): Observable<Supplier[]>;
  getById(id: string): Observable<Supplier | null>;
  create(supplier: CreateSupplierDto): Observable<Supplier>;
  update(id: string, supplier: Partial<Supplier>): Observable<Supplier>;
  delete(id: string): Observable<void>;
}
