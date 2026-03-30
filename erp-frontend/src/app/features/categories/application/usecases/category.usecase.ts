import { Injectable, inject } from '@angular/core';
import { Observable, map, catchError, of } from 'rxjs';
import { Category, CategoryFilter } from '../../domain/entities/category.entity';
import { ICategoryRepository } from '../../domain/repositories/category.repository.interface';
import { CATEGORY_REPOSITORY } from '../../domain/repositories/category.token';

export interface GetAllCategoriesResult {
  categories: Category[];
  total: number;
}

@Injectable({ providedIn: 'root' })
export class GetAllCategoriesUseCase {
  private repository = inject<ICategoryRepository>(CATEGORY_REPOSITORY);

  execute(): Observable<GetAllCategoriesResult> {
    return this.repository.getAll().pipe(
      map((categories: Category[]) => ({
        categories,
        total: categories.length,
      })),
      catchError((error: unknown) => {
        console.error('Error fetching categories:', error);
        return of({ categories: [] as Category[], total: 0 });
      }),
    );
  }
}

@Injectable({ providedIn: 'root' })
export class GetCategoriesByFilterUseCase {
  private repository = inject<ICategoryRepository>(CATEGORY_REPOSITORY);

  execute(filter: CategoryFilter): Observable<Category[]> {
    return this.repository.getByFilter(filter);
  }
}

@Injectable({ providedIn: 'root' })
export class GetCategoryByIdUseCase {
  private repository = inject<ICategoryRepository>(CATEGORY_REPOSITORY);

  execute(id: string): Observable<Category | null> {
    return this.repository.getById(id);
  }
}

@Injectable({ providedIn: 'root' })
export class CreateCategoryUseCase {
  private repository = inject<ICategoryRepository>(CATEGORY_REPOSITORY);

  execute(category: Partial<Category>): Observable<Category> {
    return this.repository.create(category as Category);
  }
}

@Injectable({ providedIn: 'root' })
export class UpdateCategoryUseCase {
  private repository = inject<ICategoryRepository>(CATEGORY_REPOSITORY);

  execute(id: string, category: Partial<Category>): Observable<Category> {
    return this.repository.update(id, category as Category);
  }
}

@Injectable({ providedIn: 'root' })
export class DeleteCategoryUseCase {
  private repository = inject<ICategoryRepository>(CATEGORY_REPOSITORY);

  execute(id: string): Observable<void> {
    return this.repository.delete(id);
  }
}
