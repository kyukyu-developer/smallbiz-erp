import { Injectable, inject } from '@angular/core';
import { Observable, map, catchError, of } from 'rxjs';
import { Brand, BrandFilter } from '../../domain/entities/brand.entity';
import { IBrandRepository } from '../../domain/repositories/brand.repository.interface';
import { BRAND_REPOSITORY } from '../../domain/repositories/brand.token';

export interface GetAllBrandsResult {
  brands: Brand[];
  total: number;
}

@Injectable({ providedIn: 'root' })
export class GetAllBrandsUseCase {
  private repository = inject<IBrandRepository>(BRAND_REPOSITORY);

  execute(): Observable<GetAllBrandsResult> {
    return this.repository.getAll().pipe(
      map((brands: Brand[]) => ({
        brands,
        total: brands.length,
      })),
      catchError((error: unknown) => {
        console.error('Error fetching brands:', error);
        return of({ brands: [] as Brand[], total: 0 });
      }),
    );
  }
}

@Injectable({ providedIn: 'root' })
export class GetBrandsByFilterUseCase {
  private repository = inject<IBrandRepository>(BRAND_REPOSITORY);

  execute(filter: BrandFilter): Observable<Brand[]> {
    return this.repository.getByFilter(filter);
  }
}

@Injectable({ providedIn: 'root' })
export class GetBrandByIdUseCase {
  private repository = inject<IBrandRepository>(BRAND_REPOSITORY);

  execute(id: string): Observable<Brand | null> {
    return this.repository.getById(id);
  }
}

@Injectable({ providedIn: 'root' })
export class CreateBrandUseCase {
  private repository = inject<IBrandRepository>(BRAND_REPOSITORY);

  execute(brand: Partial<Brand>): Observable<Brand> {
    return this.repository.create(brand as Brand);
  }
}

@Injectable({ providedIn: 'root' })
export class UpdateBrandUseCase {
  private repository = inject<IBrandRepository>(BRAND_REPOSITORY);

  execute(id: string, brand: Partial<Brand>): Observable<Brand> {
    return this.repository.update(id, brand as Brand);
  }
}

@Injectable({ providedIn: 'root' })
export class DeleteBrandUseCase {
  private repository = inject<IBrandRepository>(BRAND_REPOSITORY);

  execute(id: string): Observable<void> {
    return this.repository.delete(id);
  }
}
