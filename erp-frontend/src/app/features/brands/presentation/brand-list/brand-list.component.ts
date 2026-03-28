import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatMenuModule } from '@angular/material/menu';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { Brand } from '../../domain/entities/brand.entity';
import { GetAllBrandsUseCase, DeleteBrandUseCase, GetAllBrandsResult } from '../../application/usecases/brand.usecase';

@Component({
  selector: 'app-brand-list',
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
  templateUrl: './brand-list.component.html',
  styleUrl: './brand-list.component.scss'
})
export class BrandListComponent implements OnInit {
  private getAllBrandsUseCase = inject(GetAllBrandsUseCase);
  private deleteBrandUseCase = inject(DeleteBrandUseCase);
  private router = inject(Router);

  brands: Brand[] = [];
  filteredBrands: Brand[] = [];
  searchTerm: string = '';
  loading: boolean = false;
  totalBrands: number = 0;
  activeBrands: number = 0;
  inactiveBrands: number = 0;

  ngOnInit() {
    this.loadBrands();
  }

  loadBrands() {
    this.loading = true;
    this.getAllBrandsUseCase.execute().subscribe({
      next: (result: GetAllBrandsResult) => {
        this.brands = result.brands;
        this.totalBrands = result.total;
        this.activeBrands = result.brands.filter((b: Brand) => b.active).length;
        this.inactiveBrands = this.totalBrands - this.activeBrands;
        this.filteredBrands = [...this.brands];
        this.loading = false;
      },
      error: (error: unknown) => {
        console.error('Error loading brands:', error);
        this.loading = false;
      }
    });
  }

  applyFilter() {
    if (!this.searchTerm) {
      this.filteredBrands = [...this.brands];
      return;
    }
    const term = this.searchTerm.toLowerCase();
    this.filteredBrands = this.brands.filter(b =>
      b.name?.toLowerCase().includes(term) ||
      b.description?.toLowerCase().includes(term)
    );
  }

  viewBrand(brand: Brand) {
    this.router.navigate(['/inventory/brands', brand.id]);
  }

  editBrand(brand: Brand) {
    this.router.navigate(['/inventory/brands', brand.id]);
  }

  deleteBrand(brand: Brand) {
    if (confirm(`Are you sure you want to delete brand: ${brand.name}?`)) {
      this.deleteBrandUseCase.execute(brand.id).subscribe({
        next: () => {
          this.loadBrands();
        },
        error: (error: unknown) => {
          console.error('Error deleting brand:', error);
        }
      });
    }
  }

  addNewBrand() {
    this.router.navigate(['/inventory/brands/new']);
  }

}