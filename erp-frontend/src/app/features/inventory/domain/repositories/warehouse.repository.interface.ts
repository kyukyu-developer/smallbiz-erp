import { Observable } from 'rxjs';
import { Warehouse, CreateWarehouseDto, UpdateWarehouseDto, WarehouseFilter } from '../entities/warehouse.entity';

export interface IWarehouseRepository {
  getAll(): Observable<Warehouse[]>;
  getById(id: string): Observable<Warehouse | null>;
  getByFilter(filter: WarehouseFilter): Observable<Warehouse[]>;
  create(warehouse: CreateWarehouseDto): Observable<Warehouse>;
  update(id: string, warehouse: UpdateWarehouseDto): Observable<Warehouse>;
  delete(id: string): Observable<void>;
  getMainWarehouses(): Observable<Warehouse[]>;
}
