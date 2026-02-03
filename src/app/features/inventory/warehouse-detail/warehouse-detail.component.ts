import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Warehouse } from '../models/warehouse.model';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-warehouse-detail',
  standalone: false,
  templateUrl: './warehouse-detail.component.html',
  styleUrl: './warehouse-detail.component.scss'
})
export class WarehouseDetailComponent implements OnInit {
  warehouseForm!: FormGroup;
  isEditMode = false;
  warehouseId: number | null = null;

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.initializeForm();
    this.checkEditMode();
  }

  /**
   * Initialize the warehouse form with required fields
   * Based on ERP-Operation-Manual section 1.4: Warehouse Form
   */
  private initializeForm(): void {
    this.warehouseForm = this.fb.group({
      warehouse_name: ['', [Validators.required, Validators.minLength(2)]],
      location: ['', [Validators.required, Validators.minLength(2)]]
    });
  }

  /**
   * Check if we're in edit mode (editing existing warehouse)
   */
  private checkEditMode(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id && id !== 'new') {
      this.isEditMode = true;
      this.warehouseId = parseInt(id, 10);
      this.loadWarehouse(this.warehouseId);
    }
  }

  /**
   * Load existing warehouse data for editing
   */
  private loadWarehouse(id: number): void {
    // TODO: Implement service call to load warehouse
    // For now, this is a placeholder
    console.log('Loading warehouse:', id);
  }

  /**
   * Save warehouse (Create or Update)
   * Based on ERP-Operation-Manual:
   * - User clicks Save
   * - System validates and saves to warehouses table
   */
  onSave(): void {
    if (this.warehouseForm.invalid) {
      this.warehouseForm.markAllAsTouched();
      return;
    }

    const warehouseData: Partial<Warehouse> = {
      warehouse_name: this.warehouseForm.value.warehouse_name,
      location: this.warehouseForm.value.location
    };

    if (this.isEditMode && this.warehouseId) {
      this.updateWarehouse(this.warehouseId, warehouseData);
    } else {
      this.createWarehouse(warehouseData);
    }
  }

  /**
   * Create new warehouse
   */
  private createWarehouse(data: Partial<Warehouse>): void {
    // TODO: Implement service call to create warehouse
    console.log('Creating warehouse:', data);
    // After successful creation, navigate back to list
    // this.router.navigate(['/inventory/warehouses']);
  }

  /**
   * Update existing warehouse
   */
  private updateWarehouse(id: number, data: Partial<Warehouse>): void {
    // TODO: Implement service call to update warehouse
    console.log('Updating warehouse:', id, data);
    // After successful update, navigate back to list
    // this.router.navigate(['/inventory/warehouses']);
  }

  /**
   * Cancel and go back to warehouse list
   */
  onCancel(): void {
    this.router.navigate(['/inventory/warehouses']);
  }

  /**
   * Reset form to initial state
   */
  onReset(): void {
    this.warehouseForm.reset();
  }

  /**
   * Check if a field has error and is touched
   */
  hasError(fieldName: string, errorType: string): boolean {
    const field = this.warehouseForm.get(fieldName);
    return !!(field && field.hasError(errorType) && field.touched);
  }

  /**
   * Get error message for a field
   */
  getErrorMessage(fieldName: string): string {
    const field = this.warehouseForm.get(fieldName);
    if (field?.hasError('required')) {
      return `${this.getFieldLabel(fieldName)} is required`;
    }
    if (field?.hasError('minlength')) {
      const minLength = field.getError('minlength').requiredLength;
      return `${this.getFieldLabel(fieldName)} must be at least ${minLength} characters`;
    }
    return '';
  }

  /**
   * Get user-friendly field label
   */
  private getFieldLabel(fieldName: string): string {
    const labels: Record<string, string> = {
      warehouse_name: 'Warehouse Name',
      location: 'Location'
    };
    return labels[fieldName] || fieldName;
  }
}
