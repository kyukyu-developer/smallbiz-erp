import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatMenuModule } from '@angular/material/menu';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { Category } from '../../domain/entities/category.entity';
import { GetAllCategoriesUseCase, DeleteCategoryUseCase, GetAllCategoriesResult } from '../../application/usecases/category.usecase';

@Component({
  selector: 'app-category-list',
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
  templateUrl: './category-list.component.html',
  styleUrl: './category-list.component.scss'
})
export class CategoryListComponent implements OnInit {
  private getAllCategoriesUseCase = inject(GetAllCategoriesUseCase);
  private deleteCategoryUseCase = inject(DeleteCategoryUseCase);
  private router = inject(Router);

  categories: Category[] = [];
  filteredCategories: Category[] = [];
  searchTerm: string = '';
  loading: boolean = false;
  totalCategories: number = 0;
  activeCategories: number = 0;
  inactiveCategories: number = 0;

  ngOnInit() {
    this.loadCategories();
  }

  loadCategories() {
    this.loading = true;
    this.getAllCategoriesUseCase.execute().subscribe({
      next: (result: GetAllCategoriesResult) => {
        this.categories = result.categories;
        this.totalCategories = result.total;
        this.activeCategories = result.categories.filter((c: Category) => c.active).length;
        this.inactiveCategories = this.totalCategories - this.activeCategories;
        this.filteredCategories = [...this.categories];
        this.loading = false;
      },
      error: (error: unknown) => {
        console.error('Error loading categories:', error);
        this.loading = false;
      }
    });
  }

  applyFilter() {
    if (!this.searchTerm) {
      this.filteredCategories = [...this.categories];
      return;
    }
    const term = this.searchTerm.toLowerCase();
    this.filteredCategories = this.categories.filter(c =>
      c.name?.toLowerCase().includes(term) ||
      c.code?.toLowerCase().includes(term) ||
      c.description?.toLowerCase().includes(term)
    );
  }

  viewCategory(category: Category) {
    this.router.navigate(['/inventory/categories', category.id]);
  }

  editCategory(category: Category) {
    this.router.navigate(['/inventory/categories', category.id]);
  }

  deleteCategory(category: Category) {
    if (confirm(`Are you sure you want to delete category: ${category.name}?`)) {
      this.deleteCategoryUseCase.execute(category.id).subscribe({
        next: () => {
          this.loadCategories();
        },
        error: (error: unknown) => {
          console.error('Error deleting category:', error);
        }
      });
    }
  }

  addNewCategory() {
    this.router.navigate(['/inventory/categories/new']);
  }
}
