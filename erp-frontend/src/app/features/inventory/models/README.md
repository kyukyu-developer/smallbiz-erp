# Inventory Data Models

This directory contains TypeScript interfaces that map to the backend database schema for the ERP inventory system.

## Core Database Models

### 1. **Unit** (`unit.model.ts`)
Represents measurement units (kg, lbs, pieces, etc.)
```typescript
{
  unit_id: number;
  unit_name: string;
}
```

### 2. **UnitConversion** (`unit.model.ts`)
Defines conversion factors between different units for products
```typescript
{
  conversion_id: number;
  product_id: number;
  from_unit_id: number;
  to_unit_id: number;
  factor: number;  // e.g., 1 kg = 2.20462 lbs
}
```

### 3. **Product** (`product.model.ts`)
Core product information
```typescript
{
  product_id: number;
  name: string;
  category_id: number;
  base_unit_id: number;
  tracking_type: 'none' | 'batch' | 'serial';
  base_cost_price: number;
  base_sale_price: number;
}
```

### 4. **ProductUnitPrice** (`product-unit-price.model.ts`)
Pricing for products in different units
```typescript
{
  unit_price_id: number;
  product_id: number;
  unit_id: number;
  sale_price: number;
}
```

### 5. **ProductBatch** (`product-batch.model.ts`)
Batch tracking for products with expiry dates
```typescript
{
  batch_id: number;
  product_id: number;
  warehouse_id: number;
  batch_no: string;
  expiry_date: Date;
  quantity: number;
}
```

### 6. **ProductSerial** (`product-batch.model.ts`)
Serial number tracking for individual items
```typescript
{
  serial_id: number;
  product_id: number;
  serial_no: string;
  status: 'available' | 'sold' | 'damaged' | 'reserved';
}
```

### 7. **Category** (`category.model.ts`)
Product categorization with hierarchical support
```typescript
{
  category_id: number;
  category_name: string;
  parent_category_id?: number;  // for hierarchical categories
}
```

### 8. **Warehouse** (`warehouse.model.ts`)
Physical storage locations
```typescript
{
  warehouse_id: number;
  warehouse_name: string;
  location: string;
  // Optional UI fields
  code?: string;
  description?: string;
  manager?: string;
  capacity?: number;
  status?: 'active' | 'inactive' | 'maintenance';
}
```

### 9. **WarehouseStock** (`warehouse.model.ts`)
Stock quantity at each warehouse
```typescript
{
  warehouse_stock_id: number;
  warehouse_id: number;
  quantity: number;
}
```

### 10. **PurchaseItem** (`purchase-item.model.ts`)
Items in purchase orders
```typescript
{
  purchase_item_id: number;
  purchase_id: number;
  product_id: number;
  unit_id: number;
  quantity: number;
}
```

### 11. **StockMovement** (`stock-movement.model.ts`)
Tracks all inventory movements
```typescript
{
  movement_id: number;
  product_id: number;
  warehouse_id: number;
  movement_type: 'in' | 'out' | 'adjustment' | 'transfer';
  reference_type: 'purchase' | 'sale' | 'adjustment' | 'transfer' | 'return';
  reference_id: number;
  base_quantity: number;
  batch_id?: number;
  movement_date: Date;
}
```

### 12. **Sale** (`sales.model.ts`)
Sales order header
```typescript
{
  sale_id: number;
  customer_id: number;
  warehouse_id: number;
  sale_date: Date;
}
```

### 13. **SalesItem** (`sales.model.ts`)
Individual items in sales orders
```typescript
{
  sales_item_id: number;
  sale_id: number;
  product_id: number;
  unit_id: number;
  quantity: number;
  unit_price: number;
  base_quantity: number;
  batch_id?: number;
  serial_id?: number;
}
```

## Database Relationships

### Foreign Key Relationships:
- **Products** → Categories (category_id)
- **Products** → Units (base_unit_id)
- **ProductUnitPrice** → Products, Units
- **ProductBatch** → Products, Warehouses
- **ProductSerial** → Products
- **WarehouseStock** → Warehouses
- **PurchaseItem** → Purchases, Products, Units
- **StockMovement** → Products, Warehouses, ProductBatch
- **Sales** → Customers, Warehouses
- **SalesItem** → Sales, Products, Units, ProductBatch, ProductSerial
- **UnitConversion** → Products, Units (from/to)

## Usage

Import models from the index file:
```typescript
import { Product, Warehouse, StockMovement } from './models';
```

Or import individually:
```typescript
import { Warehouse } from './models/warehouse.model';
```

## Notes

- Primary keys use snake_case following database conventions (e.g., `product_id`, `warehouse_id`)
- Optional fields marked with `?` are for UI/display purposes and may not exist in the core database schema
- All models follow the ER diagram structure with unit conversion and pricing override support
- Tracking types allow for flexible inventory management (none, batch, serial)
