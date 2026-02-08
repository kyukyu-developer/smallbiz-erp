# ERP Microservices Architecture

A sample ASP.NET Core microservices-based ERP system with YARP API Gateway, RabbitMQ messaging, and Clean Architecture.

## Architecture Overview

```
┌──────────────────┐
│   API Gateway    │ :5000
│   (YARP)         │
└────────┬─────────┘
         │
    ┌────┴────┬────────┬──────────┐
    │         │        │          │
┌───▼──┐  ┌──▼───┐ ┌──▼────┐ ┌───▼─────┐
│ ID   │  │ INV  │ │ SALES │ │ PURCH   │
│:5001 │  │:5002 │ │:5003  │ │:5004    │
└──────┘  └──────┘ └───────┘ └─────────┘
    │         │        │          │
    └─────────┴────────┴──────────┘
              RabbitMQ
```

## Services

### 1. API Gateway (Port 5000)
- **Technology**: YARP Reverse Proxy
- **Features**:
  - Routes requests to microservices
  - JWT validation
  - CORS handling
- **Routes**:
  - `/api/identity/**` → Identity Service
  - `/api/inventory/**` → Inventory Service
  - `/api/sales/**` → Sales Service
  - `/api/purchasing/**` → Purchasing Service

### 2. Identity Service (Port 5001)
- **Database**: IdentityDb
- **Entities**: User, RefreshToken
- **Endpoints**:
  - `POST /api/identity/auth/login` - User login
  - `POST /api/identity/auth/register` - User registration
  - `GET /api/identity/auth/me` - Get current user (requires auth)
- **Events Published**:
  - UserCreated
  - UserUpdated

### 3. Inventory Service (Port 5002)
- **Database**: InventoryDb
- **Entities**: Product, Category, Unit, ProductUnitPrice, Warehouse, WarehouseStock
- **Endpoints**:
  - `GET/POST /api/inventory/products` - Products CRUD
  - `GET/POST /api/inventory/categories` - Categories CRUD
  - `GET/POST /api/inventory/warehouses` - Warehouses CRUD
  - `GET /api/inventory/stock` - Stock queries
  - `GET /api/inventory/stock/check-availability` - Check stock availability (used by Sales)
- **Events Subscribed**:
  - SaleCreatedEvent → Reduces stock
  - PurchaseReceivedEvent → Adds stock
- **Events Published**:
  - StockUpdatedEvent
  - LowStockAlertEvent

### 4. Sales Service (Port 5003)
- **Database**: SalesDb
- **Entities**: Sale, SalesItem, Customer
- **Endpoints**:
  - `GET/POST /api/sales/customers` - Customers CRUD
  - `GET/POST /api/sales/orders` - Sales orders CRUD
  - `GET /api/sales/orders/{id}` - Get sale by ID
- **External Calls**:
  - REST call to Inventory Service to validate stock before creating sale
- **Events Published**:
  - SaleCreatedEvent
  - SaleCancelledEvent

### 5. Purchasing Service (Port 5004)
- **Database**: PurchaseDb
- **Entities**: Purchase, PurchaseItem, Supplier
- **Endpoints**:
  - `GET/POST /api/purchasing/suppliers` - Suppliers CRUD
  - `GET/POST /api/purchasing/orders` - Purchase orders CRUD
  - `POST /api/purchasing/orders/{id}/receive` - Mark purchase as received
- **Events Subscribed**:
  - LowStockAlertEvent → Logs alert (placeholder for auto-reorder)
- **Events Published**:
  - PurchaseCreatedEvent
  - PurchaseReceivedEvent

## Communication Patterns

### 1. Synchronous (REST/HTTP)
- **Sales → Inventory**: Check stock availability before creating a sale
- **Frontend → Gateway**: All client requests go through the API Gateway

### 2. Asynchronous (RabbitMQ Events)
- **Sales publishes `SaleCreatedEvent`** → Inventory subscribes and reduces stock
- **Purchasing publishes `PurchaseReceivedEvent`** → Inventory subscribes and adds stock
- **Inventory publishes `LowStockAlertEvent`** → Purchasing subscribes for reorder alerts

## Technology Stack

- **.NET 8** - Framework
- **ASP.NET Core Web API** - API layer
- **Entity Framework Core 8** - ORM
- **SQL Server** - Database (one per service)
- **MediatR** - CQRS pattern
- **FluentValidation** - Request validation
- **YARP 2.1** - API Gateway
- **RabbitMQ 7.0** - Message broker
- **JWT** - Authentication
- **Swagger** - API documentation
- **Serilog** - Logging

## Getting Started

### Prerequisites
- .NET 8 SDK
- SQL Server (LocalDB or full instance)
- RabbitMQ (optional - only needed for event-driven features)

### Setup

1. **Update Connection Strings**

   Update `appsettings.json` in each service's API project with your SQL Server connection:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=localhost;Database=ERP_[ServiceName]Db;Trusted_Connection=true;TrustServerCertificate=true"
   }
   ```

2. **Install RabbitMQ** (optional)

   - Download from https://www.rabbitmq.com/download.html
   - Default connection: `amqp://guest:guest@localhost:5672`

3. **Run Database Migrations**

   Each service needs its database:
   ```bash
   # Identity Service
   cd src/Services/Identity/Identity.API
   dotnet ef database update --project ../Identity.Infrastructure

   # Inventory Service
   cd src/Services/Inventory/Inventory.API
   dotnet ef database update --project ../Inventory.Infrastructure

   # Sales Service
   cd src/Services/Sales/Sales.API
   dotnet ef database update --project ../Sales.Infrastructure

   # Purchasing Service
   cd src/Services/Purchasing/Purchasing.API
   dotnet ef database update --project ../Purchasing.Infrastructure
   ```

4. **Run Services**

   Option 1 - Run individually (recommended for development):
   ```bash
   # Terminal 1 - API Gateway
   cd src/ApiGateway/ApiGateway
   dotnet run

   # Terminal 2 - Identity Service
   cd src/Services/Identity/Identity.API
   dotnet run

   # Terminal 3 - Inventory Service
   cd src/Services/Inventory/Inventory.API
   dotnet run

   # Terminal 4 - Sales Service
   cd src/Services/Sales/Sales.API
   dotnet run

   # Terminal 5 - Purchasing Service
   cd src/Services/Purchasing/Purchasing.API
   dotnet run
   ```

   Option 2 - Use Visual Studio:
   - Open `ERP.Microservices.sln`
   - Right-click solution → Properties → Multiple startup projects
   - Select all API projects (ApiGateway, Identity.API, Inventory.API, Sales.API, Purchasing.API)

5. **Access Services**

   - **API Gateway**: http://localhost:5000
   - **Identity API**: http://localhost:5001/swagger
   - **Inventory API**: http://localhost:5002/swagger
   - **Sales API**: http://localhost:5003/swagger
   - **Purchasing API**: http://localhost:5004/swagger

## Testing the System

### 1. Register a User
```bash
POST http://localhost:5000/api/identity/auth/register
{
  "username": "admin",
  "email": "admin@example.com",
  "password": "Admin123!",
  "role": "Admin"
}
```

### 2. Login
```bash
POST http://localhost:5000/api/identity/auth/login
{
  "username": "admin",
  "password": "Admin123!"
}
# Response: { "token": "...", "refreshToken": "..." }
```

### 3. Create Product (use token from login)
```bash
POST http://localhost:5000/api/inventory/products
Authorization: Bearer <your-token>
{
  "code": "PROD001",
  "name": "Laptop",
  "categoryId": 1,
  "baseUnitId": 1,
  "minimumStock": 10,
  "isActive": true
}
```

### 4. Create Sale (triggers RabbitMQ event to reduce stock)
```bash
POST http://localhost:5000/api/sales/orders
Authorization: Bearer <your-token>
{
  "customerId": 1,
  "warehouseId": 1,
  "saleDate": "2026-02-08",
  "items": [
    {
      "productId": 1,
      "unitId": 1,
      "quantity": 2,
      "unitPrice": 1500,
      "tax": 150
    }
  ]
}
# This publishes SaleCreatedEvent → Inventory Service reduces stock
```

## Project Structure (Clean Architecture)

Each microservice follows Clean Architecture:

```
Service.Domain/          # Entities, Interfaces, Enums
├── Common/              # BaseEntity, AuditableEntity
├── Entities/            # Domain models
└── Interfaces/          # Repository, UnitOfWork interfaces

Service.Application/     # Business logic, CQRS
├── DTOs/                # Data transfer objects
├── Features/            # Commands & Queries (MediatR)
│   ├── Commands/        # Create, Update, Delete operations
│   └── Queries/         # Read operations
└── Interfaces/          # Service interfaces

Service.Infrastructure/  # Data access, external services
├── Data/                # DbContext, Configurations
├── Repositories/        # Repository implementations
└── Services/            # External service implementations

Service.API/             # API endpoints
├── Controllers/         # REST controllers
└── Program.cs           # Startup configuration
```

## Shared Libraries

### ERP.Shared.Contracts
- Integration events (SaleCreatedEvent, PurchaseReceivedEvent, etc.)
- Common DTOs
- Result wrapper class

### ERP.Shared.MessageBus
- `IMessageBus` interface
- RabbitMQ implementation
- Dependency injection extensions

## Database-per-Service Pattern

Each microservice has its own database:
- **IdentityDb** - Users, RefreshTokens
- **InventoryDb** - Products, Categories, Units, Warehouses, Stock
- **SalesDb** - Sales, SalesItems, Customers
- **PurchaseDb** - Purchases, PurchaseItems, Suppliers

This ensures:
- Service independence
- Schema isolation
- Independent scaling
- Fault isolation

## Next Steps

- [ ] Add database migrations for each service
- [ ] Add comprehensive validation with FluentValidation
- [ ] Implement proper error handling middleware
- [ ] Add health checks for dependencies (database, RabbitMQ)
- [ ] Add distributed tracing (OpenTelemetry)
- [ ] Add API versioning
- [ ] Implement rate limiting
- [ ] Add unit and integration tests
- [ ] Containerize with Docker
- [ ] Add Kubernetes manifests
- [ ] Implement circuit breakers (Polly)
- [ ] Add caching (Redis)

## Contributing

This is a sample project demonstrating microservices architecture with ASP.NET Core, YARP, and RabbitMQ.

## License

MIT
