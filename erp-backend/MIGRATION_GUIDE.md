# Database Migration Guide - String Primary Keys

## Issue
The original database was created with `int` IDENTITY primary keys, but the architecture now requires `string` (VARCHAR(50)) primary keys for all entities to support GUID-based IDs.

## Solution Options

### Option 1: Drop and Recreate Database (RECOMMENDED for Development)

This is the cleanest approach for development environments:

```bash
# 1. Drop the existing database (WARNING: All data will be lost!)
cd c:/Guu/ERP_Project/erp-backend/ERP.Infrastructure
dotnet ef database drop --startup-project ../ERP.API --context ApplicationDbContext --force

# 2. Remove old migrations
rm -rf Migrations/

# 3. Create new initial migration with string IDs
dotnet ef migrations add InitialCreate --startup-project ../ERP.API --context ApplicationDbContext

# 4. Create the database
dotnet ef database update --startup-project ../ERP.API --context ApplicationDbContext
```

### Option 2: Manual Migration Script

If you have important data to preserve, you'll need to:

1. **Backup existing data**
2. **Create temporary tables** with string IDs
3. **Migrate data** with NEWID() or custom logic
4. **Drop old tables**
5. **Rename temporary tables**

This is complex and error-prone. Only use if you have critical data.

### Option 3: Keep Existing Database, Use Dual System

- Keep old database for existing data
- Create new database for new warehouse module
- Gradually migrate features

## Recommended Steps (Option 1)

```powershell
# Navigate to Infrastructure project
cd c:/Guu/ERP_Project/erp-backend/ERP.Infrastructure

# Drop existing database
dotnet ef database drop --startup-project ../ERP.API --force

# Remove all migrations
Remove-Item -Recurse -Force Migrations/

# Create fresh migration
dotnet ef migrations add InitialCreateWithStringIds --startup-project ../ERP.API

# Apply migration
dotnet ef database update --startup-project ../ERP.API
```

## What Changed

### Warehouse Entity
- ✅ `Id` changed from `int` to `string` (VARCHAR(50))
- ✅ Uses GUID for new records: `Guid.NewGuid().ToString()`
- ✅ `ParentWarehouseId` is `string?`

### All Foreign Keys to Warehouse
- ✅ `WarehouseId` in all entities changed to `string`
  - StockTransfer (FromWarehouseId, ToWarehouseId)
  - StockMovement
  - WarehouseStock
  - ProductBatch
  - StockAdjustment
  - Purchase
  - Sale

### Future: All Entities Need String IDs

According to the documentation, **all primary keys should be string**:
- ProductId
- CustomerId
- SupplierId
- CategoryId
- UnitId
- UserId
- etc.

## After Migration

1. **Seed Initial Data**
   - Create at least one Main Warehouse
   - Create master data (Units, Categories)

2. **Test API Endpoints**
   - Test warehouse CRUD operations
   - Verify string IDs work correctly
   - Test relationships

3. **Update Frontend**
   - Ensure frontend handles string IDs
   - Update API client interfaces

## Notes

- String IDs allow for distributed systems and GUID-based identifiers
- Better for microservices architecture
- Prevents auto-increment conflicts
- Supports multi-tenant scenarios
- Database size impact is minimal (50 bytes vs 4 bytes per record)
