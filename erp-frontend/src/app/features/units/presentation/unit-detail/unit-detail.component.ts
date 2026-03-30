import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { Unit, CreateUnitDto } from '../../domain/entities/unit.entity';
import { GetUnitByIdUseCase, CreateUnitUseCase, UpdateUnitUseCase } from '../../application/usecases/unit.usecase';

@Component({
  selector: 'app-unit-detail',
  standalone: true,
  imports: [CommonModule, RouterModule, ReactiveFormsModule, MatButtonModule, MatIconModule, MatFormFieldModule, MatInputModule, MatSlideToggleModule],
  templateUrl: './unit-detail.component.html',
  styleUrl: './unit-detail.component.scss'
})
export class UnitDetailComponent implements OnInit {
  private fb = inject(FormBuilder);
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private getUnitByIdUseCase = inject(GetUnitByIdUseCase);
  private createUnitUseCase = inject(CreateUnitUseCase);
  private updateUnitUseCase = inject(UpdateUnitUseCase);

  unitForm!: FormGroup;
  isEditMode = false;
  isNewMode = false;
  unitId: string | null = null;
  loading = false;
  saving = false;

  ngOnInit(): void {
    this.initializeForm();
    this.checkMode();
  }

  private initializeForm(): void {
    this.unitForm = this.fb.group({
      name: ['', [Validators.required, Validators.minLength(2), Validators.maxLength(50)]],
      symbol: ['', [Validators.required, Validators.maxLength(10)]],
      active: [true]
    });
  }

  private checkMode(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id === 'new') {
      this.isNewMode = true;
    } else if (id) {
      this.isEditMode = true;
      this.unitId = id;
      this.loadUnit(this.unitId);
    }
  }

  private loadUnit(id: string): void {
    this.loading = true;
    this.getUnitByIdUseCase.execute(id).subscribe({
      next: (unit) => {
        if (unit) {
          this.unitForm.patchValue({ name: unit.name, symbol: unit.symbol, active: unit.active });
        }
        this.loading = false;
      },
      error: () => { this.loading = false; }
    });
  }

  onSave(): void {
    if (this.unitForm.invalid) {
      this.unitForm.markAllAsTouched();
      return;
    }

    const formValue = this.unitForm.value;
    const unitData: CreateUnitDto = { name: formValue.name, symbol: formValue.symbol, active: formValue.active };

    this.saving = true;
    const operation = this.isEditMode && this.unitId
      ? this.updateUnitUseCase.execute(this.unitId, unitData)
      : this.createUnitUseCase.execute(unitData);

    operation.subscribe({
      next: () => { this.saving = false; this.router.navigate(['/inventory/units']); },
      error: (err) => { console.error('Error saving unit:', err); this.saving = false; }
    });
  }

  onCancel(): void { this.router.navigate(['/inventory/units']); }
  onReset(): void { this.unitForm.reset({ active: true }); }

  hasError(fieldName: string, errorType: string): boolean {
    const field = this.unitForm.get(fieldName);
    return !!(field && field.hasError(errorType) && field.touched);
  }

  toggleTipsColumn(): void {}
}
