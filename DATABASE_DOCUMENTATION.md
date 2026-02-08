# ERP Inventory Management - Database Documentation

## Table of Contents
1. [Overview](#overview)
2. [Database Schema](#database-schema)
3. [Table Descriptions](#table-descriptions)
4. [Business Rules](#business-rules)
5. [Entity Relationships](#entity-relationships)
6. [Workflow Diagrams](#workflow-diagrams)
7. [Use Cases & Examples](#use-cases--examples)

---

## Overview

This database schema supports a comprehensive ERP inventory management system with the following capabilities:

- **Multi-Warehouse Management**: Hierarchical warehouse structure (Main, Branch, Sub-warehouses)
- **Product Management**: Products with groups, categories, and brands
- **Product Variants**: Support for product variations (color, size, etc.)
- **Unit of Measure (UOM)**: Flexible UOM with conversion support
- **Inventory Tracking**: Optional inventory tracking per product
- **Batch & Serial Tracking**: Support for batch and serial number tracking
- **Audit Trail**: Complete audit fields on all tables

---

## Database Schema

### Core Modules

```
┌─────────────────────────────────────────────────────────┐
│                    WAREHOUSE MODULE                      │
│  - warehouse (Main/Branch/Sub hierarchy)                │
└─────────────────────────────────────────────────────────┘

┌─────────────────────────────────────────────────────────┐
│                  PRODUCT MASTER MODULE                   │
│  - product_group                                         │
│  - product_category                                      │
│  - product_brand                                         │
│  - product                                               │
└─────────────────────────────────────────────────────────┘

┌─────────────────────────────────────────────────────────┐
│                  PRODUCT VARIANT MODULE                  │
│  - attribute                                             │
│  - attribute_value                                       │
│  - product_variant                                       │
│  - product_variant_attribute                             │
└─────────────────────────────────────────────────────────┘

┌─────────────────────────────────────────────────────────┐
│                      UOM MODULE                          │
│  - uom                                                   │
│  - product_uom                                           │
│  - product_uom_conversion                                │
└─────────────────────────────────────────────────────────┘
```

---

## Table Descriptions

### 1. Warehouse Table

**Purpose**: Stores warehouse information with hierarchical structure support.

| Column | Type | Description |
|--------|------|-------------|
| id | INT | Primary key |
| name | VARCHAR(50) | Warehouse name |
| city | VARCHAR(50) | City location |
| branch_type | VARCHAR(20) | 'Main', 'Branch', 'Sub' |
| is_main_warehouse | BOOLEAN | TRUE if main warehouse |
| parent_warehouse_id | INT | Reference to parent warehouse (NULL for main) |
| active | BOOLEAN | Active status |

**Key Business Rule**:
- ⚠️ **Only warehouses with `is_main_warehouse = TRUE` can receive stock**
- Branch and Sub warehouses receive stock via transfers from main warehouse

**Hierarchy Example**:
```
Main Warehouse (is_main_warehouse=TRUE, parent_warehouse_id=NULL)
  ├── Branch Warehouse 1 (parent_warehouse_id=1)
  │   └── Sub Warehouse 1A (parent_warehouse_id=2)
  └── Branch Warehouse 2 (parent_warehouse_id=1)
```

---

### 2. UOM (Unit of Measure) Table

**Purpose**: Defines units of measurement used across the system.

| Column | Type | Description |
|--------|------|-------------|
| id | INT | Primary key |
| name | VARCHAR(50) | Unit name (Piece, Kilogram, Box) |
| symbol | VARCHAR(20) | Unit symbol (pc, kg, bx) |
| active | BOOLEAN | Active status |

**Examples**:
- Piece (pc)
- Kilogram (kg)
- Box (bx)
- Dozen (dz)
- Liter (L)

---

### 3. Product Group Table

**Purpose**: High-level categorization of products.

| Column | Type | Description |
|--------|------|-------------|
| id | INT | Primary key |
| name | VARCHAR(50) | Group name |
| description | VARCHAR(255) | Optional description |
| active | BOOLEAN | Active status |

**Examples**:
- Food & Beverages
- Electronics
- Clothing
- Office Supplies

---

### 4. Product Category Table

**Purpose**: Detailed categorization within product groups.

| Column | Type | Description |
|--------|------|-------------|
| id | INT | Primary key |
| group_id | INT | Foreign key to product_group |
| name | VARCHAR(50) | Category name |
| description | VARCHAR(255) | Optional description |
| active | BOOLEAN | Active status |

**Example Hierarchy**:
```
Group: Food & Beverages
  ├── Category: Snacks
  ├── Category: Soft Drinks
  └── Category: Dairy Products
```

---

### 5. Product Brand Table

**Purpose**: Stores brand information for products.

| Column | Type | Description |
|--------|------|-------------|
| id | INT | Primary key |
| name | VARCHAR(50) | Brand name |
| description | VARCHAR(255) | Optional description |
| active | BOOLEAN | Active status |

**Examples**:
- Nestle
- Coca-Cola
- Samsung
- Nike

---

### 6. Attribute Table

**Purpose**: Defines product attributes for variants (e.g., Color, Size, Flavor).

| Column | Type | Description |
|--------|------|-------------|
| id | INT | Primary key |
| name | VARCHAR(50) | Attribute name |
| active | BOOLEAN | Active status |

**Common Attributes**:
- Color
- Size
- Flavor
- Material
- Weight

---

### 7. Attribute Value Table

**Purpose**: Stores possible values for each attribute.

| Column | Type | Description |
|--------|------|-------------|
| id | INT | Primary key |
| attribute_id | INT | Foreign key to attribute |
| value | VARCHAR(50) | Attribute value |
| active | BOOLEAN | Active status |

**Example**:
```
Attribute: Color
  ├── Red
  ├── Blue
  └── Green

Attribute: Size
  ├── Small
  ├── Medium
  └── Large
```

---

### 8. Product Table (Master)

**Purpose**: Central table storing product master data.

| Column | Type | Description |
|--------|------|-------------|
| id | INT | Primary key |
| code | VARCHAR(20) | Unique product code |
| name | VARCHAR(50) | Product name |
| group_id | INT | Foreign key to product_group |
| category_id | INT | Foreign key to product_category |
| brand_id | INT | Foreign key to product_brand |
| base_uom_id | INT | Foreign key to uom (base unit) |
| track_inventory | BOOLEAN | TRUE if stock tracking enabled |
| has_variant | BOOLEAN | TRUE if product has variants |
| has_batch | BOOLEAN | TRUE if batch tracking enabled |
| has_serial | BOOLEAN | TRUE if serial tracking enabled |
| reorder_level | DECIMAL(10,2) | Stock reorder threshold |
| allow_negative_stock | BOOLEAN | Allow negative stock |
| active | BOOLEAN | Active status |

**Key Business Rules**:

1. **Inventory Tracking (`track_inventory`)**:
   - `TRUE`: Product stock is tracked in warehouse
   - `FALSE`: No stock ledger entries (used for services, digital goods, non-stock items)

2. **Performance Optimization**:
   - ERP does NOT create stock ledger entries for `track_inventory = FALSE`
   - Reduces database load for non-inventory items

3. **Flexibility**:
   - Services (consulting, repairs)
   - Digital goods (software licenses)
   - Non-stock items (drop-shipped products)

4. **Reporting**:
   - Stock reports filter by `track_inventory = TRUE`

---

### 9. Product UOM Table

**Purpose**: Defines which units of measure can be used for each product.

| Column | Type | Description |
|--------|------|-------------|
| id | INT | Primary key |
| product_id | INT | Foreign key to product |
| uom_id | INT | Foreign key to uom |
| factor_to_base | DECIMAL(18,4) | Conversion factor to base unit |
| is_base | BOOLEAN | TRUE if this is the base UOM |
| active | BOOLEAN | Active status |

**Example**:
```
Product: Coca-Cola
- Piece (base) - factor_to_base = 1.0000, is_base = TRUE
- Pack of 6 - factor_to_base = 6.0000, is_base = FALSE
- Carton of 24 - factor_to_base = 24.0000, is_base = FALSE
```

---

### 10. Product UOM Conversion Table

**Purpose**: Defines conversion rules between different units for a product.

| Column | Type | Description |
|--------|------|-------------|
| id | INT | Primary key |
| product_id | INT | Foreign key to product |
| from_uom_id | INT | Source unit |
| to_uom_id | INT | Target unit |
| factor | DECIMAL(18,4) | Conversion multiplier |
| active | BOOLEAN | Active status |

**Example**:
```
Product: Coca-Cola
- From: Piece → To: Pack → Factor: 0.1667 (1/6)
- From: Pack → To: Piece → Factor: 6.0000
- From: Pack → To: Carton → Factor: 0.2500 (1/4)
- From: Carton → To: Pack → Factor: 4.0000
```

---

### 11. Product Variant Table

**Purpose**: Stores product variants (SKUs) for products with variations.

| Column | Type | Description |
|--------|------|-------------|
| id | INT | Primary key |
| product_id | INT | Foreign key to product |
| sku | VARCHAR(50) | Unique SKU (e.g., TS001-R-M) |
| base_uom_id | INT | Foreign key to uom |
| track_inventory | BOOLEAN | Track inventory for this variant |
| active | BOOLEAN | Active status |

**Example**:
```
Product: T-Shirt (TS001)
  ├── Variant: TS001-R-S (Red, Small)
  ├── Variant: TS001-R-M (Red, Medium)
  ├── Variant: TS001-B-S (Blue, Small)
  └── Variant: TS001-B-M (Blue, Medium)
```

---

### 12. Product Variant Attribute Table

**Purpose**: Links variants to their specific attribute values.

| Column | Type | Description |
|--------|------|-------------|
| id | INT | Primary key |
| variant_id | INT | Foreign key to product_variant |
| attribute_id | INT | Foreign key to attribute |
| value_id | INT | Foreign key to attribute_value |

**Example**:
```
Variant: TS001-R-M (Red, Medium T-Shirt)
  ├── Attribute: Color → Value: Red
  └── Attribute: Size → Value: Medium
```

---

## Business Rules

### Warehouse Business Rules

1. **Stock Receipt**:
   - ✅ Only main warehouses (`is_main_warehouse = TRUE`) can receive stock from suppliers
   - ❌ Branch and sub-warehouses cannot receive stock directly
   - Stock moves to branch/sub-warehouses via **Stock Transfer** transactions

2. **Warehouse Hierarchy**:
   - Main warehouse: `parent_warehouse_id = NULL`
   - Branch warehouse: `parent_warehouse_id = [main_warehouse_id]`
   - Sub warehouse: `parent_warehouse_id = [branch_warehouse_id]`

3. **Warehouse Activation**:
   - Inactive warehouses (`active = FALSE`) cannot participate in transactions

### Product Business Rules

1. **Inventory Tracking**:
   - Products with `track_inventory = FALSE`:
     - No stock ledger entries created
     - No warehouse stock tracking
     - Used for: services, digital goods, non-stock items

2. **Product Variants**:
   - If `has_variant = TRUE`:
     - Product must have at least one variant in `product_variant` table
     - Transactions reference variant SKU, not base product

3. **Batch Tracking**:
   - If `has_batch = TRUE`:
     - Each stock transaction must include batch number
     - Stock queries filter by batch number

4. **Serial Tracking**:
   - If `has_serial = TRUE`:
     - Each item has unique serial number
     - Quantity is always 1 per serial number

5. **Negative Stock**:
   - If `allow_negative_stock = FALSE`:
     - System prevents sales/issues exceeding available stock
   - If `allow_negative_stock = TRUE`:
     - System allows overselling (useful for on-demand items)

### UOM Business Rules

1. **Base UOM**:
   - Every product must have exactly one base UOM (`is_base = TRUE`)
   - All conversions reference the base UOM

2. **UOM Conversions**:
   - Conversion factor must be consistent:
     - If 1 Pack = 6 Pieces, then factor_to_base = 6.0
     - Reverse conversion: 1 Piece = 0.1667 Packs

---

## Entity Relationships

### Entity Relationship Diagram (ERD)

```
┌─────────────────┐
│  product_group  │
└────────┬────────┘
         │ 1
         │
         │ *
┌────────┴───────────┐
│ product_category   │
└────────┬───────────┘
         │ 1
         │
         │ *
┌────────┴───────────┐         ┌──────────────┐
│  product_brand     │         │     uom      │
└────────┬───────────┘         └──────┬───────┘
         │ 1                          │ 1
         │                            │
         │ *                          │ *
┌────────┴────────────────────────────┴───────┐
│              product                        │
│  - track_inventory (TRUE/FALSE)             │
│  - has_variant (TRUE/FALSE)                 │
│  - has_batch (TRUE/FALSE)                   │
│  - has_serial (TRUE/FALSE)                  │
└────────┬────────────────────────────────────┘
         │ 1
         │
         │ * (if has_variant = TRUE)
┌────────┴───────────┐
│ product_variant    │
│  - sku             │
└────────┬───────────┘
         │ 1
         │
         │ *
┌────────┴───────────────────┐
│ product_variant_attribute  │
└────────┬───────────────────┘
         │ *
         │
         │ 1
┌────────┴───────────┐       ┌──────────────────┐
│    attribute       │       │ attribute_value  │
└────────────────────┘       └──────────────────┘


┌─────────────────┐
│   warehouse     │
│  (Hierarchy)    │
└────────┬────────┘
         │
         │ Self-referencing FK
         │ (parent_warehouse_id)
         │
         └─────────────┐
                       │
         ┌─────────────┘
         │
         └─> Main Warehouse (parent_id = NULL)
               ├─> Branch 1 (parent_id = main)
               │     └─> Sub 1A (parent_id = branch1)
               └─> Branch 2 (parent_id = main)
```

### Table Relationships Summary

| Parent Table | Child Table | Relationship | Cardinality |
|-------------|-------------|--------------|-------------|
| product_group | product_category | Group has Categories | 1:N |
| product_category | product | Category has Products | 1:N |
| product_brand | product | Brand has Products | 1:N |
| uom | product | UOM is Base for Products | 1:N |
| product | product_variant | Product has Variants | 1:N |
| product_variant | product_variant_attribute | Variant has Attributes | 1:N |
| attribute | attribute_value | Attribute has Values | 1:N |
| attribute | product_variant_attribute | Attribute defines Variant Attr | 1:N |
| attribute_value | product_variant_attribute | Value assigned to Variant | 1:N |
| warehouse | warehouse | Parent-Child Hierarchy | 1:N |
| product | product_uom | Product uses UOMs | 1:N |
| uom | product_uom | UOM used by Products | 1:N |
| product | product_uom_conversion | Product has Conversions | 1:N |

---

## Workflow Diagrams

### 1. Product Creation Workflow

```
START
  │
  ├─> Create Product Group (if needed)
  │     │
  │     └─> Create Product Category (if needed)
  │           │
  │           └─> Create Product Brand (if needed)
  │                 │
  ├─────────────────┘
  │
  ├─> Create Product Master
  │     - Set: code, name
  │     - Link: group_id, category_id, brand_id
  │     - Set: base_uom_id
  │     - Configure: track_inventory, has_variant, has_batch, has_serial
  │
  ├─> IF track_inventory = FALSE
  │     └─> END (Service/Digital Product)
  │
  ├─> IF has_variant = FALSE
  │     └─> END (Simple Product)
  │
  └─> IF has_variant = TRUE
        │
        ├─> Create Attributes (Color, Size, etc.)
        │     │
        │     └─> Create Attribute Values (Red, Blue, Small, Large)
        │
        └─> Create Product Variants
              - Generate SKU (e.g., TS001-R-M)
              - Link to product_variant_attribute
              │
              END
```

### 2. Warehouse Stock Receipt Workflow

```
START: Purchase Order arrives at warehouse
  │
  ├─> Validate Warehouse
  │     │
  │     ├─> Check: is_main_warehouse = TRUE?
  │     │     │
  │     │     ├─> YES: Continue
  │     │     └─> NO: REJECT (Only main warehouses receive stock)
  │     │
  │     └─> Check: active = TRUE?
  │           │
  │           ├─> YES: Continue
  │           └─> NO: REJECT (Warehouse inactive)
  │
  ├─> FOR EACH Product in PO:
  │     │
  │     ├─> Check: track_inventory = TRUE?
  │     │     │
  │     │     ├─> NO: Skip (No stock ledger for non-inventory items)
  │     │     └─> YES: Continue
  │     │
  │     ├─> Check: has_variant = TRUE?
  │     │     │
  │     │     ├─> YES: Select specific variant (SKU)
  │     │     └─> NO: Use base product
  │     │
  │     ├─> Check: has_batch = TRUE?
  │     │     │
  │     │     ├─> YES: Enter batch number
  │     │     └─> NO: Skip
  │     │
  │     ├─> Check: has_serial = TRUE?
  │     │     │
  │     │     ├─> YES: Enter serial numbers (qty = 1 per serial)
  │     │     └─> NO: Skip
  │     │
  │     ├─> Enter Quantity & UOM
  │     │     │
  │     │     └─> Convert to Base UOM using product_uom_conversion
  │     │
  │     └─> Create Stock Ledger Entry
  │           - Type: RECEIPT
  │           - Warehouse: Main Warehouse
  │           - Quantity: [converted to base UOM]
  │           - Batch/Serial: [if applicable]
  │
  └─> Update Stock Balance
        │
        END
```

### 3. Stock Transfer Workflow (Main → Branch)

```
START: Transfer Request from Branch Warehouse
  │
  ├─> Validate Source Warehouse (Main Warehouse)
  │     │
  │     ├─> Check: is_main_warehouse = TRUE OR active branch
  │     └─> Check: Sufficient stock available
  │
  ├─> Validate Destination Warehouse (Branch/Sub)
  │     │
  │     ├─> Check: active = TRUE
  │     └─> Check: Valid hierarchy (main → branch → sub)
  │
  ├─> Create Stock Transfer Document
  │     │
  │     ├─> FOR EACH Product:
  │     │     │
  │     │     ├─> Check stock availability at source
  │     │     ├─> Reserve stock (if needed)
  │     │     └─> Add to transfer document
  │     │
  │     └─> Submit for approval
  │
  ├─> Process Transfer
  │     │
  │     ├─> Create OUTWARD Entry (Source Warehouse)
  │     │     - Type: TRANSFER OUT
  │     │     - Quantity: Negative
  │     │
  │     └─> Create INWARD Entry (Destination Warehouse)
  │           - Type: TRANSFER IN
  │           - Quantity: Positive
  │
  └─> Update Stock Balances
        │
        END
```

### 4. Product Variant Selection Workflow

```
START: User selects Product (has_variant = TRUE)
  │
  ├─> Fetch Product Variants for product_id
  │
  ├─> FOR EACH Variant:
  │     │
  │     ├─> Fetch Variant Attributes
  │     │     │
  │     │     └─> JOIN: product_variant_attribute → attribute → attribute_value
  │     │
  │     └─> Display: SKU + Attributes
  │           Example: "TS001-R-M (Red, Medium)"
  │
  ├─> User Selects Variant
  │
  ├─> Check Variant Stock (if track_inventory = TRUE)
  │     │
  │     ├─> Query stock ledger by variant_id
  │     └─> Display available quantity
  │
  └─> Proceed with Transaction
        │
        END
```

### 5. UOM Conversion Workflow

```
START: User enters quantity in non-base UOM
  │
  ├─> Example: User enters "2 Cartons" of Coca-Cola
  │
  ├─> Lookup product_uom_conversion
  │     │
  │     └─> Find: from_uom_id = Carton, to_uom_id = Piece (base)
  │           Result: factor = 24.0
  │
  ├─> Calculate Base Quantity
  │     │
  │     └─> base_qty = input_qty × factor
  │           = 2 × 24.0
  │           = 48 Pieces
  │
  ├─> Store in Database
  │     │
  │     ├─> stock_ledger.quantity = 48 (base UOM)
  │     └─> stock_ledger.uom_id = Piece (base UOM id)
  │
  └─> Display to User
        │
        └─> "2 Cartons = 48 Pieces"
        │
        END
```

---

## Use Cases & Examples

### Use Case 1: Service Product (No Inventory Tracking)

**Scenario**: Consulting company offers IT consulting services

**Setup**:
```sql
INSERT INTO product (code, name, category_id, base_uom_id, track_inventory, has_variant)
VALUES ('SVC001', 'IT Consulting Hour', 5, 1, FALSE, FALSE);
```

**Behavior**:
- ✅ Can create sales orders
- ✅ Can generate invoices
- ❌ No stock ledger entries created
- ❌ No warehouse stock checking
- ✅ Performance: Faster transactions (no inventory calculations)

---

### Use Case 2: Simple Product with Inventory

**Scenario**: Retail store selling Coca-Cola bottles

**Setup**:
```sql
-- Product
INSERT INTO product (code, name, brand_id, base_uom_id, track_inventory, has_variant)
VALUES ('BEV001', 'Coca-Cola 500ml', 2, 1, TRUE, FALSE);

-- UOM Setup
INSERT INTO product_uom (product_id, uom_id, factor_to_base, is_base)
VALUES
  (1, 1, 1.0000, TRUE),      -- Piece (base)
  (1, 2, 6.0000, FALSE),     -- Pack of 6
  (1, 3, 24.0000, FALSE);    -- Carton of 24

-- UOM Conversions
INSERT INTO product_uom_conversion (product_id, from_uom_id, to_uom_id, factor)
VALUES
  (1, 2, 1, 6.0000),    -- Pack → Piece
  (1, 1, 2, 0.1667),    -- Piece → Pack
  (1, 3, 1, 24.0000),   -- Carton → Piece
  (1, 1, 3, 0.0417);    -- Piece → Carton
```

**Transaction**:
```
Purchase: 10 Cartons
  → Converted to: 240 Pieces (10 × 24)
  → Stock Balance: 240 Pieces

Sale: 3 Packs
  → Converted to: 18 Pieces (3 × 6)
  → Stock Balance: 222 Pieces (240 - 18)
```

---

### Use Case 3: Product with Variants (T-Shirt)

**Scenario**: Clothing store selling T-shirts in different colors and sizes

**Setup**:
```sql
-- Product
INSERT INTO product (code, name, category_id, base_uom_id, track_inventory, has_variant)
VALUES ('TS001', 'Basic T-Shirt', 10, 1, TRUE, TRUE);

-- Attributes
INSERT INTO attribute (name) VALUES ('Color'), ('Size');

-- Attribute Values
INSERT INTO attribute_value (attribute_id, value) VALUES
  (1, 'Red'), (1, 'Blue'), (1, 'Green'),
  (2, 'Small'), (2, 'Medium'), (2, 'Large');

-- Variants
INSERT INTO product_variant (product_id, sku, base_uom_id) VALUES
  (1, 'TS001-R-S', 1),  -- Red Small
  (1, 'TS001-R-M', 1),  -- Red Medium
  (1, 'TS001-R-L', 1),  -- Red Large
  (1, 'TS001-B-S', 1),  -- Blue Small
  (1, 'TS001-B-M', 1),  -- Blue Medium
  (1, 'TS001-B-L', 1);  -- Blue Large

-- Variant Attributes (for TS001-R-M)
INSERT INTO product_variant_attribute (variant_id, attribute_id, value_id) VALUES
  (2, 1, 1),  -- Color: Red
  (2, 2, 2);  -- Size: Medium
```

**Stock Management**:
```
Each variant has separate stock:
- TS001-R-S: 20 pcs
- TS001-R-M: 35 pcs
- TS001-R-L: 15 pcs
- TS001-B-S: 18 pcs
- TS001-B-M: 40 pcs
- TS001-B-L: 22 pcs

Total T-Shirt Stock: 150 pcs
```

---

### Use Case 4: Batch Tracking (Pharmaceuticals)

**Scenario**: Pharmacy selling medicines with expiry dates

**Setup**:
```sql
INSERT INTO product (code, name, category_id, base_uom_id, track_inventory, has_batch)
VALUES ('MED001', 'Paracetamol 500mg', 15, 1, TRUE, TRUE);
```

**Stock Receipt**:
```
Batch 001:
  - Quantity: 100 boxes
  - Mfg Date: 2025-01-01
  - Exp Date: 2027-01-01

Batch 002:
  - Quantity: 150 boxes
  - Mfg Date: 2025-06-01
  - Exp Date: 2027-06-01
```

**Stock Query**:
```sql
-- Get stock by batch
SELECT batch_no, SUM(quantity) as stock, expiry_date
FROM stock_ledger
WHERE product_id = 1
GROUP BY batch_no, expiry_date
ORDER BY expiry_date ASC;

Result:
Batch 001: 100 boxes (Exp: 2027-01-01) ← Use first (FEFO)
Batch 002: 150 boxes (Exp: 2027-06-01)
```

---

### Use Case 5: Serial Number Tracking (Electronics)

**Scenario**: Electronics store selling laptops with unique serial numbers

**Setup**:
```sql
INSERT INTO product (code, name, category_id, base_uom_id, track_inventory, has_serial)
VALUES ('ELEC001', 'Dell Laptop i7', 20, 1, TRUE, TRUE);
```

**Stock Receipt**:
```
Receipt of 3 Laptops:
  - Serial: SN12345 (Qty: 1)
  - Serial: SN12346 (Qty: 1)
  - Serial: SN12347 (Qty: 1)

Total Stock: 3 units
```

**Sale Transaction**:
```
Sale: 1 Laptop
  - Serial Number: SN12345
  - Qty: 1

Remaining Stock:
  - SN12346 (Available)
  - SN12347 (Available)
```

**Tracking**:
- Each serial number has complete traceability
- Purchase receipt → Storage location → Sale → Customer
- Warranty and support linked to serial number

---

### Use Case 6: Warehouse Hierarchy

**Scenario**: Retail chain with main warehouse and branches

**Setup**:
```sql
-- Main Warehouse
INSERT INTO warehouse (id, name, city, branch_type, is_main_warehouse, parent_warehouse_id)
VALUES (1, 'Central Warehouse', 'Bangkok', 'Main', TRUE, NULL);

-- Branch Warehouses
INSERT INTO warehouse (id, name, city, branch_type, is_main_warehouse, parent_warehouse_id)
VALUES
  (2, 'Branch Chiang Mai', 'Chiang Mai', 'Branch', FALSE, 1),
  (3, 'Branch Phuket', 'Phuket', 'Branch', FALSE, 1);

-- Sub Warehouse
INSERT INTO warehouse (id, name, city, branch_type, is_main_warehouse, parent_warehouse_id)
VALUES (4, 'Sub Warehouse CM1', 'Chiang Mai', 'Sub', FALSE, 2);
```

**Stock Flow**:
```
1. Supplier → Central Warehouse (Bangkok)
     ↓
2. Central Warehouse → Branch Chiang Mai
     ↓
3. Branch Chiang Mai → Sub Warehouse CM1
     ↓
4. Sale from Sub Warehouse CM1
```

**Business Rules**:
- ✅ Stock receipt only at Central Warehouse (is_main_warehouse = TRUE)
- ❌ Branch warehouses cannot receive stock from suppliers
- ✅ Stock transfers: Main → Branch → Sub
- ✅ Sales can occur from any active warehouse

---

## Audit Trail

All tables include comprehensive audit fields:

| Field | Type | Description |
|-------|------|-------------|
| created_on | TIMESTAMP | Record creation timestamp |
| modified_on | TIMESTAMP | Last modification timestamp |
| created_by | VARCHAR(50) | User who created record |
| modified_by | VARCHAR(50) | User who last modified record |
| last_action | VARCHAR(50) | Last action: CREATE, UPDATE, DELETE |

**Usage**:
- Track who created/modified records
- Audit compliance and regulatory requirements
- Troubleshooting and data investigation
- Soft delete support (update last_action = 'DELETE')

---

## Performance Optimization Tips

1. **Index Recommendations**:
   ```sql
   -- Product lookups
   CREATE INDEX idx_product_code ON product(code);
   CREATE INDEX idx_product_active ON product(active);
   CREATE INDEX idx_product_track_inventory ON product(track_inventory);

   -- Warehouse hierarchy
   CREATE INDEX idx_warehouse_parent ON warehouse(parent_warehouse_id);
   CREATE INDEX idx_warehouse_main ON warehouse(is_main_warehouse);

   -- Product variants
   CREATE INDEX idx_variant_product ON product_variant(product_id);
   CREATE INDEX idx_variant_sku ON product_variant(sku);

   -- Attribute lookups
   CREATE INDEX idx_variant_attr_variant ON product_variant_attribute(variant_id);
   ```

2. **Query Optimization**:
   - Filter by `active = TRUE` to exclude inactive records
   - Filter by `track_inventory = TRUE` for stock reports
   - Use covering indexes for frequent queries

3. **Data Archival**:
   - Archive old audit records periodically
   - Soft delete instead of hard delete (set active = FALSE)

---

## Summary

This database schema provides:

✅ **Flexible Warehouse Management**: Hierarchical structure with main/branch/sub warehouses
✅ **Comprehensive Product Management**: Groups, categories, brands, variants
✅ **Advanced Inventory Control**: Batch tracking, serial tracking, optional inventory
✅ **Multi-UOM Support**: Flexible unit conversions
✅ **Performance Optimized**: Skip inventory tracking for non-stock items
✅ **Full Audit Trail**: Complete change tracking
✅ **Scalable Architecture**: Supports complex business scenarios

This design supports small to medium-sized ERP implementations with room for growth and customization.

---

**Document Version**: 1.0
**Last Updated**: 2026-02-07
**Author**: SmallBiz ERP Development Team
