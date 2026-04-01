# 04 - Development Order

## Strategy

Build from the **foundation up**. Each phase depends on the previous one.
Complete **backend + frontend** for each module before moving to the next.

### Architecture Pattern

**Backend (.NET Core)** — Clean Architecture:
```
ERP.Domain       → Entities, Enums, Interfaces
ERP.Application  → DTOs, Features (Commands/Queries), Interfaces
ERP.Infrastructure → Data (DbContext), Repositories, Services
ERP.API          → Controllers
```

**Frontend (Angular)** — Clean Architecture per feature:
```
features/{module}/
  ├── domain/          → entities, repositories (interfaces)
  ├── application/     → usecases
  ├── infrastructure/  → repositories (API implementation)
  └── presentation/    → list, detail, form components
```

---

## PHASE 1 — Project Setup ✅ Done
> Goal: Both projects running locally with database connected

### Backend (.NET Core)
- [x] Create solution with Clean Architecture projects (Domain, Application, Infrastructure, API)
- [x] Install dependencies (EF Core, JWT, Swagger, FluentValidation, MediatR)
- [x] Setup `appsettings.json` configuration
- [x] Connect MSSQL database via EF Core
- [x] Setup global response format (success/error wrapper)
- [x] Setup global exception filter
- [x] Setup Swagger (`/api-docs`)
- [x] Setup CORS
- [x] Test DB connection

### Frontend (Angular)
- [x] Create Angular project
- [x] Install dependencies (Angular Material, JWT interceptor, ngx-toastr)
- [x] Setup environment files (`apiUrl`)
- [x] Setup `AppModule` with HttpClient
- [x] Setup global HTTP interceptor (attach token + handle errors)
- [x] Setup Angular Material theme
- [x] Setup basic layout shell (sidebar + header)
- [x] Setup lazy-loaded routing

---

## PHASE 2 — Auth Module ✅ Done
> Goal: Login works, JWT token stored, protected routes working

### Backend
- [x] Create `AuthUser` entity
- [x] Create `AuthRefreshToken` entity
- [x] Hash password with bcrypt on create
- [x] Create `POST /auth/login` endpoint
- [x] Create `POST /auth/refresh` endpoint
- [x] Create `GET /auth/me` endpoint
- [x] Create JWT strategy + auth guard
- [x] Create role-based guard + `@Roles()` decorator
- [x] Seed default admin user

### Frontend
- [x] Create login page (username + password form)
- [x] Call `POST /auth/login`, save token
- [x] Create `AuthService` (login, logout, getUser, isLoggedIn)
- [x] Create `AuthGuard` (redirect to login if no token)
- [x] Create `JwtInterceptor` (attach Bearer token to all requests)
- [x] Create `ErrorInterceptor` (handle 401 → redirect to login)
- [x] Test full login flow

---

## PHASE 3 — Master Data: Units ✅ Done
> Goal: Manage measurement units (Piece, Kg, Box, etc.)

### Backend
- [x] Create `ProdUnit` entity
- [x] Create `GET /api/units` (list with search + pagination)
- [x] Create `POST /api/units` (create)
- [x] Create `GET /api/units/:id` (detail)
- [x] Create `PUT /api/units/:id` (update)
- [x] Create `DELETE /api/units/:id` (soft delete)

### Frontend
- [x] Unit list page (table with search)
- [x] Create/Edit unit form (Name, Symbol)
- [x] Active/Inactive toggle
- [x] Domain/Application/Infrastructure layers

---

## PHASE 4 — Master Data: Brands ✅ Done
> Goal: Manage product brands

### Backend
- [x] Create `ProdBrand` entity
- [x] Create `GET /api/brands` (list with search + pagination)
- [x] Create `POST /api/brands` (create)
- [x] Create `GET /api/brands/:id` (detail)
- [x] Create `PUT /api/brands/:id` (update)
- [x] Create `DELETE /api/brands/:id` (soft delete)

### Frontend
- [x] Brand list page (table with search)
- [x] Create/Edit brand form (Name, Description)
- [x] Brand detail page
- [x] Domain/Application/Infrastructure layers

---

## PHASE 5 — Master Data: Categories ✅ Done
> Goal: Manage hierarchical product categories

### Backend
- [x] Create `ProdCategory` entity (self-referencing ParentCategoryId)
- [x] Create `GET /api/categories` (list with search, parent filter, pagination)
- [x] Create `POST /api/categories` (create)
- [x] Create `GET /api/categories/:id` (detail)
- [x] Create `PUT /api/categories/:id` (update)
- [x] Create `DELETE /api/categories/:id` (soft delete)

### Frontend
- [x] Category list page (table with stats card, search)
- [x] Create/Edit category form (Code, Name, Parent Category, Description)
- [x] Category detail page (two-column layout with tips)
- [x] Parent category dropdown selector
- [x] Domain/Application/Infrastructure layers

---

## PHASE 6 — Master Data: Product Groups ✅ Done
> Goal: Manage product grouping (Finished Goods, Raw Materials, etc.)

### Backend
- [x] Create `ProdGroup` entity
- [x] Create `GET /api/product-groups` (list with search + pagination)
- [x] Create `POST /api/product-groups` (create)
- [x] Create `GET /api/product-groups/:id` (detail)
- [x] Create `PUT /api/product-groups/:id` (update)
- [x] Create `DELETE /api/product-groups/:id` (soft delete)

### Frontend
- [x] Product group list page (table with search)
- [x] Create/Edit form (Name, Description)
- [x] Product group detail page
- [x] Domain/Application/Infrastructure layers

---

## PHASE 7 — Master Data: Warehouses ✅ Done
> Goal: Manage warehouse/branch locations with hierarchy

### Backend
- [x] Create `InvWarehouse` entity (self-referencing ParentWarehouseId)
- [x] Create `GET /api/warehouses` (list with search, branchType filter, pagination)
- [x] Create `POST /api/warehouses` (create)
- [x] Create `GET /api/warehouses/:id` (detail)
- [x] Create `PUT /api/warehouses/:id` (update)
- [x] Create `DELETE /api/warehouses/:id` (soft delete)

### Frontend
- [x] Warehouse list page (table with search, filter)
- [x] Warehouse detail page
- [x] Warehouse filter dialog
- [x] Create/Edit form (Name, Branch Type, Is Main, Parent, Location, Address, Contact)
- [x] Domain/Application/Infrastructure layers

---

## PHASE 8 — Master Data: Suppliers ✅ Done
> Goal: Manage supplier/vendor contacts

### Backend
- [x] Create `PurchSupplier` entity
- [x] Create `GET /api/suppliers` (list with search + pagination)
- [x] Create `POST /api/suppliers` (create)
- [x] Create `GET /api/suppliers/:id` (detail)
- [x] Create `PUT /api/suppliers/:id` (update)
- [x] Create `DELETE /api/suppliers/:id` (soft delete)

### Frontend
- [x] Supplier list page (table with search)
- [x] Create/Edit supplier form (Code, Name, Contact, Address, Tax, Payment Terms)
- [x] Supplier detail page
- [x] Domain/Application/Infrastructure layers

---

## PHASE 9 — Master Data: Customers ✅ Done
> Goal: Manage customer contacts

### Backend
- [x] Create `SalesCustomer` entity
- [x] Create `GET /api/customers` (list with search + pagination)
- [x] Create `POST /api/customers` (create)
- [x] Create `GET /api/customers/:id` (detail)
- [x] Create `PUT /api/customers/:id` (update)
- [x] Create `DELETE /api/customers/:id` (soft delete)

### Frontend
- [x] Customer list page (table with stats, search)
- [x] Create/Edit customer form (Code, Name, Contact, Address, Tax, Credit Limit)
- [x] Customer detail page (two-column layout with tips)
- [x] Domain/Application/Infrastructure layers

---

## PHASE 10 — Master Data: Products ✅ Done
> Goal: Full product master — create, search, view detail with unit conversion & pricing

### Backend
- [x] Create `ProdItem` entity
- [x] Create `ProdUnitConversion` entity
- [x] Create `ProdUnitPrice` entity
- [x] Create `ProdBatch` entity
- [x] Create `ProdSerial` entity
- [x] Create `VwProdUnitConversion` view
- [x] Create `GET /api/products` (search by code/name, filter by category/brand/group, pagination)
- [x] Create `POST /api/products` (create)
- [x] Create `GET /api/products/:id` (detail with conversions, prices)
- [x] Create `PUT /api/products/:id` (update)
- [x] Create `DELETE /api/products/:id` (soft delete)
- [x] Create unit conversion CRUD endpoints
- [x] Create unit price CRUD endpoints

### Frontend
- [x] Product list page (table with search, filters)
- [x] Product detail page (tabs: Info, Unit Conversions, Prices, Stock)
- [x] Create/Edit product form (Code, Name, Group, Category, Brand, Base Unit, Stock Settings, Track Type)
- [x] Unit conversion management (in product detail)
- [x] Domain/Application/Infrastructure layers

---

## PHASE 11 — Inventory: Stock & Warehouse Stock ✅ Done
> Goal: View stock levels per product per warehouse

### Backend
- [x] Create `InvWarehouseStock` entity
- [x] Create `InvStockMovement` entity
- [x] Create `GET /api/stock` (list with warehouse, product, category, lowStock filters)
- [x] Create `GET /api/stock/low-stock` (items below reorder level)

### Frontend
- [x] Stock level view in warehouse detail
- [ ] Standalone stock level list page (all products across warehouses)
- [ ] Low stock alert indicators
- [ ] Filter by warehouse, category, stock status

---

## PHASE 12 — Inventory: Stock Adjustment ✅ Backend / 🔲 Frontend
> Goal: Manually adjust stock for corrections, damage, opening balance

### Backend
- [x] Create `InvStockAdjustment` entity
- [x] Create `POST /api/stock-adjustments` (create + update stock)
- [x] Create `GET /api/stock-adjustments` (list with filters)
- [x] Create `GET /api/stock-adjustments/:id` (detail)
- [ ] Auto-create `InvStockMovement` record on adjustment

### Frontend
- [ ] Stock adjustment list page (table: Adj No, Date, Warehouse, Product, Qty, Reason)
- [ ] Stock adjustment form (Warehouse, Product, show current stock, Qty +/-, Reason)
- [ ] Search + filter by warehouse, product, date range
- [ ] Domain/Application/Infrastructure layers

---

## PHASE 13 — Inventory: Stock Transfer ✅ Backend / 🔲 Frontend
> Goal: Move stock between warehouses

### Backend
- [x] Create `InvStockTransfer` entity
- [x] Create `POST /api/stock-transfers` (create)
- [x] Create `GET /api/stock-transfers` (list with filters)
- [x] Create `GET /api/stock-transfers/:id` (detail)
- [ ] Create `PATCH /api/stock-transfers/:id/confirm` (confirm + update stock both WHs)
- [ ] Create `PATCH /api/stock-transfers/:id/cancel` (cancel)
- [ ] Auto-create 2x `InvStockMovement` records on confirm (TRANSFER_OUT + TRANSFER_IN)

### Frontend
- [ ] Stock transfer list page (table: Transfer No, Date, From WH, To WH, Product, Qty, Status)
- [ ] Stock transfer form (From WH, To WH, Product, show available stock, Qty)
- [ ] Draft → Confirm → Completed status flow
- [ ] Search + filter by warehouse, status, date range
- [ ] Domain/Application/Infrastructure layers

---

## PHASE 14 — Purchase Invoice (Existing) ⚠️ Update
> Goal: Update existing purchase invoice to support PO/GRN links + payment tracking

### Backend
- [x] `PurchInvoice` entity exists
- [x] `PurchItem` entity exists
- [x] CRUD endpoints exist (PurchasesController)
- [ ] Add `PurchaseOrderId` column to PurchInvoice (nullable FK → PurchOrder)
- [ ] Add `GoodsReceiveId` column to PurchInvoice (nullable FK → PurchGoodsReceive)
- [ ] Update create/update logic to accept PO/GRN references
- [ ] Auto stock-in for standalone invoices (no GRN) via InvStockMovement

### Frontend
- [ ] Purchase invoice list page (table: Invoice No, Date, Supplier, Total, Paid, Balance, Payment Status)
- [ ] Purchase invoice form (Header + Line Items + Summary)
- [ ] Auto-load items from PO/GRN when linked
- [ ] Payment status badge (Unpaid/PartiallyPaid/Paid)
- [ ] Domain/Application/Infrastructure layers

---

## PHASE 15 — Purchase Order 🔲 New
> Goal: Create and manage purchase orders with approval workflow
> Depends on: Phase 8 (Suppliers), Phase 7 (Warehouses), Phase 10 (Products)

### Backend
- [ ] Create `PurchOrder` entity
- [ ] Create `PurchOrderItem` entity
- [ ] Create `PurchOrderStatus` enum
- [ ] Create `GET /api/purchase-orders` (list with search, filter by status/supplier/date, pagination)
- [ ] Create `POST /api/purchase-orders` (create as Draft)
- [ ] Create `GET /api/purchase-orders/:id` (detail with items)
- [ ] Create `PUT /api/purchase-orders/:id` (update — Draft only)
- [ ] Create `PATCH /api/purchase-orders/:id/approve` (Draft → Approved)
- [ ] Create `PATCH /api/purchase-orders/:id/cancel` (→ Cancelled)
- [ ] Create `DELETE /api/purchase-orders/:id` (soft delete — Draft only)
- [ ] Auto-calculate: SubTotal, TotalDiscount, TotalTax, TotalAmount on item changes
- [ ] Auto-generate PO number (PO-YYYYNNNN)

### Frontend
- [ ] PO list page (table: PO No, Date, Supplier, Warehouse, Total, Status, Actions)
- [ ] Search by PO number, supplier name
- [ ] Filter by status, supplier, date range
- [ ] Status badge colors (Draft:gray, Approved:blue, PartiallyReceived:orange, FullyReceived:green, Closed:dark, Cancelled:red)
- [ ] PO form page:
  - [ ] Header: PO No (auto), Order Date, Expected Date, Supplier dropdown, Warehouse dropdown, Notes
  - [ ] Line Items table: Product search, Unit dropdown, Qty, Unit Cost, Discount %, Tax %, Total (auto-calc)
  - [ ] Add/remove line items
  - [ ] Summary section: SubTotal, Total Discount, Total Tax, Total Amount (all auto-calc)
  - [ ] Actions: Save Draft, Approve PO
- [ ] PO detail view (read-only with status actions)
- [ ] Domain/Application/Infrastructure layers

---

## PHASE 16 — Goods Receiving Note (GRN) 🔲 New
> Goal: Receive goods into warehouse from PO, update stock
> Depends on: Phase 15 (Purchase Order)

### Backend
- [ ] Create `PurchGoodsReceive` entity
- [ ] Create `PurchGoodsReceiveItem` entity
- [ ] Create `GoodsReceiveStatus` enum
- [ ] Create `GET /api/goods-receives` (list with search, filter by status/warehouse/date, pagination)
- [ ] Create `POST /api/goods-receives` (create as Draft)
- [ ] Create `GET /api/goods-receives/:id` (detail with items)
- [ ] Create `PUT /api/goods-receives/:id` (update — Draft only)
- [ ] Create `PATCH /api/goods-receives/:id/confirm` — on confirm:
  - [ ] Validate: ReceiveQty ≤ (PO Qty - Already Received) per item
  - [ ] Create `InvStockMovement` (type: IN, refType: GRN) per item
  - [ ] Update `InvWarehouseStock` (+qty in base unit) per item
  - [ ] Create/Update `ProdBatch` or `ProdSerial` if tracked
  - [ ] Update `PurchOrderItem.ReceivedQuantity` per item
  - [ ] Update `PurchOrder.Status` (PartiallyReceived or FullyReceived)
- [ ] Create `PATCH /api/goods-receives/:id/cancel` (→ Cancelled)
- [ ] Auto-generate GRN number (GRN-YYYYNNNN)

### Frontend
- [ ] GRN list page (table: GRN No, Date, PO No, Supplier, Warehouse, Status, Actions)
- [ ] Search by GRN no, PO no, supplier
- [ ] Filter by status, warehouse, date range
- [ ] GRN form page:
  - [ ] Header: GRN No (auto), Receive Date, PO dropdown (Approved/PartiallyReceived only)
  - [ ] Auto-fill Supplier + Warehouse + Items from selected PO
  - [ ] Receive Items table: Product, Unit, PO Qty, Already Received, Receive Now, Batch/Serial
  - [ ] Show remaining qty per item (PO Qty - Already Received)
  - [ ] Actions: Save Draft, Confirm Receive
- [ ] GRN detail view (read-only with status)
- [ ] Domain/Application/Infrastructure layers

---

## PHASE 17 — Purchase Payment 🔲 New
> Goal: Record payments against purchase invoices, auto-update payment status
> Depends on: Phase 14 (Purchase Invoice)

### Backend
- [ ] Create `PurchPayment` entity
- [ ] Create `PaymentMethod` enum
- [ ] Create `GET /api/purchase-invoices/:id/payments` (list payments for invoice)
- [ ] Create `POST /api/purchase-invoices/:id/payments` (add payment)
  - [ ] Validate: Amount > 0
  - [ ] Recalculate `PurchInvoice.PaidAmount` = SUM(all payments)
  - [ ] Update `PurchInvoice.PaymentStatus` (Unpaid/PartiallyPaid/Paid)
- [ ] Create `PUT /api/purchase-invoices/:id/payments/:pid` (update payment + recalc)
- [ ] Create `DELETE /api/purchase-invoices/:id/payments/:pid` (delete + recalc)
- [ ] Auto-generate payment number (PP-YYYYNNNN)

### Frontend
- [ ] Payment history table (embedded in Purchase Invoice detail page)
  - [ ] Table: Date, Method, Amount, Reference, Notes, Actions (Edit/Delete)
- [ ] Add Payment dialog:
  - [ ] Show: Invoice No, Total, Paid, Balance
  - [ ] Fields: Payment No (auto), Payment Date, Amount, Method dropdown, Reference No, Notes
  - [ ] Validate: Amount ≤ Balance Due
- [ ] Edit/Delete payment (with recalculation)
- [ ] Payment status badge on invoice list

---

## PHASE 18 — Sales Quotation 🔲 New
> Goal: Create quotations for customers, manage status, convert to Sales Order
> Depends on: Phase 9 (Customers), Phase 10 (Products)

### Backend
- [ ] Create `SalesQuotation` entity
- [ ] Create `SalesQuotationItem` entity
- [ ] Create `QuotationStatus` enum
- [ ] Create `GET /api/sales-quotations` (list with search, filter by status/customer/date, pagination)
- [ ] Create `POST /api/sales-quotations` (create as Draft)
- [ ] Create `GET /api/sales-quotations/:id` (detail with items)
- [ ] Create `PUT /api/sales-quotations/:id` (update — Draft/Sent only)
- [ ] Create `PATCH /api/sales-quotations/:id/send` (Draft → Sent)
- [ ] Create `PATCH /api/sales-quotations/:id/accept` (Sent → Accepted)
- [ ] Create `PATCH /api/sales-quotations/:id/reject` (Sent → Rejected)
- [ ] Create `POST /api/sales-quotations/:id/convert-to-so` (Accepted → create SalesOrder with items)
- [ ] Create `DELETE /api/sales-quotations/:id` (soft delete — Draft only)
- [ ] Auto-fill UnitPrice from `ProdUnitPrice` when product + unit selected
- [ ] Auto-generate quotation number (QT-YYYYNNNN)
- [ ] Expiry check: if today > ValidUntil and status = Sent → mark as Expired

### Frontend
- [ ] Quotation list page (table: QT No, Date, Customer, Total, Valid Until, Status, Actions)
- [ ] Search by QT number, customer name
- [ ] Filter by status, customer, date range
- [ ] Status badge colors (Draft:gray, Sent:blue, Accepted:green, Rejected:red, Expired:dark)
- [ ] Action buttons: Send, Accept, Reject, Convert to SO
- [ ] Quotation form page:
  - [ ] Header: QT No (auto), Date, Valid Until, Customer dropdown, Notes
  - [ ] Line Items table: Product search, Unit dropdown, Qty, Unit Price (auto from ProdUnitPrice), Discount %, Tax %, Total (auto-calc)
  - [ ] Add/remove line items
  - [ ] Summary: SubTotal, Total Discount, Total Tax, Total Amount
  - [ ] Actions: Save Draft, Send to Customer
- [ ] Quotation detail view (read-only with status actions)
- [ ] Domain/Application/Infrastructure layers

---

## PHASE 19 — Sales Order 🔲 New
> Goal: Manage confirmed sales orders, track invoiced quantities
> Depends on: Phase 18 (Sales Quotation), Phase 7 (Warehouses)

### Backend
- [ ] Create `SalesOrder` entity
- [ ] Create `SalesOrderItem` entity
- [ ] Create `SalesOrderStatus` enum
- [ ] Create `GET /api/sales-orders` (list with search, filter by status/customer/warehouse/date, pagination)
- [ ] Create `POST /api/sales-orders` (create as Draft)
- [ ] Create `GET /api/sales-orders/:id` (detail with items + stock availability)
- [ ] Create `PUT /api/sales-orders/:id` (update — Draft only)
- [ ] Create `PATCH /api/sales-orders/:id/confirm` (Draft → Confirmed)
  - [ ] Optional: warn if item qty > available stock
- [ ] Create `PATCH /api/sales-orders/:id/cancel` (→ Cancelled)
- [ ] Create `DELETE /api/sales-orders/:id` (soft delete — Draft only)
- [ ] Auto-generate SO number (SO-YYYYNNNN)

### Frontend
- [ ] SO list page (table: SO No, Date, Customer, Warehouse, Total, Status, Actions)
- [ ] Search by SO number, customer name
- [ ] Filter by status, customer, warehouse, date range
- [ ] Status badge colors (Draft:gray, Confirmed:blue, PartiallyInvoiced:orange, FullyInvoiced:green, Closed:dark, Cancelled:red)
- [ ] Action buttons: Confirm, Create Invoice, Cancel
- [ ] SO form page:
  - [ ] Header: SO No (auto), Date, Expected Date, Customer dropdown, Quotation dropdown (optional), Warehouse dropdown, Notes
  - [ ] Auto-fill items from Quotation if selected
  - [ ] Line Items table: Product search, Unit dropdown, Qty, Unit Price, Discount %, Tax %, Total (auto-calc), Available Stock (info)
  - [ ] Summary: SubTotal, Total Discount, Total Tax, Total Amount
  - [ ] Actions: Save Draft, Confirm SO
- [ ] SO detail view (read-only with status actions)
- [ ] Domain/Application/Infrastructure layers

---

## PHASE 20 — Sales Invoice ⚠️ Update
> Goal: Update existing sales invoice to support SO link + stock deduction + payment tracking
> Depends on: Phase 19 (Sales Order)

### Backend
- [x] `SalesInvoice` entity exists
- [x] `SalesInvoiceItem` entity exists
- [x] CRUD endpoints exist (SalesController)
- [ ] Add `SalesOrderId` column to SalesInvoice (nullable FK → SalesOrder)
- [ ] Update create logic — on confirm:
  - [ ] Create `InvStockMovement` (type: OUT, refType: SalesInvoice) per item
  - [ ] Update `InvWarehouseStock` (-qty in base unit) per item
  - [ ] Deduct from `ProdBatch` / `ProdSerial` (FIFO) if tracked
  - [ ] Update `SalesOrderItem.InvoicedQuantity` per item (if linked to SO)
  - [ ] Update `SalesOrder.Status` (PartiallyInvoiced or FullyInvoiced)
- [ ] Validate: sufficient stock available (unless AllowNegativeStock)
- [ ] Support cancel: reverse stock movements

### Frontend
- [ ] Sales invoice list page (table: Invoice No, Date, Customer, Total, Paid, Balance, Payment Status, Actions)
- [ ] Search by invoice no, customer name
- [ ] Filter by payment status, customer, date range
- [ ] Payment status badge (Unpaid:red, PartiallyPaid:orange, Paid:green)
- [ ] Sales invoice form page:
  - [ ] Header: Invoice No (auto), Sale Date, Due Date, Customer dropdown, SO dropdown (optional), Warehouse dropdown, Notes
  - [ ] Auto-fill items from SO if selected
  - [ ] Line Items table: Product search, Unit dropdown, Qty, Unit Price, Discount %, Tax %, Total (auto-calc)
  - [ ] Summary: SubTotal, Total Discount, Total Tax, Total Amount, Paid Amount, Balance Due
  - [ ] Payment History section (table + Add Payment button)
  - [ ] Actions: Save, Confirm
- [ ] Invoice detail view + printable invoice
- [ ] Domain/Application/Infrastructure layers

---

## PHASE 21 — Sales Payment 🔲 New
> Goal: Record payments against sales invoices, auto-update payment status
> Depends on: Phase 20 (Sales Invoice)

### Backend
- [ ] Create `SalesPayment` entity
- [ ] Create `GET /api/sales-invoices/:id/payments` (list payments for invoice)
- [ ] Create `POST /api/sales-invoices/:id/payments` (add payment)
  - [ ] Validate: Amount > 0
  - [ ] Recalculate `SalesInvoice.PaidAmount` = SUM(all payments)
  - [ ] Update `SalesInvoice.PaymentStatus` (Unpaid/PartiallyPaid/Paid)
- [ ] Create `PUT /api/sales-invoices/:id/payments/:pid` (update + recalc)
- [ ] Create `DELETE /api/sales-invoices/:id/payments/:pid` (delete + recalc)
- [ ] Auto-generate payment number (SP-YYYYNNNN)

### Frontend
- [ ] Payment history table (embedded in Sales Invoice detail page)
  - [ ] Table: Date, Method, Amount, Reference, Notes, Actions (Edit/Delete)
- [ ] Add Payment dialog:
  - [ ] Show: Invoice No, Total, Paid, Balance
  - [ ] Fields: Payment No (auto), Payment Date, Amount, Method dropdown, Reference No, Notes
  - [ ] Validate: Amount ≤ Balance Due
- [ ] Edit/Delete payment (with recalculation)
- [ ] Payment status badge on invoice list

---

## PHASE 22 — Dashboard 🔲 Update
> Goal: Summary view for business overview with real data
> Depends on: All transaction modules above

### Backend
- [ ] Create `GET /api/dashboard/summary`:
  - [ ] Today's sales count + amount
  - [ ] Today's purchases count + amount
  - [ ] Monthly revenue (total sales this month)
  - [ ] Monthly expenses (total purchases this month)
  - [ ] Outstanding receivables (unpaid sales invoices total)
  - [ ] Outstanding payables (unpaid purchase invoices total)
  - [ ] Low stock item count
- [ ] Create `GET /api/dashboard/recent-sales` (last 10 sales invoices)
- [ ] Create `GET /api/dashboard/recent-purchases` (last 10 purchase invoices)
- [ ] Create `GET /api/dashboard/low-stock` (items below reorder level)
- [ ] Create `GET /api/dashboard/sales-chart` (monthly sales last 12 months)

### Frontend
- [ ] Summary cards row:
  - [ ] Today's Sales | Today's Purchases | Monthly Revenue | Monthly Expenses
  - [ ] Outstanding Receivables | Outstanding Payables | Low Stock Alerts
- [ ] Sales trend chart (bar/line chart — last 12 months)
- [ ] Recent sales invoices widget (table: Invoice No, Customer, Amount, Status)
- [ ] Recent purchase invoices widget (table: Invoice No, Supplier, Amount, Status)
- [ ] Low stock alert widget (table: Product, Warehouse, Available, Reorder Level)
- [ ] Quick action buttons (New PO, New SO, New Sales Invoice)

---

## PHASE 23 — Reports 🔲 New
> Goal: Business reports for purchase, sales, stock, and payments
> Depends on: All transaction modules above

### Backend
- [ ] Create `GET /api/reports/purchase-summary` (date range, group by supplier/product/month)
- [ ] Create `GET /api/reports/sales-summary` (date range, group by customer/product/month)
- [ ] Create `GET /api/reports/stock-report` (current stock, stock value, by warehouse)
- [ ] Create `GET /api/reports/stock-movement-report` (date range, by product/warehouse/type)
- [ ] Create `GET /api/reports/payment-report` (date range, receivables/payables, by customer/supplier)
- [ ] Create `GET /api/reports/profit-loss` (date range: total sales - total purchases)
- [ ] Support export to Excel (CSV download)

### Frontend
- [ ] Reports page with report type selector
- [ ] Purchase Report:
  - [ ] Filter: date range, supplier
  - [ ] Table: Supplier, Invoice Count, Total Amount, Paid, Balance
  - [ ] Summary totals
- [ ] Sales Report:
  - [ ] Filter: date range, customer
  - [ ] Table: Customer, Invoice Count, Total Amount, Paid, Balance
  - [ ] Summary totals
- [ ] Stock Report:
  - [ ] Filter: warehouse, category
  - [ ] Table: Product, Warehouse, Available, Reserved, Unit Cost, Stock Value
  - [ ] Total stock value
- [ ] Payment Report:
  - [ ] Filter: date range, type (receivable/payable)
  - [ ] Table: Invoice No, Customer/Supplier, Total, Paid, Balance, Due Date, Status
  - [ ] Aging summary (current, 30d, 60d, 90d+)
- [ ] Export to Excel button per report

---

## Milestone Summary

| Phase | Module | Depends On | Status |
|---|---|---|---|
| 1 | Project Setup | — | ✅ Done |
| 2 | Auth | Phase 1 | ✅ Done |
| 3 | Units | Phase 1 | ✅ Done |
| 4 | Brands | Phase 1 | ✅ Done |
| 5 | Categories | Phase 1 | ✅ Done |
| 6 | Product Groups | Phase 1 | ✅ Done |
| 7 | Warehouses | Phase 1 | ✅ Done |
| 8 | Suppliers | Phase 2 | ✅ Done |
| 9 | Customers | Phase 2 | ✅ Done |
| 10 | Products | Phase 3,4,5,6 | ✅ Done |
| 11 | Stock View | Phase 7,10 | ✅ Partial |
| 12 | Stock Adjustment | Phase 11 | ⚠️ Backend done, Frontend pending |
| 13 | Stock Transfer | Phase 11 | ⚠️ Backend done, Frontend pending |
| 14 | Purchase Invoice (update) | Phase 8,7,10 | ⚠️ Exists, needs update |
| 15 | Purchase Order | Phase 8,7,10 | 🔲 New |
| 16 | Goods Receiving (GRN) | Phase 15 | 🔲 New |
| 17 | Purchase Payment | Phase 14 | 🔲 New |
| 18 | Sales Quotation | Phase 9,10 | 🔲 New |
| 19 | Sales Order | Phase 18,7 | 🔲 New |
| 20 | Sales Invoice (update) | Phase 19 | ⚠️ Exists, needs update |
| 21 | Sales Payment | Phase 20 | 🔲 New |
| 22 | Dashboard (update) | All above | ⚠️ Exists, needs real data |
| 23 | Reports | All above | 🔲 New |

---

## Recommended Build Sequence

```
ALREADY COMPLETED (Phase 1–10):
  Setup → Auth → Units → Brands → Categories → Groups → Warehouses
  → Suppliers → Customers → Products

NEXT PRIORITY — Inventory Frontend (Phase 11–13):
  Stock View (complete frontend) → Stock Adjustment → Stock Transfer

THEN — Purchase Flow (Phase 14–17):
  Purchase Invoice (update) → Purchase Order → GRN → Purchase Payment

THEN — Sales Flow (Phase 18–21):
  Sales Quotation → Sales Order → Sales Invoice (update) → Sales Payment

FINALLY — Overview (Phase 22–23):
  Dashboard (update with real data) → Reports
```

### Estimated Task Count

| Area | Phases | Backend Tasks | Frontend Tasks | Total |
|---|---|---|---|---|
| Done (1–10) | 10 | ~60 | ~50 | ~110 ✅ |
| Inventory FE (11–13) | 3 | ~6 | ~15 | ~21 |
| Purchase Flow (14–17) | 4 | ~30 | ~25 | ~55 |
| Sales Flow (18–21) | 4 | ~35 | ~30 | ~65 |
| Dashboard + Reports (22–23) | 2 | ~15 | ~20 | ~35 |
| **Remaining** | **13** | **~86** | **~90** | **~176** |
