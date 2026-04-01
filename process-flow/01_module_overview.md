# 01 - Module Overview

## System Overview

| Item | Detail |
|---|---|
| Frontend | Angular (SPA) |
| Backend | .NET Core API (Clean Architecture) |
| Database | MSSQL |
| Auth | JWT (Access + Refresh Token) |
| ORM | Entity Framework Core |
| Architecture | Domain → Application → Infrastructure → API |
| Currency | Single (MMK) |
| Company | Single company |

---

## Module Map

```
┌─────────────────────────────────────────────────────────────────┐
│                         ERP SYSTEM                              │
├─────────────┬─────────────┬─────────────┬───────────────────────┤
│  FOUNDATION │ MASTER DATA │ TRANSACTION │ REPORTING             │
├─────────────┼─────────────┼─────────────┼───────────────────────┤
│ Auth        │ Products    │ Purchase    │ Dashboard             │
│ Users       │ Categories  │  ├ PO       │ Reports               │
│ Settings    │ Brands      │  ├ GRN      │  ├ Purchase Report    │
│             │ Groups      │  ├ Invoice  │  ├ Sales Report       │
│             │ Units       │  └ Payment  │  ├ Stock Report       │
│             │ Suppliers   │             │  ├ Payment Report     │
│             │ Customers   │ Sales       │  └ Profit/Loss       │
│             │ Warehouses  │  ├ Quotation│                       │
│             │             │  ├ SO       │                       │
│             │             │  ├ Invoice  │                       │
│             │             │  └ Payment  │                       │
│             │             │             │                       │
│             │             │ Inventory   │                       │
│             │             │  ├ Stock    │                       │
│             │             │  ├ Transfer │                       │
│             │             │  └ Adjust   │                       │
└─────────────┴─────────────┴─────────────┴───────────────────────┘
```

---

## Module List & Status

| # | Module | Prefix | Entities | Status |
|---|---|---|---|---|
| 1 | Auth & Users | `Auth` | AuthUser, AuthRefreshToken | ✅ Done |
| 2 | Product Master | `Prod` | ProdItem, ProdCategory, ProdGroup, ProdBrand, ProdUnit, ProdUnitConversion, ProdUnitPrice, ProdBatch, ProdSerial | ✅ Done |
| 3 | Suppliers | `Purch` | PurchSupplier | ✅ Done |
| 4 | Customers | `Sales` | SalesCustomer | ✅ Done |
| 5 | Warehouses | `Inv` | InvWarehouse, InvWarehouseStock | ✅ Done |
| 6 | Inventory | `Inv` | InvStockMovement, InvStockAdjustment, InvStockTransfer | ✅ Done |
| 7 | Purchase Order | `Purch` | PurchOrder, PurchOrderItem | 🔲 New |
| 8 | Goods Receiving | `Purch` | PurchGoodsReceive, PurchGoodsReceiveItem | 🔲 New |
| 9 | Purchase Invoice | `Purch` | PurchInvoice, PurchItem (update) | ⚠️ Update |
| 10 | Purchase Payment | `Purch` | PurchPayment | 🔲 New |
| 11 | Sales Quotation | `Sales` | SalesQuotation, SalesQuotationItem | 🔲 New |
| 12 | Sales Order | `Sales` | SalesOrder, SalesOrderItem | 🔲 New |
| 13 | Sales Invoice | `Sales` | SalesInvoice, SalesInvoiceItem (update) | ⚠️ Update |
| 14 | Sales Payment | `Sales` | SalesPayment | 🔲 New |
| 15 | Dashboard | — | No new entities | 🔲 New |
| 16 | Reports | — | No new entities | 🔲 New |

---

## Entity Count Summary

| Category | Existing | New | Updated | Total |
|---|---|---|---|---|
| Auth | 2 | 0 | 0 | 2 |
| Product | 9 | 0 | 0 | 9 |
| Inventory | 5 | 0 | 0 | 5 |
| Purchase | 3 | 5 | 2 | 8 |
| Sales | 3 | 5 | 2 | 8 |
| **Total** | **22** | **10** | **4** | **32** |
