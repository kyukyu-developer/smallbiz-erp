# 03 - All Entities (Database Schema)

## Naming Convention

- **Prefix by module**: `Auth`, `Prod`, `Inv`, `Purch`, `Sales`
- **All IDs**: `NVARCHAR(50)` (GUID string)
- **Audit fields on every table**: `Active`, `CreatedAt`, `CreatedBy`, `UpdatedAt`, `UpdatedBy`, `LastAction`
- **Status fields**: `INT` (enum mapped in C# code)
- **Database**: MSSQL (DATETIME2, NVARCHAR, BIT, DECIMAL)

---

## Entity Relationship Overview

```
┌──────────────────────────────────────────────────────────────────────────────────┐
│                              AUTH MODULE                                         │
│  AuthUser ◄──── AuthRefreshToken                                                │
└──────────────────────────────────────────────────────────────────────────────────┘

┌──────────────────────────────────────────────────────────────────────────────────┐
│                           PRODUCT MASTER                                         │
│                                                                                  │
│  ProdUnit ◄──┐                                                                   │
│              ├── ProdUnitConversion (FromUnit ↔ ToUnit per Product)               │
│              ├── ProdUnitPrice (SalePrice per Unit per Product)                   │
│              └── ProdItem.BaseUnitId                                              │
│                     │                                                            │
│  ProdBrand ◄────────┤                                                            │
│  ProdCategory ◄─────┤  (self-ref: ParentCategoryId)                              │
│  ProdGroup ◄────────┘                                                            │
│                     │                                                            │
│              ProdBatch (per Product + Warehouse)                                  │
│              ProdSerial (per Product + Warehouse)                                 │
└──────────────────────────────────────────────────────────────────────────────────┘

┌──────────────────────────────────────────────────────────────────────────────────┐
│                            INVENTORY                                             │
│                                                                                  │
│  InvWarehouse (self-ref: ParentWarehouseId)                                      │
│       │                                                                          │
│       ├── InvWarehouseStock (Product × Warehouse → Available, Reserved)           │
│       ├── InvStockMovement (audit log: IN/OUT/ADJUST/TRANSFER)                   │
│       ├── InvStockAdjustment (manual +/- corrections)                            │
│       └── InvStockTransfer (FromWarehouse → ToWarehouse)                         │
└──────────────────────────────────────────────────────────────────────────────────┘

┌──────────────────────────────────────────────────────────────────────────────────┐
│                             PURCHASE                                             │
│                                                                                  │
│  PurchSupplier                                                                   │
│       │                                                                          │
│       ├── PurchOrder ──► PurchOrderItem                          🔲 NEW           │
│       │       │                                                                  │
│       ├── PurchGoodsReceive ──► PurchGoodsReceiveItem            🔲 NEW           │
│       │       │         (→ stock IN via InvStockMovement)                         │
│       │       │                                                                  │
│       ├── PurchInvoice ──► PurchItem                             ⚠️ UPDATE        │
│       │       │                                                                  │
│       └── PurchPayment                                          🔲 NEW           │
└──────────────────────────────────────────────────────────────────────────────────┘

┌──────────────────────────────────────────────────────────────────────────────────┐
│                              SALES                                               │
│                                                                                  │
│  SalesCustomer                                                                   │
│       │                                                                          │
│       ├── SalesQuotation ──► SalesQuotationItem                 🔲 NEW           │
│       │       │                                                                  │
│       ├── SalesOrder ──► SalesOrderItem                         🔲 NEW           │
│       │       │                                                                  │
│       ├── SalesInvoice ──► SalesInvoiceItem                     ⚠️ UPDATE        │
│       │       │         (→ stock OUT via InvStockMovement)                        │
│       │       │                                                                  │
│       └── SalesPayment                                          🔲 NEW           │
└──────────────────────────────────────────────────────────────────────────────────┘

┌──────────────────────────────────────────────────────────────────────────────────┐
│                              VIEWS                                               │
│  VwProdUnitConversion (read-only, joins Product + FromUnit + ToUnit)             │
└──────────────────────────────────────────────────────────────────────────────────┘
```

---

## Complete Entity Count

| Module | Entity | Status | Table Count |
|---|---|---|---|
| **Auth** | AuthUser | ✅ Existing | 1 |
| | AuthRefreshToken | ✅ Existing | 1 |
| **Product** | ProdUnit | ✅ Existing | 1 |
| | ProdBrand | ✅ Existing | 1 |
| | ProdCategory | ✅ Existing | 1 |
| | ProdGroup | ✅ Existing | 1 |
| | ProdItem | ✅ Existing | 1 |
| | ProdUnitConversion | ✅ Existing | 1 |
| | ProdUnitPrice | ✅ Existing | 1 |
| | ProdBatch | ✅ Existing | 1 |
| | ProdSerial | ✅ Existing | 1 |
| **Inventory** | InvWarehouse | ✅ Existing | 1 |
| | InvWarehouseStock | ✅ Existing | 1 |
| | InvStockMovement | ✅ Existing | 1 |
| | InvStockAdjustment | ✅ Existing | 1 |
| | InvStockTransfer | ✅ Existing | 1 |
| **Purchase** | PurchSupplier | ✅ Existing | 1 |
| | PurchInvoice | ⚠️ Update | 1 |
| | PurchItem | ✅ Existing | 1 |
| | PurchOrder | 🔲 New | 1 |
| | PurchOrderItem | 🔲 New | 1 |
| | PurchGoodsReceive | 🔲 New | 1 |
| | PurchGoodsReceiveItem | 🔲 New | 1 |
| | PurchPayment | 🔲 New | 1 |
| **Sales** | SalesCustomer | ✅ Existing | 1 |
| | SalesInvoice | ⚠️ Update | 1 |
| | SalesInvoiceItem | ✅ Existing | 1 |
| | SalesQuotation | 🔲 New | 1 |
| | SalesQuotationItem | 🔲 New | 1 |
| | SalesOrder | 🔲 New | 1 |
| | SalesOrderItem | 🔲 New | 1 |
| | SalesPayment | 🔲 New | 1 |
| **View** | VwProdUnitConversion | ✅ Existing | — |
| | | **Total** | **32 tables + 1 view** |

---

# SECTION A — AUTH MODULE (✅ Existing)

---

## 1. `AuthUser` — Staff / System Accounts

```sql
CREATE TABLE AuthUser (
    Id              NVARCHAR(50)    PRIMARY KEY,
    Username        NVARCHAR(100)   NOT NULL,
    FirstName       NVARCHAR(100)   NOT NULL,
    LastName        NVARCHAR(100)   NOT NULL,
    Email           NVARCHAR(150)   NOT NULL,
    PasswordHash    NVARCHAR(500)   NOT NULL,              -- bcrypt / argon2
    Role            NVARCHAR(30)    NOT NULL,              -- admin, manager, staff, etc.
    Active          BIT             NOT NULL DEFAULT 1,
    CreatedAt       DATETIME2       NOT NULL,
    CreatedBy       NVARCHAR(50)    NULL,
    UpdatedAt       DATETIME2       NULL,
    UpdatedBy       NVARCHAR(50)    NULL,
    LastAction      NVARCHAR(50)    NULL
);
```

**Relationships**: Referenced by `CreatedBy` / `UpdatedBy` audit fields across all tables.

---

## 2. `AuthRefreshToken` — JWT Refresh Tokens

```sql
CREATE TABLE AuthRefreshToken (
    Id              NVARCHAR(50)    PRIMARY KEY,
    Token           NVARCHAR(500)   NOT NULL,
    UserId          NVARCHAR(50)    NOT NULL,              -- FK → AuthUser
    ExpiresAt       DATETIME2       NOT NULL,
    CreatedAt       DATETIME2       NOT NULL,
    IsRevoked       BIT             NOT NULL DEFAULT 0
);
```

**Relationships**: `UserId` → `AuthUser.Id`

---

# SECTION B — PRODUCT MASTER (✅ Existing)

---

## 3. `ProdUnit` — Measurement Units

```sql
CREATE TABLE ProdUnit (
    Id              NVARCHAR(50)    PRIMARY KEY,
    Name            NVARCHAR(100)   NOT NULL,              -- e.g., Piece, Kilogram, Box
    Symbol          NVARCHAR(20)    NOT NULL,              -- e.g., pc, kg, bx
    Active          BIT             NOT NULL DEFAULT 1,
    CreatedAt       DATETIME2       NOT NULL,
    CreatedBy       NVARCHAR(50)    NULL,
    UpdatedAt       DATETIME2       NULL,
    UpdatedBy       NVARCHAR(50)    NULL,
    LastAction      NVARCHAR(50)    NULL
);
```

**Referenced by**: ProdItem.BaseUnitId, ProdUnitConversion, ProdUnitPrice, PurchItem.UnitId, SalesInvoiceItem.UnitId, all order/quotation items.

---

## 4. `ProdBrand` — Product Brands

```sql
CREATE TABLE ProdBrand (
    Id              NVARCHAR(50)    PRIMARY KEY,
    Name            NVARCHAR(150)   NOT NULL,              -- e.g., 3M, Philips
    Description     NVARCHAR(MAX)   NULL,
    Active          BIT             NOT NULL DEFAULT 1,
    CreatedAt       DATETIME2       NOT NULL,
    UpdatedAt       DATETIME2       NULL,
    CreatedBy       NVARCHAR(50)    NULL,
    UpdatedBy       NVARCHAR(50)    NULL,
    LastAction      NVARCHAR(50)    NULL
);
```

**Referenced by**: ProdItem.BrandId

---

## 5. `ProdCategory` — Product Categories (Hierarchical)

```sql
CREATE TABLE ProdCategory (
    Id                  NVARCHAR(50)    PRIMARY KEY,
    Code                NVARCHAR(30)    NOT NULL,
    Name                NVARCHAR(150)   NOT NULL,
    ParentCategoryId    NVARCHAR(50)    NULL,              -- FK → self (parent-child tree)
    Description         NVARCHAR(MAX)   NULL,
    IsUsed              BIT             NOT NULL DEFAULT 0,
    Active              BIT             NOT NULL DEFAULT 1,
    CreatedAt           DATETIME2       NOT NULL,
    CreatedBy           NVARCHAR(50)    NULL,
    UpdatedAt           DATETIME2       NULL,
    UpdatedBy           NVARCHAR(50)    NULL,
    LastAction          NVARCHAR(50)    NULL,

    CONSTRAINT FK_ProdCategory_Parent FOREIGN KEY (ParentCategoryId) REFERENCES ProdCategory(Id)
);
```

**Self-referencing**: Supports unlimited category nesting (Electronics → Computers → Laptops).
**Referenced by**: ProdItem.CategoryId

---

## 6. `ProdGroup` — Product Groups

```sql
CREATE TABLE ProdGroup (
    Id              NVARCHAR(50)    PRIMARY KEY,
    Name            NVARCHAR(150)   NOT NULL,              -- e.g., Finished Goods, Raw Materials
    Description     NVARCHAR(MAX)   NULL,
    Active          BIT             NOT NULL DEFAULT 1,
    CreatedAt       DATETIME2       NOT NULL,
    UpdatedAt       DATETIME2       NULL,
    CreatedBy       NVARCHAR(50)    NULL,
    UpdatedBy       NVARCHAR(50)    NULL,
    LastAction      NVARCHAR(50)    NULL
);
```

**Referenced by**: ProdItem.GroupId

---

## 7. `ProdItem` — Product / Item Master

```sql
CREATE TABLE ProdItem (
    Id                  NVARCHAR(50)    PRIMARY KEY,
    Code                NVARCHAR(30)    NOT NULL,          -- e.g., PROD-001
    Name                NVARCHAR(200)   NOT NULL,
    GroupId             NVARCHAR(50)    NULL,              -- FK → ProdGroup
    CategoryId          NVARCHAR(50)    NULL,              -- FK → ProdCategory
    BrandId             NVARCHAR(50)    NULL,              -- FK → ProdBrand
    Description         NVARCHAR(MAX)   NULL,
    BaseUnitId          NVARCHAR(50)    NOT NULL,          -- FK → ProdUnit (smallest unit)
    MinimumStock        DECIMAL(18,4)   NULL,              -- alert: stock below this
    MaximumStock        DECIMAL(18,4)   NULL,
    ReorderLevel        DECIMAL(18,4)   NULL,              -- trigger reorder point
    Barcode             NVARCHAR(100)   NULL,
    TrackType           INT             NOT NULL DEFAULT 0,-- 0:None, 1:Batch, 2:Serial
    HasVariant          BIT             NOT NULL DEFAULT 0,
    AllowNegativeStock  BIT             NULL DEFAULT 0,
    Active              BIT             NOT NULL DEFAULT 1,
    CreatedAt           DATETIME2       NOT NULL,
    CreatedBy           NVARCHAR(50)    NULL,
    UpdatedAt           DATETIME2       NULL,
    UpdatedBy           NVARCHAR(50)    NULL,
    LastAction          NVARCHAR(50)    NULL,

    CONSTRAINT FK_ProdItem_Unit     FOREIGN KEY (BaseUnitId)  REFERENCES ProdUnit(Id),
    CONSTRAINT FK_ProdItem_Brand    FOREIGN KEY (BrandId)     REFERENCES ProdBrand(Id),
    CONSTRAINT FK_ProdItem_Category FOREIGN KEY (CategoryId)  REFERENCES ProdCategory(Id),
    CONSTRAINT FK_ProdItem_Group    FOREIGN KEY (GroupId)      REFERENCES ProdGroup(Id)
);
```

**Central entity** — referenced by all transaction line items, inventory, batch, serial.

**TrackType values**:
| Value | Type | Behavior |
|---|---|---|
| 0 | None | Simple stock count, no batch/serial |
| 1 | Batch | Track by ProdBatch (batch no, mfg date, expiry) |
| 2 | Serial | Track by ProdSerial (unique serial per unit) |

---

## 8. `ProdUnitConversion` — Unit Conversion Rules

```sql
CREATE TABLE ProdUnitConversion (
    Id              NVARCHAR(50)    PRIMARY KEY,
    ProductId       NVARCHAR(50)    NOT NULL,              -- FK → ProdItem
    FromUnitId      NVARCHAR(50)    NOT NULL,              -- FK → ProdUnit
    ToUnitId        NVARCHAR(50)    NOT NULL,              -- FK → ProdUnit
    Factor          DECIMAL(18,6)   NOT NULL,              -- 1 FromUnit = Factor × ToUnit
    Active          BIT             NOT NULL DEFAULT 1,
    CreatedAt       DATETIME2       NOT NULL,
    CreatedBy       NVARCHAR(50)    NULL,
    UpdatedAt       DATETIME2       NULL,
    UpdatedBy       NVARCHAR(50)    NULL,
    LastAction      NVARCHAR(50)    NULL,

    CONSTRAINT FK_ProdUnitConv_Product  FOREIGN KEY (ProductId)  REFERENCES ProdItem(Id),
    CONSTRAINT FK_ProdUnitConv_From     FOREIGN KEY (FromUnitId) REFERENCES ProdUnit(Id),
    CONSTRAINT FK_ProdUnitConv_To       FOREIGN KEY (ToUnitId)   REFERENCES ProdUnit(Id)
);
```

**Example**: Product "Resin" → 1 Box (FromUnit) = 12 Pieces (ToUnit), Factor = 12

---

## 9. `ProdUnitPrice` — Sale Price Per Unit

```sql
CREATE TABLE ProdUnitPrice (
    Id              NVARCHAR(50)    PRIMARY KEY,
    ProductId       NVARCHAR(50)    NOT NULL,              -- FK → ProdItem
    UnitId          NVARCHAR(50)    NOT NULL,              -- FK → ProdUnit
    SalePrice       DECIMAL(18,2)   NOT NULL,
    Active          BIT             NOT NULL DEFAULT 1,
    CreatedAt       DATETIME2       NOT NULL,
    CreatedBy       NVARCHAR(50)    NULL,
    UpdatedAt       DATETIME2       NULL,
    UpdatedBy       NVARCHAR(50)    NULL,
    LastAction      NVARCHAR(50)    NULL,

    CONSTRAINT FK_ProdUnitPrice_Product FOREIGN KEY (ProductId) REFERENCES ProdItem(Id),
    CONSTRAINT FK_ProdUnitPrice_Unit    FOREIGN KEY (UnitId)    REFERENCES ProdUnit(Id)
);
```

**Usage**: Auto-fills UnitPrice in Sales Quotation / Order / Invoice when product + unit selected.

---

## 10. `ProdBatch` — Batch Tracking

```sql
CREATE TABLE ProdBatch (
    Id                  NVARCHAR(50)    PRIMARY KEY,
    ProductId           NVARCHAR(50)    NOT NULL,          -- FK → ProdItem
    WarehouseId         NVARCHAR(50)    NOT NULL,          -- FK → InvWarehouse
    BatchNo             NVARCHAR(100)   NOT NULL,          -- batch number
    Quantity            DECIMAL(18,4)   NOT NULL,          -- current qty in this batch
    ManufactureDate     DATETIME2       NULL,
    ExpiryDate          DATETIME2       NULL,
    Active              BIT             NOT NULL DEFAULT 1,
    CreatedAt           DATETIME2       NOT NULL,
    CreatedBy           NVARCHAR(50)    NULL,
    UpdatedAt           DATETIME2       NULL,
    UpdatedBy           NVARCHAR(50)    NULL,
    LastAction          NVARCHAR(50)    NULL,

    CONSTRAINT FK_ProdBatch_Product   FOREIGN KEY (ProductId)   REFERENCES ProdItem(Id),
    CONSTRAINT FK_ProdBatch_Warehouse FOREIGN KEY (WarehouseId) REFERENCES InvWarehouse(Id)
);
```

**Used by**: PurchItem.BatchId, SalesInvoiceItem.BatchId, PurchGoodsReceiveItem.BatchId, InvStockMovement.BatchId
**Only for**: Products with TrackType = 1 (Batch)

---

## 11. `ProdSerial` — Serial Number Tracking

```sql
CREATE TABLE ProdSerial (
    Id                  NVARCHAR(50)    PRIMARY KEY,
    ProductId           NVARCHAR(50)    NOT NULL,          -- FK → ProdItem
    WarehouseId         NVARCHAR(50)    NOT NULL,          -- FK → InvWarehouse
    SerialNo            NVARCHAR(100)   NOT NULL,          -- unique serial number
    Quantity            DECIMAL(18,4)   NOT NULL,          -- typically 1
    ManufactureDate     DATETIME2       NULL,
    ExpiryDate          DATETIME2       NULL,
    Active              BIT             NOT NULL DEFAULT 1,
    CreatedAt           DATETIME2       NOT NULL,
    CreatedBy           NVARCHAR(50)    NULL,
    UpdatedAt           DATETIME2       NULL,
    UpdatedBy           NVARCHAR(50)    NULL,
    LastAction          NVARCHAR(50)    NULL,

    CONSTRAINT FK_ProdSerial_Product   FOREIGN KEY (ProductId)   REFERENCES ProdItem(Id),
    CONSTRAINT FK_ProdSerial_Warehouse FOREIGN KEY (WarehouseId) REFERENCES InvWarehouse(Id)
);
```

**Used by**: PurchItem.SerialId, SalesInvoiceItem.SerialId, PurchGoodsReceiveItem.SerialId, InvStockMovement.SerialId
**Only for**: Products with TrackType = 2 (Serial)

---

# SECTION C — INVENTORY MODULE (✅ Existing)

---

## 12. `InvWarehouse` — Warehouse / Storage Locations

```sql
CREATE TABLE InvWarehouse (
    Id                  NVARCHAR(50)    PRIMARY KEY,
    Name                NVARCHAR(150)   NOT NULL,
    City                NVARCHAR(100)   NULL,
    BranchType          NVARCHAR(50)    NOT NULL,          -- Main, Branch, Virtual
    IsMainWarehouse     BIT             NOT NULL DEFAULT 0,
    ParentWarehouseId   NVARCHAR(50)    NULL,              -- FK → self (parent-child)
    IsUsedWarehouse     BIT             NOT NULL DEFAULT 0,-- can transactions use this WH?
    Location            NVARCHAR(200)   NULL,
    Address             NVARCHAR(500)   NULL,
    Country             NVARCHAR(100)   NULL,
    ContactPerson       NVARCHAR(100)   NULL,
    Phone               NVARCHAR(30)    NULL,
    Active              BIT             NOT NULL DEFAULT 1,
    CreatedAt           DATETIME2       NOT NULL,
    UpdatedAt           DATETIME2       NULL,
    CreatedBy           NVARCHAR(50)    NULL,
    UpdatedBy           NVARCHAR(50)    NULL,
    LastAction          NVARCHAR(50)    NULL
);
```

**Self-referencing**: ParentWarehouseId for warehouse hierarchy.
**Referenced by**: All transactions requiring a warehouse (PO, GRN, Invoice, Transfer, etc.)

---

## 13. `InvWarehouseStock` — Current Stock Per Product Per Warehouse

```sql
CREATE TABLE InvWarehouseStock (
    Id                  NVARCHAR(50)    PRIMARY KEY,
    WarehouseId         NVARCHAR(50)    NOT NULL,          -- FK → InvWarehouse
    ProductId           NVARCHAR(50)    NOT NULL,          -- FK → ProdItem
    AvailableQuantity   DECIMAL(18,4)   NOT NULL DEFAULT 0,-- free to sell/use
    ReservedQuantity    DECIMAL(18,4)   NOT NULL DEFAULT 0,-- reserved by confirmed SO
    Active              BIT             NOT NULL DEFAULT 1,
    CreatedAt           DATETIME2       NOT NULL,
    UpdatedAt           DATETIME2       NULL,
    CreatedBy           NVARCHAR(50)    NULL,
    UpdatedBy           NVARCHAR(50)    NULL,
    LastAction          NVARCHAR(50)    NULL,

    CONSTRAINT FK_InvWHStock_Warehouse FOREIGN KEY (WarehouseId) REFERENCES InvWarehouse(Id),
    CONSTRAINT FK_InvWHStock_Product   FOREIGN KEY (ProductId)   REFERENCES ProdItem(Id)
);
```

**Updated by**: GRN (+), Sales Invoice (-), Stock Adjustment (+/-), Stock Transfer (+/-)
**Unique key**: (WarehouseId, ProductId) — one record per product per warehouse.

---

## 14. `InvStockMovement` — Stock Movement Audit Log

```sql
CREATE TABLE InvStockMovement (
    Id                  NVARCHAR(50)    PRIMARY KEY,
    ProductId           NVARCHAR(50)    NOT NULL,          -- FK → ProdItem
    WarehouseId         NVARCHAR(50)    NOT NULL,          -- FK → InvWarehouse
    MovementType        NVARCHAR(20)    NOT NULL,          -- IN, OUT, ADJUST, TRANSFER_IN, TRANSFER_OUT
    ReferenceType       INT             NOT NULL,          -- 1:GRN, 2:SalesInv, 3:Adjust, 4:Transfer, 5:PurchInv
    ReferenceId         NVARCHAR(50)    NOT NULL,          -- FK → source document Id
    BaseQuantity        DECIMAL(18,4)   NOT NULL,          -- qty in base unit (always positive)
    BatchId             NVARCHAR(50)    NULL,              -- FK → ProdBatch
    SerialId            NVARCHAR(50)    NULL,              -- FK → ProdSerial
    MovementDate        DATETIME2       NOT NULL,
    Notes               NVARCHAR(MAX)   NULL,
    Active              BIT             NOT NULL DEFAULT 1,
    CreatedAt           DATETIME2       NOT NULL,
    CreatedBy           NVARCHAR(50)    NULL,
    UpdatedAt           DATETIME2       NULL,
    UpdatedBy           NVARCHAR(50)    NULL,
    LastAction          NVARCHAR(50)    NULL,

    CONSTRAINT FK_InvMove_Product   FOREIGN KEY (ProductId)   REFERENCES ProdItem(Id),
    CONSTRAINT FK_InvMove_Warehouse FOREIGN KEY (WarehouseId) REFERENCES InvWarehouse(Id),
    CONSTRAINT FK_InvMove_Batch     FOREIGN KEY (BatchId)     REFERENCES ProdBatch(Id),
    CONSTRAINT FK_InvMove_Serial    FOREIGN KEY (SerialId)    REFERENCES ProdSerial(Id)
);
```

**Read-only audit log** — never edited directly. Created automatically by:

| ReferenceType | Source | MovementType | Effect |
|---|---|---|---|
| 1 | GRN (Goods Receiving) | IN | +qty |
| 2 | Sales Invoice | OUT | -qty |
| 3 | Stock Adjustment | ADJUST | +/- qty |
| 4 | Stock Transfer | TRANSFER_IN / TRANSFER_OUT | move between WHs |
| 5 | Purchase Invoice (direct) | IN | +qty (when no GRN) |

---

## 15. `InvStockAdjustment` — Manual Stock Corrections

```sql
CREATE TABLE InvStockAdjustment (
    Id                  NVARCHAR(50)    PRIMARY KEY,
    AdjustmentNo        NVARCHAR(30)    NOT NULL,          -- auto: ADJ-20260001
    WarehouseId         NVARCHAR(50)    NOT NULL,          -- FK → InvWarehouse
    ProductId           NVARCHAR(50)    NOT NULL,          -- FK → ProdItem
    AdjustmentQuantity  DECIMAL(18,4)   NOT NULL,          -- positive (+) or negative (-)
    Reason              NVARCHAR(MAX)   NULL,              -- Damaged, Expired, Count correction
    AdjustmentDate      DATETIME2       NOT NULL,
    Active              BIT             NOT NULL DEFAULT 1,
    CreatedAt           DATETIME2       NOT NULL,
    CreatedBy           NVARCHAR(50)    NULL,
    UpdatedAt           DATETIME2       NULL,
    UpdatedBy           NVARCHAR(50)    NULL,
    LastAction          NVARCHAR(50)    NULL,

    CONSTRAINT FK_InvAdj_Product   FOREIGN KEY (ProductId)   REFERENCES ProdItem(Id),
    CONSTRAINT FK_InvAdj_Warehouse FOREIGN KEY (WarehouseId) REFERENCES InvWarehouse(Id)
);
```

**On create**: Creates InvStockMovement (ADJUST) + updates InvWarehouseStock.

---

## 16. `InvStockTransfer` — Warehouse-to-Warehouse Transfer

```sql
CREATE TABLE InvStockTransfer (
    Id                  NVARCHAR(50)    PRIMARY KEY,
    TransferNo          NVARCHAR(30)    NOT NULL,          -- auto: TRF-20260001
    FromWarehouseId     NVARCHAR(50)    NOT NULL,          -- FK → InvWarehouse (source)
    ToWarehouseId       NVARCHAR(50)    NOT NULL,          -- FK → InvWarehouse (destination)
    ProductId           NVARCHAR(50)    NOT NULL,          -- FK → ProdItem
    Quantity            DECIMAL(18,4)   NOT NULL,
    TransferDate        DATETIME2       NOT NULL,
    Status              INT             NOT NULL DEFAULT 0,-- 0:Draft, 1:Completed, 2:Cancelled
    Notes               NVARCHAR(MAX)   NULL,
    Active              BIT             NOT NULL DEFAULT 1,
    CreatedAt           DATETIME2       NOT NULL,
    CreatedBy           NVARCHAR(50)    NULL,
    UpdatedAt           DATETIME2       NULL,
    UpdatedBy           NVARCHAR(50)    NULL,
    LastAction          NVARCHAR(50)    NULL,

    CONSTRAINT FK_InvTransfer_From    FOREIGN KEY (FromWarehouseId) REFERENCES InvWarehouse(Id),
    CONSTRAINT FK_InvTransfer_To      FOREIGN KEY (ToWarehouseId)   REFERENCES InvWarehouse(Id),
    CONSTRAINT FK_InvTransfer_Product FOREIGN KEY (ProductId)       REFERENCES ProdItem(Id)
);
```

**On confirm**: Creates 2 InvStockMovement records (TRANSFER_OUT for source, TRANSFER_IN for destination) + updates both InvWarehouseStock.

---

# SECTION D — PURCHASE MODULE

---

## 17. `PurchSupplier` — Suppliers (✅ Existing)

```sql
CREATE TABLE PurchSupplier (
    Id                  NVARCHAR(50)    PRIMARY KEY,
    Code                NVARCHAR(30)    NOT NULL,          -- e.g., SUP-001
    Name                NVARCHAR(200)   NOT NULL,
    ContactPerson       NVARCHAR(100)   NULL,
    Email               NVARCHAR(150)   NULL,
    Phone               NVARCHAR(30)    NULL,
    Address             NVARCHAR(500)   NULL,
    City                NVARCHAR(100)   NULL,
    Country             NVARCHAR(100)   NULL,
    TaxNumber           NVARCHAR(50)    NULL,              -- TIN / Tax ID
    PaymentTermDays     INT             NULL,              -- e.g., 30 (Net 30)
    Active              BIT             NOT NULL DEFAULT 1,
    CreatedAt           DATETIME2       NOT NULL,
    CreatedBy           NVARCHAR(50)    NULL,
    UpdatedAt           DATETIME2       NULL,
    UpdatedBy           NVARCHAR(50)    NULL,
    LastAction          NVARCHAR(50)    NULL
);
```

**Referenced by**: PurchOrder, PurchGoodsReceive, PurchInvoice

---

## 18. `PurchOrder` — Purchase Order Header (🔲 New)

```sql
CREATE TABLE PurchOrder (
    Id                  NVARCHAR(50)    PRIMARY KEY,
    OrderNumber         NVARCHAR(30)    NOT NULL UNIQUE,   -- auto: PO-20260001
    OrderDate           DATETIME2       NOT NULL,
    SupplierId          NVARCHAR(50)    NOT NULL,          -- FK → PurchSupplier
    WarehouseId         NVARCHAR(50)    NOT NULL,          -- FK → InvWarehouse (destination)
    SubTotal            DECIMAL(18,2)   NOT NULL DEFAULT 0,
    TotalDiscount       DECIMAL(18,2)   NULL DEFAULT 0,
    TotalTax            DECIMAL(18,2)   NULL DEFAULT 0,
    TotalAmount         DECIMAL(18,2)   NOT NULL DEFAULT 0,
    Status              INT             NOT NULL DEFAULT 0,-- 0:Draft, 1:Approved, 2:PartiallyReceived, 3:FullyReceived, 4:Closed, 5:Cancelled
    ExpectedDate        DATETIME2       NULL,
    Notes               NVARCHAR(MAX)   NULL,
    Active              BIT             NOT NULL DEFAULT 1,
    CreatedAt           DATETIME2       NOT NULL,
    CreatedBy           NVARCHAR(50)    NULL,
    UpdatedAt           DATETIME2       NULL,
    UpdatedBy           NVARCHAR(50)    NULL,
    LastAction          NVARCHAR(50)    NULL,

    CONSTRAINT FK_PurchOrder_Supplier   FOREIGN KEY (SupplierId)  REFERENCES PurchSupplier(Id),
    CONSTRAINT FK_PurchOrder_Warehouse  FOREIGN KEY (WarehouseId) REFERENCES InvWarehouse(Id)
);
```

**Referenced by**: PurchGoodsReceive.PurchaseOrderId, PurchInvoice.PurchaseOrderId

---

## 19. `PurchOrderItem` — Purchase Order Line Items (🔲 New)

```sql
CREATE TABLE PurchOrderItem (
    Id                  NVARCHAR(50)    PRIMARY KEY,
    PurchaseOrderId     NVARCHAR(50)    NOT NULL,          -- FK → PurchOrder
    ProductId           NVARCHAR(50)    NOT NULL,          -- FK → ProdItem
    UnitId              NVARCHAR(50)    NOT NULL,          -- FK → ProdUnit
    Quantity            DECIMAL(18,4)   NOT NULL,
    ReceivedQuantity    DECIMAL(18,4)   NOT NULL DEFAULT 0,-- tracks how much received via GRN
    UnitCost            DECIMAL(18,2)   NOT NULL,
    DiscountPercent     DECIMAL(18,2)   NULL DEFAULT 0,
    DiscountAmount      DECIMAL(18,2)   NULL DEFAULT 0,
    TaxPercent          DECIMAL(18,2)   NULL DEFAULT 0,
    TaxAmount           DECIMAL(18,2)   NULL DEFAULT 0,
    TotalAmount         DECIMAL(18,2)   NOT NULL DEFAULT 0,
    Notes               NVARCHAR(MAX)   NULL,
    Active              BIT             NOT NULL DEFAULT 1,
    CreatedAt           DATETIME2       NOT NULL,
    CreatedBy           NVARCHAR(50)    NULL,
    UpdatedAt           DATETIME2       NULL,
    UpdatedBy           NVARCHAR(50)    NULL,
    LastAction          NVARCHAR(50)    NULL,

    CONSTRAINT FK_POItem_Order   FOREIGN KEY (PurchaseOrderId) REFERENCES PurchOrder(Id),
    CONSTRAINT FK_POItem_Product FOREIGN KEY (ProductId)       REFERENCES ProdItem(Id),
    CONSTRAINT FK_POItem_Unit    FOREIGN KEY (UnitId)          REFERENCES ProdUnit(Id)
);
```

**ReceivedQuantity**: Updated automatically when GRN is confirmed. Used to determine PO status (partial vs full).

---

## 20. `PurchGoodsReceive` — Goods Receiving Note Header (🔲 New)

```sql
CREATE TABLE PurchGoodsReceive (
    Id                  NVARCHAR(50)    PRIMARY KEY,
    ReceiveNumber       NVARCHAR(30)    NOT NULL UNIQUE,   -- auto: GRN-20260001
    ReceiveDate         DATETIME2       NOT NULL,
    PurchaseOrderId     NVARCHAR(50)    NULL,              -- FK → PurchOrder (nullable for standalone)
    SupplierId          NVARCHAR(50)    NOT NULL,          -- FK → PurchSupplier
    WarehouseId         NVARCHAR(50)    NOT NULL,          -- FK → InvWarehouse
    Status              INT             NOT NULL DEFAULT 0,-- 0:Draft, 1:Received, 2:Cancelled
    Notes               NVARCHAR(MAX)   NULL,
    Active              BIT             NOT NULL DEFAULT 1,
    CreatedAt           DATETIME2       NOT NULL,
    CreatedBy           NVARCHAR(50)    NULL,
    UpdatedAt           DATETIME2       NULL,
    UpdatedBy           NVARCHAR(50)    NULL,
    LastAction          NVARCHAR(50)    NULL,

    CONSTRAINT FK_GRN_Order     FOREIGN KEY (PurchaseOrderId) REFERENCES PurchOrder(Id),
    CONSTRAINT FK_GRN_Supplier  FOREIGN KEY (SupplierId)      REFERENCES PurchSupplier(Id),
    CONSTRAINT FK_GRN_Warehouse FOREIGN KEY (WarehouseId)     REFERENCES InvWarehouse(Id)
);
```

**On confirm receive**: Triggers stock-in via InvStockMovement + InvWarehouseStock update.

---

## 21. `PurchGoodsReceiveItem` — GRN Line Items (🔲 New)

```sql
CREATE TABLE PurchGoodsReceiveItem (
    Id                  NVARCHAR(50)    PRIMARY KEY,
    GoodsReceiveId      NVARCHAR(50)    NOT NULL,          -- FK → PurchGoodsReceive
    PurchaseOrderItemId NVARCHAR(50)    NULL,              -- FK → PurchOrderItem (nullable for standalone)
    ProductId           NVARCHAR(50)    NOT NULL,          -- FK → ProdItem
    UnitId              NVARCHAR(50)    NOT NULL,          -- FK → ProdUnit
    Quantity            DECIMAL(18,4)   NOT NULL,          -- received qty
    UnitCost            DECIMAL(18,2)   NOT NULL,
    BatchId             NVARCHAR(50)    NULL,              -- FK → ProdBatch (if TrackType=1)
    SerialId            NVARCHAR(50)    NULL,              -- FK → ProdSerial (if TrackType=2)
    Notes               NVARCHAR(MAX)   NULL,
    Active              BIT             NOT NULL DEFAULT 1,
    CreatedAt           DATETIME2       NOT NULL,
    CreatedBy           NVARCHAR(50)    NULL,
    UpdatedAt           DATETIME2       NULL,
    UpdatedBy           NVARCHAR(50)    NULL,
    LastAction          NVARCHAR(50)    NULL,

    CONSTRAINT FK_GRNItem_GRN     FOREIGN KEY (GoodsReceiveId)      REFERENCES PurchGoodsReceive(Id),
    CONSTRAINT FK_GRNItem_POItem  FOREIGN KEY (PurchaseOrderItemId)  REFERENCES PurchOrderItem(Id),
    CONSTRAINT FK_GRNItem_Product FOREIGN KEY (ProductId)            REFERENCES ProdItem(Id),
    CONSTRAINT FK_GRNItem_Unit    FOREIGN KEY (UnitId)               REFERENCES ProdUnit(Id),
    CONSTRAINT FK_GRNItem_Batch   FOREIGN KEY (BatchId)              REFERENCES ProdBatch(Id),
    CONSTRAINT FK_GRNItem_Serial  FOREIGN KEY (SerialId)             REFERENCES ProdSerial(Id)
);
```

---

## 22. `PurchInvoice` — Purchase Invoice Header (⚠️ Update)

```sql
CREATE TABLE PurchInvoice (
    Id                  NVARCHAR(50)    PRIMARY KEY,
    PurchaseOrderNumber NVARCHAR(30)    NOT NULL,          -- auto: PI-20260001
    PurchaseDate        DATETIME2       NOT NULL,
    SupplierId          NVARCHAR(50)    NOT NULL,          -- FK → PurchSupplier
    WarehouseId         NVARCHAR(50)    NOT NULL,          -- FK → InvWarehouse
    PurchaseOrderId     NVARCHAR(50)    NULL,              -- FK → PurchOrder         ⬅ NEW COLUMN
    GoodsReceiveId      NVARCHAR(50)    NULL,              -- FK → PurchGoodsReceive  ⬅ NEW COLUMN
    SubTotal            DECIMAL(18,2)   NOT NULL DEFAULT 0,
    TotalDiscount       DECIMAL(18,2)   NULL DEFAULT 0,
    TotalTax            DECIMAL(18,2)   NULL DEFAULT 0,
    TotalAmount         DECIMAL(18,2)   NOT NULL DEFAULT 0,
    PaidAmount          DECIMAL(18,2)   NULL DEFAULT 0,    -- auto-calc from PurchPayment SUM
    PaymentStatus       INT             NOT NULL DEFAULT 0,-- 0:Unpaid, 1:PartiallyPaid, 2:Paid
    Status              INT             NOT NULL DEFAULT 0,-- 0:Draft, 1:Confirmed, 2:Cancelled
    ExpectedDate        DATETIME2       NULL,              -- payment due date
    ReceivedDate        DATETIME2       NULL,
    Notes               NVARCHAR(MAX)   NULL,
    Active              BIT             NOT NULL DEFAULT 1,
    CreatedAt           DATETIME2       NOT NULL,
    CreatedBy           NVARCHAR(50)    NULL,
    UpdatedAt           DATETIME2       NULL,
    UpdatedBy           NVARCHAR(50)    NULL,
    LastAction          NVARCHAR(50)    NULL,

    CONSTRAINT FK_PurchInv_Supplier  FOREIGN KEY (SupplierId)      REFERENCES PurchSupplier(Id),
    CONSTRAINT FK_PurchInv_Warehouse FOREIGN KEY (WarehouseId)     REFERENCES InvWarehouse(Id),
    CONSTRAINT FK_PurchInv_PO        FOREIGN KEY (PurchaseOrderId) REFERENCES PurchOrder(Id),
    CONSTRAINT FK_PurchInv_GRN       FOREIGN KEY (GoodsReceiveId)  REFERENCES PurchGoodsReceive(Id)
);
```

**Changes from existing**:
- Added `PurchaseOrderId` — links invoice to PO (nullable for standalone invoices)
- Added `GoodsReceiveId` — links invoice to GRN (nullable)

---

## 23. `PurchItem` — Purchase Invoice Line Items (✅ Existing)

```sql
CREATE TABLE PurchItem (
    Id                  NVARCHAR(50)    PRIMARY KEY,
    PurchaseId          NVARCHAR(50)    NOT NULL,          -- FK → PurchInvoice
    ProductId           NVARCHAR(50)    NOT NULL,          -- FK → ProdItem
    UnitId              NVARCHAR(50)    NOT NULL,          -- FK → ProdUnit
    Quantity            DECIMAL(18,4)   NOT NULL,
    UnitCost            DECIMAL(18,2)   NOT NULL,
    DiscountPercent     DECIMAL(18,2)   NULL DEFAULT 0,
    DiscountAmount      DECIMAL(18,2)   NULL DEFAULT 0,
    TaxPercent          DECIMAL(18,2)   NULL DEFAULT 0,
    TaxAmount           DECIMAL(18,2)   NULL DEFAULT 0,
    TotalAmount         DECIMAL(18,2)   NOT NULL DEFAULT 0,
    Notes               NVARCHAR(MAX)   NULL,
    BatchId             NVARCHAR(50)    NULL,              -- FK → ProdBatch
    SerialId            NVARCHAR(50)    NULL,              -- FK → ProdSerial
    Active              BIT             NOT NULL DEFAULT 1,
    CreatedAt           DATETIME2       NOT NULL,
    CreatedBy           NVARCHAR(50)    NULL,
    UpdatedAt           DATETIME2       NULL,
    UpdatedBy           NVARCHAR(50)    NULL,
    LastAction          NVARCHAR(50)    NULL,

    CONSTRAINT FK_PurchItem_Invoice FOREIGN KEY (PurchaseId) REFERENCES PurchInvoice(Id),
    CONSTRAINT FK_PurchItem_Product FOREIGN KEY (ProductId)  REFERENCES ProdItem(Id),
    CONSTRAINT FK_PurchItem_Unit    FOREIGN KEY (UnitId)     REFERENCES ProdUnit(Id),
    CONSTRAINT FK_PurchItem_Batch   FOREIGN KEY (BatchId)    REFERENCES ProdBatch(Id),
    CONSTRAINT FK_PurchItem_Serial  FOREIGN KEY (SerialId)   REFERENCES ProdSerial(Id)
);
```

---

## 24. `PurchPayment` — Purchase Payment Records (🔲 New)

```sql
CREATE TABLE PurchPayment (
    Id                  NVARCHAR(50)    PRIMARY KEY,
    PaymentNumber       NVARCHAR(30)    NOT NULL UNIQUE,   -- auto: PP-20260001
    PurchaseInvoiceId   NVARCHAR(50)    NOT NULL,          -- FK → PurchInvoice
    Amount              DECIMAL(18,2)   NOT NULL,
    PaymentDate         DATETIME2       NOT NULL,
    PaymentMethod       INT             NOT NULL DEFAULT 0,-- 0:Cash, 1:BankTransfer, 2:Cheque, 3:Other
    ReferenceNumber     NVARCHAR(100)   NULL,              -- cheque no, transfer ref
    Notes               NVARCHAR(MAX)   NULL,
    Active              BIT             NOT NULL DEFAULT 1,
    CreatedAt           DATETIME2       NOT NULL,
    CreatedBy           NVARCHAR(50)    NULL,
    UpdatedAt           DATETIME2       NULL,
    UpdatedBy           NVARCHAR(50)    NULL,
    LastAction          NVARCHAR(50)    NULL,

    CONSTRAINT FK_PurchPay_Invoice FOREIGN KEY (PurchaseInvoiceId) REFERENCES PurchInvoice(Id)
);
```

**On create/update/delete**: Recalculate PurchInvoice.PaidAmount = SUM(PurchPayment.Amount) and update PaymentStatus.

---

# SECTION E — SALES MODULE

---

## 25. `SalesCustomer` — Customers (✅ Existing)

```sql
CREATE TABLE SalesCustomer (
    Id                  NVARCHAR(50)    PRIMARY KEY,
    Code                NVARCHAR(30)    NOT NULL,          -- e.g., CUS-001
    Name                NVARCHAR(200)   NOT NULL,
    ContactPerson       NVARCHAR(100)   NULL,
    Email               NVARCHAR(150)   NULL,
    Phone               NVARCHAR(30)    NULL,
    Address             NVARCHAR(500)   NULL,
    City                NVARCHAR(100)   NULL,
    Country             NVARCHAR(100)   NULL,
    TaxNumber           NVARCHAR(50)    NULL,              -- TIN / Tax ID
    CreditLimit         DECIMAL(18,2)   NULL,              -- max outstanding balance
    Active              BIT             NOT NULL DEFAULT 1,
    CreatedAt           DATETIME2       NOT NULL,
    CreatedBy           NVARCHAR(50)    NULL,
    UpdatedAt           DATETIME2       NULL,
    UpdatedBy           NVARCHAR(50)    NULL,
    LastAction          NVARCHAR(50)    NULL
);
```

**Referenced by**: SalesQuotation, SalesOrder, SalesInvoice

---

## 26. `SalesQuotation` — Sales Quotation Header (🔲 New)

```sql
CREATE TABLE SalesQuotation (
    Id                  NVARCHAR(50)    PRIMARY KEY,
    QuotationNumber     NVARCHAR(30)    NOT NULL UNIQUE,   -- auto: QT-20260001
    QuotationDate       DATETIME2       NOT NULL,
    ValidUntil          DATETIME2       NULL,              -- expiry date
    CustomerId          NVARCHAR(50)    NOT NULL,          -- FK → SalesCustomer
    SubTotal            DECIMAL(18,2)   NOT NULL DEFAULT 0,
    TotalDiscount       DECIMAL(18,2)   NULL DEFAULT 0,
    TotalTax            DECIMAL(18,2)   NULL DEFAULT 0,
    TotalAmount         DECIMAL(18,2)   NOT NULL DEFAULT 0,
    Status              INT             NOT NULL DEFAULT 0,-- 0:Draft, 1:Sent, 2:Accepted, 3:Rejected, 4:Expired
    Notes               NVARCHAR(MAX)   NULL,
    Active              BIT             NOT NULL DEFAULT 1,
    CreatedAt           DATETIME2       NOT NULL,
    CreatedBy           NVARCHAR(50)    NULL,
    UpdatedAt           DATETIME2       NULL,
    UpdatedBy           NVARCHAR(50)    NULL,
    LastAction          NVARCHAR(50)    NULL,

    CONSTRAINT FK_SalesQuot_Customer FOREIGN KEY (CustomerId) REFERENCES SalesCustomer(Id)
);
```

**Referenced by**: SalesOrder.QuotationId (when converting quotation → SO)

---

## 27. `SalesQuotationItem` — Quotation Line Items (🔲 New)

```sql
CREATE TABLE SalesQuotationItem (
    Id                  NVARCHAR(50)    PRIMARY KEY,
    QuotationId         NVARCHAR(50)    NOT NULL,          -- FK → SalesQuotation
    ProductId           NVARCHAR(50)    NOT NULL,          -- FK → ProdItem
    UnitId              NVARCHAR(50)    NOT NULL,          -- FK → ProdUnit
    Quantity            DECIMAL(18,4)   NOT NULL,
    UnitPrice           DECIMAL(18,2)   NOT NULL,          -- from ProdUnitPrice or manual
    DiscountPercent     DECIMAL(18,2)   NULL DEFAULT 0,
    DiscountAmount      DECIMAL(18,2)   NULL DEFAULT 0,
    TaxPercent          DECIMAL(18,2)   NULL DEFAULT 0,
    TaxAmount           DECIMAL(18,2)   NULL DEFAULT 0,
    TotalAmount         DECIMAL(18,2)   NOT NULL DEFAULT 0,
    Notes               NVARCHAR(MAX)   NULL,
    Active              BIT             NOT NULL DEFAULT 1,
    CreatedAt           DATETIME2       NOT NULL,
    CreatedBy           NVARCHAR(50)    NULL,
    UpdatedAt           DATETIME2       NULL,
    UpdatedBy           NVARCHAR(50)    NULL,
    LastAction          NVARCHAR(50)    NULL,

    CONSTRAINT FK_SalesQuotItem_Quot    FOREIGN KEY (QuotationId) REFERENCES SalesQuotation(Id),
    CONSTRAINT FK_SalesQuotItem_Product FOREIGN KEY (ProductId)   REFERENCES ProdItem(Id),
    CONSTRAINT FK_SalesQuotItem_Unit    FOREIGN KEY (UnitId)      REFERENCES ProdUnit(Id)
);
```

---

## 28. `SalesOrder` — Sales Order Header (🔲 New)

```sql
CREATE TABLE SalesOrder (
    Id                  NVARCHAR(50)    PRIMARY KEY,
    OrderNumber         NVARCHAR(30)    NOT NULL UNIQUE,   -- auto: SO-20260001
    OrderDate           DATETIME2       NOT NULL,
    CustomerId          NVARCHAR(50)    NOT NULL,          -- FK → SalesCustomer
    QuotationId         NVARCHAR(50)    NULL,              -- FK → SalesQuotation (nullable)
    WarehouseId         NVARCHAR(50)    NOT NULL,          -- FK → InvWarehouse (source)
    SubTotal            DECIMAL(18,2)   NOT NULL DEFAULT 0,
    TotalDiscount       DECIMAL(18,2)   NULL DEFAULT 0,
    TotalTax            DECIMAL(18,2)   NULL DEFAULT 0,
    TotalAmount         DECIMAL(18,2)   NOT NULL DEFAULT 0,
    Status              INT             NOT NULL DEFAULT 0,-- 0:Draft, 1:Confirmed, 2:PartiallyInvoiced, 3:FullyInvoiced, 4:Closed, 5:Cancelled
    ExpectedDate        DATETIME2       NULL,
    Notes               NVARCHAR(MAX)   NULL,
    Active              BIT             NOT NULL DEFAULT 1,
    CreatedAt           DATETIME2       NOT NULL,
    CreatedBy           NVARCHAR(50)    NULL,
    UpdatedAt           DATETIME2       NULL,
    UpdatedBy           NVARCHAR(50)    NULL,
    LastAction          NVARCHAR(50)    NULL,

    CONSTRAINT FK_SalesOrder_Customer  FOREIGN KEY (CustomerId)  REFERENCES SalesCustomer(Id),
    CONSTRAINT FK_SalesOrder_Quotation FOREIGN KEY (QuotationId) REFERENCES SalesQuotation(Id),
    CONSTRAINT FK_SalesOrder_Warehouse FOREIGN KEY (WarehouseId) REFERENCES InvWarehouse(Id)
);
```

**Referenced by**: SalesInvoice.SalesOrderId

---

## 29. `SalesOrderItem` — Sales Order Line Items (🔲 New)

```sql
CREATE TABLE SalesOrderItem (
    Id                  NVARCHAR(50)    PRIMARY KEY,
    SalesOrderId        NVARCHAR(50)    NOT NULL,          -- FK → SalesOrder
    ProductId           NVARCHAR(50)    NOT NULL,          -- FK → ProdItem
    UnitId              NVARCHAR(50)    NOT NULL,          -- FK → ProdUnit
    Quantity            DECIMAL(18,4)   NOT NULL,
    InvoicedQuantity    DECIMAL(18,4)   NOT NULL DEFAULT 0,-- tracks how much invoiced
    UnitPrice           DECIMAL(18,2)   NOT NULL,
    DiscountPercent     DECIMAL(18,2)   NULL DEFAULT 0,
    DiscountAmount      DECIMAL(18,2)   NULL DEFAULT 0,
    TaxPercent          DECIMAL(18,2)   NULL DEFAULT 0,
    TaxAmount           DECIMAL(18,2)   NULL DEFAULT 0,
    TotalAmount         DECIMAL(18,2)   NOT NULL DEFAULT 0,
    Notes               NVARCHAR(MAX)   NULL,
    Active              BIT             NOT NULL DEFAULT 1,
    CreatedAt           DATETIME2       NOT NULL,
    CreatedBy           NVARCHAR(50)    NULL,
    UpdatedAt           DATETIME2       NULL,
    UpdatedBy           NVARCHAR(50)    NULL,
    LastAction          NVARCHAR(50)    NULL,

    CONSTRAINT FK_SOItem_Order   FOREIGN KEY (SalesOrderId) REFERENCES SalesOrder(Id),
    CONSTRAINT FK_SOItem_Product FOREIGN KEY (ProductId)    REFERENCES ProdItem(Id),
    CONSTRAINT FK_SOItem_Unit    FOREIGN KEY (UnitId)       REFERENCES ProdUnit(Id)
);
```

**InvoicedQuantity**: Updated automatically when Sales Invoice is created. Used to determine SO status (partial vs full).

---

## 30. `SalesInvoice` — Sales Invoice Header (⚠️ Update)

```sql
CREATE TABLE SalesInvoice (
    Id                  NVARCHAR(50)    PRIMARY KEY,
    InvoiceNumber       NVARCHAR(30)    NOT NULL UNIQUE,   -- auto: SI-20260001
    SaleDate            DATETIME2       NOT NULL,
    CustomerId          NVARCHAR(50)    NOT NULL,          -- FK → SalesCustomer
    WarehouseId         NVARCHAR(50)    NOT NULL,          -- FK → InvWarehouse (source)
    SalesOrderId        NVARCHAR(50)    NULL,              -- FK → SalesOrder          ⬅ NEW COLUMN
    SubTotal            DECIMAL(18,2)   NOT NULL DEFAULT 0,
    TotalDiscount       DECIMAL(18,2)   NULL DEFAULT 0,
    TotalTax            DECIMAL(18,2)   NULL DEFAULT 0,
    TotalAmount         DECIMAL(18,2)   NOT NULL DEFAULT 0,
    PaidAmount          DECIMAL(18,2)   NULL DEFAULT 0,    -- auto-calc from SalesPayment SUM
    PaymentStatus       INT             NOT NULL DEFAULT 0,-- 0:Unpaid, 1:PartiallyPaid, 2:Paid
    Status              INT             NOT NULL DEFAULT 0,-- 0:Draft, 1:Confirmed, 2:Cancelled
    DueDate             DATETIME2       NULL,              -- payment due date
    Notes               NVARCHAR(MAX)   NULL,
    Active              BIT             NOT NULL DEFAULT 1,
    CreatedAt           DATETIME2       NOT NULL,
    CreatedBy           NVARCHAR(50)    NULL,
    UpdatedAt           DATETIME2       NULL,
    UpdatedBy           NVARCHAR(50)    NULL,
    LastAction          NVARCHAR(50)    NULL,

    CONSTRAINT FK_SalesInv_Customer  FOREIGN KEY (CustomerId)   REFERENCES SalesCustomer(Id),
    CONSTRAINT FK_SalesInv_Warehouse FOREIGN KEY (WarehouseId)  REFERENCES InvWarehouse(Id),
    CONSTRAINT FK_SalesInv_SO        FOREIGN KEY (SalesOrderId) REFERENCES SalesOrder(Id)
);
```

**Changes from existing**:
- Added `SalesOrderId` — links invoice to SO (nullable for standalone/direct sales)

**On confirm**: Triggers stock-out via InvStockMovement + InvWarehouseStock update.

---

## 31. `SalesInvoiceItem` — Sales Invoice Line Items (✅ Existing)

```sql
CREATE TABLE SalesInvoiceItem (
    Id                  NVARCHAR(50)    PRIMARY KEY,
    SaleId              NVARCHAR(50)    NOT NULL,          -- FK → SalesInvoice
    ProductId           NVARCHAR(50)    NOT NULL,          -- FK → ProdItem
    UnitId              NVARCHAR(50)    NOT NULL,          -- FK → ProdUnit
    Quantity            DECIMAL(18,4)   NOT NULL,
    UnitPrice           DECIMAL(18,2)   NOT NULL,
    DiscountPercent     DECIMAL(18,2)   NULL DEFAULT 0,
    DiscountAmount      DECIMAL(18,2)   NULL DEFAULT 0,
    TaxPercent          DECIMAL(18,2)   NULL DEFAULT 0,
    TaxAmount           DECIMAL(18,2)   NULL DEFAULT 0,
    TotalAmount         DECIMAL(18,2)   NOT NULL DEFAULT 0,
    Notes               NVARCHAR(MAX)   NULL,
    BatchId             NVARCHAR(50)    NULL,              -- FK → ProdBatch
    SerialId            NVARCHAR(50)    NULL,              -- FK → ProdSerial
    Active              BIT             NOT NULL DEFAULT 1,
    CreatedAt           DATETIME2       NOT NULL,
    CreatedBy           NVARCHAR(50)    NULL,
    UpdatedAt           DATETIME2       NULL,
    UpdatedBy           NVARCHAR(50)    NULL,
    LastAction          NVARCHAR(50)    NULL,

    CONSTRAINT FK_SalesItem_Invoice FOREIGN KEY (SaleId)    REFERENCES SalesInvoice(Id),
    CONSTRAINT FK_SalesItem_Product FOREIGN KEY (ProductId) REFERENCES ProdItem(Id),
    CONSTRAINT FK_SalesItem_Unit    FOREIGN KEY (UnitId)    REFERENCES ProdUnit(Id),
    CONSTRAINT FK_SalesItem_Batch   FOREIGN KEY (BatchId)   REFERENCES ProdBatch(Id),
    CONSTRAINT FK_SalesItem_Serial  FOREIGN KEY (SerialId)  REFERENCES ProdSerial(Id)
);
```

---

## 32. `SalesPayment` — Sales Payment Records (🔲 New)

```sql
CREATE TABLE SalesPayment (
    Id                  NVARCHAR(50)    PRIMARY KEY,
    PaymentNumber       NVARCHAR(30)    NOT NULL UNIQUE,   -- auto: SP-20260001
    SalesInvoiceId      NVARCHAR(50)    NOT NULL,          -- FK → SalesInvoice
    Amount              DECIMAL(18,2)   NOT NULL,
    PaymentDate         DATETIME2       NOT NULL,
    PaymentMethod       INT             NOT NULL DEFAULT 0,-- 0:Cash, 1:BankTransfer, 2:Cheque, 3:Other
    ReferenceNumber     NVARCHAR(100)   NULL,              -- cheque no, transfer ref
    Notes               NVARCHAR(MAX)   NULL,
    Active              BIT             NOT NULL DEFAULT 1,
    CreatedAt           DATETIME2       NOT NULL,
    CreatedBy           NVARCHAR(50)    NULL,
    UpdatedAt           DATETIME2       NULL,
    UpdatedBy           NVARCHAR(50)    NULL,
    LastAction          NVARCHAR(50)    NULL,

    CONSTRAINT FK_SalesPay_Invoice FOREIGN KEY (SalesInvoiceId) REFERENCES SalesInvoice(Id)
);
```

**On create/update/delete**: Recalculate SalesInvoice.PaidAmount = SUM(SalesPayment.Amount) and update PaymentStatus.

---

# SECTION F — VIEWS (✅ Existing)

---

## VwProdUnitConversion — Unit Conversion View (Read-Only)

```sql
CREATE VIEW VwProdUnitConversion AS
SELECT
    c.Id,
    c.ProductId,
    p.Code        AS ProductCode,
    p.Name        AS ProductName,
    c.FromUnitId,
    fu.Name       AS FromUnitName,
    fu.Symbol     AS FromUnitSymbol,
    c.ToUnitId,
    tu.Name       AS ToUnitName,
    tu.Symbol     AS ToUnitSymbol,
    c.Factor,
    c.Active,
    c.CreatedAt,
    c.CreatedBy,
    c.UpdatedAt,
    c.UpdatedBy,
    c.LastAction
FROM ProdUnitConversion c
JOIN ProdItem p  ON c.ProductId  = p.Id
JOIN ProdUnit fu ON c.FromUnitId = fu.Id
JOIN ProdUnit tu ON c.ToUnitId   = tu.Id;
```

**Purpose**: Flat view joining conversion with product and unit names for easy querying.

---

# SECTION G — ENUMS

---

## All Enum Definitions

### PurchOrderStatus
```csharp
public enum PurchOrderStatus
{
    Draft = 0,
    Approved = 1,
    PartiallyReceived = 2,
    FullyReceived = 3,
    Closed = 4,
    Cancelled = 5
}
```

### GoodsReceiveStatus
```csharp
public enum GoodsReceiveStatus
{
    Draft = 0,
    Received = 1,
    Cancelled = 2
}
```

### InvoiceStatus (used by PurchInvoice & SalesInvoice)
```csharp
public enum InvoiceStatus
{
    Draft = 0,
    Confirmed = 1,
    Cancelled = 2
}
```

### PaymentStatus (used by PurchInvoice & SalesInvoice)
```csharp
public enum PaymentStatus
{
    Unpaid = 0,
    PartiallyPaid = 1,
    Paid = 2
}
```

### PaymentMethod (used by PurchPayment & SalesPayment)
```csharp
public enum PaymentMethod
{
    Cash = 0,
    BankTransfer = 1,
    Cheque = 2,
    Other = 3
}
```

### QuotationStatus
```csharp
public enum QuotationStatus
{
    Draft = 0,
    Sent = 1,
    Accepted = 2,
    Rejected = 3,
    Expired = 4
}
```

### SalesOrderStatus
```csharp
public enum SalesOrderStatus
{
    Draft = 0,
    Confirmed = 1,
    PartiallyInvoiced = 2,
    FullyInvoiced = 3,
    Closed = 4,
    Cancelled = 5
}
```

### TransferStatus
```csharp
public enum TransferStatus
{
    Draft = 0,
    Completed = 1,
    Cancelled = 2
}
```

### StockMovementType
```csharp
public enum StockMovementType
{
    In = 0,
    Out = 1,
    Adjust = 2,
    TransferIn = 3,
    TransferOut = 4
}
```

### StockReferenceType (for InvStockMovement.ReferenceType)
```csharp
public enum StockReferenceType
{
    GoodsReceive = 1,
    SalesInvoice = 2,
    Adjustment = 3,
    Transfer = 4,
    PurchaseInvoice = 5    // direct purchase without GRN
}
```

### ProductTrackType (for ProdItem.TrackType)
```csharp
public enum ProductTrackType
{
    None = 0,              // simple stock count
    Batch = 1,             // tracked by batch number
    Serial = 2             // tracked by serial number
}
```

---

# SECTION H — AUTO-NUMBER FORMAT

---

| Entity | Prefix | Format | Example |
|---|---|---|---|
| PurchOrder | PO | PO-YYYYNNNN | PO-20260001 |
| PurchGoodsReceive | GRN | GRN-YYYYNNNN | GRN-20260001 |
| PurchInvoice | PI | PI-YYYYNNNN | PI-20260001 |
| PurchPayment | PP | PP-YYYYNNNN | PP-20260001 |
| SalesQuotation | QT | QT-YYYYNNNN | QT-20260001 |
| SalesOrder | SO | SO-YYYYNNNN | SO-20260001 |
| SalesInvoice | SI | SI-YYYYNNNN | SI-20260001 |
| SalesPayment | SP | SP-YYYYNNNN | SP-20260001 |
| InvStockAdjustment | ADJ | ADJ-YYYYNNNN | ADJ-20260001 |
| InvStockTransfer | TRF | TRF-YYYYNNNN | TRF-20260001 |
| PurchSupplier | SUP | SUP-NNNN | SUP-0001 |
| SalesCustomer | CUS | CUS-NNNN | CUS-0001 |

**Auto-number logic**: YYYY = current year, NNNN = sequential reset per year.

---

# SECTION I — LINE ITEM CALCULATION FORMULA

---

All transaction line items (PO, GRN, Invoice, Quotation, SO) use the same formula:

```
DiscountAmount = (UnitPrice × Quantity) × DiscountPercent / 100
TaxAmount      = (UnitPrice × Quantity - DiscountAmount) × TaxPercent / 100
TotalAmount    = (UnitPrice × Quantity) - DiscountAmount + TaxAmount
```

Header totals:
```
SubTotal       = SUM(UnitPrice × Quantity)          per all line items
TotalDiscount  = SUM(DiscountAmount)                per all line items
TotalTax       = SUM(TaxAmount)                     per all line items
TotalAmount    = SubTotal - TotalDiscount + TotalTax
```

---

# SECTION J — MIGRATION CHANGES (for existing tables)

---

```sql
-- 1. Add PO/GRN link to PurchInvoice
ALTER TABLE PurchInvoice ADD PurchaseOrderId NVARCHAR(50) NULL;
ALTER TABLE PurchInvoice ADD GoodsReceiveId  NVARCHAR(50) NULL;
ALTER TABLE PurchInvoice ADD CONSTRAINT FK_PurchInv_PO  FOREIGN KEY (PurchaseOrderId) REFERENCES PurchOrder(Id);
ALTER TABLE PurchInvoice ADD CONSTRAINT FK_PurchInv_GRN FOREIGN KEY (GoodsReceiveId)  REFERENCES PurchGoodsReceive(Id);

-- 2. Add SO link to SalesInvoice
ALTER TABLE SalesInvoice ADD SalesOrderId NVARCHAR(50) NULL;
ALTER TABLE SalesInvoice ADD CONSTRAINT FK_SalesInv_SO FOREIGN KEY (SalesOrderId) REFERENCES SalesOrder(Id);
```
