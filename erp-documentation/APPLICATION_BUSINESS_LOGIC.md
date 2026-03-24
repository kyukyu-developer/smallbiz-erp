# ERP Application Layer - Business Logic Documentation

> **Purpose:** Complete documentation of all business logic, rules, and use cases in the ERP Application layer.

---

## Table of Contents

1. [Architecture Overview](#1-architecture-overview)
2. [Common Patterns](#2-common-patterns)
3. [Modules Documentation](#3-modules-documentation)
   - [Auth](#31-auth)
   - [Brands](#32-brands)
   - [Categories](#33-categories)
   - [Customers](#34-customers)
   - [Product Groups](#35-product-groups)
   - [Products](#36-products)
   - [Product Unit Conversion](#37-product-unit-conversion)
   - [Purchases](#38-purchases)
   - [Sales](#39-sales)
   - [Stock](#310-stock)
   - [Suppliers](#311-suppliers)
   - [Units](#312-units)
   - [Warehouses](#313-warehouses)
4. [Business Rules Summary](#4-business-rules-summary)
5. [Data Flow Diagrams](#5-data-flow-diagrams)

---

## 1. Architecture Overview

### 1.1 Layer Structure

```
ERP.Application/
├── DTOs/                    # Data Transfer Objects
│   ├── Common/              # Result<T>, PagedResult
│   ├── Brands/
│   ├── Categories/
│   ├── Customers/
│   ├── ProductGroup/
│   ├── Products/
│   ├── ProductUnitConversion/
│   ├── Purchases/
│   ├── Sales/
│   ├── Stock/
│   ├── Suppliers/
│   ├── Units/
│   └── Warehouses/
│
└── Features/                # Use Cases (CQRS)
    ├── Auth/
    │   └── Commands/        # Login, Register
    ├── Brands/
    │   ├── Commands/        # Create, Update, Delete
    │   └── Queries/         # GetAll, GetById
    ├── Categories/
    │   ├── Commands/
    │   └── Queries/
    ├── Customers/
    │   └── Commands/
    │   └── Queries/
    ├── ProductGroup/
    │   ├── Commands/
    │   └── Queries/
    ├── Products/
    │   ├── Commands/
    │   └── Queries/
    ├── ProductUnitConversion/
    │   ├── Commands/
    │   └── Queries/
    ├── Purchases/
    │   ├── Commands/
    │   └── Queries/
    ├── Sales/
    │   ├── Commands/
    │   └── Queries/
    ├── Stock/
    │   └── Queries/
    ├── Suppliers/
    │   ├── Commands/
    │   └── Queries/
    ├── Units/
    │   ├── Commands/
    │   └── Queries/
    └── Warehouses/
        ├── Commands/
        └── Queries/
```

### 1.2 Module Count Summary

| Category | Modules |
|----------|---------|
| **Master Data** | Brands, Categories, Units, ProductGroups, Products |
| **Business Partners** | Suppliers, Customers |
| **Inventory** | Warehouses, Stock, ProductUnitConversion |
| **Transactions** | Purchases, Sales |
| **Security** | Auth |

---

## 2. Common Patterns

### 2.1 CQRS (Command Query Responsibility Segregation)

**Commands** (Write operations):
- `CreateXxxCommand` → `CreateXxxCommandHandler`
- `UpdateXxxCommand` → `UpdateXxxCommandHandler`
- `DeleteXxxCommand` → `DeleteXxxCommandHandler`

**Queries** (Read operations):
- `GetXxxsQuery` → `GetXxxsQueryHandler` (Get all with filters)
- `GetXxxByIdQuery` → `GetXxxByIdQueryHandler`

### 2.2 Result Pattern

All operations return `Result<T>`:

```csharp
public class Result<T>
{
    public bool IsSuccess { get; set; }
    public T? Data { get; set; }
    public string? ErrorMessage { get; set; }
    
    public static Result<T> Success(T data) => new() { IsSuccess = true, Data = data };
    public static Result<T> Failure(string error) => new() { IsSuccess = false, ErrorMessage = error };
}
```

### 2.3 MediatR Pattern

```csharp
// Command definition
public class CreateXxxCommand : IRequest<Result<XxxDto>> { }

// Handler implementation
public class CreateXxxCommandHandler : IRequestHandler<CreateXxxCommand, Result<XxxDto>>
{
    public async Task<Result<XxxDto>> Handle(CreateXxxCommand request, CancellationToken ct)
    {
        // Business logic
    }
}
```

### 2.4 Validation Pattern (FluentValidation)

```csharp
public class CreateXxxCommandValidator : AbstractValidator<CreateXxxCommand>
{
    public CreateXxxCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(50);
    }
}
```

---

## 3. Modules Documentation

---

### 3.1 Auth

**Location:** `Features/Auth/Commands/`

#### 3.1.1 Login Command

| Property | Description |
|----------|-------------|
| **Input** | Email, Password |
| **Output** | JWT Token + Refresh Token |
| **Business Logic** | 1. Validate credentials<br>2. Check user exists and active<br>3. Verify password hash<br>4. Generate JWT token (15 min expiry)<br>5. Generate refresh token (7 days expiry)<br>6. Store refresh token |

```csharp
public class LoginCommand : IRequest<Result<AuthResponseDto>>
{
    public string Email { get; set; }
    public string Password { get; set; }
}
```

**Validation Rules:**
- Email: Required, valid email format
- Password: Required, minimum 6 characters

#### 3.1.2 Register Command

| Property | Description |
|----------|-------------|
| **Input** | Name, Email, Password, ConfirmPassword |
| **Output** | User ID + Success message |
| **Business Logic** | 1. Validate input<br>2. Check email uniqueness<br>3. Hash password<br>4. Create user record<br>5. Return success |

```csharp
public class RegisterCommand : IRequest<Result<string>>
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string ConfirmPassword { get; set; }
}
```

---

### 3.2 Brands

**Location:** `Features/Brands/`

#### Operations Matrix

| Operation | Command/Query | Business Rules |
|-----------|---------------|----------------|
| Create | `CreateBrandCommand` | - Name required, max 50 chars<br>- Duplicate name check |
| Update | `UpdateBrandCommand` | - ID required<br>- Check not in use by products |
| Delete | `DeleteBrandCommand` | - Soft delete (Active = false)<br>- Cannot delete if products exist |
| GetAll | `GetBrandsQuery` | - Returns active brands only |
| GetById | `GetBrandByIdQuery` | - Returns 404 if not found |

#### Create Brand Business Logic

```csharp
// 1. Validate input
// 2. Check duplicate name
var existing = await _repository.GetByName(request.Name);
if (existing != null)
    return Failure("Brand with name 'X' already exists");

// 3. Create entity
var brand = new Brand
{
    Id = Guid.NewGuid().ToString(),
    Name = request.Name,
    Active = true,
    CreatedBy = "System",
    LastAction = "CREATE"
};

// 4. Save and return
```

---

### 3.3 Categories

**Location:** `Features/Categories/`

#### Operations Matrix

| Operation | Command/Query | Business Rules |
|-----------|---------------|----------------|
| Create | `CreateCategoryCommand` | - Name required, max 50 chars<br>- Optional ParentCategoryId (hierarchical)<br>- Parent must exist if provided |
| Update | `UpdateCategoryCommand` | - Cannot create circular reference<br>- Parent must exist |
| Delete | `DeleteCategoryCommand` | - Soft delete<br>- Cannot delete if has children |
| GetAll | `GetCategoriesQuery` | - Returns tree structure |
| GetById | `GetCategoryByIdQuery` | - Returns full details |

#### Category Hierarchy Business Logic

```
Category Structure:
├── Electronics (Parent = null)
│   ├── Mobile Phones
│   │   ├── Smartphones
│   │   └── Feature Phones
│   └── Computers
└── Clothing
    ├── Men's Wear
    └── Women's Wear
```

**Validation Rules:**
- Parent category must exist if provided
- Cannot assign category as its own parent
- Delete only allowed if no child categories exist

---

### 3.4 Customers

**Location:** `Features/Customers/`

> **Note:** This module has partial implementation (Create + GetAll only).

#### Operations Matrix

| Operation | Command/Query | Business Rules |
|-----------|---------------|----------------|
| Create | `CreateCustomerCommand` | - Name, Phone required<br>- Optional email |
| GetAll | `GetCustomersQuery` | - Returns active customers |

#### Customer Entity Fields

```csharp
public class Customer
{
    public string Id { get; set; }
    public string Name { get; set; }           // Required
    public string? Phone { get; set; }          // Optional
    public string? Email { get; set; }          // Optional
    public string? Address { get; set; }        // Optional
    public bool Active { get; set; }
}
```

---

### 3.5 Product Groups

**Location:** `Features/ProductGroup/`

#### Operations Matrix

| Operation | Command/Query | Business Rules |
|-----------|---------------|----------------|
| Create | `CreateProductGroupCommand` | - Name required, max 50 chars<br>- Description optional<br>- Duplicate name check |
| Update | `UpdateProductGroupCommand` | - ID required<br>- Audit trail (UpdatedBy, UpdatedAt, LastAction) |
| Delete | `DeleteProductGroupCommand` | - Soft delete<br>- Sets Active = false |
| GetAll | `GetProductGroupQuery` | - Returns active groups |
| GetById | `GetProductGroupByIdQuery` | - Returns 404 if not found |

#### Audit Trail Logic

```csharp
// Create
entity.CreatedAt = DateTime.UtcNow;
entity.CreatedBy = "System";
entity.LastAction = "CREATE";

// Update
entity.UpdatedAt = DateTime.UtcNow;
entity.UpdatedBy = "System";
entity.LastAction = "UPDATE";

// Delete (soft)
entity.Active = false;
entity.UpdatedAt = DateTime.UtcNow;
entity.UpdatedBy = "System";
entity.LastAction = "DELETE";
```

---

### 3.6 Products

**Location:** `Features/Products/`

#### Operations Matrix

| Operation | Command/Query | Business Rules |
|-----------|---------------|----------------|
| Create | `CreateProductCommand` | - Code unique (max 20)<br>- Name required (max 100)<br>- Must have BaseUnit<br>- Optional: Group, Category, Brand |
| Update | `UpdateProductCommand` | - Code uniqueness preserved<br>- Audit trail |
| Delete | `DeleteProductCommand` | - Soft delete<br>- Check stock levels |
| GetAll | `GetProductsQuery` | - Filters: GroupId, CategoryId, BrandId, Search |
| GetById | `GetProductByIdQuery` | - Returns full details with relations |

#### Product Entity Structure

```csharp
public class Product : AuditableEntity
{
    public string Code { get; set; }              // SKU (unique)
    public string Name { get; set; }
    public string? Description { get; set; }
    
    // Foreign Keys (optional)
    public string? GroupId { get; set; }
    public string? CategoryId { get; set; }
    public string? BrandId { get; set; }
    public string BaseUnitId { get; set; }        // Required
    
    // Stock Configuration
    public decimal MinimumStock { get; set; }
    public decimal MaximumStock { get; set; }
    public decimal ReorderLevel { get; set; }
    public bool AllowNegativeStock { get; set; }
    
    // Variant Support
    public bool HasVariant { get; set; }
    public string? TrackType { get; set; }        // "Batch", "Serial", "None"
    
    // Pricing
    public decimal CostPrice { get; set; }
    public decimal SellingPrice { get; set; }
    
    public bool Active { get; set; }
}
```

#### Business Rules

1. **Product Code Validation**
   - Must be unique across all products
   - Maximum 20 characters
   - Cannot be changed after creation

2. **Stock Threshold Rules**
   - `MinimumStock` ≥ 0
   - `MaximumStock` > `MinimumStock`
   - `ReorderLevel` ≥ `MinimumStock`

3. **Variant Support**
   - If `HasVariant = true`, parent product cannot have stock
   - Stock tracked at variant level

---

### 3.7 Product Unit Conversion

**Location:** `Features/ProductUnitConversion/`

#### Operations Matrix

| Operation | Command/Query | Business Rules |
|-----------|---------------|----------------|
| Create | `CreateProductUnitConversionCommand` | - Unique Product+FromUnit+ToUnit combo<br>- Factor > 0<br>- ToUnit ≠ FromUnit |
| Update | `UpdateProductUnitConversionCommand` | - Preserve uniqueness<br>- Factor > 0 |
| Delete | `DeleteProductUnitConversionCommand` | - Soft delete |
| GetAll | `GetProductUnitConversionQuery` | - Includes Product, FromUnit, ToUnit |
| GetById | `GetProductUnitConversionByIdQuery` | - Full details with relations |

#### Conversion Formula

```
Quantity in ToUnit = Quantity in FromUnit × Factor

Example:
  Product: Rice
  FromUnit: Box (1 unit)
  ToUnit: Bag (1 unit)
  Factor: 25
  
  1 Box = 1 × 25 = 25 Bags
```

#### Validator Rules

```csharp
// CreateProductUnitConversionCommandValidator
RuleFor(x => x.ProductId).NotEmpty();
RuleFor(x => x.FromUnitId).NotEmpty();
RuleFor(x => x.ToUnitId)
    .NotEmpty()
    .NotEqual(x => x.FromUnitId)  // ToUnit ≠ FromUnit
    .WithMessage("To Unit must be different from From Unit");
RuleFor(x => x.Factor).GreaterThan(0);  // Factor > 0
```

---

### 3.8 Purchases

**Location:** `Features/Purchases/`

#### Operations Matrix

| Operation | Command/Query | Business Rules |
|-----------|---------------|----------------|
| Create | `CreatePurchaseCommand` | - Complex calculations<br>- Line items required<br>- Supplier required<br>- Auto-calculate totals |
| GetAll | `GetPurchasesQuery` | - Filters: SupplierId, Status, DateRange |
| GetById | `GetPurchaseByIdQuery` | - Full details with line items |

#### Purchase Calculation Business Logic

```csharp
public class PurchaseCalculation
{
    // Line Item Calculations
    public decimal CalculateLineTotal(decimal quantity, decimal unitPrice)
    {
        return quantity * unitPrice;
    }
    
    public decimal CalculateLineDiscount(decimal lineTotal, decimal discountPercent, decimal discountAmount)
    {
        // Discount can be percentage OR fixed amount
        if (discountPercent > 0)
            return lineTotal * (discountPercent / 100);
        return discountAmount;
    }
    
    public decimal CalculateLineTax(decimal taxableAmount, decimal taxPercent, decimal taxAmount)
    {
        if (taxPercent > 0)
            return taxableAmount * (taxPercent / 100);
        return taxAmount;
    }
    
    // Header Calculations
    public decimal CalculateSubtotal(List<PurchaseLineDto> lines)
    {
        return lines.Sum(l => l.Quantity * l.UnitPrice);
    }
    
    public decimal CalculateTotal(decimal subtotal, decimal discountAmount, decimal taxAmount)
    {
        return subtotal - discountAmount + taxAmount;
    }
}
```

#### Purchase Entity Structure

```csharp
public class Purchase : AuditableEntity
{
    public string InvoiceNo { get; set; }         // Auto-generated
    public string SupplierId { get; set; }
    public DateTime PurchaseDate { get; set; }
    public string Status { get; set; }            // Draft, Confirmed, Received, Cancelled
    public string PaymentStatus { get; set; }     // Pending, Partial, Paid
    public string? PaymentMethod { get; set; }
    
    // Totals
    public decimal SubTotal { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal PaidAmount { get; set; }
    public decimal BalanceAmount { get; set; }
    
    // Navigation
    public ICollection<PurchaseLine> Lines { get; set; }
}

public class PurchaseLine
{
    public string Id { get; set; }
    public string PurchaseId { get; set; }
    public string ProductId { get; set; }
    public string UnitId { get; set; }
    public decimal Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal DiscountPercent { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal TaxPercent { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal LineTotal { get; set; }
}
```

---

### 3.9 Sales

**Location:** `Features/Sales/`

#### Operations Matrix

| Operation | Command/Query | Business Rules |
|-----------|---------------|----------------|
| Create | `CreateSaleCommand` | - Similar to Purchases<br>- Stock validation<br>- Price from product master |
| GetAll | `GetSalesQuery` | - Filters: CustomerId, Status, DateRange |
| GetById | `GetSaleByIdQuery` | - Full details with line items |

#### Sales Calculation Business Logic

```csharp
// Same pattern as Purchases
// Line Total = Quantity × UnitPrice
// Subtotal = Sum of all line totals
// Total = Subtotal - Discount + Tax

// Additional Sales Rules:
// 1. Check stock availability (unless AllowNegativeStock = true)
// 2. Use selling price from product master
// 3. Customer loyalty/discount calculations (if implemented)
```

#### Sales Status Flow

```
Draft → Confirmed → Delivered → Completed
  ↓         ↓
Cancelled  Cancelled
```

---

### 3.10 Stock

**Location:** `Features/Stock/`

#### Operations Matrix

| Operation | Command/Query | Business Rules |
|-----------|---------------|----------------|
| GetAll | `GetStockLevelsQuery` | - Read-only<br>- Aggregated by warehouse/product<br>- Alerts for low stock |

#### Stock Level Calculations

```csharp
public class StockLevelDto
{
    public string WarehouseId { get; set; }
    public string ProductId { get; set; }
    
    // Quantities
    public decimal TotalQuantity { get; set; }        // Total in stock
    public decimal ReservedQuantity { get; set; }      // Reserved for orders
    public decimal AvailableQuantity { get; set; }     // Total - Reserved
    
    // Thresholds (from Product)
    public decimal MinimumStock { get; set; }
    public decimal MaximumStock { get; set; }
    public decimal ReorderLevel { get; set; }
    
    // Alerts
    public bool IsBelowMinimum { get; set; }           // Available < Minimum
    public bool NeedsReorder { get; set; }             // Available <= ReorderLevel
}

// Calculation Logic:
AvailableQuantity = TotalQuantity - ReservedQuantity;
IsBelowMinimum = AvailableQuantity < product.MinimumStock;
NeedsReorder = AvailableQuantity <= product.ReorderLevel;
```

#### Stock Query Filters

```csharp
public class GetStockLevelsQuery : IRequest<Result<List<StockLevelDto>>>
{
    public string? WarehouseId { get; set; }     // Filter by warehouse
    public string? ProductId { get; set; }       // Filter by product
    public bool? LowStockOnly { get; set; }      // Only items below minimum
    public bool? NeedsReorderOnly { get; set; }  // Only items needing reorder
}
```

---

### 3.11 Suppliers

**Location:** `Features/Suppliers/`

> **Note:** This module has partial implementation (Create + GetAll only).

#### Operations Matrix

| Operation | Command/Query | Business Rules |
|-----------|---------------|----------------|
| Create | `CreateSupplierCommand` | - Name required<br>- Contact info optional |
| GetAll | `GetSuppliersQuery` | - Returns active suppliers |

#### Supplier Entity Fields

```csharp
public class Supplier : AuditableEntity
{
    public string Code { get; set; }
    public string Name { get; set; }
    public string? ContactPerson { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }
    public decimal CreditLimit { get; set; }
    public int PaymentDays { get; set; }
    public bool Active { get; set; }
}
```

---

### 3.12 Units

**Location:** `Features/Units/`

#### Operations Matrix

| Operation | Command/Query | Business Rules |
|-----------|---------------|----------------|
| Create | `CreateUnitCommand` | - Name required (max 50)<br>- Symbol required (unique)<br>- Duplicate name check |
| Update | `UpdateUnitCommand` | - Symbol uniqueness<br>- Cannot change if conversions exist |
| Delete | `DeleteUnitCommand` | - Soft delete<br>- Cannot delete if used in conversions |
| GetAll | `GetUnitsQuery` | - Returns active units |
| GetById | `GetUnitByIdQuery` | - Full details |

#### Common Units

```
Length:      mm, cm, m, km, inch, ft, yard, mile
Weight:      mg, g, kg, ton, oz, lb
Volume:      ml, L, gallon, quart, pint
Count:       pcs, box, pack, carton, pallet
Time:        sec, min, hr, day
```

---

### 3.13 Warehouses

**Location:** `Features/Warehouses/`

#### Operations Matrix

| Operation | Command/Query | Business Rules |
|-----------|---------------|----------------|
| Create | `CreateWarehouseCommand` | - Name + City unique<br>- Main warehouses: no parent<br>- Branch/Sub: parent required |
| Update | `UpdateWarehouseCommand` | - Cannot move Main to Branch<br>- Parent validation |
| Delete | `DeleteWarehouseCommand` | - Cannot delete if stock exists<br>- Cannot delete if has children |
| GetAll | `GetWarehousesQuery` | - Filters: BranchType, MainWarehousesOnly |
| GetById | `GetWarehouseByIdQuery` | - Full details |

#### Warehouse Hierarchy

```
Warehouse Structure:
├── Main Warehouse (BranchType = Main)
│   ├── Branch North (BranchType = Branch)
│   │   ├── Sub North-1 (BranchType = Sub)
│   │   └── Sub North-2 (BranchType = Sub)
│   └── Branch South (BranchType = Branch)
│       ├── Sub South-1
│       └── Sub South-2
└── Main Warehouse 2
    └── ...
```

#### Branch Types

| Type | Description | Rules |
|------|-------------|-------|
| **Main** | Headquarters | - No parent allowed<br>- Can receive from suppliers<br>- Can transfer to branches |
| **Branch** | Regional warehouse | - Must have Main parent<br>- Can transfer to sub-warehouses |
| **Sub** | Local warehouse | - Must have Main/Branch parent<br>- Cannot transfer to others |

#### Validation Rules

```csharp
// Main warehouse cannot have parent
RuleFor(x => x.ParentWarehouseId)
    .Empty()
    .When(x => x.BranchType == BranchType.Main);

// Branch/Sub must have parent
RuleFor(x => x.ParentWarehouseId)
    .NotEmpty()
    .When(x => x.BranchType == BranchType.Branch || x.BranchType == BranchType.Sub);
```

---

## 4. Business Rules Summary

### 4.1 Common Validations

| Rule | Applied To |
|------|------------|
| Name required, max 50 chars | All entities |
| Code/SKU unique | Products, Suppliers |
| Duplicate name check | Brands, Categories, Units, Warehouses |
| Soft delete (Active=false) | All entities |
| Audit trail (CreatedBy, UpdatedBy, LastAction) | All entities |
| Parent must exist if provided | Categories, Warehouses |

### 4.2 Calculation Rules

| Module | Calculation |
|--------|-------------|
| **Purchases/Sales** | LineTotal = Qty × Price |
| | Subtotal = Sum(Lines) |
| | Total = Subtotal - Discount + Tax |
| **Stock** | Available = Total - Reserved |
| | IsBelowMinimum = Available < MinStock |
| | NeedsReorder = Available ≤ ReorderLevel |
| **Unit Conversion** | TargetQty = SourceQty × Factor |

### 4.3 Status Workflows

```
Purchase Flow:
Draft → Confirmed → Received → Completed
  ↓         ↓
Cancelled  Cancelled

Sale Flow:
Draft → Confirmed → Delivered → Completed
  ↓         ↓
Cancelled  Cancelled

Stock Movement:
Purchase Receipt → +Stock
Sale Issue → -Stock
Transfer → -Source, +Destination
Adjustment → ±Stock
```

---

## 5. Data Flow Diagrams

### 5.1 Create Entity Flow

```
┌─────────────┐
│   Client    │
└──────┬──────┘
       │ POST /api/products
       ▼
┌─────────────────────────────────────────┐
│           Controller                    │
│  1. Receive request                     │
│  2. Send command via MediatR            │
└──────────────┬──────────────────────────┘
               │ MediatR.Send(command)
               ▼
┌─────────────────────────────────────────┐
│          Command Handler                │
│  1. Validate (FluentValidation)         │
│  2. Check business rules (duplicates)   │
│  3. Create entity                       │
│  4. Set audit fields                    │
│  5. Save via repository                 │
│  6. Return DTO                          │
└──────────────┬──────────────────────────┘
               │
               ▼
┌─────────────────────────────────────────┐
│          Repository Layer               │
│  1. EF Core operations                  │
│  2. Active filter                       │
│  3. Navigation includes                 │
└─────────────────────────────────────────┘
```

### 5.2 Purchase Calculation Flow

```
Purchase Command Input:
┌─────────────────────────────────────────┐
│ SupplierId: SUP-001                     │
│ Lines:                                  │
│   - Product: P001, Qty: 10, Price: 100 │
│   - Product: P002, Qty: 5, Price: 200  │
│ Discount: 5%                            │
│ Tax: 7%                                 │
└──────────────┬──────────────────────────┘
               │
               ▼
Handler Processing:
┌─────────────────────────────────────────┐
│ Line 1: 10 × 100 = 1,000              │
│ Line 2: 5 × 200 = 1,000               │
│                                         │
│ Subtotal: 2,000                         │
│ Discount (5%): 100                      │
│ Taxable: 1,900                          │
│ Tax (7%): 133                           │
│                                         │
│ Total: 2,033                            │
└─────────────────────────────────────────┘
```

### 5.3 Stock Alert Flow

```
GetStockLevels Query:
┌─────────────────────────────────────────┐
│ WarehouseId: WH-001                     │
│ LowStockOnly: true                      │
└──────────────┬──────────────────────────┘
               │
               ▼
Query Handler:
┌─────────────────────────────────────────┐
│ For each product in warehouse:          │
│                                         │
│ TotalQty: 50                            │
│ ReservedQty: 10                         │
│ AvailableQty: 40                        │
│                                         │
│ Product.MinStock: 100                   │
│ Product.ReorderLevel: 150               │
│                                         │
│ IsBelowMinimum: 40 < 100 = TRUE ✓     │
│ NeedsReorder: 40 ≤ 150 = TRUE ✓       │
└─────────────────────────────────────────┘
```

---

## Appendix A: Module Completeness

| Module | Create | Update | Delete | GetAll | GetById | Status |
|--------|--------|--------|--------|--------|---------|--------|
| Auth | Login, Register | - | - | - | - | ✅ Complete |
| Brands | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ Complete |
| Categories | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ Complete |
| Customers | ✅ | ❌ | ❌ | ✅ | ❌ | ⚠️ Partial |
| ProductGroups | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ Complete |
| Products | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ Complete |
| ProductUnitConversion | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ Complete |
| Purchases | ✅ | ❌ | ❌ | ✅ | ✅ | ⚠️ Partial |
| Sales | ✅ | ❌ | ❌ | ✅ | ✅ | ⚠️ Partial |
| Stock | - | - | - | ✅ | - | ✅ Read-only |
| Suppliers | ✅ | ❌ | ❌ | ✅ | ❌ | ⚠️ Partial |
| Units | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ Complete |
| Warehouses | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ Complete |

---

*Version: 2026-03-24 | ERP Application Business Logic Documentation*
