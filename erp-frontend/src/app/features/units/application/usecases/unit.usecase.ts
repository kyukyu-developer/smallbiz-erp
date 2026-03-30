import { Injectable, inject } from '@angular/core';
import { Observable, map, catchError, of } from 'rxjs';
import { Unit } from '../../domain/entities/unit.entity';
import { IUnitRepository } from '../../domain/repositories/unit.repository.interface';
import { UNIT_REPOSITORY } from '../../domain/repositories/unit.token';

export interface GetAllUnitsResult {
  units: Unit[];
  total: number;
}

@Injectable({ providedIn: 'root' })
export class GetAllUnitsUseCase {
  private repository = inject<IUnitRepository>(UNIT_REPOSITORY);

  execute(): Observable<GetAllUnitsResult> {
    return this.repository.getAll().pipe(
      map((units: Unit[]) => ({ units, total: units.length })),
      catchError((error: unknown) => {
        console.error('Error fetching units:', error);
        return of({ units: [] as Unit[], total: 0 });
      }),
    );
  }
}

@Injectable({ providedIn: 'root' })
export class GetUnitByIdUseCase {
  private repository = inject<IUnitRepository>(UNIT_REPOSITORY);
  execute(id: string): Observable<Unit | null> {
    return this.repository.getById(id);
  }
}

@Injectable({ providedIn: 'root' })
export class CreateUnitUseCase {
  private repository = inject<IUnitRepository>(UNIT_REPOSITORY);
  execute(unit: Partial<Unit>): Observable<Unit> {
    return this.repository.create(unit as Unit);
  }
}

@Injectable({ providedIn: 'root' })
export class UpdateUnitUseCase {
  private repository = inject<IUnitRepository>(UNIT_REPOSITORY);
  execute(id: string, unit: Partial<Unit>): Observable<Unit> {
    return this.repository.update(id, unit as Unit);
  }
}

@Injectable({ providedIn: 'root' })
export class DeleteUnitUseCase {
  private repository = inject<IUnitRepository>(UNIT_REPOSITORY);
  execute(id: string): Observable<void> {
    return this.repository.delete(id);
  }
}
