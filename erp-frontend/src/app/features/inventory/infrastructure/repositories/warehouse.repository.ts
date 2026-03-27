import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of, delay } from 'rxjs';
import { Warehouse, CreateWarehouseDto, WarehouseFilter } from '../../domain/entities/warehouse.entity';
import { IWarehouseRepository } from '../../domain/repositories/warehouse.repository.interface';

@Injectable({ providedIn: 'root' })
export class WarehouseRepository implements IWarehouseRepository {
  private http = inject(HttpClient);
  private apiUrl = '/api/warehouses';

  private mockWarehouses: Warehouse[] = [
    {
      id: 'WH001',
      name: 'Main Warehouse - Central',
      city: 'New York',
      branch_type: 'Main',
      is_main_warehouse: true,
      parent_warehouse_id: null,
      is_used_warehouse: true,
      active: true,
      address: '123 Industrial Ave, New York, NY 10001',
      phone: '+1 (555) 123-4567',
      email: 'main@warehouse.com',
      created_on: new Date('2024-01-01'),
      created_by: 'admin',
      last_action: 'CREATE'
    },
    {
      id: 'WH002',
      name: 'Main Warehouse - West Coast',
      city: 'Los Angeles',
      branch_type: 'Main',
      is_main_warehouse: true,
      parent_warehouse_id: null,
      is_used_warehouse: true,
      active: true,
      address: '456 Commerce Blvd, Los Angeles, CA 90001',
      phone: '+1 (555) 987-6543',
      email: 'west@warehouse.com',
      created_on: new Date('2024-02-15'),
      created_by: 'admin',
      last_action: 'CREATE'
    },
    {
      id: 'WH003',
      name: 'Branch Warehouse - Boston',
      city: 'Boston',
      branch_type: 'Branch',
      is_main_warehouse: false,
      parent_warehouse_id: 'WH001',
      parent_warehouse_name: 'Main Warehouse - Central',
      is_used_warehouse: true,
      active: true,
      address: '789 Warehouse Ln, Boston, MA 02101',
      phone: '+1 (555) 456-7890',
      email: 'boston@warehouse.com',
      created_on: new Date('2024-03-10'),
      created_by: 'admin',
      last_action: 'CREATE'
    },
    {
      id: 'WH004',
      name: 'Sub Warehouse - Manhattan',
      city: 'New York',
      branch_type: 'Sub',
      is_main_warehouse: false,
      parent_warehouse_id: 'WH001',
      parent_warehouse_name: 'Main Warehouse - Central',
      is_used_warehouse: true,
      active: false,
      address: '321 Storage Dr, New York, NY 10002',
      phone: '+1 (555) 321-0987',
      email: 'manhattan@warehouse.com',
      created_on: new Date('2023-12-01'),
      created_by: 'admin',
      last_action: 'UPDATE'
    }
  ];

  getAll(): Observable<Warehouse[]> {
    return of(this.mockWarehouses).pipe(delay(500));
  }

  getById(id: string): Observable<Warehouse | null> {
    const warehouse = this.mockWarehouses.find(w => w.id === id) || null;
    return of(warehouse).pipe(delay(300));
  }

  getByFilter(filter: WarehouseFilter): Observable<Warehouse[]> {
    let filtered = [...this.mockWarehouses];
    
    if (filter.search) {
      const search = filter.search.toLowerCase();
      filtered = filtered.filter(w => 
        w.name?.toLowerCase().includes(search) || 
        w.city?.toLowerCase().includes(search) ||
        w.branch_type?.toLowerCase().includes(search)
      );
    }
    
    if (filter.branchTypes && filter.branchTypes.length > 0) {
      filtered = filtered.filter(w => filter.branchTypes?.includes(w.branch_type));
    }
    
    if (filter.cities && filter.cities.length > 0) {
      filtered = filtered.filter(w => filter.cities?.includes(w.city));
    }
    
    if (filter.isUsed !== null && filter.isUsed !== undefined) {
      filtered = filtered.filter(w => w.is_used_warehouse === filter.isUsed);
    }
    
    if (filter.isActive !== null && filter.isActive !== undefined) {
      filtered = filtered.filter(w => w.active === filter.isActive);
    }
    
    return of(filtered).pipe(delay(300));
  }

  create(warehouse: CreateWarehouseDto): Observable<Warehouse> {
    const newWarehouse: Warehouse = {
      ...warehouse,
      id: `WH${String(this.mockWarehouses.length + 1).padStart(3, '0')}`,
      created_on: new Date(),
      created_by: 'admin',
      last_action: 'CREATE'
    };
    
    this.mockWarehouses.push(newWarehouse);
    return of(newWarehouse).pipe(delay(500));
  }

  update(id: string, warehouse: Partial<Warehouse>): Observable<Warehouse> {
    const index = this.mockWarehouses.findIndex(w => w.id === id);
    if (index >= 0) {
      this.mockWarehouses[index] = { 
        ...this.mockWarehouses[index], 
        ...warehouse,
        modified_on: new Date(),
        last_action: 'UPDATE'
      };
      return of(this.mockWarehouses[index]).pipe(delay(500));
    }
    throw new Error('Warehouse not found');
  }

  delete(id: string): Observable<void> {
    const index = this.mockWarehouses.findIndex(w => w.id === id);
    if (index >= 0) {
      this.mockWarehouses.splice(index, 1);
    }
    return of(void 0).pipe(delay(500));
  }

  getMainWarehouses(): Observable<Warehouse[]> {
    const mainWarehouses = this.mockWarehouses.filter(w => w.is_main_warehouse);
    return of(mainWarehouses).pipe(delay(300));
  }
}