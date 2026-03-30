import { Injectable, inject } from '@angular/core';
import { Observable, map, catchError, of } from 'rxjs';
import { UnitConversion } from '../../domain/entities/unit-conversion.entity';
import { IUnitConversionRepository } from '../../domain/repositories/unit-conversion.repository.interface';
import { UNIT_CONVERSION_REPOSITORY } from '../../domain/repositories/unit-conversion.token';

@Injectable({ providedIn: 'root' })
export class GetAllUnitConversionsUseCase {
  private repository = inject<IUnitConversionRepository>(UNIT_CONVERSION_REPOSITORY);
  execute(): Observable<{ conversions: UnitConversion[]; total: number }> {
    return this.repository.getAll().pipe(
      map((conversions: UnitConversion[]) => ({ conversions, total: conversions.length })),
      catchError(() => of({ conversions: [] as UnitConversion[], total: 0 })),
    );
  }
}

@Injectable({ providedIn: 'root' })
export class GetUnitConversionByIdUseCase {
  private repository = inject<IUnitConversionRepository>(UNIT_CONVERSION_REPOSITORY);
  execute(id: string): Observable<UnitConversion | null> { return this.repository.getById(id); }
}

@Injectable({ providedIn: 'root' })
export class CreateUnitConversionUseCase {
  private repository = inject<IUnitConversionRepository>(UNIT_CONVERSION_REPOSITORY);
  execute(conversion: Partial<UnitConversion>): Observable<UnitConversion> { return this.repository.create(conversion as UnitConversion); }
}

@Injectable({ providedIn: 'root' })
export class UpdateUnitConversionUseCase {
  private repository = inject<IUnitConversionRepository>(UNIT_CONVERSION_REPOSITORY);
  execute(id: string, conversion: Partial<UnitConversion>): Observable<UnitConversion> { return this.repository.update(id, conversion as UnitConversion); }
}

@Injectable({ providedIn: 'root' })
export class DeleteUnitConversionUseCase {
  private repository = inject<IUnitConversionRepository>(UNIT_CONVERSION_REPOSITORY);
  execute(id: string): Observable<void> { return this.repository.delete(id); }
}
