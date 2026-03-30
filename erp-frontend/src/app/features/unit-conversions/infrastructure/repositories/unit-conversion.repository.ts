import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { UnitConversion, CreateUnitConversionDto } from '../../domain/entities/unit-conversion.entity';
import { IUnitConversionRepository } from '../../domain/repositories/unit-conversion.repository.interface';
import { environment } from '../../../../../environments/environment';

@Injectable({ providedIn: 'root' })
export class UnitConversionRepository implements IUnitConversionRepository {
  private http = inject(HttpClient);
  private apiUrl = `${environment.apiUrl}/productunitconversion`;

  getAll(): Observable<UnitConversion[]> { return this.http.get<UnitConversion[]>(this.apiUrl); }
  getById(id: string): Observable<UnitConversion | null> { return this.http.get<UnitConversion>(`${this.apiUrl}/${id}`); }
  create(conversion: CreateUnitConversionDto): Observable<UnitConversion> { return this.http.post<UnitConversion>(this.apiUrl, conversion); }
  update(id: string, conversion: Partial<UnitConversion>): Observable<UnitConversion> { return this.http.put<UnitConversion>(`${this.apiUrl}/${id}`, conversion); }
  delete(id: string): Observable<void> { return this.http.delete<void>(`${this.apiUrl}/${id}`); }
}
