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
import { Customer, CreateCustomerDto } from '../../domain/entities/customer.entity';
import { GetCustomerByIdUseCase, CreateCustomerUseCase, UpdateCustomerUseCase } from '../../application/usecases/customer.usecase';

@Component({
  selector: 'app-customer-detail',
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
  templateUrl: './customer-detail.component.html',
  styleUrl: './customer-detail.component.scss'
})
export class CustomerDetailComponent implements OnInit, AfterViewInit, OnDestroy {
  private fb = inject(FormBuilder);
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private getCustomerByIdUseCase = inject(GetCustomerByIdUseCase);
  private createCustomerUseCase = inject(CreateCustomerUseCase);
  private updateCustomerUseCase = inject(UpdateCustomerUseCase);

  @ViewChild('formActionsRef') formActionsRef!: ElementRef;

  customerForm!: FormGroup;
  isEditMode = false;
  isNewMode = false;
  customerId: string | null = null;
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
    this.customerForm = this.fb.group({
      code: ['', [Validators.required, Validators.minLength(2), Validators.maxLength(20)]],
      name: ['', [Validators.required, Validators.minLength(2), Validators.maxLength(100)]],
      contactPerson: ['', [Validators.maxLength(100)]],
      email: ['', [Validators.email, Validators.maxLength(100)]],
      phone: ['', [Validators.maxLength(20)]],
      address: ['', [Validators.maxLength(255)]],
      city: ['', [Validators.maxLength(50)]],
      country: ['', [Validators.maxLength(50)]],
      taxNumber: ['', [Validators.maxLength(50)]],
      creditLimit: [null],
      active: [true]
    });
  }

  private checkMode(): void {
    const id = this.route.snapshot.paramMap.get('id');
    
    if (id === 'new') {
      this.isNewMode = true;
    } else if (id) {
      this.isEditMode = true;
      this.customerId = id;
      this.loadCustomer(this.customerId);
    }
  }

  private loadCustomer(id: string): void {
    this.loading = true;
    this.getCustomerByIdUseCase.execute(id).subscribe({
      next: (customer) => {
        if (customer) {
          this.customerForm.patchValue({
            code: customer.code,
            name: customer.name,
            contactPerson: customer.contactPerson,
            email: customer.email,
            phone: customer.phone,
            address: customer.address,
            city: customer.city,
            country: customer.country,
            taxNumber: customer.taxNumber,
            creditLimit: customer.creditLimit,
            active: customer.active
          });
        }
        this.loading = false;
      },
      error: (error) => {
        console.error('Error loading customer:', error);
        this.loading = false;
      }
    });
  }

  onSave(): void {
    if (this.customerForm.invalid) {
      this.customerForm.markAllAsTouched();
      return;
    }

    const formValue = this.customerForm.value;
    const customerData: CreateCustomerDto = {
      code: formValue.code,
      name: formValue.name,
      contactPerson: formValue.contactPerson,
      email: formValue.email,
      phone: formValue.phone,
      address: formValue.address,
      city: formValue.city,
      country: formValue.country,
      taxNumber: formValue.taxNumber,
      creditLimit: formValue.creditLimit,
      active: formValue.active
    };

    this.saving = true;

    if (this.isEditMode && this.customerId) {
      this.updateCustomerUseCase.execute(this.customerId, customerData).subscribe({
        next: () => this.handleSaveSuccess(),
        error: (error) => this.handleSaveError(error)
      });
    } else {
      this.createCustomerUseCase.execute(customerData).subscribe({
        next: () => this.handleSaveSuccess(),
        error: (error) => this.handleSaveError(error)
      });
    }
  }

  private handleSaveSuccess(): void {
    this.saving = false;
    this.router.navigate(['/sales/customers']);
  }

  private handleSaveError(error: unknown): void {
    console.error('Error saving customer:', error);
    this.saving = false;
  }

  onCancel(): void {
    this.router.navigate(['/sales/customers']);
  }

  onReset(): void {
    this.customerForm.reset({ active: true });
  }

  hasError(fieldName: string, errorType: string): boolean {
    const field = this.customerForm.get(fieldName);
    return !!(field && field.hasError(errorType) && field.touched);
  }

  getErrorMessage(fieldName: string): string {
    const field = this.customerForm.get(fieldName);
    if (field?.hasError('required')) {
      return `${this.getFieldLabel(fieldName)} is required`;
    }
    if (field?.hasError('minlength')) {
      const minLength = field.getError('minlength').requiredLength;
      return `${this.getFieldLabel(fieldName)} must be at least ${minLength} characters`;
    }
    if (field?.hasError('email')) {
      return 'Please enter a valid email address';
    }
    return '';
  }

  private getFieldLabel(fieldName: string): string {
    const labels: Record<string, string> = {
      code: 'Customer Code',
      name: 'Customer Name',
      contactPerson: 'Contact Person',
      email: 'Email',
      phone: 'Phone',
      address: 'Address',
      city: 'City',
      country: 'Country',
      taxNumber: 'Tax Number',
      creditLimit: 'Credit Limit'
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
