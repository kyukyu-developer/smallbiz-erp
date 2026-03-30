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
  imports: [CommonModule, RouterModule, FormsModule, MatButtonModule, MatIconModule, MatMenuModule, MatProgressSpinnerModule],
  templateUrl: './supplier-list.component.html',
  styleUrl: './supplier-list.component.scss'
})
export class SupplierListComponent implements OnInit {
  private getAllSuppliersUseCase = inject(GetAllSuppliersUseCase);
  private deleteSupplierUseCase = inject(DeleteSupplierUseCase);
  private router = inject(Router);

  suppliers: Supplier[] = [];
  filteredSuppliers: Supplier[] = [];
  searchTerm = '';
  loading = false;
  totalSuppliers = 0;
  activeSuppliers = 0;
  inactiveSuppliers = 0;

  ngOnInit() { this.loadSuppliers(); }

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
      error: () => { this.loading = false; }
    });
  }

  applyFilter() {
    if (!this.searchTerm) { this.filteredSuppliers = [...this.suppliers]; return; }
    const term = this.searchTerm.toLowerCase();
    this.filteredSuppliers = this.suppliers.filter(s => s.name?.toLowerCase().includes(term) || s.code?.toLowerCase().includes(term) || s.email?.toLowerCase().includes(term));
  }

  viewSupplier(s: Supplier) { this.router.navigate(['/purchases/suppliers', s.id]); }
  editSupplier(s: Supplier) { this.router.navigate(['/purchases/suppliers', s.id]); }
  deleteSupplier(s: Supplier) {
    if (confirm(`Delete supplier: ${s.name}?`)) {
      this.deleteSupplierUseCase.execute(s.id).subscribe({ next: () => this.loadSuppliers(), error: (err) => console.error(err) });
    }
  }

  addNewSupplier() { this.router.navigate(['/purchases/suppliers/new']); }
}
