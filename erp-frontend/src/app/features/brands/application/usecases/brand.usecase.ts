import { Injectable, inject } from '@angular/core';
import { Observable, map, catchError, of } from 'rxjs';
import { Brand, BrandFilter } from '../../domain/entities/brand.entity';
import { BrandRepository } from '../../infrastructure/repositories/brand.repository';

export interface GetAllBrandsResult {
  brands: Brand[];
  total: number;
}

@Injectable({ providedIn: 'root' })
export class GetAllBrandsUseCase {
  private repository = inject(BrandRepository);

  execute(): Observable<GetAllBrandsResult> {
    return this.repository.getAll().pipe(
      map((brands: Brand[]) => ({
        brands,
        total: brands.length
      })),
      catchError((error: unknown) => {
        console.error('Error fetching brands:', error);
        return of({ brands: [] as Brand[], total: 0 });
      })
    );
  }
}

@Injectable({ providedIn: 'root' })
export class GetBrandsByFilterUseCase {
  private repository = inject(BrandRepository);

  execute(filter: BrandFilter): Observable<Brand[]> {
    return this.repository.getByFilter(filter);
  }
}

@Injectable({ providedIn: 'root' })
export class GetBrandByIdUseCase {
  private repository = inject(BrandRepository);

  execute(id: string): Observable<Brand | null> {
    return this.repository.getById(id);
  }
}

@Injectable({ providedIn: 'root' })
export class CreateBrandUseCase {
  private repository = inject(BrandRepository);

  execute(brand: Partial<Brand>): Observable<Brand> {
    return this.repository.create(brand as Brand);
  }
}

@Injectable({ providedIn: 'root' })
export class UpdateBrandUseCase {
  private repository = inject(BrandRepository);

  execute(id: string, brand: Partial<Brand>): Observable<Brand> {
    return this.repository.update(id, brand as Brand);
  }
}

@Injectable({ providedIn: 'root' })
export class DeleteBrandUseCase {
  private repository = inject(BrandRepository);

  execute(id: string): Observable<void> {
    return this.repository.delete(id);
  }
}