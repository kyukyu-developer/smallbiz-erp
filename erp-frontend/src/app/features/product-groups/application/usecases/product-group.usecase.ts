import { Injectable, inject } from '@angular/core';
import { Observable, map, catchError, of } from 'rxjs';
import { ProductGroup, ProductGroupFilter } from '../../domain/entities/product-group.entity';
import { IProductGroupRepository } from '../../domain/repositories/product-group.repository.interface';
import { PRODUCT_GROUP_REPOSITORY } from '../../domain/repositories/product-group.token';

export interface GetAllProductGroupsResult {
  productGroups: ProductGroup[];
  total: number;
}

@Injectable({ providedIn: 'root' })
export class GetAllProductGroupsUseCase {
  private repository = inject<IProductGroupRepository>(PRODUCT_GROUP_REPOSITORY);

  execute(): Observable<GetAllProductGroupsResult> {
    return this.repository.getAll().pipe(
      map((productGroups: ProductGroup[]) => ({
        productGroups,
        total: productGroups.length,
      })),
      catchError((error: unknown) => {
        console.error('Error fetching product groups:', error);
        return of({ productGroups: [] as ProductGroup[], total: 0 });
      }),
    );
  }
}

@Injectable({ providedIn: 'root' })
export class GetProductGroupsByFilterUseCase {
  private repository = inject<IProductGroupRepository>(PRODUCT_GROUP_REPOSITORY);

  execute(filter: ProductGroupFilter): Observable<ProductGroup[]> {
    return this.repository.getByFilter(filter);
  }
}

@Injectable({ providedIn: 'root' })
export class GetProductGroupByIdUseCase {
  private repository = inject<IProductGroupRepository>(PRODUCT_GROUP_REPOSITORY);

  execute(id: string): Observable<ProductGroup | null> {
    return this.repository.getById(id);
  }
}

@Injectable({ providedIn: 'root' })
export class CreateProductGroupUseCase {
  private repository = inject<IProductGroupRepository>(PRODUCT_GROUP_REPOSITORY);

  execute(productGroup: Partial<ProductGroup>): Observable<ProductGroup> {
    return this.repository.create(productGroup as ProductGroup);
  }
}

@Injectable({ providedIn: 'root' })
export class UpdateProductGroupUseCase {
  private repository = inject<IProductGroupRepository>(PRODUCT_GROUP_REPOSITORY);

  execute(id: string, productGroup: Partial<ProductGroup>): Observable<ProductGroup> {
    return this.repository.update(id, productGroup as ProductGroup);
  }
}

@Injectable({ providedIn: 'root' })
export class DeleteProductGroupUseCase {
  private repository = inject<IProductGroupRepository>(PRODUCT_GROUP_REPOSITORY);

  execute(id: string): Observable<void> {
    return this.repository.delete(id);
  }
}
