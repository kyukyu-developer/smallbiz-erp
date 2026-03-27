import { Component, Inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { MatCardModule } from '@angular/material/card';
import { MatDividerModule } from '@angular/material/divider';

export interface WarehouseFilter {
  branchTypes: string[];
  cities: string[];
  isUsed?: boolean | null;
  isActive?: boolean | null;
}

export interface FilterDialogData {
  filters: WarehouseFilter;
  availableBranchTypes: string[];
  availableCities: string[];
}

@Component({
  selector: 'app-warehouse-filter-dialog',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatDialogModule,
    MatButtonModule,
    MatIconModule,
    MatCheckboxModule,
    MatFormFieldModule,
    MatSelectModule,
    MatCardModule,
    MatDividerModule
  ],
  templateUrl: './warehouse-filter-dialog.component.html',
  styleUrl: './warehouse-filter-dialog.component.scss'
})
export class WarehouseFilterDialogComponent implements OnInit {
  filters: WarehouseFilter;
  availableBranchTypes: string[];
  availableCities: string[];
  activeFilterCount: number = 0;

  constructor(
    public dialogRef: MatDialogRef<WarehouseFilterDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: FilterDialogData
  ) {
    // Create a deep copy of filters to avoid mutating the original
    this.filters = {
      branchTypes: [...data.filters.branchTypes],
      cities: [...data.filters.cities],
      isUsed: data.filters.isUsed,
      isActive: data.filters.isActive
    };
    this.availableBranchTypes = data.availableBranchTypes;
    this.availableCities = data.availableCities;
  }

  ngOnInit(): void {
    this.updateActiveFilterCount();
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
    this.updateActiveFilterCount();
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
    this.updateActiveFilterCount();
  }

  /**
   * Set status filter
   */
  setStatusFilter(status: 'used' | 'active'): void {
    if (status === 'used') {
      this.filters.isUsed = this.filters.isUsed === true ? null : true;
    } else if (status === 'active') {
      this.filters.isActive = this.filters.isActive === true ? null : true;
    }
    this.updateActiveFilterCount();
  }

  /**
   * Clear all filters
   */
  clearAllFilters(): void {
    this.filters = {
      branchTypes: [],
      cities: [],
      isUsed: null,
      isActive: null
    };
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
   * Close dialog without applying filters
   */
  onClose(): void {
    this.dialogRef.close();
  }

  /**
   * Apply filters and close dialog
   */
  onApply(): void {
    this.dialogRef.close(this.filters);
  }
}
