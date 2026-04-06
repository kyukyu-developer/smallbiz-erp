# 02 - Process Flow

## Master Data Modules

> Master data must be set up **before** any transactions. These are the foundation
> entities referenced by Purchase, Sales, and Inventory modules.

```
┌─────────────────────────────────────────────────────────────────────────┐
│                          MASTER DATA                                    │
├───────────────────────┬───────────────────────┬─────────────────────────┤
│   PRODUCT MASTER      │   CONTACTS            │   WAREHOUSE             │
├───────────────────────┼───────────────────────┼─────────────────────────┤
│ Units                 │ Suppliers             │ Warehouses              │
│ Brands                │ Customers             │                         │
│ Categories            │                       │                         │
│ Product Groups        │                       │                         │
│ Products              │                       │                         │
│ Unit Conversions      │                       │                         │
│ Unit Prices           │                       │                         │
└───────────────────────┴───────────────────────┴─────────────────────────┘
```

### Setup Order (Dependencies)

```
1. Units           ── no dependency (set up first)
2. Brands          ── no dependency
3. Categories      ── self-referencing (parent category)
4. Product Groups  ── no dependency
5. Warehouses      ── self-referencing (parent warehouse)
6. Suppliers       ── no dependency
7. Customers       ── no dependency
8. Products        ── depends on: Units, Brands, Categories, Product Groups
9. Unit Conversions── depends on: Products, Units
10. Unit Prices    ── depends on: Products, Units
```

---

### 1. Units (ProdUnit)

> Base measurement units used across all products.

```
FORM FIELDS:
┌─────────────────────────────────────────────┐
│  Unit Form                                  │
├─────────────────────────────────────────────┤
│  Name *          [___________________]      │  e.g., Piece, Kilogram, Box, Bottle
│  Symbol *        [___________________]      │  e.g., pc, kg, bx, btl
│  Active          [✓]                        │
└─────────────────────────────────────────────┘

LIST PAGE:
  - Table: Name, Symbol, Active, Actions (Edit/Delete)
  - Search by name or symbol

API:
  GET    /api/units              — list (search, pagination)
  POST   /api/units              — create
  GET    /api/units/:id          — detail
  PUT    /api/units/:id          — update
  DELETE /api/units/:id          — soft delete (set Active = false)
```

---

### 2. Brands (ProdBrand)

> Product brand/manufacturer classification.

```
FORM FIELDS:
┌─────────────────────────────────────────────┐
│  Brand Form                                 │
├─────────────────────────────────────────────┤
│  Name *          [___________________]      │  e.g., 3M, Philips, Samsung
│  Description     [___________________]      │
│  Active          [✓]                        │
└─────────────────────────────────────────────┘

LIST PAGE:
  - Table: Name, Description, Active, Actions (Edit/Delete)
  - Search by name

API:
  GET    /api/brands             — list (search, pagination)
  POST   /api/brands             — create
  GET    /api/brands/:id         — detail
  PUT    /api/brands/:id         — update
  DELETE /api/brands/:id         — soft delete
```

---

### 3. Categories (ProdCategory)

> Hierarchical product categories (supports parent-child nesting).

```
FORM FIELDS:
┌─────────────────────────────────────────────┐
│  Category Form                              │
├─────────────────────────────────────────────┤
│  Code *          [___________________]      │  e.g., CAT-001
│  Name *          [___________________]      │  e.g., Electronics, Raw Materials
│  Parent Category [_____ dropdown ____]      │  nullable (top-level if empty)
│  Description     [___________________]      │
│  Active          [✓]                        │
└─────────────────────────────────────────────┘

LIST PAGE:
  - Table: Code, Name, Parent Category, Active, Actions
  - Tree view or flat list with indent
  - Search by code or name
  - Filter by parent category

HIERARCHY EXAMPLE:
  Electronics
  ├── Computers
  │   ├── Laptops
  │   └── Desktops
  ├── Phones
  └── Accessories

API:
  GET    /api/categories          — list (search, pagination, parentId filter)
  POST   /api/categories          — create
  GET    /api/categories/:id      — detail
  PUT    /api/categories/:id      — update
  DELETE /api/categories/:id      — soft delete
```

---

### 4. Product Groups (ProdGroup)

> Logical grouping of products for reporting/filtering (e.g., "Finished Goods", "Raw Materials").

```
FORM FIELDS:
┌─────────────────────────────────────────────┐
│  Product Group Form                         │
├─────────────────────────────────────────────┤
│  Name *          [___________________]      │  e.g., Finished Goods, Raw Materials
│  Description     [___________________]      │
│  Active          [✓]                        │
└─────────────────────────────────────────────┘

LIST PAGE:
  - Table: Name, Description, Active, Actions (Edit/Delete)
  - Search by name

API:
  GET    /api/product-groups      — list (search, pagination)
  POST   /api/product-groups      — create
  GET    /api/product-groups/:id  — detail
  PUT    /api/product-groups/:id  — update
  DELETE /api/product-groups/:id  — soft delete
```

---

### 5. Warehouses (InvWarehouse)

> Physical or logical storage locations. Supports parent-child (main warehouse → sub-locations).

```
FORM FIELDS:
┌─────────────────────────────────────────────────────────┐
│  Warehouse Form                                         │
├─────────────────────────────────────────────────────────┤
│  Name *              [___________________]              │  e.g., Main Warehouse, Branch A
│  Branch Type *       [_____ dropdown ____]              │  e.g., Main, Branch, Virtual
│  Is Main Warehouse   [✓]                               │
│  Parent Warehouse    [_____ dropdown ____]              │  nullable
│  Is Used Warehouse   [✓]                               │  can transactions use this WH?
│                                                         │
│  ── Location Info ──                                    │
│  Location            [___________________]              │
│  Address             [___________________]              │
│  City                [___________________]              │
│  Country             [___________________]              │
│                                                         │
│  ── Contact ──                                          │
│  Contact Person      [___________________]              │
│  Phone               [___________________]              │
│  Active              [✓]                                │
└─────────────────────────────────────────────────────────┘

LIST PAGE:
  - Table: Name, Branch Type, City, Is Main, Is Used, Contact, Active, Actions
  - Search by name or city
  - Filter by branch type

HIERARCHY EXAMPLE:
  Main Warehouse (Main)
  ├── Zone A - Electronics
  ├── Zone B - Raw Materials
  Branch Office Mandalay (Branch)
  └── Mandalay Store

API:
  GET    /api/warehouses          — list (search, pagination, branchType filter)
  POST   /api/warehouses          — create
  GET    /api/warehouses/:id      — detail
  PUT    /api/warehouses/:id      — update
  DELETE /api/warehouses/:id      — soft delete
```

---

### 6. Suppliers (PurchSupplier)

> Vendors/suppliers from whom the business purchases goods.

```
FORM FIELDS:
┌─────────────────────────────────────────────────────────┐
│  Supplier Form                                          │
├─────────────────────────────────────────────────────────┤
│  Code *              [___________________]              │  e.g., SUP-001 (auto or manual)
│  Name *              [___________________]              │  e.g., ABC Trading Co.
│                                                         │
│  ── Contact Info ──                                     │
│  Contact Person      [___________________]              │
│  Phone               [___________________]              │
│  Email               [___________________]              │
│                                                         │
│  ── Address ──                                          │
│  Address             [___________________]              │
│  City                [___________________]              │
│  Country             [___________________]              │
│                                                         │
│  ── Business Info ──                                    │
│  Tax Number          [___________________]              │  TIN / Tax ID
│  Payment Term (days) [___________________]              │  e.g., 30 (Net 30)
│  Active              [✓]                                │
└─────────────────────────────────────────────────────────┘

LIST PAGE:
  - Table: Code, Name, Contact Person, Phone, City, Payment Term, Active, Actions
  - Search by code, name, phone
  - Filter by city, active status

API:
  GET    /api/suppliers           — list (search, pagination)
  POST   /api/suppliers           — create
  GET    /api/suppliers/:id       — detail
  PUT    /api/suppliers/:id       — update
  DELETE /api/suppliers/:id       — soft delete
```

---

### 7. Customers (SalesCustomer)

> Buyers/customers to whom the business sells goods.

```
FORM FIELDS:
┌─────────────────────────────────────────────────────────┐
│  Customer Form                                          │
├─────────────────────────────────────────────────────────┤
│  Code *              [___________________]              │  e.g., CUS-001 (auto or manual)
│  Name *              [___________________]              │  e.g., XYZ Corporation
│                                                         │
│  ── Contact Info ──                                     │
│  Contact Person      [___________________]              │
│  Phone               [___________________]              │
│  Email               [___________________]              │
│                                                         │
│  ── Address ──                                          │
│  Address             [___________________]              │
│  City                [___________________]              │
│  Country             [___________________]              │
│                                                         │
│  ── Business Info ──                                    │
│  Tax Number          [___________________]              │  TIN / Tax ID
│  Credit Limit        [___________________]              │  e.g., 5,000,000 MMK
│  Active              [✓]                                │
└─────────────────────────────────────────────────────────┘

LIST PAGE:
  - Table: Code, Name, Contact Person, Phone, City, Credit Limit, Active, Actions
  - Search by code, name, phone
  - Filter by city, active status

API:
  GET    /api/customers           — list (search, pagination)
  POST   /api/customers           — create
  GET    /api/customers/:id       — detail
  PUT    /api/customers/:id       — update
  DELETE /api/customers/:id       — soft delete
```

---

### 8. Products (ProdItem)

> Core product/item master — references Units, Brands, Categories, Groups.

```
FORM FIELDS:
┌─────────────────────────────────────────────────────────┐
│  Product Form                                           │
├─────────────────────────────────────────────────────────┤
│  ── Basic Info ──                                       │
│  Code *              [___________________]              │  e.g., PROD-001
│  Name *              [___________________]              │  e.g., Composite Resin A2
│  Description         [___________________]              │
│  Barcode             [___________________]              │  optional
│                                                         │
│  ── Classification ──                                   │
│  Group               [_____ dropdown ____]              │  → ProdGroup
│  Category            [_____ dropdown ____]              │  → ProdCategory
│  Brand               [_____ dropdown ____]              │  → ProdBrand
│  Base Unit *         [_____ dropdown ____]              │  → ProdUnit (smallest unit)
│                                                         │
│  ── Stock Settings ──                                   │
│  Minimum Stock       [___________________]              │  alert threshold
│  Maximum Stock       [___________________]              │
│  Reorder Level       [___________________]              │  reorder point
│  Allow Negative Stock [✓]                               │
│                                                         │
│  ── Tracking ──                                         │
│  Track Type *        [_____ dropdown ____]              │  0:None, 1:Batch, 2:Serial
│  Has Variant         [✓]                                │
│  Active              [✓]                                │
└─────────────────────────────────────────────────────────┘

LIST PAGE:
  - Table: Code, Name, Category, Brand, Base Unit, Track Type, Active, Actions
  - Search by code, name, barcode
  - Filter by category, brand, group, track type, active status

TRACK TYPE:
  0 = None    → no batch/serial tracking (simple stock count)
  1 = Batch   → tracked by ProdBatch (batch no, manufacture date, expiry date)
  2 = Serial  → tracked by ProdSerial (unique serial no per unit)

API:
  GET    /api/products            — list (search, pagination, filters)
  POST   /api/products            — create
  GET    /api/products/:id        — detail (with unit conversions, unit prices)
  PUT    /api/products/:id        — update
  DELETE /api/products/:id        — soft delete
```

---

### 9. Unit Conversions (ProdUnitConversion)

> Defines how units convert for a specific product (e.g., 1 Box = 12 Pieces).

```
FORM FIELDS:
┌─────────────────────────────────────────────────────────┐
│  Unit Conversion Form                                   │
├─────────────────────────────────────────────────────────┤
│  Product *           [_____ dropdown ____]              │  → ProdItem
│  From Unit *         [_____ dropdown ____]              │  → ProdUnit (e.g., Box)
│  To Unit *           [_____ dropdown ____]              │  → ProdUnit (e.g., Piece)
│  Factor *            [___________________]              │  e.g., 12 (1 Box = 12 Pieces)
│  Active              [✓]                                │
└─────────────────────────────────────────────────────────┘

EXAMPLE:
  Product: Composite Resin
  1 Box    = 12 Pieces   (Factor: 12)
  1 Carton = 6 Boxes     (Factor: 6)
  → System can calculate: 1 Carton = 72 Pieces

MANAGED FROM:
  Product detail page → Unit Conversions tab
  OR standalone list page

API:
  GET    /api/product-unit-conversions              — list (filter by productId)
  POST   /api/product-unit-conversions              — create
  GET    /api/product-unit-conversions/:id          — detail
  PUT    /api/product-unit-conversions/:id          — update
  DELETE /api/product-unit-conversions/:id          — soft delete
```

---

### 10. Unit Prices (ProdUnitPrice)

> Sale price per unit for a product (e.g., per Piece = 500 MMK, per Box = 5,000 MMK).

```
FORM FIELDS:
┌─────────────────────────────────────────────────────────┐
│  Unit Price Form                                        │
├─────────────────────────────────────────────────────────┤
│  Product *           [_____ dropdown ____]              │  → ProdItem
│  Unit *              [_____ dropdown ____]              │  → ProdUnit
│  Sale Price *        [___________________]              │  e.g., 5000.00
│  Active              [✓]                                │
└─────────────────────────────────────────────────────────┘

EXAMPLE:
  Product: Composite Resin
  Per Piece  = 500 MMK
  Per Box    = 5,000 MMK (12 pcs discount)
  Per Carton = 28,000 MMK (72 pcs bulk discount)

MANAGED FROM:
  Product detail page → Unit Prices tab

NOTE:
  - Used to auto-fill unit price when adding items in Sales Quotation/Order/Invoice
  - Purchase side uses UnitCost (entered manually per PO, not from master price)

API:
  GET    /api/products/:id/unit-prices       — list by product
  POST   /api/products/:id/unit-prices       — create
  PUT    /api/products/:id/unit-prices/:pid  — update
  DELETE /api/products/:id/unit-prices/:pid  — soft delete
```

---

### Master Data — Frontend Page Summary

| Module | List Page | Detail Page | Form (Create/Edit) |
|---|---|---|---|
| Units | Table + search | — | Dialog / Inline |
| Brands | Table + search | — | Dialog / Inline |
| Categories | Table/Tree + search | — | Dialog (with parent selector) |
| Product Groups | Table + search | — | Dialog / Inline |
| Warehouses | Table + search + filter | Detail (stock levels, child WHs) | Full page form |
| Suppliers | Table + search + filter | Detail (PO history, payment summary) | Full page form |
| Customers | Table + search + filter | Detail (SO history, payment summary, credit) | Full page form |
| Products | Table + search + filters | Detail (tabs: Info, Conversions, Prices, Stock) | Full page form |
| Unit Conversions | Managed in Product detail | — | Dialog |
| Unit Prices | Managed in Product detail | — | Dialog |

---

## Purchase Process Flow

```
┌──────────────┐     ┌──────────────┐     ┌──────────────┐     ┌──────────────┐
│   Purchase   │     │    Goods     │     │   Purchase   │     │   Purchase   │
│    Order     │────►│  Receiving   │────►│   Invoice    │────►│   Payment    │
│    (PO)      │     │   (GRN)      │     │   (Bill)     │     │              │
└──────────────┘     └──────────────┘     └──────────────┘     └──────────────┘
     │                     │                    │                     │
     │ Create PO           │ Receive goods      │ Record bill         │ Pay supplier
     │ from Supplier       │ into Warehouse     │ from supplier       │
     │                     │ + Stock IN         │                     │
     ▼                     ▼                    ▼                     ▼
  Status:               Status:              Status:              Updates:
  Draft                 Draft                Draft                PaidAmount
  Approved              Received             Unpaid               PaymentStatus
  Partially Received    Partial              Partially Paid       on Invoice
  Fully Received                             Paid
  Cancelled                                  Cancelled
  Closed
```

### Purchase Flow — Step by Step

```
1. CREATE PURCHASE ORDER (PO)
   ├── Select Supplier
   ├── Select Warehouse (destination)
   ├── Add Items (product, unit, qty, unit cost, discount, tax)
   ├── Auto-calculate: SubTotal, TotalDiscount, TotalTax, TotalAmount
   ├── Save as Draft
   └── Approve PO → Status: Approved
       │
2. GOODS RECEIVING NOTE (GRN)
   ├── Select PO (only Approved / Partially Received)
   ├── Auto-load PO items with remaining qty
   ├── Enter received qty per item (can be partial)
   ├── Enter batch/serial if tracked
   ├── Confirm Receive:
   │   ├── Create InvStockMovement (type: IN, ref: GRN)
   │   ├── Update InvWarehouseStock (+qty)
   │   ├── Create/Update ProdBatch or ProdSerial
   │   └── Update PO status:
   │       ├── All items fully received → "Fully Received"
   │       └── Some items remaining → "Partially Received"
   │
3. PURCHASE INVOICE
   ├── Select PO (Fully Received / Partially Received)
   │   OR create standalone invoice (no PO — direct purchase)
   ├── Auto-load items from PO/GRN
   ├── Adjust prices if needed (actual vs quoted)
   ├── Save → Status: Unpaid
   │
4. PURCHASE PAYMENT
   ├── Select Purchase Invoice (Unpaid / Partially Paid)
   ├── Enter: amount, payment date, payment method, reference, notes
   ├── Save Payment:
   │   ├── Update PurchInvoice.PaidAmount (SUM of payments)
   │   └── Update PurchInvoice.PaymentStatus:
   │       ├── PaidAmount >= TotalAmount → "Paid"
   │       ├── PaidAmount > 0 → "Partially Paid"
   │       └── PaidAmount = 0 → "Unpaid"
   └── Done
```

### Purchase Order — Form

```
┌──────────────────────────────────────────────────────────────────────────────┐
│  Purchase Order                                            [Draft ▼] [Save]  │
├──────────────────────────────────────────────────────────────────────────────┤
│                                                                              │
│  ── Header ──                                                                │
│  PO Number *         [PO-20260001______]   (auto-generated)                  │
│  Order Date *        [2026-04-01_______]                                     │
│  Expected Date       [________________]                                      │
│  Supplier *          [_____ dropdown / search ____]    → PurchSupplier       │
│  Warehouse *         [_____ dropdown _____________]    → InvWarehouse        │
│  Notes               [________________________________]                      │
│                                                                              │
│  ── Line Items ──                                              [+ Add Item]  │
│  ┌─────┬──────────┬──────┬─────┬──────────┬─────────┬────────┬─────────────┐ │
│  │  #  │ Product  │ Unit │ Qty │ UnitCost │ Disc %  │ Tax %  │ Total       │ │
│  ├─────┼──────────┼──────┼─────┼──────────┼─────────┼────────┼─────────────┤ │
│  │  1  │ [search] │ [dd] │ [__]│ [_______]│ [______]│ [_____]│  auto-calc  │ │
│  │  2  │ [search] │ [dd] │ [__]│ [_______]│ [______]│ [_____]│  auto-calc  │ │
│  └─────┴──────────┴──────┴─────┴──────────┴─────────┴────────┴─────────────┘ │
│                                                                              │
│  ── Summary (auto-calculated) ──                                             │
│                                          SubTotal:      500,000 MMK          │
│                                          Total Discount:  5,000 MMK          │
│                                          Total Tax:      25,000 MMK          │
│                                          ─────────────────────────           │
│                                          Total Amount:  520,000 MMK          │
│                                                                              │
│  [Cancel]                                       [Save Draft] [Approve PO]    │
└──────────────────────────────────────────────────────────────────────────────┘

LINE ITEM CALC:
  DiscountAmount = UnitCost × Qty × DiscountPercent / 100
  TaxAmount      = (UnitCost × Qty - DiscountAmount) × TaxPercent / 100
  TotalAmount    = (UnitCost × Qty) - DiscountAmount + TaxAmount

LIST PAGE:
  - Table: PO No, Date, Supplier, Warehouse, Total, Status, Actions
  - Search by PO no, supplier name
  - Filter by status, supplier, date range
  - Status badge color: Draft(gray), Approved(blue), PartiallyReceived(orange),
    FullyReceived(green), Closed(dark), Cancelled(red)

API:
  GET    /api/purchase-orders              — list (search, pagination, filters)
  POST   /api/purchase-orders              — create (draft)
  GET    /api/purchase-orders/:id          — detail (with items)
  PUT    /api/purchase-orders/:id          — update (draft only)
  PATCH  /api/purchase-orders/:id/approve  — approve
  PATCH  /api/purchase-orders/:id/cancel   — cancel
  DELETE /api/purchase-orders/:id          — soft delete (draft only)
```

---

### Goods Receiving Note (GRN) — Form

```
┌──────────────────────────────────────────────────────────────────────────────┐
│  Goods Receiving Note                                      [Draft ▼] [Save]  │
├──────────────────────────────────────────────────────────────────────────────┤
│                                                                              │
│  ── Header ──                                                                │
│  GRN Number *        [GRN-20260001_____]   (auto-generated)                  │
│  Receive Date *      [2026-04-01_______]                                     │
│  Purchase Order      [_____ dropdown ____]  → PurchOrder (Approved/Partial)  │
│      ↳ auto-fill: Supplier, Warehouse, Items with remaining qty             │
│  Supplier *          [_____ auto-filled __]  → PurchSupplier                 │
│  Warehouse *         [_____ auto-filled __]  → InvWarehouse                  │
│  Notes               [________________________________]                      │
│                                                                              │
│  ── Receive Items ──                                                         │
│  ┌─────┬──────────┬──────┬──────────┬──────────┬──────────┬─────────────────┐│
│  │  #  │ Product  │ Unit │ PO Qty   │ Already  │ Receive  │ Batch/Serial    ││
│  │     │          │      │ (total)  │ Received │ Now *    │                 ││
│  ├─────┼──────────┼──────┼──────────┼──────────┼──────────┼─────────────────┤│
│  │  1  │ Resin A2 │ Box  │   10     │    0     │ [______] │ [batch search]  ││
│  │  2  │ Gloves   │ Box  │   20     │   10     │ [______] │ —               ││
│  └─────┴──────────┴──────┴──────────┴──────────┴──────────┴─────────────────┘│
│                                                                              │
│  [Cancel]                                     [Save Draft] [Confirm Receive] │
└──────────────────────────────────────────────────────────────────────────────┘

ON CONFIRM RECEIVE:
  1. Validate: ReceiveQty ≤ (PO Qty - Already Received) per item
  2. For each item:
     ├── Create/update ProdBatch or ProdSerial (if tracked)
     ├── Create InvStockMovement (type: IN, refType: GRN)
     └── Update InvWarehouseStock (+ReceiveQty in base unit)
  3. Update PurchOrderItem.ReceivedQuantity
  4. Update PurchOrder.Status:
     ├── All items fully received → FullyReceived
     └── Otherwise → PartiallyReceived

LIST PAGE:
  - Table: GRN No, Date, PO No, Supplier, Warehouse, Status, Actions
  - Search by GRN no, PO no, supplier
  - Filter by status, warehouse, date range

API:
  GET    /api/goods-receives               — list (search, pagination, filters)
  POST   /api/goods-receives               — create (draft)
  GET    /api/goods-receives/:id           — detail (with items)
  PUT    /api/goods-receives/:id           — update (draft only)
  PATCH  /api/goods-receives/:id/confirm   — confirm receive (+ stock in)
  PATCH  /api/goods-receives/:id/cancel    — cancel
```

---

### Purchase Invoice — Form

```
┌──────────────────────────────────────────────────────────────────────────────┐
│  Purchase Invoice                                          [Unpaid ▼]        │
├──────────────────────────────────────────────────────────────────────────────┤
│                                                                              │
│  ── Header ──                                                                │
│  Invoice No *        [PI-20260001______]   (auto-generated)                  │
│  Purchase Date *     [2026-04-01_______]                                     │
│  Expected Date       [________________]    (payment due date)                │
│  Purchase Order      [_____ dropdown ____]  → PurchOrder (optional link)     │
│      ↳ auto-fill: Supplier, Warehouse, Items from PO/GRN                     │
│  Supplier *          [_____ dropdown / search ____]                          │
│  Warehouse *         [_____ dropdown _____________]                          │
│  Notes               [________________________________]                      │
│                                                                              │
│  ── Line Items ──                                              [+ Add Item]  │
│  ┌─────┬──────────┬──────┬─────┬──────────┬─────────┬────────┬─────────────┐ │
│  │  #  │ Product  │ Unit │ Qty │ UnitCost │ Disc %  │ Tax %  │ Total       │ │
│  ├─────┼──────────┼──────┼─────┼──────────┼─────────┼────────┼─────────────┤ │
│  │  1  │ [search] │ [dd] │ [__]│ [_______]│ [______]│ [_____]│  auto-calc  │ │
│  └─────┴──────────┴──────┴─────┴──────────┴─────────┴────────┴─────────────┘ │
│                                                                              │
│  ── Summary ──                                                               │
│                                          SubTotal:      500,000 MMK          │
│                                          Total Discount:  5,000 MMK          │
│                                          Total Tax:      25,000 MMK          │
│                                          ─────────────────────────           │
│                                          Total Amount:  520,000 MMK          │
│                                          Paid Amount:         0 MMK          │
│                                          Balance Due:   520,000 MMK          │
│                                                                              │
│  ── Payment History ──                                     [+ Add Payment]   │
│  ┌──────┬────────────┬────────────┬───────────┬────────────┬────────────────┐│
│  │  #   │ Date       │ Method     │ Amount    │ Reference  │ Actions        ││
│  ├──────┼────────────┼────────────┼───────────┼────────────┼────────────────┤│
│  │  (no payments yet)                                                       ││
│  └──────┴────────────┴────────────┴───────────┴────────────┴────────────────┘│
│                                                                              │
│  [Cancel]                                                           [Save]   │
└──────────────────────────────────────────────────────────────────────────────┘

NOTE: For standalone invoices (no PO/GRN), stock-in happens at invoice confirm.

LIST PAGE:
  - Table: Invoice No, Date, Supplier, Total, Paid, Balance, Payment Status, Status, Actions
  - Search by invoice no, supplier name
  - Filter by payment status, supplier, date range
  - Payment status badge: Unpaid(red), PartiallyPaid(orange), Paid(green)

API:
  GET    /api/purchase-invoices            — list (search, pagination, filters)
  POST   /api/purchase-invoices            — create
  GET    /api/purchase-invoices/:id        — detail (with items + payments)
  PUT    /api/purchase-invoices/:id        — update
  PATCH  /api/purchase-invoices/:id/cancel — cancel
  DELETE /api/purchase-invoices/:id        — soft delete (draft only)
```

---

### Purchase Payment — Form

```
┌─────────────────────────────────────────────────────────┐
│  Purchase Payment (Dialog)                              │
├─────────────────────────────────────────────────────────┤
│                                                         │
│  Invoice:  PI-20260001 — ABC Trading Co.                │
│  Total:    520,000 MMK                                  │
│  Paid:           0 MMK                                  │
│  Balance:  520,000 MMK                                  │
│                                                         │
│  Payment No *    [PP-20260001______]  (auto)            │
│  Payment Date *  [2026-04-01_______]                    │
│  Amount *        [___________________]                  │
│  Method *        [_____ dropdown ____]                  │
│                   Cash / Bank Transfer / Cheque / Other │
│  Reference No    [___________________]                  │
│  Notes           [___________________]                  │
│                                                         │
│  [Cancel]                                [Save Payment] │
└─────────────────────────────────────────────────────────┘

VALIDATION:
  - Amount > 0
  - Amount ≤ Balance Due (or allow overpayment with warning)

ON SAVE:
  1. Create PurchPayment record
  2. Recalculate PurchInvoice.PaidAmount = SUM(PurchPayment.Amount)
  3. Update PurchInvoice.PaymentStatus:
     ├── PaidAmount ≥ TotalAmount → Paid
     ├── PaidAmount > 0 → PartiallyPaid
     └── PaidAmount = 0 → Unpaid

API:
  GET    /api/purchase-invoices/:id/payments          — list payments
  POST   /api/purchase-invoices/:id/payments          — add payment
  PUT    /api/purchase-invoices/:id/payments/:pid     — update payment
  DELETE /api/purchase-invoices/:id/payments/:pid     — delete payment (+ recalc)
```

---

### Purchase — Flexible Entry Points

```
FULL FLOW:        PO → GRN → Invoice → Payment
SKIP PO:          GRN (standalone) → Invoice → Payment
DIRECT PURCHASE:  Invoice (standalone, auto stock-in) → Payment
```

> The system supports all 3 paths. PO and GRN are optional — a business can
> start with direct purchase invoices and adopt PO/GRN workflow later.

---

## Sales Process Flow

```
┌──────────────┐     ┌──────────────┐     ┌──────────────┐     ┌──────────────┐
│    Sales     │     │    Sales     │     │    Sales     │     │    Sales     │
│  Quotation   │────►│    Order     │────►│   Invoice    │────►│   Payment    │
│   (Quote)    │     │    (SO)      │     │   (Bill)     │     │              │
└──────────────┘     └──────────────┘     └──────────────┘     └──────────────┘
     │                     │                    │                     │
     │ Propose to          │ Confirm order      │ Bill customer       │ Receive payment
     │ customer            │                    │ + Stock OUT         │ from customer
     │                     │                    │                     │
     ▼                     ▼                    ▼                     ▼
  Status:               Status:              Status:              Updates:
  Draft                 Draft                Draft                PaidAmount
  Sent                  Confirmed            Unpaid               PaymentStatus
  Accepted              Partially Invoiced   Partially Paid       on Invoice
  Rejected              Fully Invoiced       Paid
  Expired               Cancelled            Cancelled
                        Closed
```

### Sales Flow — Step by Step

```
1. CREATE SALES QUOTATION
   ├── Select Customer
   ├── Add Items (product, unit, qty, unit price, discount, tax)
   ├── Auto-calculate: SubTotal, TotalDiscount, TotalTax, TotalAmount
   ├── Set validity period (valid until date)
   ├── Save as Draft
   └── Send to Customer → Status: Sent
       │
       ├── Customer Accepts → Status: Accepted → Can convert to SO
       ├── Customer Rejects → Status: Rejected
       └── Past valid date → Status: Expired
       │
2. SALES ORDER (SO)
   ├── Convert from Quotation (auto-load items)
   │   OR create standalone SO
   ├── Select Customer + Warehouse (source)
   ├── Add/Edit Items
   ├── Check stock availability (optional warning)
   ├── Save as Draft
   └── Confirm SO → Status: Confirmed
       │
3. SALES INVOICE
   ├── Select SO (Confirmed / Partially Invoiced)
   │   OR create standalone invoice (no SO — walk-in / direct sale)
   ├── Select Warehouse (source for stock deduction)
   ├── Auto-load items from SO
   ├── Confirm Invoice:
   │   ├── Create InvStockMovement (type: OUT, ref: Sales Invoice)
   │   ├── Update InvWarehouseStock (-qty)
   │   ├── Deduct from ProdBatch or ProdSerial (FIFO)
   │   ├── Update SO status:
   │   │   ├── All items fully invoiced → "Fully Invoiced"
   │   │   └── Some items remaining → "Partially Invoiced"
   │   └── Invoice Status: Unpaid
   │
4. SALES PAYMENT
   ├── Select Sales Invoice (Unpaid / Partially Paid)
   ├── Enter: amount, payment date, payment method, reference, notes
   ├── Save Payment:
   │   ├── Update SalesInvoice.PaidAmount (SUM of payments)
   │   └── Update SalesInvoice.PaymentStatus:
   │       ├── PaidAmount >= TotalAmount → "Paid"
   │       ├── PaidAmount > 0 → "Partially Paid"
   │       └── PaidAmount = 0 → "Unpaid"
   └── Done
```

### Sales Quotation — Form

```
┌──────────────────────────────────────────────────────────────────────────────┐
│  Sales Quotation                                           [Draft ▼] [Save] │
├──────────────────────────────────────────────────────────────────────────────┤
│                                                                              │
│  ── Header ──                                                                │
│  Quotation No *      [QT-20260001______]   (auto-generated)                  │
│  Quotation Date *    [2026-04-01_______]                                     │
│  Valid Until         [2026-04-15_______]   (expiry date)                     │
│  Customer *          [_____ dropdown / search ____]    → SalesCustomer       │
│  Notes               [________________________________]                      │
│                                                                              │
│  ── Line Items ──                                              [+ Add Item]  │
│  ┌─────┬──────────┬──────┬─────┬──────────┬─────────┬────────┬─────────────┐ │
│  │  #  │ Product  │ Unit │ Qty │ UnitPrice│ Disc %  │ Tax %  │ Total       │ │
│  ├─────┼──────────┼──────┼─────┼──────────┼─────────┼────────┼─────────────┤ │
│  │  1  │ [search] │ [dd] │ [__]│ [auto*]  │ [______]│ [_____]│  auto-calc  │ │
│  │  2  │ [search] │ [dd] │ [__]│ [auto*]  │ [______]│ [_____]│  auto-calc  │ │
│  └─────┴──────────┴──────┴─────┴──────────┴─────────┴────────┴─────────────┘ │
│  * UnitPrice auto-fills from ProdUnitPrice, but can be overridden            │
│                                                                              │
│  ── Summary (auto-calculated) ──                                             │
│                                          SubTotal:      800,000 MMK          │
│                                          Total Discount: 10,000 MMK          │
│                                          Total Tax:      40,000 MMK          │
│                                          ─────────────────────────           │
│                                          Total Amount:  830,000 MMK          │
│                                                                              │
│  [Cancel]                                [Save Draft] [Send to Customer]     │
└──────────────────────────────────────────────────────────────────────────────┘

ACTIONS:
  - Send → Status: Sent
  - Accept → Status: Accepted → enables "Convert to SO" button
  - Reject → Status: Rejected
  - System auto-check: if today > ValidUntil → Expired

LIST PAGE:
  - Table: QT No, Date, Customer, Total, Valid Until, Status, Actions
  - Search by QT no, customer name
  - Filter by status, customer, date range
  - Action buttons: Send, Accept, Reject, Convert to SO
  - Status badge: Draft(gray), Sent(blue), Accepted(green), Rejected(red), Expired(dark)

API:
  GET    /api/sales-quotations                   — list (search, pagination, filters)
  POST   /api/sales-quotations                   — create (draft)
  GET    /api/sales-quotations/:id               — detail (with items)
  PUT    /api/sales-quotations/:id               — update (draft/sent only)
  PATCH  /api/sales-quotations/:id/send          — mark as sent
  PATCH  /api/sales-quotations/:id/accept        — mark as accepted
  PATCH  /api/sales-quotations/:id/reject        — mark as rejected
  POST   /api/sales-quotations/:id/convert-to-so — create SO from quotation
  DELETE /api/sales-quotations/:id               — soft delete (draft only)
```

---

### Sales Order (SO) — Form

```
┌──────────────────────────────────────────────────────────────────────────────┐
│  Sales Order                                               [Draft ▼] [Save] │
├──────────────────────────────────────────────────────────────────────────────┤
│                                                                              │
│  ── Header ──                                                                │
│  SO Number *         [SO-20260001______]   (auto-generated)                  │
│  Order Date *        [2026-04-01_______]                                     │
│  Expected Date       [________________]                                      │
│  Customer *          [_____ dropdown / search ____]    → SalesCustomer       │
│  Quotation           [_____ dropdown ____]  → SalesQuotation (optional link) │
│      ↳ auto-fill: Customer, Items from quotation                             │
│  Warehouse *         [_____ dropdown _____________]    → InvWarehouse        │
│  Notes               [________________________________]                      │
│                                                                              │
│  ── Line Items ──                                              [+ Add Item]  │
│  ┌─────┬──────────┬──────┬─────┬──────────┬─────────┬────────┬──────┬──────┐ │
│  │  #  │ Product  │ Unit │ Qty │ UnitPrice│ Disc %  │ Tax %  │ Total│ Stock│ │
│  ├─────┼──────────┼──────┼─────┼──────────┼─────────┼────────┼──────┼──────┤ │
│  │  1  │ [search] │ [dd] │ [__]│ [auto*]  │ [______]│ [_____]│ calc │  45  │ │
│  │  2  │ [search] │ [dd] │ [__]│ [auto*]  │ [______]│ [_____]│ calc │   3  │ │
│  └─────┴──────────┴──────┴─────┴──────────┴─────────┴────────┴──────┴──────┘ │
│  * Stock column shows available qty in selected warehouse (info only)         │
│                                                                              │
│  ── Summary (auto-calculated) ──                                             │
│                                          SubTotal:      800,000 MMK          │
│                                          Total Discount: 10,000 MMK          │
│                                          Total Tax:      40,000 MMK          │
│                                          ─────────────────────────           │
│                                          Total Amount:  830,000 MMK          │
│                                                                              │
│  [Cancel]                                       [Save Draft] [Confirm SO]    │
└──────────────────────────────────────────────────────────────────────────────┘

ON CONFIRM:
  - Validate all required fields
  - Optional: warn if any item qty > available stock
  - Status → Confirmed
  - Stock is NOT deducted yet (deducted at Invoice time)

LIST PAGE:
  - Table: SO No, Date, Customer, Warehouse, Total, Status, Actions
  - Search by SO no, customer name
  - Filter by status, customer, warehouse, date range
  - Action: Confirm, Create Invoice, Cancel
  - Status badge: Draft(gray), Confirmed(blue), PartiallyInvoiced(orange),
    FullyInvoiced(green), Closed(dark), Cancelled(red)

API:
  GET    /api/sales-orders                   — list (search, pagination, filters)
  POST   /api/sales-orders                   — create (draft)
  GET    /api/sales-orders/:id               — detail (with items)
  PUT    /api/sales-orders/:id               — update (draft only)
  PATCH  /api/sales-orders/:id/confirm       — confirm
  PATCH  /api/sales-orders/:id/cancel        — cancel
  DELETE /api/sales-orders/:id               — soft delete (draft only)
```

---

### Sales Invoice — Form

```
┌──────────────────────────────────────────────────────────────────────────────┐
│  Sales Invoice                                             [Unpaid ▼]       │
├──────────────────────────────────────────────────────────────────────────────┤
│                                                                              │
│  ── Header ──                                                                │
│  Invoice No *        [SI-20260001______]   (auto-generated)                  │
│  Sale Date *         [2026-04-01_______]                                     │
│  Due Date            [________________]    (payment due date)                │
│  Customer *          [_____ dropdown / search ____]                          │
│  Sales Order         [_____ dropdown ____]  → SalesOrder (optional link)     │
│      ↳ auto-fill: Customer, Warehouse, Items from SO                         │
│  Warehouse *         [_____ dropdown _____________]                          │
│  Notes               [________________________________]                      │
│                                                                              │
│  ── Line Items ──                                              [+ Add Item]  │
│  ┌─────┬──────────┬──────┬─────┬──────────┬─────────┬────────┬─────────────┐ │
│  │  #  │ Product  │ Unit │ Qty │ UnitPrice│ Disc %  │ Tax %  │ Total       │ │
│  ├─────┼──────────┼──────┼─────┼──────────┼─────────┼────────┼─────────────┤ │
│  │  1  │ [search] │ [dd] │ [__]│ [_______]│ [______]│ [_____]│  auto-calc  │ │
│  └─────┴──────────┴──────┴─────┴──────────┴─────────┴────────┴─────────────┘ │
│                                                                              │
│  ── Summary ──                                                               │
│                                          SubTotal:      800,000 MMK          │
│                                          Total Discount: 10,000 MMK          │
│                                          Total Tax:      40,000 MMK          │
│                                          ─────────────────────────           │
│                                          Total Amount:  830,000 MMK          │
│                                          Paid Amount:         0 MMK          │
│                                          Balance Due:   830,000 MMK          │
│                                                                              │
│  ── Payment History ──                                     [+ Add Payment]   │
│  ┌──────┬────────────┬────────────┬───────────┬────────────┬────────────────┐ │
│  │  #   │ Date       │ Method     │ Amount    │ Reference  │ Actions        │ │
│  ├──────┼────────────┼────────────┼───────────┼────────────┼────────────────┤ │
│  │  (no payments yet)                                                      │ │
│  └──────┴────────────┴────────────┴───────────┴────────────┴────────────────┘ │
│                                                                              │
│  [Cancel]                                                           [Save]   │
└──────────────────────────────────────────────────────────────────────────────┘

ON SAVE/CONFIRM:
  1. For each item:
     ├── Create InvStockMovement (type: OUT, refType: SalesInvoice)
     ├── Update InvWarehouseStock (-qty in base unit)
     └── Deduct from ProdBatch / ProdSerial (FIFO) if tracked
  2. If linked to SO, update SalesOrderItem.InvoicedQuantity
  3. Update SalesOrder.Status:
     ├── All items fully invoiced → FullyInvoiced
     └── Otherwise → PartiallyInvoiced

LIST PAGE:
  - Table: Invoice No, Date, Customer, Total, Paid, Balance, Payment Status, Actions
  - Search by invoice no, customer name
  - Filter by payment status, customer, date range
  - Print invoice button

API:
  GET    /api/sales-invoices                — list (search, pagination, filters)
  POST   /api/sales-invoices                — create (+ stock out)
  GET    /api/sales-invoices/:id            — detail (with items + payments)
  PUT    /api/sales-invoices/:id            — update
  PATCH  /api/sales-invoices/:id/cancel     — cancel (+ reverse stock)
  DELETE /api/sales-invoices/:id            — soft delete (draft only)
```

---

### Sales Payment — Form

```
┌─────────────────────────────────────────────────────────┐
│  Sales Payment (Dialog)                                 │
├─────────────────────────────────────────────────────────┤
│                                                         │
│  Invoice:  SI-20260001 — XYZ Corporation                │
│  Total:    830,000 MMK                                  │
│  Paid:           0 MMK                                  │
│  Balance:  830,000 MMK                                  │
│                                                         │
│  Payment No *    [SP-20260001______]  (auto)            │
│  Payment Date *  [2026-04-01_______]                    │
│  Amount *        [___________________]                  │
│  Method *        [_____ dropdown ____]                  │
│                   Cash / Bank Transfer / Cheque / Other │
│  Reference No    [___________________]                  │
│  Notes           [___________________]                  │
│                                                         │
│  [Cancel]                                [Save Payment] │
└─────────────────────────────────────────────────────────┘

VALIDATION:
  - Amount > 0
  - Amount ≤ Balance Due (or allow overpayment with warning)

ON SAVE:
  1. Create SalesPayment record
  2. Recalculate SalesInvoice.PaidAmount = SUM(SalesPayment.Amount)
  3. Update SalesInvoice.PaymentStatus:
     ├── PaidAmount ≥ TotalAmount → Paid
     ├── PaidAmount > 0 → PartiallyPaid
     └── PaidAmount = 0 → Unpaid

API:
  GET    /api/sales-invoices/:id/payments          — list payments
  POST   /api/sales-invoices/:id/payments          — add payment
  PUT    /api/sales-invoices/:id/payments/:pid     — update payment
  DELETE /api/sales-invoices/:id/payments/:pid     — delete payment (+ recalc)
```

---

### Sales — Flexible Entry Points

```
FULL FLOW:       Quotation → SO → Invoice → Payment
SKIP QUOTATION:  SO (standalone) → Invoice → Payment
DIRECT SALE:     Invoice (standalone, auto stock-out) → Payment
```

> Same flexibility as purchase side. Walk-in customers can skip Quotation and SO.

---

## Inventory Process Flow

```
┌─────────────────────────────────────────────────────────┐
│                    STOCK MOVEMENT                       │
├──────────────────┬──────────────────┬───────────────────┤
│    STOCK IN      │   STOCK OUT      │   INTERNAL        │
├──────────────────┼──────────────────┼───────────────────┤
│ GRN (Purchase)   │ Sales Invoice    │ Stock Transfer    │
│ Stock Adjustment │ Stock Adjustment │  (WH → WH)        │
│  (+)             │  (-)             │ Stock Adjustment  │
│                  │                  │  (+/-)            │
└──────────────────┴──────────────────┴───────────────────┘
                         │
                         ▼
              ┌──────────────────────┐
              │  InvStockMovement    │
              │  (audit trail log)   │
              │                      │
              │  MovementType:       │
              │   IN / OUT / ADJUST  │
              │   TRANSFER_IN        │
              │   TRANSFER_OUT       │
              │                      │
              │  ReferenceType:      │
              │   1 = GRN            │
              │   2 = Sales Invoice  │
              │   3 = Adjustment     │
              │   4 = Transfer       │
              │   5 = Purch Invoice  │
              │     (direct purch)   │
              └──────────────────────┘
```

### Stock Movement — Reference Types

| ReferenceType | Description | MovementType | Stock Effect |
|---|---|---|---|
| 1 | Goods Receiving (GRN) | IN | + qty |
| 2 | Sales Invoice | OUT | - qty |
| 3 | Stock Adjustment | ADJUST | +/- qty |
| 4 | Stock Transfer | TRANSFER_IN / TRANSFER_OUT | move between warehouses |
| 5 | Purchase Invoice (direct) | IN | + qty (when no GRN) |

---

### Warehouse Stock View (InvWarehouseStock)

> Current stock levels per product per warehouse.

```
LIST PAGE:
┌─────────────────────────────────────────────────────────────────────────────┐
│  Stock Levels                                                [Filter ▼]     │
├──────┬──────────┬────────────┬───────────┬──────────┬────────┬──────────────┤
│ Code │ Product  │ Warehouse  │ Available │ Reserved │ Total  │ Status       │
├──────┼──────────┼────────────┼───────────┼──────────┼────────┼──────────────┤
│ P001 │ Resin A2 │ Main WH    │   45      │    5     │   50   │ ✅ OK        │
│ P002 │ Gloves   │ Main WH    │    3      │    0     │    3   │ ⚠️ Low Stock │
│ P003 │ Cement   │ Branch A   │    0      │    0     │    0   │ 🔴 Out       |
└──────┴──────────┴────────────┴───────────┴──────────┴────────┴──────────────┘

FILTERS:
  - Warehouse (dropdown)
  - Product (search)
  - Category
  - Stock status: All / Low Stock / Out of Stock / OK

INDICATORS:
  Available ≤ 0              → 🔴 Out of Stock
  Available ≤ ReorderLevel   → ⚠️ Low Stock (needs reorder)
  Available ≤ MinimumStock   → ⚠️ Below Minimum
  Otherwise                  → ✅ OK

API:
  GET  /api/stock                — list (warehouseId, productId, category, lowStock filters)
  GET  /api/stock/low-stock      — items below reorder level
```

---

### Stock Adjustment (InvStockAdjustment)

> Manually adjust stock quantity — for corrections, damage, loss, opening balance.

```
FORM FIELDS:
┌─────────────────────────────────────────────────────────┐
│  Stock Adjustment Form                                  │
├─────────────────────────────────────────────────────────┤
│  Adjustment No *     [___________________]              │  auto: ADJ-20260001
│  Adjustment Date *   [___________________]              │
│  Warehouse *         [_____ dropdown ____]              │  → InvWarehouse
│  Product *           [_____ dropdown ____]              │  → ProdItem
│  Adjustment Qty *    [___________________]              │  positive (+) or negative (-)
│  Reason *            [___________________]              │  e.g., Damaged, Expired, Count correction
│  Active              [✓]                                │
└─────────────────────────────────────────────────────────┘

FLOW:
  1. Select warehouse + product
  2. Show current stock level
  3. Enter adjustment qty (+10 to add, -5 to deduct)
  4. Enter reason
  5. Confirm:
     ├── Create InvStockAdjustment record
     ├── Create InvStockMovement (type: ADJUST, ref: Adjustment)
     └── Update InvWarehouseStock (Available ± qty)

LIST PAGE:
  - Table: Adj No, Date, Warehouse, Product, Qty (+/-), Reason, Created By, Actions
  - Search by adj no, product
  - Filter by warehouse, date range

API:
  GET    /api/stock-adjustments           — list (search, pagination, filters)
  POST   /api/stock-adjustments           — create (+ update stock)
  GET    /api/stock-adjustments/:id       — detail
```

---

### Stock Transfer (InvStockTransfer)

> Move stock between warehouses.

```
FORM FIELDS:
┌─────────────────────────────────────────────────────────┐
│  Stock Transfer Form                                    │
├─────────────────────────────────────────────────────────┤
│  Transfer No *       [___________________]              │  auto: TRF-20260001
│  Transfer Date *     [___________________]              │
│  From Warehouse *    [_____ dropdown ____]              │  → InvWarehouse (source)
│  To Warehouse *      [_____ dropdown ____]              │  → InvWarehouse (destination)
│  Product *           [_____ dropdown ____]              │  → ProdItem
│  Quantity *          [___________________]              │  must be ≤ available stock
│  Status              [_____ dropdown ____]              │  0:Draft, 1:Completed, 2:Cancelled
│  Notes               [___________________]              │
└─────────────────────────────────────────────────────────┘

VALIDATION:
  - From ≠ To warehouse
  - Quantity ≤ available stock in source warehouse

FLOW:
  1. Select source/destination warehouse + product
  2. Show available stock in source warehouse
  3. Enter transfer qty
  4. Save as Draft or Confirm immediately:
     ├── Create InvStockTransfer record
     ├── Create InvStockMovement (type: TRANSFER_OUT, ref: Transfer) for source
     ├── Create InvStockMovement (type: TRANSFER_IN, ref: Transfer) for destination
     ├── Update InvWarehouseStock (source: -qty)
     └── Update InvWarehouseStock (destination: +qty)

STATUS FLOW:
  Draft ──► Completed
    │
    └──► Cancelled

LIST PAGE:
  - Table: Transfer No, Date, From WH, To WH, Product, Qty, Status, Actions
  - Search by transfer no, product
  - Filter by warehouse, status, date range

API:
  GET    /api/stock-transfers             — list (search, pagination, filters)
  POST   /api/stock-transfers             — create
  GET    /api/stock-transfers/:id         — detail
  PUT    /api/stock-transfers/:id         — update (draft only)
  PATCH  /api/stock-transfers/:id/confirm — confirm transfer (+ update stock)
  PATCH  /api/stock-transfers/:id/cancel  — cancel

```

---

### Stock Movement Log (InvStockMovement)

> Read-only audit trail — every stock change is logged here automatically.

```
LIST PAGE (READ-ONLY):
┌──────────────────────────────────────────────────────────────────────────────────────┐
│  Stock Movement Log                                                  [Filter ▼]     │
├──────┬──────────┬────────────┬──────────┬──────┬────────────┬────────┬──────────────┤
│ Date │ Product  │ Warehouse  │ Type     │ Qty  │ Ref Type   │ Ref No │ Batch/Serial │
├──────┼──────────┼────────────┼──────────┼──────┼────────────┼────────┼──────────────┤
│ 4/1  │ Resin A2 │ Main WH    │ IN       │ +50  │ GRN        │ GRN-01 │ B-2026-001   │
│ 4/1  │ Resin A2 │ Main WH    │ OUT      │ -10  │ Sales Inv  │ SI-001 │ B-2026-001   │
│ 4/1  │ Gloves   │ Main WH    │ ADJUST   │  -2  │ Adjustment │ ADJ-01 │ —            │
│ 4/2  │ Cement   │ Main WH    │ XFER_OUT │ -20  │ Transfer   │ TRF-01 │ —            │
│ 4/2  │ Cement   │ Branch A   │ XFER_IN  │ +20  │ Transfer   │ TRF-01 │ —            │
└──────┴──────────┴────────────┴──────────┴──────┴────────────┴────────┴──────────────┘

FILTERS:
  - Product (search)
  - Warehouse (dropdown)
  - Movement Type (IN / OUT / ADJUST / TRANSFER)
  - Reference Type (GRN / Sales Invoice / Adjustment / Transfer)
  - Date range

NOTE: This table is NEVER edited directly. Records are created automatically
      by GRN, Sales Invoice, Adjustment, and Transfer operations.

API:
  GET  /api/stock-movements    — list (search, pagination, filters)
```

---

## Status Flow Diagrams

### Purchase Order Status

```
  Draft ──► Approved ──► Partially Received ──► Fully Received ──► Closed
    │                          │                                      │
    └──► Cancelled             └──────────────────────────────────────┘
```

### Goods Receiving Status

```
  Draft ──► Received
    │
    └──► Cancelled
```

### Purchase Invoice Payment Status

```
  Unpaid ──► Partially Paid ──► Paid
    │
    └──► Cancelled
```

### Sales Quotation Status

```
  Draft ──► Sent ──► Accepted ──► (convert to SO)
              │          
              ├──► Rejected
              └──► Expired
```

### Sales Order Status

```
  Draft ──► Confirmed ──► Partially Invoiced ──► Fully Invoiced ──► Closed
    │                           │                                     │
    └──► Cancelled              └─────────────────────────────────────┘
```

### Sales Invoice Payment Status

```
  Unpaid ──► Partially Paid ──► Paid
    │
    └──► Cancelled
```

---

## Cross-Module Relationships

```
                    ┌─────────────┐
                    │ PurchSupplier│
                    └──────┬──────┘
                           │
              ┌────────────▼────────────┐
              │      PurchOrder         │
              │  (PurchOrderItem[])     │
              └────────────┬────────────┘
                           │ 1:N (partial receiving)
              ┌────────────▼────────────┐
              │   PurchGoodsReceive     │──────► InvStockMovement (IN)
              │  (PurchGoodsReceiveItem[])│──────► InvWarehouseStock (+)
              └────────────┬────────────┘        ProdBatch / ProdSerial
                           │
              ┌────────────▼────────────┐
              │     PurchInvoice        │
              │  (PurchItem[])          │
              └────────────┬────────────┘
                           │ 1:N
              ┌────────────▼────────────┐
              │     PurchPayment        │
              └─────────────────────────┘


                    ┌─────────────┐
                    │SalesCustomer│
                    └──────┬──────┘
                           │
              ┌────────────▼────────────┐
              │    SalesQuotation       │
              │ (SalesQuotationItem[])  │
              └────────────┬────────────┘
                           │ convert
              ┌────────────▼────────────┐
              │      SalesOrder         │
              │  (SalesOrderItem[])     │
              └────────────┬────────────┘
                           │
              ┌────────────▼────────────┐
              │     SalesInvoice        │──────► InvStockMovement (OUT)
              │ (SalesInvoiceItem[])    │──────► InvWarehouseStock (-)
              └────────────┬────────────┘        ProdBatch / ProdSerial
                           │ 1:N
              ┌────────────▼────────────┐
              │     SalesPayment        │
              └─────────────────────────┘
```
