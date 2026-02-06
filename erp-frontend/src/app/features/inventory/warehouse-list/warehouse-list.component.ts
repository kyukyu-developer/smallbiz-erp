import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Warehouse } from '../models/warehouse.model';

@Component({
  selector: 'app-warehouse-list',
  standalone: false,
  templateUrl: './warehouse-list.component.html',
  styleUrl: './warehouse-list.component.scss'
})
export class WarehouseListComponent implements OnInit {
  warehouses: Warehouse[] = [];
  displayedColumns: string[] = ['code', 'name', 'location', 'manager', 'capacity', 'occupancy', 'status', 'actions'];
  searchTerm: string = '';

  // Sample data (aligned with database schema)
  sampleWarehouses: Warehouse[] = [
    {
      warehouse_id: 1,
      warehouse_name: 'Main Warehouse',
      location: 'New York, NY',
      code: 'WH001',
      description: 'Primary storage facility',
      address: '123 Industrial Ave',
      city: 'New York',
      state: 'NY',
      zipCode: '10001',
      country: 'USA',
      phone: '+1 (555) 123-4567',
      email: 'main@warehouse.com',
      manager: 'John Smith',
      capacity: 10000,
      currentOccupancy: 7500,
      status: 'active',
      created_at: new Date('2024-01-01'),
      updated_at: new Date()
    },
    {
      warehouse_id: 2,
      warehouse_name: 'West Coast Distribution Center',
      location: 'Los Angeles, CA',
      code: 'WH002',
      description: 'Regional distribution hub',
      address: '456 Commerce Blvd',
      city: 'Los Angeles',
      state: 'CA',
      zipCode: '90001',
      country: 'USA',
      phone: '+1 (555) 987-6543',
      email: 'west@warehouse.com',
      manager: 'Sarah Johnson',
      capacity: 15000,
      currentOccupancy: 12000,
      status: 'active',
      created_at: new Date('2024-02-15'),
      updated_at: new Date()
    },
    {
      warehouse_id: 3,
      warehouse_name: 'East Coast Facility',
      location: 'Boston, MA',
      code: 'WH003',
      description: 'Coastal warehouse facility',
      address: '789 Warehouse Ln',
      city: 'Boston',
      state: 'MA',
      zipCode: '02101',
      country: 'USA',
      phone: '+1 (555) 456-7890',
      email: 'east@warehouse.com',
      manager: 'Mike Davis',
      capacity: 8000,
      currentOccupancy: 3000,
      status: 'active',
      created_at: new Date('2024-03-10'),
      updated_at: new Date()
    },
    {
      warehouse_id: 4,
      warehouse_name: 'Maintenance Depot',
      location: 'Chicago, IL',
      code: 'WH004',
      description: 'Under renovation',
      address: '321 Storage Dr',
      city: 'Chicago',
      state: 'IL',
      zipCode: '60601',
      country: 'USA',
      phone: '+1 (555) 321-0987',
      email: 'chicago@warehouse.com',
      manager: 'Emily Brown',
      capacity: 5000,
      currentOccupancy: 0,
      status: 'maintenance',
      created_at: new Date('2023-12-01'),
      updated_at: new Date()
    }
  ];

  constructor(private router: Router) {}

  ngOnInit() {
    this.warehouses = this.sampleWarehouses;
  }

  viewWarehouse(warehouse: Warehouse) {
    this.router.navigate(['/inventory/warehouses', warehouse.warehouse_id]);
  }

  editWarehouse(warehouse: Warehouse) {
    this.router.navigate(['/inventory/warehouses', warehouse.warehouse_id, 'edit']);
  }

  deleteWarehouse(warehouse: Warehouse) {
    if (confirm(`Are you sure you want to delete warehouse: ${warehouse.warehouse_name}?`)) {
      console.log('Delete warehouse:', warehouse);
    }
  }

  addNewWarehouse() {
    this.router.navigate(['/inventory/warehouses/new']);
  }

  applyFilter() {
    if (this.searchTerm) {
      this.warehouses = this.sampleWarehouses.filter(w =>
        w.warehouse_name?.toLowerCase().includes(this.searchTerm.toLowerCase()) ||
        w.code?.toLowerCase().includes(this.searchTerm.toLowerCase()) ||
        w.location?.toLowerCase().includes(this.searchTerm.toLowerCase()) ||
        w.city?.toLowerCase().includes(this.searchTerm.toLowerCase()) ||
        w.manager?.toLowerCase().includes(this.searchTerm.toLowerCase())
      );
    } else {
      this.warehouses = this.sampleWarehouses;
    }
  }

  getOccupancyPercentage(warehouse: Warehouse): number {
    if (!warehouse.capacity || !warehouse.currentOccupancy) return 0;
    return (warehouse.currentOccupancy / warehouse.capacity) * 100;
  }

  getOccupancyStatus(warehouse: Warehouse): string {
    const percentage = this.getOccupancyPercentage(warehouse);
    if (percentage >= 90) return 'critical';
    if (percentage >= 70) return 'warning';
    return 'normal';
  }

  getActiveCount(): number {
    return this.warehouses.filter(w => w.status === 'active').length;
  }

  getTotalCapacity(): number {
    return this.warehouses.reduce((sum, w) => sum + (w.capacity || 0), 0);
  }

  getAverageOccupancy(): number {
    if (this.warehouses.length === 0) return 0;
    const totalOccupancy = this.warehouses.reduce((sum, w) => sum + this.getOccupancyPercentage(w), 0);
    return totalOccupancy / this.warehouses.length;
  }
}
