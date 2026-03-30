import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { Supplier, CreateSupplierDto } from '../../domain/entities/supplier.entity';
import { GetSupplierByIdUseCase, CreateSupplierUseCase, UpdateSupplierUseCase } from '../../application/usecases/supplier.usecase';

@Component({
  selector: 'app-supplier-detail',
  standalone: true,
  imports: [CommonModule, RouterModule, ReactiveFormsModule, MatButtonModule, MatIconModule, MatFormFieldModule, MatInputModule, MatSlideToggleModule],
  templateUrl: './supplier-detail.component.html',
  styleUrl: './supplier-detail.component.scss'
})
export class SupplierDetailComponent implements OnInit {
  private fb = inject(FormBuilder);
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private getSupplierByIdUseCase = inject(GetSupplierByIdUseCase);
  private createSupplierUseCase = inject(CreateSupplierUseCase);
  private updateSupplierUseCase = inject(UpdateSupplierUseCase);

  supplierForm!: FormGroup;
  isEditMode = false;
  isNewMode = false;
  supplierId: string | null = null;
  loading = false;
  saving = false;

  ngOnInit(): void {
    this.supplierForm = this.fb.group({
      code: ['', [Validators.required, Validators.minLength(2), Validators.maxLength(20)]],
      name: ['', [Validators.required, Validators.minLength(2), Validators.maxLength(100)]],
      contactPerson: [''],
      email: ['', [Validators.email]],
      phone: [''],
      address: [''],
      city: [''],
      country: [''],
      taxNumber: [''],
      paymentTermDays: [null],
      active: [true]
    });
    this.checkMode();
  }

  private checkMode(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id === 'new') { this.isNewMode = true; }
    else if (id) { this.isEditMode = true; this.supplierId = id; this.loadSupplier(id); }
  }

  private loadSupplier(id: string): void {
    this.loading = true;
    this.getSupplierByIdUseCase.execute(id).subscribe({
      next: (s) => { if (s) this.supplierForm.patchValue(s); this.loading = false; },
      error: () => { this.loading = false; }
    });
  }

  onSave(): void {
    if (this.supplierForm.invalid) { this.supplierForm.markAllAsTouched(); return; }
    this.saving = true;
    const data = this.supplierForm.value as CreateSupplierDto;
    const operation = this.isEditMode && this.supplierId ? this.updateSupplierUseCase.execute(this.supplierId, data) : this.createSupplierUseCase.execute(data);
    operation.subscribe({ next: () => { this.saving = false; this.router.navigate(['/purchases/suppliers']); }, error: (e) => { console.error(e); this.saving = false; } });
  }

  onCancel(): void { this.router.navigate(['/purchases/suppliers']); }
  onReset(): void { this.supplierForm.reset({ active: true }); }
  hasError(field: string, error: string): boolean { const c = this.supplierForm.get(field); return !!(c && c.hasError(error) && c.touched); }
}
