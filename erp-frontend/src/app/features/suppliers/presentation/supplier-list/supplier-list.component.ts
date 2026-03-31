import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatMenuModule } from '@angular/material/menu';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { Supplier } from '../../domain/entities/supplier.entity';
import { GetAllSuppliersUseCase, DeleteSupplierUseCase } from '../../application/usecases/supplier.usecase';

@Component({
  selector: 'app-supplier-list',
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
  templateUrl: './supplier-list.component.html',
  styleUrl: './supplier-list.component.scss'
})
export class SupplierListComponent implements OnInit {
  private getAllSuppliersUseCase = inject(GetAllSuppliersUseCase);
  private deleteSupplierUseCase = inject(DeleteSupplierUseCase);
  private router = inject(Router);

  suppliers: Supplier[] = [];
  filteredSuppliers: Supplier[] = [];
  searchTerm: string = '';
  loading: boolean = false;
  totalSuppliers: number = 0;
  activeSuppliers: number = 0;
  inactiveSuppliers: number = 0;

  ngOnInit() {
    this.loadSuppliers();
  }

  loadSuppliers() {
    this.loading = true;
    this.getAllSuppliersUseCase.execute().subscribe({
      next: (result) => {
        this.suppliers = result.suppliers;
        this.totalSuppliers = result.total;
        this.activeSuppliers = result.suppliers.filter((s: Supplier) => s.active).length;
        this.inactiveSuppliers = this.totalSuppliers - this.activeSuppliers;
        this.filteredSuppliers = [...this.suppliers];
        this.loading = false;
      },
      error: (error: unknown) => {
        console.error('Error loading suppliers:', error);
        this.loading = false;
      }
    });
  }

  applyFilter() {
    if (!this.searchTerm) {
      this.filteredSuppliers = [...this.suppliers];
      return;
    }
    const term = this.searchTerm.toLowerCase();
    this.filteredSuppliers = this.suppliers.filter(s =>
      s.name?.toLowerCase().includes(term) ||
      s.code?.toLowerCase().includes(term) ||
      s.email?.toLowerCase().includes(term) ||
      s.phone?.includes(term)
    );
  }

  viewSupplier(supplier: Supplier) {
    this.router.navigate(['/purchases/suppliers', supplier.id]);
  }

  editSupplier(supplier: Supplier) {
    this.router.navigate(['/purchases/suppliers', supplier.id]);
  }

  deleteSupplier(supplier: Supplier) {
    if (confirm(`Are you sure you want to delete supplier: ${supplier.name}?`)) {
      this.deleteSupplierUseCase.execute(supplier.id).subscribe({
        next: () => {
          this.loadSuppliers();
        },
        error: (error: unknown) => {
          console.error('Error deleting supplier:', error);
        }
      });
    }
  }

  addNewSupplier() {
    this.router.navigate(['/purchases/suppliers/new']);
  }
}
