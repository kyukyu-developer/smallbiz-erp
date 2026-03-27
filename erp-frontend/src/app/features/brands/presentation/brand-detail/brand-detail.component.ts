import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators, FormsModule } from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatCardModule } from '@angular/material/card';
import { MatDividerModule } from '@angular/material/divider';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { Brand, CreateBrandDto } from '../../domain/entities/brand.entity';
import { GetBrandByIdUseCase, CreateBrandUseCase, UpdateBrandUseCase } from '../../application/usecases/brand.usecase';

@Component({
  selector: 'app-brand-detail',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    ReactiveFormsModule,
    FormsModule,
    MatButtonModule,
    MatIconModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatCheckboxModule,
    MatCardModule,
    MatDividerModule,
    MatTooltipModule,
    MatSlideToggleModule
  ],
  templateUrl: './brand-detail.component.html',
  styleUrl: './brand-detail.component.scss'
})
export class BrandDetailComponent implements OnInit {
  private fb = inject(FormBuilder);
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private getBrandByIdUseCase = inject(GetBrandByIdUseCase);
  private createBrandUseCase = inject(CreateBrandUseCase);
  private updateBrandUseCase = inject(UpdateBrandUseCase);

  brandForm!: FormGroup;
  isEditMode = false;
  isNewMode = false;
  brandId: string | null = null;
  loading = false;
  saving = false;
  showTipsColumn = true;
  showStickyButtons = false;

  ngOnInit(): void {
    this.initializeForm();
    this.checkMode();
    this.setupIntersectionObserver();
  }

  private initializeForm(): void {
    this.brandForm = this.fb.group({
      name: ['', [Validators.required, Validators.minLength(2), Validators.maxLength(50)]],
      description: ['', [Validators.maxLength(255)]],
      active: [true]
    });
  }

  private checkMode(): void {
    const id = this.route.snapshot.paramMap.get('id');
    
    if (id === 'new') {
      this.isNewMode = true;
    } else if (id) {
      this.isEditMode = true;
      this.brandId = id;
      this.loadBrand(this.brandId);
    }
  }

  private loadBrand(id: string): void {
    this.loading = true;
    this.getBrandByIdUseCase.execute(id).subscribe({
      next: (brand) => {
        if (brand) {
          this.brandForm.patchValue({
            name: brand.name,
            description: brand.description,
            active: brand.active
          });
        }
        this.loading = false;
      },
      error: (error) => {
        console.error('Error loading brand:', error);
        this.loading = false;
      }
    });
  }

  onSave(): void {
    if (this.brandForm.invalid) {
      this.brandForm.markAllAsTouched();
      return;
    }

    const formValue = this.brandForm.value;
    const brandData: CreateBrandDto = {
      name: formValue.name,
      description: formValue.description,
      active: formValue.active
    };

    this.saving = true;

    if (this.isEditMode && this.brandId) {
      this.updateBrandUseCase.execute(this.brandId, brandData).subscribe({
        next: () => this.handleSaveSuccess(),
        error: (error) => this.handleSaveError(error)
      });
    } else {
      this.createBrandUseCase.execute(brandData).subscribe({
        next: () => this.handleSaveSuccess(),
        error: (error) => this.handleSaveError(error)
      });
    }
  }

  private handleSaveSuccess(): void {
    this.saving = false;
    this.router.navigate(['/inventory/brands']);
  }

  private handleSaveError(error: unknown): void {
    console.error('Error saving brand:', error);
    this.saving = false;
  }

  onCancel(): void {
    this.router.navigate(['/inventory/brands']);
  }

  onReset(): void {
    this.brandForm.reset({ active: true });
  }

  hasError(fieldName: string, errorType: string): boolean {
    const field = this.brandForm.get(fieldName);
    return !!(field && field.hasError(errorType) && field.touched);
  }

  getErrorMessage(fieldName: string): string {
    const field = this.brandForm.get(fieldName);
    if (field?.hasError('required')) {
      return `${this.getFieldLabel(fieldName)} is required`;
    }
    if (field?.hasError('minlength')) {
      const minLength = field.getError('minlength').requiredLength;
      return `${this.getFieldLabel(fieldName)} must be at least ${minLength} characters`;
    }
    return '';
  }

  private getFieldLabel(fieldName: string): string {
    const labels: Record<string, string> = {
      name: 'Brand Name',
      description: 'Description'
    };
    return labels[fieldName] || fieldName;
  }

  toggleTipsColumn(): void {
    this.showTipsColumn = !this.showTipsColumn;
  }

  private setupIntersectionObserver(): void {
    setTimeout(() => {
      const buttonSection = document.querySelector('.form-actions');
      if (buttonSection) {
        const observer = new IntersectionObserver(
          (entries) => {
            entries.forEach((entry) => {
              this.showStickyButtons = entry.intersectionRatio < 0.8;
            });
          },
          { threshold: [0, 0.25, 0.5, 0.75, 0.8, 1], rootMargin: '0px 0px -80px 0px' }
        );
        observer.observe(buttonSection);
      }
    }, 100);
  }
}