import { Observable } from 'rxjs';
import { UnitConversion, CreateUnitConversionDto } from '../entities/unit-conversion.entity';

export interface IUnitConversionRepository {
  getAll(): Observable<UnitConversion[]>;
  getById(id: string): Observable<UnitConversion | null>;
  create(conversion: CreateUnitConversionDto): Observable<UnitConversion>;
  update(id: string, conversion: Partial<UnitConversion>): Observable<UnitConversion>;
  delete(id: string): Observable<void>;
}
