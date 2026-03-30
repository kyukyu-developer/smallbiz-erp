import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Unit, CreateUnitDto } from '../../domain/entities/unit.entity';
import { IUnitRepository } from '../../domain/repositories/unit.repository.interface';
import { environment } from '../../../../../environments/environment';

@Injectable({ providedIn: 'root' })
export class UnitRepository implements IUnitRepository {
  private http = inject(HttpClient);
  private apiUrl = `${environment.apiUrl}/units`;

  getAll(): Observable<Unit[]> {
    return this.http.get<Unit[]>(this.apiUrl);
  }

  getById(id: string): Observable<Unit | null> {
    return this.http.get<Unit>(`${this.apiUrl}/${id}`);
  }

  create(unit: CreateUnitDto): Observable<Unit> {
    return this.http.post<Unit>(this.apiUrl, unit);
  }

  update(id: string, unit: Partial<Unit>): Observable<Unit> {
    return this.http.put<Unit>(`${this.apiUrl}/${id}`, unit);
  }

  delete(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
