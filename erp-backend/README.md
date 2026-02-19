# ERP Backend API - Clean Architecture

## Overview
This is a .NET 8 Web API project implementing an ERP system with Clean Architecture principles, focusing initially on the Warehouse Management module.

## Architecture

### Project Structure
```
erp-backend/
├── ERP.Domain/           # Core business entities and interfaces
├── ERP.Application/      # Business logic, DTOs, Commands, Queries
├── ERP.Infrastructure/   # Data access, EF Core, Repositories
└── ERP.API/             # Web API controllers, middleware
```

### Technologies Used
- **.NET 8**
- **ASP.NET Core Web API**
- **Entity Framework Core 8** with SQL Server
- **MediatR** - CQRS pattern implementation
- **FluentValidation** - Input validation
- **JWT Authentication** - Secure API access
- **Swagger/OpenAPI** - API documentation

## Warehouse Module Implementation

### Features Implemented

#### 1. **Warehouse Entity** (Domain Layer)
- Hierarchical structure: Main → Branch → Sub warehouses
- Branch types: Main, Branch, Sub (BranchType enum)
- Only Main warehouses can receive stock from suppliers
- Parent-child relationships for warehouse hierarchy
- Audit fields (CreatedAt, UpdatedAt, CreatedBy, ModifiedBy)
- Soft delete support (Active flag)

#### 2. **Repository Pattern** (Infrastructure Layer)
Custom warehouse queries:
- `GetByNameAndCityAsync()` - Prevent duplicate warehouses
- `GetByBranchTypeAsync()` - Filter by warehouse type
- `GetMainWarehousesAsync()` - Get all main warehouses
- `GetChildWarehousesAsync()` - Get child warehouses
- `GetWarehouseHierarchyAsync()` - Get complete hierarchy

#### 3. **CQRS Commands** (Application Layer)
- **CreateWarehouseCommand** - Create new warehouse
  - Validates parent warehouse exists
  - Prevents duplicate name+city combinations
  - Auto-sets IsMainWarehouse based on BranchType
  - Generates GUID for warehouse ID

- **UpdateWarehouseCommand** - Update existing warehouse
  - Validates parent warehouse
  - Prevents circular references
  - Tracks last action and modification timestamp

#### 4. **CQRS Queries** (Application Layer)
- **GetWarehousesQuery** - Get all warehouses with filtering
  - Filter by active/inactive status
  - Filter by branch type
  - Filter main warehouses only

- **GetWarehouseByIdQuery** - Get specific warehouse details
  - Includes parent warehouse name

#### 5. **Validation** (FluentValidation)
- Name: Required, max 50 characters
- City: Max 50 characters
- BranchType: Valid enum value
- ParentWarehouseId:
  - Required for Branch/Sub warehouses
  - Must be null for Main warehouses
  - Must exist in database

### API Endpoints

Base URL: `https://localhost:5001/api`

#### Get All Warehouses
```http
GET /api/warehouses?includeInactive=false&branchType=Main&mainWarehousesOnly=true
Authorization: Bearer {jwt_token}
```

**Query Parameters:**
- `includeInactive` (bool, optional) - Include inactive warehouses
- `branchType` (int, optional) - Filter by warehouse type (1=Main, 2=Branch, 3=Sub)
- `mainWarehousesOnly` (bool, optional) - Get only main warehouses

**Response:**
```json
[
  {
    "id": "550e8400-e29b-41d4-a716-446655440000",
    "name": "Main Warehouse - Jakarta",
    "city": "Jakarta",
    "branchType": "Main",
    "isMainWarehouse": true,
    "parentWarehouseId": null,
    "parentWarehouseName": null,
    "isUsedWarehouse": true,
    "active": true,
    "location": "Central Jakarta",
    "address": "Jl. Sudirman No. 123",
    "country": "Indonesia",
    "contactPerson": "John Doe",
    "phone": "+62-21-1234567",
    "createdOn": "2026-02-15T10:30:00Z",
    "modifiedOn": null,
    "createdBy": "System",
    "modifiedBy": null,
    "lastAction": "CREATE"
  }
]
```

#### Get Warehouse By ID
```http
GET /api/warehouses/550e8400-e29b-41d4-a716-446655440000
Authorization: Bearer {jwt_token}
```

#### Create Warehouse
```http
POST /api/warehouses
Authorization: Bearer {jwt_token}
Content-Type: application/json

{
  "name": "Main Warehouse - Jakarta",
  "city": "Jakarta",
  "branchType": 1,
  "parentWarehouseId": null,
  "location": "Central Jakarta",
  "address": "Jl. Sudirman No. 123",
  "country": "Indonesia",
  "contactPerson": "John Doe",
  "phone": "+62-21-1234567"
}
```

**BranchType Values:**
- `1` - Main (can receive stock from suppliers)
- `2` - Branch (receives from Main)
- `3` - Sub (receives from Main or Branch)

#### Update Warehouse
```http
PUT /api/warehouses/550e8400-e29b-41d4-a716-446655440000
Authorization: Bearer {jwt_token}
Content-Type: application/json

{
  "id": "550e8400-e29b-41d4-a716-446655440000",
  "name": "Main Warehouse - Jakarta Updated",
  "city": "Jakarta",
  "branchType": 1,
  "parentWarehouseId": null,
  "active": true,
  "location": "Central Jakarta",
  "address": "Jl. Sudirman No. 123",
  "country": "Indonesia",
  "contactPerson": "John Doe",
  "phone": "+62-21-1234567"
}
```

## Database Configuration

### Connection String
Located in `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=ERPDatabase;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true"
  }
}
```

### Database Schema
```sql
CREATE TABLE Warehouses (
    Id NVARCHAR(50) PRIMARY KEY,
    Name NVARCHAR(50) NOT NULL,
    City NVARCHAR(50),
    BranchType NVARCHAR(20) NOT NULL,
    IsMainWarehouse BIT NOT NULL,
    ParentWarehouseId NVARCHAR(50) NULL,
    IsUsedWarehouse BIT NOT NULL DEFAULT 1,
    Active BIT NOT NULL DEFAULT 1,
    LastAction NVARCHAR(50),
    Location NVARCHAR(100),
    Address NVARCHAR(255),
    Country NVARCHAR(50),
    ContactPerson NVARCHAR(100),
    Phone NVARCHAR(20),
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME2,
    CreatedBy NVARCHAR(50),
    UpdatedBy NVARCHAR(50),
    CONSTRAINT FK_Warehouse_ParentWarehouse FOREIGN KEY (ParentWarehouseId)
        REFERENCES Warehouses(Id)
);
```

## Running the Project

### Prerequisites
- .NET 8 SDK
- SQL Server (LocalDB or Full)
- Visual Studio 2022 / VS Code / Rider

### Steps

1. **Update Database Connection String**
   Edit `appsettings.json` with your SQL Server connection.

2. **Run Migrations**
   ```bash
   cd ERP.Infrastructure
   dotnet ef database update --startup-project ../ERP.API
   ```

3. **Run the API**
   ```bash
   cd ERP.API
   dotnet run
   ```

4. **Access Swagger UI**
   Navigate to: `https://localhost:5001/swagger`

5. **Authentication**
   - First, call `/api/auth/login` to get JWT token
   - Use the token in Authorization header: `Bearer {token}`

## Business Rules

### Warehouse Management
1. ✅ **Warehouse Hierarchy**: Supports 3-level hierarchy (Main → Branch → Sub)
2. ✅ **Stock Receipt Rule**: Only Main warehouses can receive stock from suppliers
3. ✅ **Parent Validation**:
   - Main warehouses cannot have a parent
   - Branch and Sub warehouses must have a valid parent
4. ✅ **Duplicate Prevention**: Unique combination of Name + City
5. ✅ **Soft Delete**: Records are marked inactive, not physically deleted
6. ✅ **Audit Trail**: All create/update operations tracked with timestamps and user

## Known Issues & Next Steps

### Pending Fixes
- [ ] Update Sale/Purchase handlers to use string WarehouseId
- [ ] Update Stock handlers to use string WarehouseId
- [ ] Complete database migration after handler fixes

### Future Enhancements
1. **Stock Receive Module** (GRN - Goods Receipt Note)
   - Only allow stock receipt at Main warehouses
   - Link to Purchase Orders
   - Support batch and serial number tracking

2. **Stock Transfer Module**
   - Transfer stock between warehouses
   - Respect warehouse hierarchy rules
   - Track transfer status

3. **Stock Balance Queries**
   - Real-time stock levels per warehouse
   - Stock movement history
   - Low stock alerts

4. **Additional Warehouse Endpoints**
   - Soft delete warehouse
   - Get warehouse hierarchy tree
   - Get available warehouses for transfers

## Project Documentation

- See `WAREHOUSE_IMPLEMENTATION.md` for detailed warehouse module documentation
- See `erp-documentation/INVENTORY_DATABASE_NOTES.txt` for inventory database schema
- See `erp-documentation/COMPLETE_WORKFLOW_OVERVIEW.md` for business process flows

## Contributing

This is a development project. When making changes:
1. Follow Clean Architecture principles
2. Keep business logic in Application layer
3. Use MediatR for CQRS commands/queries
4. Add FluentValidation for all commands
5. Update EF Core configurations for entity changes
6. Write unit tests for business logic
7. Update API documentation

## License

Internal project for ERP system development.
