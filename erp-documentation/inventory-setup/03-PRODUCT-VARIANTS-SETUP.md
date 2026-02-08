# Product Variants Setup Guide

## Overview

This guide explains how to set up product variants for products that come in multiple options (colors, sizes, flavors, etc.).

Product variants enable you to:
- Track separate stock levels for each variant
- Generate unique SKUs for each combination
- Price variants differently
- Report on variant-specific sales

---

## When to Use Product Variants

### Use Variants When:
- ✅ Product comes in multiple sizes (Small, Medium, Large)
- ✅ Product has different colors (Red, Blue, Green)
- ✅ Product has different flavors (Chocolate, Vanilla, Strawberry)
- ✅ Product has different materials (Cotton, Polyester, Silk)
- ✅ You need separate stock tracking for each option
- ✅ Each combination has different cost/price

### Don't Use Variants When:
- ❌ Product is completely different (use separate products)
- ❌ Difference is minor and doesn't affect stock (use product description)
- ❌ Only one option available

---

## Variant Setup Process

```
Step 1: Create Attributes
   ↓
Step 2: Create Attribute Values
   ↓
Step 3: Create Product (with has_variant = TRUE)
   ↓
Step 4: Create Product Variants
   ↓
Step 5: Link Variant Attributes
```

---

## Setup Steps

### Step 1: Create Attributes

Attributes define the characteristics that differentiate variants.

```sql
INSERT INTO attribute (name, active, created_by, last_action)
VALUES
    ('Color', TRUE, 'admin', 'CREATE'),
    ('Size', TRUE, 'admin', 'CREATE'),
    ('Flavor', TRUE, 'admin', 'CREATE'),
    ('Material', TRUE, 'admin', 'CREATE'),
    ('Weight', TRUE, 'admin', 'CREATE'),
    ('Volume', TRUE, 'admin', 'CREATE');
```

**Common Attributes**:
- Color (for clothing, electronics)
- Size (for clothing, shoes, portions)
- Flavor (for food, beverages)
- Material (for clothing, furniture)
- Weight (for packaged goods)
- Volume (for liquids)
- Style (for clothing, accessories)
- Pattern (for fabrics, wallpapers)

---

### Step 2: Create Attribute Values

Values are the specific options for each attribute.

```sql
-- Color Values
INSERT INTO attribute_value (attribute_id, value, active, created_by, last_action)
VALUES
    (1, 'Red', TRUE, 'admin', 'CREATE'),
    (1, 'Blue', TRUE, 'admin', 'CREATE'),
    (1, 'Green', TRUE, 'admin', 'CREATE'),
    (1, 'Black', TRUE, 'admin', 'CREATE'),
    (1, 'White', TRUE, 'admin', 'CREATE'),
    (1, 'Yellow', TRUE, 'admin', 'CREATE');

-- Size Values
INSERT INTO attribute_value (attribute_id, value, active, created_by, last_action)
VALUES
    (2, 'XS', TRUE, 'admin', 'CREATE'),
    (2, 'Small', TRUE, 'admin', 'CREATE'),
    (2, 'Medium', TRUE, 'admin', 'CREATE'),
    (2, 'Large', TRUE, 'admin', 'CREATE'),
    (2, 'XL', TRUE, 'admin', 'CREATE'),
    (2, 'XXL', TRUE, 'admin', 'CREATE');

-- Flavor Values
INSERT INTO attribute_value (attribute_id, value, active, created_by, last_action)
VALUES
    (3, 'Chocolate', TRUE, 'admin', 'CREATE'),
    (3, 'Vanilla', TRUE, 'admin', 'CREATE'),
    (3, 'Strawberry', TRUE, 'admin', 'CREATE'),
    (3, 'Mint', TRUE, 'admin', 'CREATE'),
    (3, 'Caramel', TRUE, 'admin', 'CREATE');

-- Material Values
INSERT INTO attribute_value (attribute_id, value, active, created_by, last_action)
VALUES
    (4, 'Cotton', TRUE, 'admin', 'CREATE'),
    (4, 'Polyester', TRUE, 'admin', 'CREATE'),
    (4, 'Silk', TRUE, 'admin', 'CREATE'),
    (4, 'Wool', TRUE, 'admin', 'CREATE'),
    (4, 'Leather', TRUE, 'admin', 'CREATE');
```

---

### Step 3: Create Base Product

The base product must have `has_variant = TRUE`.

```sql
INSERT INTO product (
    code,
    name,
    group_id,
    category_id,
    brand_id,
    base_uom_id,
    track_inventory,
    has_variant,              -- Important: Set to TRUE
    active,
    created_by,
    last_action
)
VALUES (
    'TS001',
    'Basic T-Shirt',
    3,                        -- Clothing & Apparel
    8,                        -- T-Shirts category
    7,                        -- Nike brand
    1,                        -- Piece
    TRUE,
    TRUE,                     -- Has variants
    TRUE,
    'admin',
    'CREATE'
);
```

---

### Step 4: Create Product Variants

Create a variant for each combination of attributes.

**Example: T-Shirt with Color and Size**

```sql
-- Red Small
INSERT INTO product_variant (product_id, sku, base_uom_id, track_inventory, active, created_by, last_action)
VALUES (1, 'TS001-R-S', 1, TRUE, TRUE, 'admin', 'CREATE');

-- Red Medium
INSERT INTO product_variant (product_id, sku, base_uom_id, track_inventory, active, created_by, last_action)
VALUES (1, 'TS001-R-M', 1, TRUE, TRUE, 'admin', 'CREATE');

-- Red Large
INSERT INTO product_variant (product_id, sku, base_uom_id, track_inventory, active, created_by, last_action)
VALUES (1, 'TS001-R-L', 1, TRUE, TRUE, 'admin', 'CREATE');

-- Blue Small
INSERT INTO product_variant (product_id, sku, base_uom_id, track_inventory, active, created_by, last_action)
VALUES (1, 'TS001-B-S', 1, TRUE, TRUE, 'admin', 'CREATE');

-- Blue Medium
INSERT INTO product_variant (product_id, sku, base_uom_id, track_inventory, active, created_by, last_action)
VALUES (1, 'TS001-B-M', 1, TRUE, TRUE, 'admin', 'CREATE');

-- Blue Large
INSERT INTO product_variant (product_id, sku, base_uom_id, track_inventory, active, created_by, last_action)
VALUES (1, 'TS001-B-L', 1, TRUE, TRUE, 'admin', 'CREATE');
```

**SKU Naming Convention**:
- Format: `[PRODUCT_CODE]-[ATTR1]-[ATTR2]-[ATTR3]`
- Example: `TS001-R-M` = T-Shirt 001, Red, Medium
- Example: `SH001-BLK-42` = Shoe 001, Black, Size 42
- Example: `IC001-VAN-500ML` = Ice Cream 001, Vanilla, 500ml

---

### Step 5: Link Variant Attributes

Connect each variant to its attribute values.

```sql
-- TS001-R-S (Red, Small) - variant_id = 1
INSERT INTO product_variant_attribute (variant_id, attribute_id, value_id, created_by, last_action)
VALUES
    (1, 1, 1, 'admin', 'CREATE'),  -- Color: Red (attribute_id=1, value_id=1)
    (1, 2, 2, 'admin', 'CREATE');  -- Size: Small (attribute_id=2, value_id=2)

-- TS001-R-M (Red, Medium) - variant_id = 2
INSERT INTO product_variant_attribute (variant_id, attribute_id, value_id, created_by, last_action)
VALUES
    (2, 1, 1, 'admin', 'CREATE'),  -- Color: Red
    (2, 2, 3, 'admin', 'CREATE');  -- Size: Medium

-- TS001-R-L (Red, Large) - variant_id = 3
INSERT INTO product_variant_attribute (variant_id, attribute_id, value_id, created_by, last_action)
VALUES
    (3, 1, 1, 'admin', 'CREATE'),  -- Color: Red
    (3, 2, 4, 'admin', 'CREATE');  -- Size: Large

-- TS001-B-S (Blue, Small) - variant_id = 4
INSERT INTO product_variant_attribute (variant_id, attribute_id, value_id, created_by, last_action)
VALUES
    (4, 1, 2, 'admin', 'CREATE'),  -- Color: Blue
    (4, 2, 2, 'admin', 'CREATE');  -- Size: Small

-- TS001-B-M (Blue, Medium) - variant_id = 5
INSERT INTO product_variant_attribute (variant_id, attribute_id, value_id, created_by, last_action)
VALUES
    (5, 1, 2, 'admin', 'CREATE'),  -- Color: Blue
    (5, 2, 3, 'admin', 'CREATE');  -- Size: Medium

-- TS001-B-L (Blue, Large) - variant_id = 6
INSERT INTO product_variant_attribute (variant_id, attribute_id, value_id, created_by, last_action)
VALUES
    (6, 1, 2, 'admin', 'CREATE'),  -- Color: Blue
    (6, 2, 4, 'admin', 'CREATE');  -- Size: Large
```

---

## Variant Matrix

### Example: T-Shirt with 2 Colors × 3 Sizes = 6 Variants

|         | Red | Blue |
|---------|-----|------|
| Small   | TS001-R-S | TS001-B-S |
| Medium  | TS001-R-M | TS001-B-M |
| Large   | TS001-R-L | TS001-B-L |

### Example: Ice Cream with 3 Flavors × 2 Sizes = 6 Variants

|         | Chocolate | Vanilla | Strawberry |
|---------|-----------|---------|------------|
| 500ml   | IC001-CHO-500 | IC001-VAN-500 | IC001-STR-500 |
| 1000ml  | IC001-CHO-1000 | IC001-VAN-1000 | IC001-STR-1000 |

---

## Complete Example: Clothing Store T-Shirt

### SQL Script

```sql
-- 1. Create Attributes (if not exists)
INSERT INTO attribute (id, name, active) VALUES
    (1, 'Color', TRUE),
    (2, 'Size', TRUE);

-- 2. Create Attribute Values
INSERT INTO attribute_value (id, attribute_id, value, active) VALUES
    -- Colors
    (1, 1, 'Red', TRUE),
    (2, 1, 'Blue', TRUE),
    (3, 1, 'Green', TRUE),

    -- Sizes
    (4, 2, 'Small', TRUE),
    (5, 2, 'Medium', TRUE),
    (6, 2, 'Large', TRUE);

-- 3. Create Base Product
INSERT INTO product (id, code, name, category_id, base_uom_id, track_inventory, has_variant, active)
VALUES (1, 'TS001', 'Basic T-Shirt', 8, 1, TRUE, TRUE, TRUE);

-- 4. Create Variants (3 colors × 3 sizes = 9 variants)
INSERT INTO product_variant (id, product_id, sku, base_uom_id, track_inventory, active) VALUES
    -- Red variants
    (1, 1, 'TS001-R-S', 1, TRUE, TRUE),
    (2, 1, 'TS001-R-M', 1, TRUE, TRUE),
    (3, 1, 'TS001-R-L', 1, TRUE, TRUE),

    -- Blue variants
    (4, 1, 'TS001-B-S', 1, TRUE, TRUE),
    (5, 1, 'TS001-B-M', 1, TRUE, TRUE),
    (6, 1, 'TS001-B-L', 1, TRUE, TRUE),

    -- Green variants
    (7, 1, 'TS001-G-S', 1, TRUE, TRUE),
    (8, 1, 'TS001-G-M', 1, TRUE, TRUE),
    (9, 1, 'TS001-G-L', 1, TRUE, TRUE);

-- 5. Link Variant Attributes
INSERT INTO product_variant_attribute (variant_id, attribute_id, value_id) VALUES
    -- Red Small
    (1, 1, 1), (1, 2, 4),
    -- Red Medium
    (2, 1, 1), (2, 2, 5),
    -- Red Large
    (3, 1, 1), (3, 2, 6),

    -- Blue Small
    (4, 1, 2), (4, 2, 4),
    -- Blue Medium
    (5, 1, 2), (5, 2, 5),
    -- Blue Large
    (6, 1, 2), (6, 2, 6),

    -- Green Small
    (7, 1, 3), (7, 2, 4),
    -- Green Medium
    (8, 1, 3), (8, 2, 5),
    -- Green Large
    (9, 1, 3), (9, 2, 6);
```

---

## Variant Queries

### Get All Variants for a Product

```sql
SELECT
    p.code AS product_code,
    p.name AS product_name,
    pv.sku,
    pv.track_inventory,
    pv.active
FROM product p
INNER JOIN product_variant pv ON p.id = pv.product_id
WHERE p.id = 1
AND pv.active = TRUE
ORDER BY pv.sku;
```

### Get Variant with Attributes (Detailed View)

```sql
SELECT
    p.code AS product_code,
    p.name AS product_name,
    pv.sku,
    a.name AS attribute_name,
    av.value AS attribute_value
FROM product p
INNER JOIN product_variant pv ON p.id = pv.product_id
INNER JOIN product_variant_attribute pva ON pv.id = pva.variant_id
INNER JOIN attribute a ON pva.attribute_id = a.id
INNER JOIN attribute_value av ON pva.value_id = av.id
WHERE p.id = 1
AND pv.active = TRUE
ORDER BY pv.sku, a.name;
```

**Result**:
```
product_code | product_name     | sku        | attribute_name | attribute_value
-------------|------------------|------------|----------------|----------------
TS001        | Basic T-Shirt    | TS001-R-S  | Color          | Red
TS001        | Basic T-Shirt    | TS001-R-S  | Size           | Small
TS001        | Basic T-Shirt    | TS001-R-M  | Color          | Red
TS001        | Basic T-Shirt    | TS001-R-M  | Size           | Medium
...
```

### Get Variant with Attributes (Pivoted View)

```sql
SELECT
    pv.sku,
    MAX(CASE WHEN a.name = 'Color' THEN av.value END) AS color,
    MAX(CASE WHEN a.name = 'Size' THEN av.value END) AS size,
    MAX(CASE WHEN a.name = 'Flavor' THEN av.value END) AS flavor
FROM product_variant pv
INNER JOIN product_variant_attribute pva ON pv.id = pva.variant_id
INNER JOIN attribute a ON pva.attribute_id = a.id
INNER JOIN attribute_value av ON pva.value_id = av.id
WHERE pv.product_id = 1
AND pv.active = TRUE
GROUP BY pv.sku
ORDER BY pv.sku;
```

**Result**:
```
sku        | color | size   | flavor
-----------|-------|--------|--------
TS001-R-S  | Red   | Small  | NULL
TS001-R-M  | Red   | Medium | NULL
TS001-B-S  | Blue  | Small  | NULL
...
```

### Find Variant by Attribute Combination

```sql
-- Find: Red, Medium T-Shirt
SELECT pv.sku, pv.id
FROM product_variant pv
WHERE pv.product_id = 1
AND EXISTS (
    SELECT 1 FROM product_variant_attribute pva
    INNER JOIN attribute_value av ON pva.value_id = av.id
    WHERE pva.variant_id = pv.id
    AND av.value = 'Red'
)
AND EXISTS (
    SELECT 1 FROM product_variant_attribute pva
    INNER JOIN attribute_value av ON pva.value_id = av.id
    WHERE pva.variant_id = pv.id
    AND av.value = 'Medium'
);
```

---

## Stock Management with Variants

### Stock Ledger Entry for Variant

When receiving/selling variants, reference the variant_id (SKU).

```sql
-- Example stock receipt for TS001-R-M (Red, Medium)
INSERT INTO stock_ledger (
    warehouse_id,
    product_id,
    variant_id,        -- Important: Reference variant
    quantity,
    transaction_type,
    reference_no
)
VALUES (
    1,                 -- Main warehouse
    1,                 -- TS001 (base product)
    2,                 -- TS001-R-M (Red Medium variant)
    100,               -- Quantity
    'RECEIPT',
    'PO-001'
);
```

### Stock Balance by Variant

```sql
SELECT
    pv.sku,
    w.name AS warehouse,
    SUM(sl.quantity) AS stock_balance
FROM stock_ledger sl
INNER JOIN product_variant pv ON sl.variant_id = pv.id
INNER JOIN warehouse w ON sl.warehouse_id = w.id
WHERE sl.product_id = 1
GROUP BY pv.sku, w.name
ORDER BY pv.sku;
```

**Result**:
```
sku        | warehouse              | stock_balance
-----------|------------------------|---------------
TS001-R-S  | Central Warehouse      | 50
TS001-R-M  | Central Warehouse      | 100
TS001-R-L  | Central Warehouse      | 75
TS001-B-S  | Branch Chiang Mai      | 30
TS001-B-M  | Branch Chiang Mai      | 60
...
```

---

## Variant Combinations Calculator

### 2 Attributes
- Colors: 3 options (Red, Blue, Green)
- Sizes: 3 options (Small, Medium, Large)
- **Total Variants**: 3 × 3 = **9 variants**

### 3 Attributes
- Colors: 3 options
- Sizes: 3 options
- Materials: 2 options (Cotton, Polyester)
- **Total Variants**: 3 × 3 × 2 = **18 variants**

### 4 Attributes
- Colors: 3 options
- Sizes: 4 options (S, M, L, XL)
- Materials: 2 options
- Patterns: 2 options (Plain, Striped)
- **Total Variants**: 3 × 4 × 2 × 2 = **48 variants**

---

## Best Practices

1. **Use Consistent SKU Format**
   - Define a standard: `[PRODUCT]-[ATTR1]-[ATTR2]`
   - Example: `TS001-R-M`, `SH001-BLK-42`, `IC001-VAN-1L`

2. **Create All Attribute Values First**
   - Set up complete attribute library before creating variants
   - Easier to assign values later

3. **Don't Over-Complicate**
   - Use 2-3 attributes maximum
   - Too many attributes = too many variants
   - Consider if all combinations will actually be sold

4. **Inactive Variants**
   - Don't delete discontinued variants (historical data)
   - Set `active = FALSE` instead

5. **Separate Stock for Each Variant**
   - Each variant has independent stock
   - Report stock by variant SKU

6. **Use Variants for Stock Differentiation**
   - Only use variants if stock must be tracked separately
   - If stock is pooled, use single product with description

7. **Consider Pricing**
   - Variants can have different prices
   - Link pricing table to variant_id

---

## Common Scenarios

### Scenario 1: Clothing Store
```
Product: T-Shirt
Attributes: Color, Size
Variants: 12 (4 colors × 3 sizes)
```

### Scenario 2: Ice Cream Shop
```
Product: Ice Cream Cup
Attributes: Flavor, Size
Variants: 6 (3 flavors × 2 sizes)
```

### Scenario 3: Electronics Accessories
```
Product: Phone Case
Attributes: Color, Phone Model
Variants: 15 (5 colors × 3 models)
```

### Scenario 4: Beverage Company
```
Product: Energy Drink
Attributes: Flavor, Volume
Variants: 9 (3 flavors × 3 volumes: 250ml, 500ml, 1L)
```

---

## Troubleshooting

### Issue: Too Many Variants Created
**Cause**: Over-specification of attributes

**Solution**:
- Review if all combinations are necessary
- Mark unused variants as inactive
- Consider reducing attribute options

### Issue: Cannot Find Specific Variant
**Cause**: SKU naming inconsistent

**Solution**:
- Use standardized SKU format
- Create SKU lookup index
- Use variant attribute query (see queries above)

### Issue: Stock Showing for Wrong Variant
**Cause**: Incorrect variant_id in stock ledger

**Solution**:
```sql
-- Verify stock entries
SELECT * FROM stock_ledger
WHERE product_id = 1
AND variant_id IS NOT NULL
ORDER BY created_on DESC;
```

---

**Last Updated**: 2026-02-07
**Version**: 1.0
