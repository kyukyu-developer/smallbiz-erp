import { Injectable, inject } from '@angular/core';
import { Observable, map } from 'rxjs';
import { Product, ProductFilter } from '../../domain/entities/product.entity';
import { IProductRepository } from '../../domain/repositories/product.repository.interface';
import { PRODUCT_REPOSITORY } from '../../domain/repositories/product.token';

export interface GetAllProductsResult {
  products: Product[];
  total: number;
}

@Injectable({ providedIn: 'root' })
export class GetAllProductsUseCase {
  private repository = inject<IProductRepository>(PRODUCT_REPOSITORY);

  execute(): Observable<GetAllProductsResult> {
    return this.repository.getAll().pipe(
      map(products => ({
        products,
        total: products.length
      }))
    );
  }
}

@Injectable({ providedIn: 'root' })
export class GetProductsByFilterUseCase {
  private repository = inject<IProductRepository>(PRODUCT_REPOSITORY);

  execute(filter: ProductFilter): Observable<Product[]> {
    return this.repository.getByFilter(filter);
  }
}

@Injectable({ providedIn: 'root' })
export class GetProductByIdUseCase {
  private repository = inject<IProductRepository>(PRODUCT_REPOSITORY);

  execute(id: number): Observable<Product | null> {
    return this.repository.getById(id);
  }
}

@Injectable({ providedIn: 'root' })
export class CreateProductUseCase {
  private repository = inject<IProductRepository>(PRODUCT_REPOSITORY);

  execute(product: Partial<Product>): Observable<Product> {
    return this.repository.create(product as any);
  }
}

@Injectable({ providedIn: 'root' })
export class UpdateProductUseCase {
  private repository = inject<IProductRepository>(PRODUCT_REPOSITORY);

  execute(id: number, product: Partial<Product>): Observable<Product> {
    return this.repository.update(id, product as any);
  }
}

@Injectable({ providedIn: 'root' })
export class DeleteProductUseCase {
  private repository = inject<IProductRepository>(PRODUCT_REPOSITORY);

  execute(id: number): Observable<void> {
    return this.repository.delete(id);
  }
}
