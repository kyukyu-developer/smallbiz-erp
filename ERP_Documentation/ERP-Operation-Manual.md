# ERP Operations Manual / Form-wise Step Documentation

## 1. INVENTORY MODULE

### 1.1 Product Form (Product Master)
**Module:** Inventory  
**Purpose:** Create and manage products, base unit, price, and tracking type.

**User Steps:**
1. Open **Product Form**.
2. Click **Add New Product**.
3. Enter **Product Name**.
4. Select **Category**.
5. Select **Base Unit** (e.g., PC).
6. Choose **Tracking Type**: None / Batch / Serial.
7. Enter **Base Cost Price**.
8. Enter **Base Sale Price**.
9. Click **Save**.

**System Processing:**
- Validates base unit and tracking type.
- Stores product in `products` table.
- Checks for duplicate names.

**Tables Used:** `products`, `units`

---

### 1.2 Unit Conversion Form
**Module:** Inventory  
**Purpose:** Define conversion factor between different units of a product.

**User Steps:**
1. Open **Unit Conversion Form**.
2. Select a **Product**.
3. Select **From Unit** (e.g., BOX).
4. Select **To Unit** (Base Unit, e.g., PC).
5. Enter **Conversion Factor** (e.g., 1 BOX = 10 PC).
6. Click **Save**.

**System Processing:**
- Validates that target unit is base unit.
- Prevents duplicate conversion entries.
- Saves conversion in `unit_conversions` table.

**Tables Used:** `unit_conversions`, `products`, `units`

---

### 1.3 Unit Price Override Form
**Module:** Inventory  
**Purpose:** Set custom sale price for specific units.

**User Steps:**
1. Open **Unit Price Override Form**.
2. Select **Product**.
3. Select **Unit**.
4. Enter **Sale Price**.
5. Click **Save**.

**System Processing:**
- Overrides calculated sale price from base unit.
- Saves in `product_unit_prices`.

**Tables Used:** `product_unit_prices`, `products`, `units`

---

### 1.4 Warehouse Form
**Module:** Inventory  
**Purpose:** Add and manage warehouse locations.

**User Steps:**
1. Open **Warehouse Form**.
2. Click **Add New Warehouse**.
3. Enter **Warehouse Name**.
4. Enter **Location**.
5. Click **Save**.

**System Processing:**
- Saves warehouse record.
- Updates warehouse list for stock operations.

**Tables Used:** `warehouses`

---

### 1.5 Opening Stock Form
**Module:** Inventory  
**Purpose:** Initialize stock quantities for products in warehouse.

**User Steps:**
1. Open **Opening Stock Form**.
2. Select **Warehouse**.
3. Select **Product**.
4. Enter **Quantity**.
5. If Batch Product → enter **Batch Number & Expiry**.
6. If Serial Product → enter **Serial Numbers**.
7. Click **Save**.

**System Processing:**
- Converts quantity to base unit.
- Updates `warehouse_stock`.
- Adds `product_batches` or `product_serials` records if applicable.
- Logs action in `stock_movements`.

**Tables Used:** `warehouse_stock`, `product_batches`, `product_serials`, `stock_movements`

## 2. PURCHASE MODULE

### 2.1 Purchase Invoice Form
**Module:** Purchase  
**Purpose:** Record purchased products and add stock.

**User Steps:**
1. Open **Purchase Invoice Form**.
2. Select **Supplier**.
3. Select **Warehouse**.
4. Add products with **Unit and Quantity**.
5. Enter **Unit Cost Price**.
6. Add **Batch / Serial Numbers** if required.
7. Click **Save**.

**System Processing:**
- Converts quantity to base unit.
- Updates `warehouse_stock`.
- Adds `product_batches` / `product_serials` records.
- Logs purchase in `stock_movements`.

**Tables Used:** `purchases`, `purchase_items`, `warehouse_stock`, `product_batches`, `product_serials`, `stock_movements`

## 3. SALES MODULE

### 3.1 Sales Invoice Form
**Module:** Sales  
**Purpose:** Sell products and reduce stock.

**User Steps:**
1. Open **Sales Invoice Form**.
2. Select **Customer**.
3. Select **Warehouse**.
4. Add products with **Unit and Quantity**.
5. Select **Batch or Serial Numbers** if applicable.
6. Click **Save**.

**System Processing:**
- Checks stock availability in warehouse.
- Applies unit price override if defined.
- Converts quantity to base unit.
- Deducts stock from `warehouse_stock`.
- Updates `product_batches` or `product_serials` status.
- Logs transaction in `stock_movements`.

**Tables Used:** `sales`, `sales_items`, `warehouse_stock`, `product_batches`, `product_serials`, `stock_movements`, `product_unit_prices`

## 4. INVENTORY OPERATIONS

### 4.1 Stock Transfer Form
**Module:** Inventory  
**Purpose:** Transfer stock between warehouses.

**User Steps:**
1. Open **Stock Transfer Form**.
2. Select **Source Warehouse** and **Destination Warehouse**.
3. Select **Product**.
4. Enter **Quantity / Batch / Serial**.
5. Click **Save**.

**System Processing:**
- Deducts stock from source warehouse.
- Adds stock to destination warehouse.
- Updates batch/serial records.
- Logs movement in `stock_movements`.

**Tables Used:** `warehouse_stock`, `product_batches`, `product_serials`, `stock_movements`

---

### 4.2 Stock Adjustment Form
**Module:** Inventory  
**Purpose:** Correct stock discrepancies.

**User Steps:**
1. Open **Stock Adjustment Form**.
2. Select **Warehouse** and **Product**.
3. Enter **Adjustment Quantity (+/-)**.
4. Provide **Reason**.
5. Click **Save**.

**System Processing:**
- Updates `warehouse_stock`.
- Logs adjustment in `stock_movements`.

**Tables Used:** `warehouse_stock`, `stock_movements`

## 5. REPORTS MODULE

### 5.1 Stock Report Form
**Purpose:** Generates stock levels per warehouse.
**Tables Used:** `warehouse_stock`, `products`, `warehouses`

### 5.2 Batch Expiry Report Form
**Purpose:** Lists expiring batches.
**Tables Used:** `product_batches`, `products`, `warehouses`

### 5.3 Sales Report Form
**Purpose:** Tracks sales by product/customer.
**Tables Used:** `sales`, `sales_items`, `customers`

