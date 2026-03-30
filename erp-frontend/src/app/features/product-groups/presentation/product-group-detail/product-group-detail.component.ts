import { Component, OnInit, AfterViewInit, OnDestroy, ViewChild, ElementRef, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators, FormsModule } from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { MatTooltipModule } from '@angular/material/tooltip';
import { ProductGroup, CreateProductGroupDto } from '../../domain/entities/product-group.entity';
import { GetProductGroupByIdUseCase, CreateProductGroupUseCase, UpdateProductGroupUseCase } from '../../application/usecases/product-group.usecase';

@Component({
  selector: 'app-product-group-detail',
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
    MatSlideToggleModule,
    MatTooltipModule
  ],
  templateUrl: './product-group-detail.component.html',
  styleUrl: './product-group-detail.component.scss'
})
export class ProductGroupDetailComponent implements OnInit, AfterViewInit, OnDestroy {
  private fb = inject(FormBuilder);
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private getProductGroupByIdUseCase = inject(GetProductGroupByIdUseCase);
  private createProductGroupUseCase = inject(CreateProductGroupUseCase);
  private updateProductGroupUseCase = inject(UpdateProductGroupUseCase);

  @ViewChild('formActionsRef') formActionsRef!: ElementRef;

  productGroupForm!: FormGroup;
  isEditMode = false;
  isNewMode = false;
  productGroupId: string | null = null;
  loading = false;
  saving = false;
  showTipsColumn = true;
  showStickyButtons = false;

  private observer: IntersectionObserver | null = null;
  private scrollHandler: (() => void) | null = null;

  ngOnInit(): void {
    this.initializeForm();
    this.checkMode();
  }

  private initializeForm(): void {
    this.productGroupForm = this.fb.group({
      name: ['', [Validators.required, Validators.minLength(2), Validators.maxLength(100)]],
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
      this.productGroupId = id;
      this.loadProductGroup(this.productGroupId);
    }
  }

  private loadProductGroup(id: string): void {
    this.loading = true;
    this.getProductGroupByIdUseCase.execute(id).subscribe({
      next: (productGroup) => {
        if (productGroup) {
          this.productGroupForm.patchValue({
            name: productGroup.name,
            description: productGroup.description,
            active: productGroup.active
          });
        }
        this.loading = false;
      },
      error: (error) => {
        console.error('Error loading product group:', error);
        this.loading = false;
      }
    });
  }

  onSave(): void {
    if (this.productGroupForm.invalid) {
      this.productGroupForm.markAllAsTouched();
      return;
    }

    const formValue = this.productGroupForm.value;
    const productGroupData: CreateProductGroupDto = {
      name: formValue.name,
      description: formValue.description,
      active: formValue.active
    };

    this.saving = true;

    if (this.isEditMode && this.productGroupId) {
      this.updateProductGroupUseCase.execute(this.productGroupId, productGroupData).subscribe({
        next: () => this.handleSaveSuccess(),
        error: (error) => this.handleSaveError(error)
      });
    } else {
      this.createProductGroupUseCase.execute(productGroupData).subscribe({
        next: () => this.handleSaveSuccess(),
        error: (error) => this.handleSaveError(error)
      });
    }
  }

  private handleSaveSuccess(): void {
    this.saving = false;
    this.router.navigate(['/inventory/product-groups']);
  }

  private handleSaveError(error: unknown): void {
    console.error('Error saving product group:', error);
    this.saving = false;
  }

  onCancel(): void {
    this.router.navigate(['/inventory/product-groups']);
  }

  onReset(): void {
    this.productGroupForm.reset({ active: true });
  }

  hasError(fieldName: string, errorType: string): boolean {
    const field = this.productGroupForm.get(fieldName);
    return !!(field && field.hasError(errorType) && field.touched);
  }

  getErrorMessage(fieldName: string): string {
    const field = this.productGroupForm.get(fieldName);
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
      name: 'Product Group Name',
      description: 'Description'
    };
    return labels[fieldName] || fieldName;
  }

  toggleTipsColumn(): void {
    this.showTipsColumn = !this.showTipsColumn;
  }

  ngAfterViewInit(): void {
    this.setupScrollDetection();
  }

  ngOnDestroy(): void {
    if (this.observer) {
      this.observer.disconnect();
    }
    if (this.scrollHandler) {
      document.removeEventListener('scroll', this.scrollHandler, true);
    }
  }

  private setupScrollDetection(): void {
    if (!this.formActionsRef) return;

    const el = this.formActionsRef.nativeElement as HTMLElement;

    this.scrollHandler = () => {
      const rect = el.getBoundingClientRect();
      this.showStickyButtons = rect.top >= window.innerHeight;
    };

    document.addEventListener('scroll', this.scrollHandler, true);

    this.scrollHandler();
  }
}
