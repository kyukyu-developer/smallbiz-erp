import { Injectable, inject } from '@angular/core';
import { Observable, map } from 'rxjs';
import { Warehouse, WarehouseFilter } from '../../domain/entities/warehouse.entity';
import { IWarehouseRepository } from '../../domain/repositories/warehouse.repository.interface';

export interface GetAllWarehousesResult {
  warehouses: Warehouse[];
  total: number;
}

export interface WarehouseGroup {
  parent: Warehouse;
  children: Warehouse[];
  expanded: boolean;
}

@Injectable({ providedIn: 'root' })
export class GetAllWarehousesUseCase {
  private repository = inject(IWarehouseRepository);

  execute(): Observable<GetAllWarehousesResult> {
    return this.repository.getAll().pipe(
      map(warehouses => ({
        warehouses,
        total: warehouses.length
      }))
    );
  }
}

@Injectable({ providedIn: 'root' })
export class GetWarehousesByFilterUseCase {
  private repository = inject(IWarehouseRepository);

  execute(filter: WarehouseFilter): Observable<Warehouse[]> {
    return this.repository.getByFilter(filter);
  }
}

@Injectable({ providedIn: 'root' })
export class GetWarehouseByIdUseCase {
  private repository = inject(IWarehouseRepository);

  execute(id: string): Observable<Warehouse | null> {
    return this.repository.getById(id);
  }
}

@Injectable({ providedIn: 'root' })
export class CreateWarehouseUseCase {
  private repository = inject(IWarehouseRepository);

  execute(warehouse: Partial<Warehouse>): Observable<Warehouse> {
    return this.repository.create(warehouse as any);
  }
}

@Injectable({ providedIn: 'root' })
export class UpdateWarehouseUseCase {
  private repository = inject(IWarehouseRepository);

  execute(id: string, warehouse: Partial<Warehouse>): Observable<Warehouse> {
    return this.repository.update(id, warehouse as any);
  }
}

@Injectable({ providedIn: 'root' })
export class DeleteWarehouseUseCase {
  private repository = inject(IWarehouseRepository);

  execute(id: string): Observable<void> {
    return this.repository.delete(id);
  }
}

@Injectable({ providedIn: 'root' })
export class GetMainWarehousesUseCase {
  private repository = inject(IWarehouseRepository);

  execute(): Observable<Warehouse[]> {
    return this.repository.getMainWarehouses();
  }
}

@Injectable({ providedIn: 'root' })
export class GetWarehouseGroupsUseCase {
  private repository = inject(IWarehouseRepository);

  execute(): Observable<WarehouseGroup[]> {
    return this.repository.getAll().pipe(
      map(warehouses => {
        const childWarehouses = warehouses.filter(w => !w.is_main_warehouse);
        const parentIdsNeeded = new Set(
          childWarehouses
            .map(w => w.parent_warehouse_id)
            .filter(id => id !== null && id !== undefined) as string[]
        );

        const parentsToDisplay = warehouses.filter(w => {
          return w.is_main_warehouse === true && w.id && (
            parentIdsNeeded.has(w.id) || warehouses.includes(w)
          );
        });

        return parentsToDisplay.map(parent => ({
          parent,
          children: warehouses.filter(w => w.parent_warehouse_id === parent.id && !w.is_main_warehouse),
          expanded: true
        }));
      })
    );
  }
}