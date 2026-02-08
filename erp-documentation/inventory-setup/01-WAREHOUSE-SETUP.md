# Warehouse Setup Guide

## Overview

This guide explains how to set up warehouses in the ERP system with proper hierarchy and configuration.

## Warehouse Types

### 1. Main Warehouse
- **Purpose**: Primary warehouse that receives stock from suppliers
- **Configuration**: `is_main_warehouse = TRUE`, `parent_warehouse_id = NULL`
- **Capabilities**:
  - ✅ Receive stock from suppliers (Purchase Orders)
  - ✅ Transfer stock to branch warehouses
  - ✅ Process sales orders
  - ✅ Stock adjustments

### 2. Branch Warehouse
- **Purpose**: Regional distribution centers
- **Configuration**: `is_main_warehouse = FALSE`, `parent_warehouse_id = [main_warehouse_id]`
- **Capabilities**:
  - ❌ Cannot receive stock from suppliers directly
  - ✅ Receive stock transfers from main warehouse
  - ✅ Transfer stock to sub-warehouses
  - ✅ Process sales orders
  - ✅ Stock adjustments

### 3. Sub Warehouse
- **Purpose**: Local storage or retail outlets
- **Configuration**: `is_main_warehouse = FALSE`, `parent_warehouse_id = [branch_warehouse_id]`
- **Capabilities**:
  - ❌ Cannot receive stock from suppliers
  - ✅ Receive stock transfers from parent branch
  - ✅ Process sales orders
  - ✅ Stock adjustments

---

## Setup Steps

### Step 1: Create Main Warehouse

```sql
INSERT INTO warehouse (
    name,
    city,
    branch_type,
    is_main_warehouse,
    parent_warehouse_id,
    active,
    created_by,
    last_action
)
VALUES (
    'Central Warehouse Bangkok',
    'Bangkok',
    'Main',
    TRUE,
    NULL,
    TRUE,
    'admin',
    'CREATE'
);
```

**Validation Checklist**:
- [ ] `is_main_warehouse = TRUE`
- [ ] `parent_warehouse_id = NULL`
- [ ] `active = TRUE`
- [ ] Unique warehouse name

---

### Step 2: Create Branch Warehouses

```sql
INSERT INTO warehouse (
    name,
    city,
    branch_type,
    is_main_warehouse,
    parent_warehouse_id,
    active,
    created_by,
    last_action
)
VALUES
    ('Branch Warehouse Chiang Mai', 'Chiang Mai', 'Branch', FALSE, 1, TRUE, 'admin', 'CREATE'),
    ('Branch Warehouse Phuket', 'Phuket', 'Branch', FALSE, 1, TRUE, 'admin', 'CREATE'),
    ('Branch Warehouse Pattaya', 'Pattaya', 'Branch', FALSE, 1, TRUE, 'admin', 'CREATE');
```

**Validation Checklist**:
- [ ] `is_main_warehouse = FALSE`
- [ ] `parent_warehouse_id` points to main warehouse (id = 1)
- [ ] `branch_type = 'Branch'`
- [ ] `active = TRUE`

---

### Step 3: Create Sub Warehouses (Optional)

```sql
INSERT INTO warehouse (
    name,
    city,
    branch_type,
    is_main_warehouse,
    parent_warehouse_id,
    active,
    created_by,
    last_action
)
VALUES
    ('Sub Warehouse CM-North', 'Chiang Mai', 'Sub', FALSE, 2, TRUE, 'admin', 'CREATE'),
    ('Sub Warehouse CM-South', 'Chiang Mai', 'Sub', FALSE, 2, TRUE, 'admin', 'CREATE');
```

**Validation Checklist**:
- [ ] `is_main_warehouse = FALSE`
- [ ] `parent_warehouse_id` points to branch warehouse (id = 2)
- [ ] `branch_type = 'Sub'`
- [ ] `active = TRUE`

---

## Warehouse Hierarchy Diagram

```
┌─────────────────────────────────────────────────────────────────┐
│              Central Warehouse Bangkok (Main)                    │
│              ID: 1                                               │
│              is_main_warehouse: TRUE                             │
│              parent_warehouse_id: NULL                           │
│              ✅ Receives stock from suppliers                    │
└────────────┬──────────────┬──────────────┬────────────────────┘
             │              │              │
    ┌────────┴────┐  ┌──────┴──────┐  ┌───┴──────────┐
    │ Branch CM   │  │ Branch PKT  │  │ Branch PTY   │
    │ ID: 2       │  │ ID: 3       │  │ ID: 4        │
    │ parent: 1   │  │ parent: 1   │  │ parent: 1    │
    └──┬────┬─────┘  └─────────────┘  └──────────────┘
       │    │
   ┌───┴─┐  └───┐
   │ Sub │  │ Sub│
   │CM-N │  │CM-S│
   │ID: 5│  │ID:6│
   │par:2│  │p:2 │
   └─────┘  └────┘
```

---

## Stock Flow Rules

### Rule 1: Stock Receipt
```
✅ ALLOWED:
Supplier → Main Warehouse

❌ NOT ALLOWED:
Supplier → Branch Warehouse
Supplier → Sub Warehouse
```

### Rule 2: Stock Transfer
```
✅ ALLOWED:
Main Warehouse → Branch Warehouse → Sub Warehouse
Main Warehouse → Branch Warehouse
Branch Warehouse → Sub Warehouse

❌ NOT ALLOWED:
Branch Warehouse → Main Warehouse (reverse flow)
Sub Warehouse → Branch Warehouse (reverse flow)
Branch A → Branch B (lateral transfer)
```

### Rule 3: Sales Processing
```
✅ ALLOWED from ANY active warehouse:
- Main Warehouse
- Branch Warehouse
- Sub Warehouse

Condition: Sufficient stock available
```

---

## Validation Queries

### Check Warehouse Hierarchy
```sql
SELECT
    w1.id,
    w1.name AS warehouse_name,
    w1.branch_type,
    w1.is_main_warehouse,
    w2.name AS parent_warehouse_name
FROM warehouse w1
LEFT JOIN warehouse w2 ON w1.parent_warehouse_id = w2.id
WHERE w1.active = TRUE
ORDER BY
    w1.is_main_warehouse DESC,
    w1.parent_warehouse_id,
    w1.id;
```

### Find All Main Warehouses
```sql
SELECT *
FROM warehouse
WHERE is_main_warehouse = TRUE
AND active = TRUE;
```

### Find Branch Warehouses for a Main Warehouse
```sql
SELECT *
FROM warehouse
WHERE parent_warehouse_id = 1  -- Main warehouse ID
AND branch_type = 'Branch'
AND active = TRUE;
```

### Find Sub Warehouses for a Branch
```sql
SELECT *
FROM warehouse
WHERE parent_warehouse_id = 2  -- Branch warehouse ID
AND branch_type = 'Sub'
AND active = TRUE;
```

---

## Common Setup Scenarios

### Scenario 1: Single Location Business
```sql
-- Only need one main warehouse
INSERT INTO warehouse (name, city, branch_type, is_main_warehouse, parent_warehouse_id, active)
VALUES ('Main Warehouse', 'Bangkok', 'Main', TRUE, NULL, TRUE);
```

### Scenario 2: Multi-Branch Retail Chain
```sql
-- 1 Main + 3 Branches
INSERT INTO warehouse (name, city, branch_type, is_main_warehouse, parent_warehouse_id, active)
VALUES
    ('Central DC', 'Bangkok', 'Main', TRUE, NULL, TRUE),
    ('Store North', 'Chiang Mai', 'Branch', FALSE, 1, TRUE),
    ('Store South', 'Phuket', 'Branch', FALSE, 1, TRUE),
    ('Store East', 'Pattaya', 'Branch', FALSE, 1, TRUE);
```

### Scenario 3: Regional Distribution with Sub-Locations
```sql
-- 1 Main + 2 Branches + 4 Subs
INSERT INTO warehouse (name, city, branch_type, is_main_warehouse, parent_warehouse_id, active)
VALUES
    -- Main
    ('National DC', 'Bangkok', 'Main', TRUE, NULL, TRUE),

    -- Branches
    ('Regional DC North', 'Chiang Mai', 'Branch', FALSE, 1, TRUE),
    ('Regional DC South', 'Phuket', 'Branch', FALSE, 1, TRUE),

    -- Subs under North
    ('Store CM-1', 'Chiang Mai', 'Sub', FALSE, 2, TRUE),
    ('Store CM-2', 'Chiang Mai', 'Sub', FALSE, 2, TRUE),

    -- Subs under South
    ('Store PKT-1', 'Phuket', 'Sub', FALSE, 3, TRUE),
    ('Store PKT-2', 'Phuket', 'Sub', FALSE, 3, TRUE);
```

---

## Troubleshooting

### Issue: Cannot receive stock at branch warehouse
**Cause**: `is_main_warehouse = FALSE`

**Solution**:
1. Use stock transfer from main warehouse instead
2. OR change warehouse to main warehouse (not recommended if hierarchy is needed)

### Issue: Circular reference in hierarchy
**Cause**: Parent-child relationship creates loop

**Solution**:
```sql
-- Check for circular references
WITH RECURSIVE warehouse_hierarchy AS (
    SELECT id, parent_warehouse_id, name, 1 AS level
    FROM warehouse
    WHERE parent_warehouse_id IS NULL

    UNION ALL

    SELECT w.id, w.parent_warehouse_id, w.name, wh.level + 1
    FROM warehouse w
    INNER JOIN warehouse_hierarchy wh ON w.parent_warehouse_id = wh.id
    WHERE wh.level < 10  -- Prevent infinite recursion
)
SELECT * FROM warehouse_hierarchy
ORDER BY level, id;
```

### Issue: Cannot delete warehouse
**Cause**: Has child warehouses or existing stock

**Solution**:
1. Move or delete child warehouses first
2. Transfer out all stock
3. Soft delete: `UPDATE warehouse SET active = FALSE WHERE id = X`

---

## Best Practices

1. **Always have at least one main warehouse**
   - System requires at least one `is_main_warehouse = TRUE`

2. **Use meaningful names**
   - Include location and type: "Central DC Bangkok", "Branch Store Chiang Mai"

3. **Maintain proper hierarchy**
   - Don't skip levels: Main → Branch → Sub (not Main → Sub directly)

4. **Use soft delete**
   - Set `active = FALSE` instead of deleting records
   - Preserves audit trail and historical data

5. **Document warehouse codes**
   - Use consistent naming convention for warehouse codes/IDs

---

**Last Updated**: 2026-02-07
**Version**: 1.0
