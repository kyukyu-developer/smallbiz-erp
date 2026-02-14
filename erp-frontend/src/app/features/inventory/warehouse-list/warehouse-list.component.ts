import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';
import { Warehouse } from '../models/warehouse.model';
import { WarehouseFilterDialogComponent } from '../warehouse-filter-dialog/warehouse-filter-dialog.component';

interface WarehouseGroup {
  parent: Warehouse;
  children: Warehouse[];
  expanded: boolean;
}

interface WarehouseFilter {
  branchTypes: string[];
  cities: string[];
  isUsed?: boolean | null;
  isActive?: boolean | null;
}

@Component({
  selector: 'app-warehouse-list',
  standalone: false,
  templateUrl: './warehouse-list.component.html',
  styleUrl: './warehouse-list.component.scss'
})
export class WarehouseListComponent implements OnInit {
  warehouses: Warehouse[] = [];
  warehouseGroups: WarehouseGroup[] = [];
  displayedColumns: string[] = ['name', 'city', 'branch_type', 'parent', 'status', 'actions'];
  searchTerm: string = '';
  showFilters: boolean = false;

  // Filter options
  filters: WarehouseFilter = {
    branchTypes: [],
    cities: [],
    isUsed: null,
    isActive: null
  };

  // Available filter options
  availableBranchTypes: string[] = ['Main', 'Branch', 'Sub'];
  availableCities: string[] = [];
  activeFilterCount: number = 0;

  // Sample data (aligned with new database schema from INVENTORY_DATABASE_NOTES.txt)
  sampleWarehouses: Warehouse[] = [
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
      modified_on: new Date(),
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
      modified_on: new Date(),
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
      modified_on: new Date(),
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
      modified_on: new Date(),
      created_by: 'admin',
      last_action: 'UPDATE'
    }
  ];

  constructor(
    private router: Router,
    private dialog: MatDialog
  ) {}

  ngOnInit() {
    this.warehouses = this.sampleWarehouses;
    this.extractFilterOptions();
    this.buildWarehouseGroups();
  }

  /**
   * Extract unique cities from warehouses for filter options
   */
  extractFilterOptions(): void {
    const cities = new Set<string>();
    this.sampleWarehouses.forEach(w => {
      if (w.city) cities.add(w.city);
    });
    this.availableCities = Array.from(cities).sort();
  }

  /**
   * Open filter dialog
   */
  toggleFilters(): void {
    const dialogRef = this.dialog.open(WarehouseFilterDialogComponent, {
      width: '600px',
      maxWidth: '90vw',
      maxHeight: '90vh',
      panelClass: 'warehouse-filter-dialog',
      data: {
        filters: this.filters,
        availableBranchTypes: this.availableBranchTypes,
        availableCities: this.availableCities
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        // Apply the filters returned from the dialog
        this.filters = result;
        this.applyAllFilters();
      }
    });
  }

  /**
   * Toggle branch type filter
   */
  toggleBranchTypeFilter(type: string): void {
    const index = this.filters.branchTypes.indexOf(type);
    if (index > -1) {
      this.filters.branchTypes.splice(index, 1);
    } else {
      this.filters.branchTypes.push(type);
    }
    this.applyAllFilters();
  }

  /**
   * Toggle city filter
   */
  toggleCityFilter(city: string): void {
    const index = this.filters.cities.indexOf(city);
    if (index > -1) {
      this.filters.cities.splice(index, 1);
    } else {
      this.filters.cities.push(city);
    }
    this.applyAllFilters();
  }

  /**
   * Set status filter
   */
  setStatusFilter(status: 'used' | 'active' | null): void {
    if (status === 'used') {
      this.filters.isUsed = this.filters.isUsed === true ? null : true;
    } else if (status === 'active') {
      this.filters.isActive = this.filters.isActive === true ? null : true;
    }
    this.applyAllFilters();
  }

  /**
   * Clear all filters
   */
  clearFilters(): void {
    this.filters = {
      branchTypes: [],
      cities: [],
      isUsed: null,
      isActive: null
    };
    this.applyAllFilters();
  }

  /**
   * Apply all filters and rebuild groups
   */
  applyAllFilters(): void {
    let filtered = this.sampleWarehouses;

    // Apply search term filter
    if (this.searchTerm) {
      filtered = filtered.filter(w =>
        w.name?.toLowerCase().includes(this.searchTerm.toLowerCase()) ||
        w.city?.toLowerCase().includes(this.searchTerm.toLowerCase()) ||
        w.branch_type?.toLowerCase().includes(this.searchTerm.toLowerCase()) ||
        w.parent_warehouse_name?.toLowerCase().includes(this.searchTerm.toLowerCase())
      );
    }

    // Apply branch type filter
    if (this.filters.branchTypes.length > 0) {
      filtered = filtered.filter(w =>
        this.filters.branchTypes.includes(w.branch_type)
      );
    }

    // Apply city filter
    if (this.filters.cities.length > 0) {
      filtered = filtered.filter(w =>
        this.filters.cities.includes(w.city)
      );
    }

    // Apply "In Use" filter
    if (this.filters.isUsed !== null) {
      filtered = filtered.filter(w => w.is_used_warehouse === this.filters.isUsed);
    }

    // Apply "Active" filter
    if (this.filters.isActive !== null) {
      filtered = filtered.filter(w => w.active === this.filters.isActive);
    }

    this.warehouses = filtered;
    this.buildWarehouseGroups();
    this.updateActiveFilterCount();
  }

  /**
   * Update the count of active filters
   */
  updateActiveFilterCount(): void {
    let count = 0;
    count += this.filters.branchTypes.length;
    count += this.filters.cities.length;
    if (this.filters.isUsed !== null) count++;
    if (this.filters.isActive !== null) count++;
    this.activeFilterCount = count;
  }

  /**
   * Check if a branch type is selected
   */
  isBranchTypeSelected(type: string): boolean {
    return this.filters.branchTypes.includes(type);
  }

  /**
   * Check if a city is selected
   */
  isCitySelected(city: string): boolean {
    return this.filters.cities.includes(city);
  }

  /**
   * Build warehouse groups by parent
   */
  buildWarehouseGroups(): void {
    // Get all child warehouses from filtered results
    const childWarehouses = this.warehouses.filter(w => !w.is_main_warehouse);

    // Find parent IDs of filtered child warehouses
    const parentIdsNeeded = new Set(
      childWarehouses
        .map(w => w.parent_warehouse_id)
        .filter(id => id !== null && id !== undefined) as string[]
    );

    // Get parent warehouses that are either in filtered results OR have filtered children
    const parentsToDisplay = this.sampleWarehouses.filter(w => {
      return w.is_main_warehouse === true && w.id && (
        this.warehouses.includes(w) || // Parent is in filtered results
        parentIdsNeeded.has(w.id)      // Parent has a child in filtered results
      );
    });

    this.warehouseGroups = parentsToDisplay.map(parent => {
      // Find all children of this parent that are in the filtered results
      const children = this.warehouses.filter(w =>
        w.parent_warehouse_id === parent.id && !w.is_main_warehouse
      );

      return {
        parent: parent,
        children: children,
        expanded: true // Default to expanded
      };
    });
  }

  /**
   * Toggle group expansion
   */
  toggleGroup(group: WarehouseGroup): void {
    group.expanded = !group.expanded;
  }

  viewWarehouse(warehouse: Warehouse) {
    this.router.navigate(['/inventory/warehouses', warehouse.id]);
  }

  editWarehouse(warehouse: Warehouse) {
    this.router.navigate(['/inventory/warehouses', warehouse.id, 'edit']);
  }

  deleteWarehouse(warehouse: Warehouse) {
    if (confirm(`Are you sure you want to delete warehouse: ${warehouse.name}?`)) {
      console.log('Delete warehouse:', warehouse);
      // TODO: Implement service call to delete warehouse
    }
  }

  addNewWarehouse() {
    this.router.navigate(['/inventory/warehouses/new']);
  }

  applyFilter() {
    this.applyAllFilters();
  }

  /**
   * Get count of active warehouses
   */
  getActiveCount(): number {
    return this.warehouses.filter(w => w.active === true).length;
  }

  /**
   * Get count of main warehouses
   */
  getMainWarehouseCount(): number {
    return this.warehouses.filter(w => w.is_main_warehouse === true).length;
  }

  /**
   * Get count of warehouses in use
   */
  getUsedWarehouseCount(): number {
    return this.warehouses.filter(w => w.is_used_warehouse === true).length;
  }

  /**
   * Get count by branch type
   */
  getBranchTypeCount(type: 'Main' | 'Branch' | 'Sub'): number {
    return this.warehouses.filter(w => w.branch_type === type).length;
  }

  /**
   * Get badge class for warehouse type
   */
  getBranchTypeBadgeClass(type: string): string {
    switch (type) {
      case 'Main':
        return 'badge-main';
      case 'Branch':
        return 'badge-branch';
      case 'Sub':
        return 'badge-sub';
      default:
        return '';
    }
  }

  /**
   * Check if warehouse can receive stock
   */
  canReceiveStock(warehouse: Warehouse): boolean {
    return warehouse.is_main_warehouse === true;
  }
}
