# Product Master Setup Guide

## Overview

This guide explains how to set up products in the ERP system, including groups, categories, brands, and product master data.

---

## Product Hierarchy

```
Product Group (Food & Beverages)
    │
    ├─── Product Category (Soft Drinks)
    │       │
    │       ├─── Product Brand (Coca-Cola)
    │       │       │
    │       │       └─── Product (Coca-Cola 500ml)
    │       │
    │       └─── Product Brand (Pepsi)
    │               └─── Product (Pepsi 500ml)
    │
    └─── Product Category (Snacks)
            │
            └─── Product Brand (Lays)
                    └─── Product (Lays Classic 50g)
```

---

## Setup Steps

### Step 1: Create Product Groups

Product groups are high-level categories that organize products.

```sql
INSERT INTO product_group (name, description, active, created_by, last_action)
VALUES
    ('Food & Beverages', 'All food and beverage items', TRUE, 'admin', 'CREATE'),
    ('Electronics', 'Electronic devices and accessories', TRUE, 'admin', 'CREATE'),
    ('Clothing & Apparel', 'Clothing and fashion items', TRUE, 'admin', 'CREATE'),
    ('Office Supplies', 'Office and stationery items', TRUE, 'admin', 'CREATE'),
    ('Services', 'Service offerings', TRUE, 'admin', 'CREATE');
```

**Examples**:
- Food & Beverages
- Electronics
- Clothing & Apparel
- Pharmaceuticals
- Automotive Parts
- Office Supplies
- Services

---

### Step 2: Create Product Categories

Categories provide detailed classification within groups.

```sql
INSERT INTO product_category (group_id, name, description, active, created_by, last_action)
VALUES
    -- Food & Beverages (group_id = 1)
    (1, 'Soft Drinks', 'Carbonated and non-carbonated drinks', TRUE, 'admin', 'CREATE'),
    (1, 'Snacks', 'Chips, crackers, and snack items', TRUE, 'admin', 'CREATE'),
    (1, 'Dairy Products', 'Milk, cheese, yogurt', TRUE, 'admin', 'CREATE'),
    (1, 'Bakery', 'Bread, cakes, pastries', TRUE, 'admin', 'CREATE'),

    -- Electronics (group_id = 2)
    (2, 'Mobile Phones', 'Smartphones and accessories', TRUE, 'admin', 'CREATE'),
    (2, 'Laptops', 'Laptops and notebooks', TRUE, 'admin', 'CREATE'),
    (2, 'Accessories', 'Cables, chargers, cases', TRUE, 'admin', 'CREATE'),

    -- Clothing (group_id = 3)
    (3, 'T-Shirts', 'Casual t-shirts', TRUE, 'admin', 'CREATE'),
    (3, 'Jeans', 'Denim jeans', TRUE, 'admin', 'CREATE'),
    (3, 'Shoes', 'Footwear', TRUE, 'admin', 'CREATE');
```

---

### Step 3: Create Product Brands

Brands identify manufacturers or product lines.

```sql
INSERT INTO product_brand (name, description, active, created_by, last_action)
VALUES
    -- Beverage Brands
    ('Coca-Cola', 'The Coca-Cola Company', TRUE, 'admin', 'CREATE'),
    ('Pepsi', 'PepsiCo', TRUE, 'admin', 'CREATE'),
    ('Nestle', 'Nestle Group', TRUE, 'admin', 'CREATE'),

    -- Electronics Brands
    ('Apple', 'Apple Inc.', TRUE, 'admin', 'CREATE'),
    ('Samsung', 'Samsung Electronics', TRUE, 'admin', 'CREATE'),
    ('Dell', 'Dell Technologies', TRUE, 'admin', 'CREATE'),

    -- Clothing Brands
    ('Nike', 'Nike Inc.', TRUE, 'admin', 'CREATE'),
    ('Adidas', 'Adidas AG', TRUE, 'admin', 'CREATE'),
    ('Levi''s', 'Levi Strauss & Co.', TRUE, 'admin', 'CREATE'),

    -- Generic/No Brand
    ('Generic', 'Generic products', TRUE, 'admin', 'CREATE');
```

---

### Step 4: Create UOM (Unit of Measure)

```sql
INSERT INTO uom (name, symbol, active, created_by, last_action)
VALUES
    ('Piece', 'pc', TRUE, 'admin', 'CREATE'),
    ('Kilogram', 'kg', TRUE, 'admin', 'CREATE'),
    ('Gram', 'g', TRUE, 'admin', 'CREATE'),
    ('Liter', 'L', TRUE, 'admin', 'CREATE'),
    ('Milliliter', 'ml', TRUE, 'admin', 'CREATE'),
    ('Box', 'box', TRUE, 'admin', 'CREATE'),
    ('Pack', 'pack', TRUE, 'admin', 'CREATE'),
    ('Carton', 'ctn', TRUE, 'admin', 'CREATE'),
    ('Dozen', 'dz', TRUE, 'admin', 'CREATE'),
    ('Meter', 'm', TRUE, 'admin', 'CREATE');
```

---

### Step 5: Create Products

#### Example 1: Simple Product (Inventory Tracked)

```sql
INSERT INTO product (
    code,
    name,
    group_id,
    category_id,
    brand_id,
    base_uom_id,
    track_inventory,
    has_variant,
    has_batch,
    has_serial,
    reorder_level,
    allow_negative_stock,
    active,
    created_by,
    last_action
)
VALUES (
    'BEV001',                  -- Product code
    'Coca-Cola 500ml',         -- Product name
    1,                         -- Food & Beverages group
    1,                         -- Soft Drinks category
    1,                         -- Coca-Cola brand
    1,                         -- Piece (base UOM)
    TRUE,                      -- Track inventory
    FALSE,                     -- No variants
    FALSE,                     -- No batch tracking
    FALSE,                     -- No serial tracking
    100,                       -- Reorder at 100 pieces
    FALSE,                     -- Don't allow negative stock
    TRUE,                      -- Active
    'admin',
    'CREATE'
);
```

#### Example 2: Service Product (No Inventory)

```sql
INSERT INTO product (
    code,
    name,
    group_id,
    category_id,
    brand_id,
    base_uom_id,
    track_inventory,
    has_variant,
    has_batch,
    has_serial,
    active,
    created_by,
    last_action
)
VALUES (
    'SVC001',
    'IT Consulting - Hourly',
    5,                         -- Services group
    NULL,                      -- No category
    NULL,                      -- No brand
    1,                         -- Hour (UOM)
    FALSE,                     -- DON'T track inventory
    FALSE,
    FALSE,
    FALSE,
    TRUE,
    'admin',
    'CREATE'
);
```

#### Example 3: Product with Batch Tracking

```sql
INSERT INTO product (
    code,
    name,
    group_id,
    category_id,
    brand_id,
    base_uom_id,
    track_inventory,
    has_variant,
    has_batch,                 -- Batch tracking enabled
    has_serial,
    reorder_level,
    active,
    created_by,
    last_action
)
VALUES (
    'MED001',
    'Paracetamol 500mg',
    6,                         -- Pharmaceuticals
    15,                        -- Tablets category
    10,                        -- Generic brand
    6,                         -- Box
    TRUE,
    FALSE,
    TRUE,                      -- Batch tracking for expiry dates
    FALSE,
    50,
    TRUE,
    'admin',
    'CREATE'
);
```

#### Example 4: Product with Serial Tracking

```sql
INSERT INTO product (
    code,
    name,
    group_id,
    category_id,
    brand_id,
    base_uom_id,
    track_inventory,
    has_variant,
    has_batch,
    has_serial,                -- Serial tracking enabled
    reorder_level,
    active,
    created_by,
    last_action
)
VALUES (
    'ELEC001',
    'Dell Laptop Inspiron 15',
    2,                         -- Electronics
    6,                         -- Laptops category
    6,                         -- Dell brand
    1,                         -- Piece
    TRUE,
    FALSE,
    FALSE,
    TRUE,                      -- Each laptop has unique serial number
    5,
    TRUE,
    'admin',
    'CREATE'
);
```

---

## Product Configuration Matrix

| Product Type | track_inventory | has_variant | has_batch | has_serial | Use Case |
|-------------|----------------|-------------|-----------|-----------|----------|
| Simple Product | TRUE | FALSE | FALSE | FALSE | Regular stocked items (soft drinks, snacks) |
| Service | FALSE | FALSE | FALSE | FALSE | Consulting, subscriptions, digital products |
| Variant Product | TRUE | TRUE | FALSE | FALSE | Clothing (sizes/colors), multi-flavor items |
| Batch Tracked | TRUE | FALSE | TRUE | FALSE | Pharmaceuticals, perishables with expiry |
| Serial Tracked | TRUE | FALSE | FALSE | TRUE | Electronics, high-value items |
| Batch + Serial | TRUE | FALSE | TRUE | TRUE | Medical devices (rare combination) |
| Variant + Batch | TRUE | TRUE | TRUE | FALSE | Food products with flavors and batches |

---

## UOM Setup for Products

After creating a product, configure its UOMs and conversions.

### Example: Coca-Cola with Multiple UOMs

```sql
-- Base UOM: Piece
INSERT INTO product_uom (product_id, uom_id, factor_to_base, is_base, active, created_by, last_action)
VALUES
    (1, 1, 1.0000, TRUE, TRUE, 'admin', 'CREATE'),      -- Piece (base)
    (1, 7, 6.0000, FALSE, TRUE, 'admin', 'CREATE'),     -- Pack of 6
    (1, 8, 24.0000, FALSE, TRUE, 'admin', 'CREATE');    -- Carton of 24

-- UOM Conversions
INSERT INTO product_uom_conversion (product_id, from_uom_id, to_uom_id, factor, active, created_by, last_action)
VALUES
    -- Piece ↔ Pack
    (1, 1, 7, 0.1667, TRUE, 'admin', 'CREATE'),  -- 1 Piece = 0.1667 Pack
    (1, 7, 1, 6.0000, TRUE, 'admin', 'CREATE'),  -- 1 Pack = 6 Pieces

    -- Piece ↔ Carton
    (1, 1, 8, 0.0417, TRUE, 'admin', 'CREATE'),  -- 1 Piece = 0.0417 Carton
    (1, 8, 1, 24.0000, TRUE, 'admin', 'CREATE'), -- 1 Carton = 24 Pieces

    -- Pack ↔ Carton
    (1, 7, 8, 0.2500, TRUE, 'admin', 'CREATE'),  -- 1 Pack = 0.25 Carton
    (1, 8, 7, 4.0000, TRUE, 'admin', 'CREATE');  -- 1 Carton = 4 Packs
```

**Conversion Table**:
```
1 Carton = 4 Packs = 24 Pieces
1 Pack = 6 Pieces
1 Piece = 1 Piece (base)
```

---

## Product Validation Queries

### Check Product Configuration
```sql
SELECT
    p.code,
    p.name,
    pg.name AS group_name,
    pc.name AS category_name,
    pb.name AS brand_name,
    u.name AS base_uom,
    p.track_inventory,
    p.has_variant,
    p.has_batch,
    p.has_serial,
    p.active
FROM product p
LEFT JOIN product_group pg ON p.group_id = pg.id
LEFT JOIN product_category pc ON p.category_id = pc.id
LEFT JOIN product_brand pb ON p.brand_id = pb.id
LEFT JOIN uom u ON p.base_uom_id = u.id
WHERE p.active = TRUE
ORDER BY p.code;
```

### Find All Inventory-Tracked Products
```sql
SELECT code, name
FROM product
WHERE track_inventory = TRUE
AND active = TRUE;
```

### Find All Service Products (Non-Inventory)
```sql
SELECT code, name
FROM product
WHERE track_inventory = FALSE
AND active = TRUE;
```

### Find Products with Variants
```sql
SELECT code, name
FROM product
WHERE has_variant = TRUE
AND active = TRUE;
```

### Check Product UOM Configuration
```sql
SELECT
    p.code,
    p.name,
    u.name AS uom_name,
    pu.factor_to_base,
    pu.is_base
FROM product p
INNER JOIN product_uom pu ON p.id = pu.product_id
INNER JOIN uom u ON pu.uom_id = u.id
WHERE p.id = 1
AND pu.active = TRUE
ORDER BY pu.is_base DESC, pu.factor_to_base;
```

---

## Common Setup Scenarios

### Scenario 1: Retail Store (Simple Products)
```sql
-- Soft drinks, snacks, groceries
-- track_inventory = TRUE, has_variant = FALSE
INSERT INTO product (code, name, category_id, brand_id, base_uom_id, track_inventory, has_variant)
VALUES
    ('BEV001', 'Coca-Cola 500ml', 1, 1, 1, TRUE, FALSE),
    ('SNK001', 'Lays Classic 50g', 2, 8, 1, TRUE, FALSE),
    ('DRY001', 'Nestle Milk 1L', 3, 3, 1, TRUE, FALSE);
```

### Scenario 2: Clothing Store (Variant Products)
```sql
-- T-shirts with sizes and colors
-- track_inventory = TRUE, has_variant = TRUE
INSERT INTO product (code, name, category_id, brand_id, base_uom_id, track_inventory, has_variant)
VALUES
    ('TS001', 'Basic T-Shirt', 8, 7, 1, TRUE, TRUE),
    ('JN001', 'Slim Fit Jeans', 9, 9, 1, TRUE, TRUE);

-- Variants will be created in separate step (see 03-PRODUCT-VARIANTS-SETUP.md)
```

### Scenario 3: Pharmacy (Batch Tracking)
```sql
-- Medicines with expiry dates
-- track_inventory = TRUE, has_batch = TRUE
INSERT INTO product (code, name, category_id, brand_id, base_uom_id, track_inventory, has_batch)
VALUES
    ('MED001', 'Paracetamol 500mg', 15, 10, 6, TRUE, TRUE),
    ('MED002', 'Ibuprofen 400mg', 15, 10, 6, TRUE, TRUE);
```

### Scenario 4: Electronics Store (Serial Tracking)
```sql
-- High-value electronics
-- track_inventory = TRUE, has_serial = TRUE
INSERT INTO product (code, name, category_id, brand_id, base_uom_id, track_inventory, has_serial)
VALUES
    ('ELEC001', 'Dell Laptop i7', 6, 6, 1, TRUE, TRUE),
    ('ELEC002', 'iPhone 15 Pro', 5, 4, 1, TRUE, TRUE);
```

### Scenario 5: Service Company
```sql
-- Services without inventory
-- track_inventory = FALSE
INSERT INTO product (code, name, group_id, base_uom_id, track_inventory)
VALUES
    ('SVC001', 'IT Consulting Hour', 5, 1, FALSE),
    ('SVC002', 'Software License - Annual', 5, 1, FALSE);
```

---

## Best Practices

1. **Use Consistent Product Codes**
   - Format: `[CATEGORY][NUMBER]` (e.g., BEV001, SNK001)
   - Or: `[BRAND][CATEGORY][NUMBER]` (e.g., COKE-BEV-001)

2. **Enable Inventory Tracking Selectively**
   - `track_inventory = TRUE`: Physical products requiring stock management
   - `track_inventory = FALSE`: Services, subscriptions, digital products

3. **Use Batch Tracking for Perishables**
   - Pharmaceuticals (expiry dates)
   - Food products (manufacturing/expiry dates)
   - Chemicals (lot numbers)

4. **Use Serial Tracking for High-Value Items**
   - Electronics (warranty tracking)
   - Vehicles (VIN numbers)
   - Equipment (asset tracking)

5. **Configure Reorder Levels**
   - Set based on lead time and average consumption
   - Example: If you sell 50 units/week and lead time is 2 weeks, set reorder_level = 100

6. **Use Product Groups and Categories**
   - Organize products logically
   - Enables better reporting and analysis
   - Helps with navigation in UI

7. **Maintain Product Brands**
   - Even if using "Generic" brand, create a brand record
   - Enables brand-based reporting

---

## Troubleshooting

### Issue: Cannot sell product below zero
**Cause**: `allow_negative_stock = FALSE`

**Solution**:
```sql
-- Allow negative stock (use with caution)
UPDATE product
SET allow_negative_stock = TRUE
WHERE id = 1;
```

### Issue: Product not showing in stock reports
**Cause**: `track_inventory = FALSE`

**Solution**:
```sql
-- Enable inventory tracking
UPDATE product
SET track_inventory = TRUE
WHERE id = 1;
```

### Issue: Missing UOM conversions
**Cause**: UOM conversions not configured

**Solution**:
```sql
-- Add missing conversions
INSERT INTO product_uom_conversion (product_id, from_uom_id, to_uom_id, factor)
VALUES (1, 7, 1, 6.0000);  -- Pack to Piece
```

---

**Last Updated**: 2026-02-07
**Version**: 1.0
