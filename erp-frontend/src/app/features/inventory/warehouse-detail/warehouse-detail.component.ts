import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Warehouse } from '../models/warehouse.model';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-warehouse-detail',
  standalone: false,
  templateUrl: './warehouse-detail.component.html',
  styleUrls: ['./warehouse-detail.component.scss']
})
export class WarehouseDetailComponent implements OnInit {
  warehouseForm!: FormGroup;
  isEditMode = false;
  warehouseId: string | null = null;

  // Dropdown options
  branchTypes = ['Main', 'Branch', 'Sub'];
  availableParentWarehouses: Warehouse[] = [];

  // Show/hide parent warehouse based on branch type
  showParentWarehouse = false;

  // Show/hide tips column (right sidebar)
  showTipsColumn = true;

  // Show/hide sticky button section
  showStickyButtons = false;

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.initializeForm();
    this.loadParentWarehouses();
    this.checkEditMode();
    this.setupFormListeners();
    this.setupIntersectionObserver();
  }

  /**
   * Initialize the warehouse form with required fields
   * Based on INVENTORY_DATABASE_NOTES.txt
   *
   * Business Rules:
   * - Only Main Warehouses can receive stock (is_main_warehouse = TRUE)
   * - Hierarchical structure: Main → Branch → Sub
   * - Branch/Sub must have parent_warehouse_id
   * - UNIQUE(name, city) validation
   */
  private initializeForm(): void {
    this.warehouseForm = this.fb.group({
      name: ['', [Validators.required, Validators.minLength(2), Validators.maxLength(50)]],
      city: ['', [Validators.required, Validators.minLength(2), Validators.maxLength(50)]],
      branch_type: ['Main', [Validators.required]],
      is_main_warehouse: [true],
      parent_warehouse_id: [null],
      is_used_warehouse: [true],
      active: [true],

      // Optional fields
      description: ['', [Validators.maxLength(255)]],
      address: ['', [Validators.maxLength(255)]],
      phone: ['', [Validators.maxLength(20)]],
      email: ['', [Validators.email, Validators.maxLength(100)]]
    });
  }

  /**
   * Setup form value change listeners
   */
  private setupFormListeners(): void {
    // Listen to branch_type changes
    this.warehouseForm.get('branch_type')?.valueChanges.subscribe(branchType => {
      this.onBranchTypeChange(branchType);
    });
  }

  /**
   * Handle branch type changes
   * - Main: is_main_warehouse = TRUE, parent_warehouse_id = NULL
   * - Branch/Sub: is_main_warehouse = FALSE, parent_warehouse_id = REQUIRED
   */
  onBranchTypeChange(branchType: string): void {
    const parentControl = this.warehouseForm.get('parent_warehouse_id');
    const isMainControl = this.warehouseForm.get('is_main_warehouse');

    if (branchType === 'Main') {
      // Main warehouse
      this.showParentWarehouse = false;
      isMainControl?.setValue(true);
      parentControl?.setValue(null);
      parentControl?.clearValidators();
    } else {
      // Branch or Sub warehouse
      this.showParentWarehouse = true;
      isMainControl?.setValue(false);
      parentControl?.setValidators([Validators.required]);
    }

    parentControl?.updateValueAndValidity();
  }

  /**
   * Load available parent warehouses for Branch/Sub selection
   */
  private loadParentWarehouses(): void {
    // TODO: Implement service call to load warehouses
    // For now, using sample data
    this.availableParentWarehouses = [
      {
        id: '1',
        name: 'Main Warehouse - Central',
        city: 'New York',
        branch_type: 'Main',
        is_main_warehouse: true,
        is_used_warehouse: true,
        active: true
      },
      {
        id: '2',
        name: 'Main Warehouse - East Coast',
        city: 'Boston',
        branch_type: 'Main',
        is_main_warehouse: true,
        is_used_warehouse: true,
        active: true
      }
    ];
  }

  /**
   * Check if we're in edit mode (editing existing warehouse)
   */
  private checkEditMode(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id && id !== 'new') {
      this.isEditMode = true;
      this.warehouseId = id;
      this.loadWarehouse(this.warehouseId);
    }
  }

  /**
   * Load existing warehouse data for editing
   */
  private loadWarehouse(id: string): void {
    // TODO: Implement service call to load warehouse
    // For now, this is a placeholder
    console.log('Loading warehouse:', id);
  }

  /**
   * Save warehouse (Create or Update)
   * Validates business rules:
   * - Branch/Sub must have parent_warehouse_id
   * - Main warehouse: is_main_warehouse = TRUE, parent_warehouse_id = NULL
   */
  onSave(): void {
    if (this.warehouseForm.invalid) {
      this.warehouseForm.markAllAsTouched();
      return;
    }

    const formValue = this.warehouseForm.value;
    const warehouseData: Partial<Warehouse> = {
      name: formValue.name,
      city: formValue.city,
      branch_type: formValue.branch_type,
      is_main_warehouse: formValue.is_main_warehouse,
      parent_warehouse_id: formValue.parent_warehouse_id,
      is_used_warehouse: formValue.is_used_warehouse,
      active: formValue.active,
      description: formValue.description,
      address: formValue.address,
      phone: formValue.phone,
      email: formValue.email
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
  private updateWarehouse(id: string, data: Partial<Warehouse>): void {
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
      name: 'Warehouse Name',
      city: 'City',
      branch_type: 'Branch Type',
      parent_warehouse_id: 'Parent Warehouse',
      description: 'Description',
      address: 'Address',
      phone: 'Phone',
      email: 'Email'
    };
    return labels[fieldName] || fieldName;
  }

  /**
   * Toggle tips column visibility
   */
  toggleTipsColumn(): void {
    this.showTipsColumn = !this.showTipsColumn;
  }

  /**
   * Setup Intersection Observer to detect when original button section is visible
   */
  private setupIntersectionObserver(): void {
    // Wait for the DOM to be ready
    setTimeout(() => {
      const buttonSection = document.querySelector('.form-actions');
      if (buttonSection) {
        const observer = new IntersectionObserver(
          (entries) => {
            entries.forEach((entry) => {
              // Show sticky buttons when original buttons are NOT fully visible
              // Use a higher threshold to ensure original buttons are mostly visible before hiding sticky ones
              this.showStickyButtons = entry.intersectionRatio < 0.8;
            });
          },
          {
            threshold: [0, 0.25, 0.5, 0.75, 0.8, 1], // Multiple thresholds for more precise detection
            rootMargin: '0px 0px -80px 0px' // Add margin at bottom to trigger earlier
          }
        );

        observer.observe(buttonSection);
      }
    }, 100);
  }
}
