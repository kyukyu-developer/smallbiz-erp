import { Injectable, inject } from '@angular/core';
import { Observable, map, catchError, of } from 'rxjs';
import { Customer, CustomerFilter } from '../../domain/entities/customer.entity';
import { ICustomerRepository } from '../../domain/repositories/customer.repository.interface';
import { CUSTOMER_REPOSITORY } from '../../domain/repositories/customer.token';

export interface GetAllCustomersResult {
  customers: Customer[];
  total: number;
}

@Injectable({ providedIn: 'root' })
export class GetAllCustomersUseCase {
  private repository = inject<ICustomerRepository>(CUSTOMER_REPOSITORY);

  execute(): Observable<GetAllCustomersResult> {
    return this.repository.getAll().pipe(
      map((customers: Customer[]) => ({
        customers,
        total: customers.length,
      })),
      catchError((error: unknown) => {
        console.error('Error fetching customers:', error);
        return of({ customers: [] as Customer[], total: 0 });
      }),
    );
  }
}

@Injectable({ providedIn: 'root' })
export class GetCustomersByFilterUseCase {
  private repository = inject<ICustomerRepository>(CUSTOMER_REPOSITORY);

  execute(filter: CustomerFilter): Observable<Customer[]> {
    return this.repository.getByFilter(filter);
  }
}

@Injectable({ providedIn: 'root' })
export class GetCustomerByIdUseCase {
  private repository = inject<ICustomerRepository>(CUSTOMER_REPOSITORY);

  execute(id: string): Observable<Customer | null> {
    return this.repository.getById(id);
  }
}

@Injectable({ providedIn: 'root' })
export class CreateCustomerUseCase {
  private repository = inject<ICustomerRepository>(CUSTOMER_REPOSITORY);

  execute(customer: Partial<Customer>): Observable<Customer> {
    return this.repository.create(customer as Customer);
  }
}

@Injectable({ providedIn: 'root' })
export class UpdateCustomerUseCase {
  private repository = inject<ICustomerRepository>(CUSTOMER_REPOSITORY);

  execute(id: string, customer: Partial<Customer>): Observable<Customer> {
    return this.repository.update(id, customer as Customer);
  }
}

@Injectable({ providedIn: 'root' })
export class DeleteCustomerUseCase {
  private repository = inject<ICustomerRepository>(CUSTOMER_REPOSITORY);

  execute(id: string): Observable<void> {
    return this.repository.delete(id);
  }
}
