import { ProductRepository } from './repositories/product.repository';
import { WarehouseRepository } from './repositories/warehouse.repository';

export const INVENTORY_REPOSITORIES = [
  ProductRepository,
  WarehouseRepository
];

export * from './repositories/product.repository';
export * from './repositories/warehouse.repository';