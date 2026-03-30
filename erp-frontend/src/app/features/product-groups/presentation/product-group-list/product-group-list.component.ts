import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatMenuModule } from '@angular/material/menu';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { ProductGroup } from '../../domain/entities/product-group.entity';
import { GetAllProductGroupsUseCase, DeleteProductGroupUseCase, GetAllProductGroupsResult } from '../../application/usecases/product-group.usecase';

@Component({
  selector: 'app-product-group-list',
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
  templateUrl: './product-group-list.component.html',
  styleUrl: './product-group-list.component.scss'
})
export class ProductGroupListComponent implements OnInit {
  private getAllProductGroupsUseCase = inject(GetAllProductGroupsUseCase);
  private deleteProductGroupUseCase = inject(DeleteProductGroupUseCase);
  private router = inject(Router);

  productGroups: ProductGroup[] = [];
  filteredProductGroups: ProductGroup[] = [];
  searchTerm: string = '';
  loading: boolean = false;
  totalProductGroups: number = 0;
  activeProductGroups: number = 0;
  inactiveProductGroups: number = 0;

  ngOnInit() {
    this.loadProductGroups();
  }

  loadProductGroups() {
    this.loading = true;
    this.getAllProductGroupsUseCase.execute().subscribe({
      next: (result: GetAllProductGroupsResult) => {
        this.productGroups = result.productGroups;
        this.totalProductGroups = result.total;
        this.activeProductGroups = result.productGroups.filter((pg: ProductGroup) => pg.active).length;
        this.inactiveProductGroups = this.totalProductGroups - this.activeProductGroups;
        this.filteredProductGroups = [...this.productGroups];
        this.loading = false;
      },
      error: (error: unknown) => {
        console.error('Error loading product groups:', error);
        this.loading = false;
      }
    });
  }

  applyFilter() {
    if (!this.searchTerm) {
      this.filteredProductGroups = [...this.productGroups];
      return;
    }
    const term = this.searchTerm.toLowerCase();
    this.filteredProductGroups = this.productGroups.filter(pg =>
      pg.name?.toLowerCase().includes(term) ||
      pg.description?.toLowerCase().includes(term)
    );
  }

  viewProductGroup(productGroup: ProductGroup) {
    this.router.navigate(['/inventory/product-groups', productGroup.id]);
  }

  editProductGroup(productGroup: ProductGroup) {
    this.router.navigate(['/inventory/product-groups', productGroup.id]);
  }

  deleteProductGroup(productGroup: ProductGroup) {
    if (confirm(`Are you sure you want to delete product group: ${productGroup.name}?`)) {
      this.deleteProductGroupUseCase.execute(productGroup.id).subscribe({
        next: () => {
          this.loadProductGroups();
        },
        error: (error: unknown) => {
          console.error('Error deleting product group:', error);
        }
      });
    }
  }

  addNewProductGroup() {
    this.router.navigate(['/inventory/product-groups/new']);
  }
}
