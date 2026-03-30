import { Observable } from 'rxjs';
import { Unit, CreateUnitDto, UpdateUnitDto } from '../entities/unit.entity';

export interface IUnitRepository {
  getAll(): Observable<Unit[]>;
  getById(id: string): Observable<Unit | null>;
  create(unit: CreateUnitDto): Observable<Unit>;
  update(id: string, unit: UpdateUnitDto): Observable<Unit>;
  delete(id: string): Observable<void>;
}
