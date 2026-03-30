import { Component, OnInit, AfterViewInit, OnDestroy, ViewChild, ElementRef, inject } from '@angular/core';
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
import { Category, CreateCategoryDto } from '../../domain/entities/category.entity';
import { GetCategoryByIdUseCase, CreateCategoryUseCase, UpdateCategoryUseCase, GetAllCategoriesUseCase } from '../../application/usecases/category.usecase';

@Component({
  selector: 'app-category-detail',
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
  templateUrl: './category-detail.component.html',
  styleUrl: './category-detail.component.scss'
})
export class CategoryDetailComponent implements OnInit, AfterViewInit, OnDestroy {
  private fb = inject(FormBuilder);
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private getCategoryByIdUseCase = inject(GetCategoryByIdUseCase);
  private createCategoryUseCase = inject(CreateCategoryUseCase);
  private updateCategoryUseCase = inject(UpdateCategoryUseCase);
  private getAllCategoriesUseCase = inject(GetAllCategoriesUseCase);

  @ViewChild('formActionsRef') formActionsRef!: ElementRef;

  categoryForm!: FormGroup;
  isEditMode = false;
  isNewMode = false;
  categoryId: string | null = null;
  loading = false;
  saving = false;
  showTipsColumn = true;
  showStickyButtons = false;
  
  parentCategories: Category[] = [];
  selectedParentCategory: string | null = null;

  private observer: IntersectionObserver | null = null;
  private scrollHandler: (() => void) | null = null;

  ngOnInit(): void {
    this.initializeForm();
    this.loadParentCategories();
    this.checkMode();
  }

  private initializeForm(): void {
    this.categoryForm = this.fb.group({
      code: ['', [Validators.required, Validators.minLength(2), Validators.maxLength(20)]],
      name: ['', [Validators.required, Validators.minLength(2), Validators.maxLength(100)]],
      description: ['', [Validators.maxLength(255)]],
      parentCategoryId: [''],
      active: [true]
    });
  }

  private loadParentCategories(): void {
    this.getAllCategoriesUseCase.execute().subscribe({
      next: (result) => {
        this.parentCategories = result.categories.filter(c => c.id !== this.categoryId);
      },
      error: (error) => {
        console.error('Error loading parent categories:', error);
      }
    });
  }

  private checkMode(): void {
    const id = this.route.snapshot.paramMap.get('id');
    
    if (id === 'new') {
      this.isNewMode = true;
    } else if (id) {
      this.isEditMode = true;
      this.categoryId = id;
      this.loadCategory(this.categoryId);
    }
  }

  private loadCategory(id: string): void {
    this.loading = true;
    this.getCategoryByIdUseCase.execute(id).subscribe({
      next: (category) => {
        if (category) {
          this.categoryForm.patchValue({
            code: category.code,
            name: category.name,
            description: category.description,
            parentCategoryId: category.parentCategoryId || '',
            active: category.active
          });
          this.selectedParentCategory = category.parentCategoryId || null;
        }
        this.loading = false;
      },
      error: (error) => {
        console.error('Error loading category:', error);
        this.loading = false;
      }
    });
  }

  onSave(): void {
    if (this.categoryForm.invalid) {
      this.categoryForm.markAllAsTouched();
      return;
    }

    const formValue = this.categoryForm.value;
    const categoryData: CreateCategoryDto = {
      code: formValue.code,
      name: formValue.name,
      description: formValue.description,
      parentCategoryId: formValue.parentCategoryId || null,
      active: formValue.active
    };

    this.saving = true;

    if (this.isEditMode && this.categoryId) {
      this.updateCategoryUseCase.execute(this.categoryId, categoryData).subscribe({
        next: () => this.handleSaveSuccess(),
        error: (error) => this.handleSaveError(error)
      });
    } else {
      this.createCategoryUseCase.execute(categoryData).subscribe({
        next: () => this.handleSaveSuccess(),
        error: (error) => this.handleSaveError(error)
      });
    }
  }

  private handleSaveSuccess(): void {
    this.saving = false;
    this.router.navigate(['/inventory/categories']);
  }

  private handleSaveError(error: unknown): void {
    console.error('Error saving category:', error);
    this.saving = false;
  }

  onCancel(): void {
    this.router.navigate(['/inventory/categories']);
  }

  onReset(): void {
    this.categoryForm.reset({ active: true });
    this.selectedParentCategory = null;
  }

  hasError(fieldName: string, errorType: string): boolean {
    const field = this.categoryForm.get(fieldName);
    return !!(field && field.hasError(errorType) && field.touched);
  }

  getErrorMessage(fieldName: string): string {
    const field = this.categoryForm.get(fieldName);
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
      code: 'Category Code',
      name: 'Category Name',
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

    const scrollParent = this.findScrollParent(el);

    this.scrollHandler = () => {
      const rect = el.getBoundingClientRect();
      this.showStickyButtons = rect.top >= window.innerHeight;
    };

    document.addEventListener('scroll', this.scrollHandler, true);

    this.scrollHandler();
  }

  private findScrollParent(el: HTMLElement): HTMLElement | Window {
    let parent = el.parentElement;
    while (parent) {
      const style = window.getComputedStyle(parent);
      if (style.overflow === 'auto' || style.overflow === 'scroll' ||
          style.overflowY === 'auto' || style.overflowY === 'scroll') {
        return parent;
      }
      parent = parent.parentElement;
    }
    return window;
  }
}
