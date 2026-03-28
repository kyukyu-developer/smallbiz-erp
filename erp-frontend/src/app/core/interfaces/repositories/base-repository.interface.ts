import { Observable } from 'rxjs';
import { BaseEntity, PaginationParams, PaginatedResult, FilterParams } from '../../models/base.model';

export interface IBaseRepository<T extends BaseEntity> {
  getAll(): Observable<T[]>;
  getById(id: string): Observable<T | null>;
  create(entity: Partial<T>): Observable<T>;
  update(id: string, entity: Partial<T>): Observable<T>;
  delete(id: string): Observable<void>;
  getPaginated(params: PaginationParams, filters?: FilterParams): Observable<PaginatedResult<T>>;
}
