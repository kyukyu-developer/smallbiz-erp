# ERP Inventory System - Entity Relationship Diagram

## Full ERD Diagram

```mermaid
erDiagram
    warehouse ||--o{ warehouse : "parent_of"
    warehouse ||--o{ stock_receive : "receives_stock"
    warehouse ||--o{ stock_delivery : "delivers_stock"
    warehouse ||--o{ stock_ledger : "tracks_stock"

    product_group ||--o{ product_category : "contains"
    product_group ||--o{ product : "categorizes"

    product_category ||--o{ product : "categorizes"

    product_brand ||--o{ product : "brands"

    uom ||--o{ product : "base_unit"
    uom ||--o{ product_uom : "measures"
    uom ||--o{ product_uom_conversion : "from_unit"
    uom ||--o{ product_uom_conversion : "to_unit"
    uom ||--o{ product_variant : "base_unit"
    uom ||--o{ stock_receive_line : "transaction_unit"
    uom ||--o{ stock_receive_line : "base_unit"
    uom ||--o{ stock_delivery_line : "transaction_unit"
    uom ||--o{ stock_delivery_line : "base_unit"
    uom ||--o{ stock_ledger : "base_unit"

    product ||--o{ product_uom : "has"
    product ||--o{ product_uom_conversion : "converts"
    product ||--o{ product_variant : "has_variants"
    product ||--o{ stock_receive_line : "receives"
    product ||--o{ stock_delivery_line : "delivers"
    product ||--o{ stock_ledger : "tracks"

    product_variant ||--o{ product_variant_attribute : "has_attributes"
    product_variant ||--o{ stock_receive_line : "receives"
    product_variant ||--o{ stock_delivery_line : "delivers"
    product_variant ||--o{ stock_ledger : "tracks"

    attribute ||--o{ attribute_value : "has_values"
    attribute ||--o{ product_variant_attribute : "defines"

    attribute_value ||--o{ product_variant_attribute : "assigns"

    stock_receive ||--o{ stock_receive_line : "contains"

    stock_receive_line ||--o{ stock_receive_line_detail : "details"

    stock_delivery ||--o{ stock_delivery_line : "contains"

    stock_delivery_line ||--o{ stock_delivery_line_detail : "details"

    warehouse {
        varchar id PK
        varchar name
        varchar city
        varchar branch_type
        boolean is_main_warehouse
        varchar parent_warehouse_id FK
        boolean is_used_warehouse
        boolean active
        timestamp created_on
        timestamp modified_on
        varchar created_by
        varchar modified_by
        varchar last_action
    }

    uom {
        varchar id PK
        varchar name
        varchar symbol
        boolean active
        timestamp created_on
        timestamp modified_on
        varchar created_by
        varchar modified_by
        varchar last_action
    }

    product_group {
        varchar id PK
        varchar name
        varchar description
        boolean active
        timestamp created_on
        timestamp modified_on
        varchar created_by
        varchar modified_by
        varchar last_action
    }

    product_category {
        varchar id PK
        varchar group_id FK
        varchar name
        varchar description
        boolean active
        timestamp created_on
        timestamp modified_on
        varchar created_by
        varchar modified_by
        varchar last_action
    }

    product_brand {
        varchar id PK
        varchar name
        varchar description
        boolean active
        timestamp created_on
        timestamp modified_on
        varchar created_by
        varchar modified_by
        varchar last_action
    }

    attribute {
        varchar id PK
        varchar name
        boolean active
        timestamp created_on
        timestamp modified_on
        varchar created_by
        varchar modified_by
        varchar last_action
    }

    attribute_value {
        varchar id PK
        varchar attribute_id FK
        varchar value
        boolean active
        timestamp created_on
        timestamp modified_on
        varchar created_by
        varchar modified_by
        varchar last_action
    }

    product {
        varchar id PK
        varchar code UK
        varchar name
        varchar group_id FK
        varchar category_id FK
        varchar brand_id FK
        varchar base_uom_id FK
        boolean track_inventory
        boolean has_variant
        boolean has_batch
        boolean has_serial
        decimal reorder_level
        boolean allow_negative_stock
        boolean active
        timestamp created_on
        timestamp modified_on
        varchar created_by
        varchar modified_by
        varchar last_action
    }

    product_uom {
        varchar id PK
        varchar product_id FK
        varchar uom_id FK
        decimal factor_to_base
        boolean is_base
        boolean active
        timestamp created_on
        timestamp modified_on
        varchar created_by
        varchar modified_by
        varchar last_action
    }

    product_uom_conversion {
        varchar id PK
        varchar product_id FK
        varchar from_uom_id FK
        varchar to_uom_id FK
        decimal factor
        boolean active
        timestamp created_on
        timestamp modified_on
        varchar created_by
        varchar modified_by
        varchar last_action
    }

    product_variant {
        varchar id PK
        varchar product_id FK
        varchar sku UK
        varchar base_uom_id FK
        boolean track_inventory
        boolean active
        timestamp created_on
        timestamp modified_on
        varchar created_by
        varchar modified_by
        varchar last_action
    }

    product_variant_attribute {
        varchar id PK
        varchar variant_id FK
        varchar attribute_id FK
        varchar value_id FK
        boolean active
        timestamp created_on
        timestamp modified_on
        varchar created_by
        varchar modified_by
        varchar last_action
    }

    stock_receive {
        varchar id PK
        timestamp receive_date
        varchar reference_no
        varchar warehouse_id FK
        varchar supplier_id
        varchar remarks
        boolean active
        timestamp created_on
        timestamp modified_on
        varchar created_by
        varchar modified_by
        varchar last_action
    }

    stock_receive_line {
        varchar id PK
        varchar receive_id FK
        varchar product_id FK
        varchar variant_id FK
        varchar uom_id FK
        decimal quantity
        decimal unit_price
        decimal line_total
        varchar base_uom_id FK
        decimal base_quantity
        decimal base_unit_price
        decimal base_line_total
        boolean active
        timestamp created_on
        timestamp modified_on
        varchar created_by
        varchar modified_by
        varchar last_action
    }

    stock_receive_line_detail {
        varchar id PK
        varchar line_id FK
        varchar batch_no
        varchar serial_no
        decimal quantity
        boolean active
        timestamp created_on
        timestamp modified_on
        varchar created_by
        varchar modified_by
        varchar last_action
    }

    stock_delivery {
        varchar id PK
        varchar delivery_no
        timestamp delivery_date
        varchar delivery_type
        varchar warehouse_id FK
        varchar customer_id
        varchar department_id
        varchar remarks
        boolean active
        timestamp created_on
        timestamp modified_on
        varchar created_by
        varchar modified_by
        varchar last_action
    }

    stock_delivery_line {
        varchar id PK
        varchar delivery_id FK
        varchar product_id FK
        varchar variant_id FK
        varchar uom_id FK
        decimal quantity
        varchar base_uom_id FK
        decimal base_quantity
        decimal unit_cost
        decimal line_total_cost
        boolean active
        timestamp created_on
        timestamp modified_on
        varchar created_by
        varchar modified_by
        varchar last_action
    }

    stock_delivery_line_detail {
        varchar id PK
        varchar line_id FK
        varchar batch_no
        varchar serial_no
        decimal quantity
        boolean active
        timestamp created_on
        timestamp modified_on
        varchar created_by
        varchar modified_by
        varchar last_action
    }

    stock_ledger {
        varchar id PK
        varchar warehouse_id FK
        varchar product_id FK
        varchar variant_id FK
        varchar base_uom_id FK
        decimal quantity
        decimal value
        varchar stock_type
        varchar reference_type
        varchar reference_id
        timestamp created_on
        timestamp modified_on
        varchar created_by
        varchar modified_by
        varchar last_action
    }
```

## Simplified View by Module

### Master Data Module
```mermaid
erDiagram
    product_group ||--o{ product_category : "contains"
    product_brand ||--o{ product : "brands"
    product_category ||--o{ product : "categorizes"
    product_group ||--o{ product : "categorizes"
    uom ||--o{ product : "base_unit"

    product ||--o{ product_uom : "has_alternate_units"
    product ||--o{ product_variant : "has_variants"

    attribute ||--o{ attribute_value : "has_values"
    product_variant ||--o{ product_variant_attribute : "described_by"
    attribute ||--o{ product_variant_attribute : "defines"
    attribute_value ||--o{ product_variant_attribute : "assigns"
```

### Warehouse Module
```mermaid
erDiagram
    warehouse ||--o{ warehouse : "parent_of"
    warehouse ||--o{ stock_ledger : "tracks"

    warehouse {
        varchar id PK
        varchar name
        varchar city
        varchar branch_type
        boolean is_main_warehouse
        varchar parent_warehouse_id FK
    }
```

### Stock Receiving Module
```mermaid
erDiagram
    warehouse ||--o{ stock_receive : "receives"
    stock_receive ||--o{ stock_receive_line : "contains"
    stock_receive_line ||--o{ stock_receive_line_detail : "batch_serial_details"

    product ||--o{ stock_receive_line : "received"
    product_variant ||--o{ stock_receive_line : "received"
    uom ||--o{ stock_receive_line : "measured_in"
```

### Stock Delivery Module
```mermaid
erDiagram
    warehouse ||--o{ stock_delivery : "delivers"
    stock_delivery ||--o{ stock_delivery_line : "contains"
    stock_delivery_line ||--o{ stock_delivery_line_detail : "batch_serial_details"

    product ||--o{ stock_delivery_line : "delivered"
    product_variant ||--o{ stock_delivery_line : "delivered"
    uom ||--o{ stock_delivery_line : "measured_in"
```

### Stock Ledger Module
```mermaid
erDiagram
    warehouse ||--o{ stock_ledger : "tracks"
    product ||--o{ stock_ledger : "tracked"
    product_variant ||--o{ stock_ledger : "tracked"

    stock_ledger {
        varchar id PK
        varchar warehouse_id FK
        varchar product_id FK
        varchar variant_id FK
        decimal quantity
        decimal value
        varchar stock_type "IN/OUT"
        varchar reference_type
        varchar reference_id
    }
```

## Key Relationships Summary

### One-to-Many Relationships
- **Warehouse → Warehouse** (hierarchical: main → branch → sub)
- **Product Group → Product Category**
- **Product → Product Variant** (with attributes)
- **Product → Product UOM** (alternate units)
- **Attribute → Attribute Value**
- **Stock Receive → Stock Receive Line → Stock Receive Line Detail**
- **Stock Delivery → Stock Delivery Line → Stock Delivery Line Detail**

### Many-to-One Relationships
- **Product → Product Group, Product Category, Product Brand**
- **Product → UOM** (base unit)
- **Stock Receive → Warehouse** (must be main warehouse)
- **Stock Delivery → Warehouse**
- **Stock Ledger → Warehouse, Product, Product Variant**

### Junction Tables (Many-to-Many)
- **product_variant_attribute** links Variant ↔ Attribute ↔ Attribute Value
- **product_uom** links Product ↔ UOM
- **product_uom_conversion** links Product ↔ UOM (from) ↔ UOM (to)

## Business Rules Highlighted in ERD

1. **Warehouse Hierarchy**: `parent_warehouse_id` creates self-referencing relationship
2. **Main Warehouse Only**: Stock can only be received at warehouses where `is_main_warehouse = TRUE`
3. **Product Variants**: Only exist when `product.has_variant = TRUE`
4. **Batch/Serial Tracking**: `stock_receive_line_detail` and `stock_delivery_line_detail` store batch/serial numbers
5. **UOM Conversion**: Both `product_uom` and `product_uom_conversion` handle unit conversions
6. **Stock Ledger**: Immutable log of all stock movements (IN/OUT)
7. **Base Quantity**: All stock movements store both transaction UOM and base UOM quantities

## Notes
- PK = Primary Key
- FK = Foreign Key
- UK = Unique Key
- All tables include audit fields (created_on, modified_on, created_by, modified_by, last_action)
- All tables include `active` boolean for soft delete

---

# System Flowcharts

## Overall System Architecture Flow
```mermaid
flowchart TB
    subgraph MasterData["Master Data Layer"]
        PG[Product Group]
        PC[Product Category]
        PB[Product Brand]
        PR[Product]
        PV[Product Variant]
        AT[Attributes]
        UOM[Units of Measure]
    end

    subgraph Warehouse["Warehouse Management"]
        WH[Warehouse Hierarchy]
        MW[Main Warehouse]
        BW[Branch Warehouse]
    end

    subgraph StockOps["Stock Operations"]
        SR[Stock Receive]
        SD[Stock Delivery]
        SL[Stock Ledger]
    end

    subgraph External["External Systems"]
        SUP[Suppliers]
        CUST[Customers]
        DEPT[Departments]
    end

    PG --> PC
    PC --> PR
    PB --> PR
    PR --> PV
    AT --> PV
    UOM --> PR
    UOM --> PV

    WH --> MW
    MW --> BW

    SUP --> SR
    MW --> SR
    PR --> SR
    PV --> SR
    UOM --> SR

    SR --> SL

    BW --> SD
    MW --> SD
    PR --> SD
    PV --> SD
    UOM --> SD

    SD --> SL

    CUST --> SD
    DEPT --> SD

    style MasterData fill:#e1f5ff
    style Warehouse fill:#fff4e1
    style StockOps fill:#e8f5e9
    style External fill:#fce4ec
```

## Stock Receiving Process Flow
```mermaid
flowchart TD
    Start([Start Stock Receive]) --> ValidateWH{Is Main<br/>Warehouse?}

    ValidateWH -->|No| ErrorWH[Error: Only Main<br/>Warehouse can receive]
    ValidateWH -->|Yes| CreateHeader[Create Stock Receive Header]

    CreateHeader --> InputDetails[Input Receive Details:<br/>- Date<br/>- Supplier<br/>- Reference No]

    InputDetails --> AddLines{Add<br/>Product Lines}

    AddLines --> SelectProduct[Select Product/<br/>Variant]
    SelectProduct --> SelectUOM[Select UOM]
    SelectUOM --> EnterQty[Enter Quantity<br/>& Unit Price]
    EnterQty --> CalcBase[Calculate Base<br/>Quantity & Price]

    CalcBase --> CheckBatch{Has Batch/<br/>Serial?}

    CheckBatch -->|Yes| EnterBatch[Enter Batch/Serial<br/>Details in Line Detail]
    CheckBatch -->|No| AddMoreLines{Add More<br/>Lines?}

    EnterBatch --> AddMoreLines

    AddMoreLines -->|Yes| AddLines
    AddMoreLines -->|No| SaveReceive[Save Stock Receive]

    SaveReceive --> UpdateLedger[Create Stock Ledger<br/>Entry: Type=IN]

    UpdateLedger --> UpdateStock[Update Stock Balance<br/>in Warehouse]

    UpdateStock --> End([End])
    ErrorWH --> End

    style Start fill:#4caf50,color:#fff
    style End fill:#f44336,color:#fff
    style ErrorWH fill:#ff9800,color:#fff
    style UpdateLedger fill:#2196f3,color:#fff
    style UpdateStock fill:#2196f3,color:#fff
```

## Stock Delivery Process Flow
```mermaid
flowchart TD
    Start([Start Stock Delivery]) --> SelectType{Delivery Type}

    SelectType -->|Customer| InputCust[Input Customer Info]
    SelectType -->|Internal| InputDept[Input Department Info]

    InputCust --> CreateHeader
    InputDept --> CreateHeader

    CreateHeader[Create Delivery Header] --> SelectWH[Select Warehouse<br/>Main or Branch]

    SelectWH --> AddLines{Add<br/>Product Lines}

    AddLines --> SelectProduct[Select Product/<br/>Variant]
    SelectProduct --> SelectUOM[Select UOM]
    SelectUOM --> CheckStock{Stock<br/>Available?}

    CheckStock -->|No| CheckNegative{Allow<br/>Negative<br/>Stock?}
    CheckStock -->|Yes| EnterQty

    CheckNegative -->|No| ErrorStock[Error: Insufficient<br/>Stock]
    CheckNegative -->|Yes| EnterQty[Enter Quantity]

    EnterQty --> CalcBase[Calculate Base<br/>Quantity & Cost]

    CalcBase --> CheckBatch{Has Batch/<br/>Serial?}

    CheckBatch -->|Yes| SelectBatch[Select Batch/Serial<br/>from Available Stock]
    CheckBatch -->|No| AddMoreLines{Add More<br/>Lines?}

    SelectBatch --> AddMoreLines

    AddMoreLines -->|Yes| AddLines
    AddMoreLines -->|No| SaveDelivery[Save Stock Delivery]

    SaveDelivery --> UpdateLedger[Create Stock Ledger<br/>Entry: Type=OUT]

    UpdateLedger --> DeductStock[Deduct from Stock<br/>Balance in Warehouse]

    DeductStock --> End([End])
    ErrorStock --> End

    style Start fill:#4caf50,color:#fff
    style End fill:#f44336,color:#fff
    style ErrorStock fill:#ff9800,color:#fff
    style UpdateLedger fill:#2196f3,color:#fff
    style DeductStock fill:#2196f3,color:#fff
```

## Product Master Data Setup Flow
```mermaid
flowchart LR
    Start([Setup Product]) --> CreateGroup[Create Product Group]

    CreateGroup --> CreateCat[Create Product Category<br/>under Group]

    CreateCat --> CreateBrand[Create Product Brand]

    CreateBrand --> DefineUOM[Define Base UOM]

    DefineUOM --> CreateProduct[Create Product Master:<br/>- Code<br/>- Name<br/>- Group/Category<br/>- Brand<br/>- Base UOM]

    CreateProduct --> AddAltUOM[Add Alternate UOMs<br/>with Conversion Factors]

    AddAltUOM --> HasVariant{Has<br/>Variants?}

    HasVariant -->|No| SetTracking[Set Tracking Options:<br/>- Inventory<br/>- Batch<br/>- Serial]

    HasVariant -->|Yes| DefineAttr[Define Attributes:<br/>Size, Color, etc.]

    DefineAttr --> CreateAttrVal[Create Attribute Values]

    CreateAttrVal --> CreateVariant[Create Product Variants<br/>with SKU]

    CreateVariant --> AssignAttr[Assign Attribute Values<br/>to Each Variant]

    AssignAttr --> SetTracking

    SetTracking --> SetReorder[Set Reorder Level &<br/>Negative Stock Policy]

    SetReorder --> End([Product Ready])

    style Start fill:#4caf50,color:#fff
    style End fill:#2196f3,color:#fff
```

## Stock Ledger Update Flow
```mermaid
flowchart TD
    Trigger([Stock Transaction<br/>Triggered]) --> CheckType{Transaction<br/>Type}

    CheckType -->|Receive| GetReceiveData[Get Stock Receive<br/>Line Data]
    CheckType -->|Delivery| GetDeliveryData[Get Stock Delivery<br/>Line Data]

    GetReceiveData --> PrepareIN
    GetDeliveryData --> PrepareOUT

    PrepareIN[Prepare Ledger Entry:<br/>- Type: IN<br/>- Quantity: +<br/>- Value: +] --> CreateEntry

    PrepareOUT[Prepare Ledger Entry:<br/>- Type: OUT<br/>- Quantity: -<br/>- Value: -] --> CreateEntry

    CreateEntry[Create Stock Ledger Record] --> RecordDetails[Record:<br/>- Warehouse<br/>- Product/Variant<br/>- Base UOM Quantity<br/>- Value<br/>- Reference Type & ID]

    RecordDetails --> CalcBalance{Calculate New<br/>Stock Balance}

    CalcBalance --> CheckNegative{New Balance<br/>< 0?}

    CheckNegative -->|Yes| CheckPolicy{Allow<br/>Negative?}
    CheckNegative -->|No| UpdateSuccess

    CheckPolicy -->|No| Rollback[Rollback Transaction]
    CheckPolicy -->|Yes| WarnNegative[Warning: Negative<br/>Stock Created]

    Rollback --> End([Failed])

    WarnNegative --> UpdateSuccess[Update Stock Balance]
    UpdateSuccess --> LogAudit[Log Audit Trail]

    LogAudit --> Success([Success])

    style Trigger fill:#4caf50,color:#fff
    style Success fill:#2196f3,color:#fff
    style End fill:#f44336,color:#fff
    style Rollback fill:#ff9800,color:#fff
    style WarnNegative fill:#ff9800,color:#fff
```

## UOM Conversion Flow
```mermaid
flowchart LR
    Input([Transaction in<br/>Non-Base UOM]) --> GetTxnUOM[Get Transaction UOM<br/>& Quantity]

    GetTxnUOM --> GetProductUOM[Get Product UOM<br/>Conversion Table]

    GetProductUOM --> FindConversion{Conversion<br/>Factor Exists?}

    FindConversion -->|Yes| ApplyDirect[Apply Direct Conversion:<br/>Base Qty = Txn Qty × Factor]

    FindConversion -->|No| CheckConvTable[Check UOM<br/>Conversion Table]

    CheckConvTable --> ConvExists{Conversion<br/>Path Found?}

    ConvExists -->|Yes| ApplyChain[Apply Conversion Chain:<br/>From UOM → To UOM]
    ConvExists -->|No| Error[Error: No Conversion<br/>Path Available]

    ApplyDirect --> CalcValue
    ApplyChain --> CalcValue

    CalcValue[Calculate Base Values:<br/>- Base Quantity<br/>- Base Unit Price<br/>- Base Line Total] --> StoreData[Store Both:<br/>Transaction Values<br/>+ Base Values]

    StoreData --> End([Continue Transaction])
    Error --> End

    style Input fill:#4caf50,color:#fff
    style End fill:#2196f3,color:#fff
    style Error fill:#f44336,color:#fff
```

## Warehouse Hierarchy Flow
```mermaid
flowchart TD
    Top([Warehouse Structure]) --> MainWH[Main Warehouse<br/>is_main_warehouse = TRUE]

    MainWH --> CanReceive[✓ Can Receive Stock]
    MainWH --> CanDeliver[✓ Can Deliver Stock]
    MainWH --> HasBranch{Has Branch<br/>Warehouses?}

    HasBranch -->|Yes| Branch1[Branch Warehouse 1<br/>parent_warehouse_id = Main]
    HasBranch -->|Yes| Branch2[Branch Warehouse 2<br/>parent_warehouse_id = Main]

    Branch1 --> NoReceive1[✗ Cannot Receive Stock]
    Branch1 --> CanDeliver1[✓ Can Deliver Stock]
    Branch1 --> HasSub1{Has Sub<br/>Warehouses?}

    Branch2 --> NoReceive2[✗ Cannot Receive Stock]
    Branch2 --> CanDeliver2[✓ Can Deliver Stock]

    HasSub1 -->|Yes| Sub1[Sub Warehouse<br/>parent_warehouse_id = Branch1]

    Sub1 --> NoReceiveSub[✗ Cannot Receive Stock]
    Sub1 --> CanDeliverSub[✓ Can Deliver Stock]

    CanDeliver1 --> Transfer[Stock Transfers:<br/>Main → Branch → Sub]
    CanDeliver2 --> Transfer
    CanDeliverSub --> Transfer

    Transfer --> Ledger[Each Warehouse has<br/>Separate Stock Ledger]

    Ledger --> End([Independent Stock<br/>Balances per Warehouse])

    style Top fill:#4caf50,color:#fff
    style MainWH fill:#2196f3,color:#fff
    style Branch1 fill:#ff9800,color:#fff
    style Branch2 fill:#ff9800,color:#fff
    style Sub1 fill:#ffeb3b,color:#000
    style End fill:#9c27b0,color:#fff
```
