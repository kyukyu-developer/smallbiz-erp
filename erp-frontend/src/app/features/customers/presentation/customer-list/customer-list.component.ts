import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatMenuModule } from '@angular/material/menu';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { Customer } from '../../domain/entities/customer.entity';
import { GetAllCustomersUseCase, DeleteCustomerUseCase, GetAllCustomersResult } from '../../application/usecases/customer.usecase';

@Component({
  selector: 'app-customer-list',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    FormsModule,
    MatButtonModule,
    MatIconModule,
    MatMenuModule,
    MatProgressSpinnerModule
  ],
  templateUrl: './customer-list.component.html',
  styleUrl: './customer-list.component.scss'
})
export class CustomerListComponent implements OnInit {
  private getAllCustomersUseCase = inject(GetAllCustomersUseCase);
  private deleteCustomerUseCase = inject(DeleteCustomerUseCase);
  private router = inject(Router);

  customers: Customer[] = [];
  filteredCustomers: Customer[] = [];
  searchTerm: string = '';
  loading: boolean = false;
  totalCustomers: number = 0;
  activeCustomers: number = 0;
  inactiveCustomers: number = 0;

  ngOnInit() {
    this.loadCustomers();
  }

  loadCustomers() {
    this.loading = true;
    this.getAllCustomersUseCase.execute().subscribe({
      next: (result: GetAllCustomersResult) => {
        this.customers = result.customers;
        this.totalCustomers = result.total;
        this.activeCustomers = result.customers.filter((c: Customer) => c.active).length;
        this.inactiveCustomers = this.totalCustomers - this.activeCustomers;
        this.filteredCustomers = [...this.customers];
        this.loading = false;
      },
      error: (error: unknown) => {
        console.error('Error loading customers:', error);
        this.loading = false;
      }
    });
  }

  applyFilter() {
    if (!this.searchTerm) {
      this.filteredCustomers = [...this.customers];
      return;
    }
    const term = this.searchTerm.toLowerCase();
    this.filteredCustomers = this.customers.filter(c =>
      c.name?.toLowerCase().includes(term) ||
      c.code?.toLowerCase().includes(term) ||
      c.email?.toLowerCase().includes(term) ||
      c.phone?.includes(term)
    );
  }

  viewCustomer(customer: Customer) {
    this.router.navigate(['/sales/customers', customer.id]);
  }

  editCustomer(customer: Customer) {
    this.router.navigate(['/sales/customers', customer.id]);
  }

  deleteCustomer(customer: Customer) {
    if (confirm(`Are you sure you want to delete customer: ${customer.name}?`)) {
      this.deleteCustomerUseCase.execute(customer.id).subscribe({
        next: () => {
          this.loadCustomers();
        },
        error: (error: unknown) => {
          console.error('Error deleting customer:', error);
        }
      });
    }
  }

  addNewCustomer() {
    this.router.navigate(['/sales/customers/new']);
  }
}
