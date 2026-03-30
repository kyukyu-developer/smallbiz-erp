import { Injectable, inject } from '@angular/core';
import { Observable, map, catchError, of } from 'rxjs';
import { Supplier } from '../../domain/entities/supplier.entity';
import { ISupplierRepository } from '../../domain/repositories/supplier.repository.interface';
import { SUPPLIER_REPOSITORY } from '../../domain/repositories/supplier.token';

@Injectable({ providedIn: 'root' })
export class GetAllSuppliersUseCase {
  private repository = inject<ISupplierRepository>(SUPPLIER_REPOSITORY);
  execute(): Observable<{ suppliers: Supplier[]; total: number }> {
    return this.repository.getAll().pipe(
      map((suppliers: Supplier[]) => ({ suppliers, total: suppliers.length })),
      catchError(() => of({ suppliers: [] as Supplier[], total: 0 })),
    );
  }
}

@Injectable({ providedIn: 'root' })
export class GetSupplierByIdUseCase {
  private repository = inject<ISupplierRepository>(SUPPLIER_REPOSITORY);
  execute(id: string): Observable<Supplier | null> { return this.repository.getById(id); }
}

@Injectable({ providedIn: 'root' })
export class CreateSupplierUseCase {
  private repository = inject<ISupplierRepository>(SUPPLIER_REPOSITORY);
  execute(supplier: Partial<Supplier>): Observable<Supplier> { return this.repository.create(supplier as Supplier); }
}

@Injectable({ providedIn: 'root' })
export class UpdateSupplierUseCase {
  private repository = inject<ISupplierRepository>(SUPPLIER_REPOSITORY);
  execute(id: string, supplier: Partial<Supplier>): Observable<Supplier> { return this.repository.update(id, supplier as Supplier); }
}

@Injectable({ providedIn: 'root' })
export class DeleteSupplierUseCase {
  private repository = inject<ISupplierRepository>(SUPPLIER_REPOSITORY);
  execute(id: string): Observable<void> { return this.repository.delete(id); }
}
