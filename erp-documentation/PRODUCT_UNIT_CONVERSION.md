# Product Unit Conversion Module

> **Purpose:** This document explains the ProductUnitConversion module implementation following Clean Architecture and CQRS patterns.

---

## 1. Overview

The Product Unit Conversion module manages conversion rates between different units for products. For example: `1 Box = 12 Pieces`.

---

## 2. Entity Definition

**Location:** `erp-backend/ERP.Domain/Entities/ProdUnitConversion.cs`

```csharp
public partial class ProdUnitConversion
{
    public string Id { get; set; }              // Unique ID (GUID)
    public string ProductId { get; set; }        // Product reference
    public string FromUnitId { get; set; }       // Source unit (e.g., "Box")
    public string ToUnitId { get; set; }        // Target unit (e.g., "Piece")
    public decimal Factor { get; set; }          // Conversion factor (e.g., 12)
    public bool Active { get; set; }             // Soft delete flag
    public DateTime CreatedAt { get; set; }      // Creation timestamp
    public string CreatedBy { get; set; }         // Creator
    public DateTime? UpdatedAt { get; set; }     // Update timestamp
    public string UpdatedBy { get; set; }         // Updater
    public string LastAction { get; set; }        // "CREATE", "UPDATE", "DELETE"

    // Navigation Properties
    public virtual ProdUnit FromUnit { get; set; }
    public virtual ProdUnit ToUnit { get; set; }
    public virtual ProdItem Product { get; set; }
}
```

---

## 3. Architecture Layers

### 3.1 File Structure

```
erp-backend/
├── ERP.Domain/
│   ├── Entities/ProdUnitConversion.cs
│   └── Interfaces/IProductUnitConversionRepository.cs
│
├── ERP.Application/
│   ├── DTOs/ProductUnitConversion/
│   │   ├── GetProductUnitConversionDto.cs
│   │   └── GetProductUnitConversionByIdDto.cs
│   └── Features/ProductUnitConversion/
│       ├── Commands/
│       │   ├── CreateProductUnitConversionCommand.cs
│       │   ├── CreateProductUnitConversionCommandHandler.cs
│       │   ├── CreateProductUnitConversionCommandValidator.cs
│       │   ├── UpdateProductUnitConversionCommand.cs
│       │   ├── UpdateProductUnitConversionCommandHandler.cs
│       │   ├── UpdateProductUnitConversionCommandValidator.cs
│       │   ├── DeleteProductUnitConversionCommand.cs
│       │   └── DeleteProductUnitConversionCommandHandler.cs
│       └── Queries/
│           ├── GetProductUnitConversionQuery.cs
│           ├── GetProductUnitConversionQueryHandler.cs
│           ├── GetProductUnitConversionByIdQuery.cs
│           └── GetProductUnitConversionByIdQueryHandler.cs
│
├── ERP.Infrastructure/
│   └── Repositories/ProductUnitConversionRepository.cs
│
└── ERP.Tests/
    └── ProductUnitConversion/
        ├── Commands/
        │   ├── CreateProductUnitConversionCommandHandlerTests.cs
        │   ├── UpdateProductUnitConversionCommandHandlerTests.cs
        │   └── DeleteProductUnitConversionCommandHandlerTests.cs
        └── Validators/
            ├── CreateProductUnitConversionCommandValidatorTests.cs
            └── UpdateProductUnitConversionCommandValidatorTests.cs
```

---

## 4. Repository Implementation

### 4.1 Interface

**Location:** `erp-backend/ERP.Domain/Interfaces/IProductUnitConversionRepository.cs`

```csharp
public interface IProductUnitConversionRepository : IRepository<ProdUnitConversion>
{
    // Query Methods
    Task<ProdUnitConversion?> GetByName(string name);
    Task<IEnumerable<ProdUnitConversion>> GetByProductIdAsync(string productId);
    Task<IEnumerable<ProdUnitConversion>> GetByFromUnitIdAsync(string fromUnitId);
    Task<IEnumerable<ProdUnitConversion>> GetByToUnitIdAsync(string toUnitId);
    Task<ProdUnitConversion?> GetByProductAndUnitsAsync(string productId, string fromUnitId, string toUnitId);
    Task<bool> ExistsByProductAndUnitsAsync(string productId, string fromUnitId, string toUnitId);
    Task<IEnumerable<ProdUnitConversion>> GetAllWithInactiveAsync();
    
    // Override base methods with navigation includes
    new Task<ProdUnitConversion?> GetByIdAsync(int id);
    new Task<ProdUnitConversion?> GetByIdAsync(string id);
    new Task<bool> ExistsAsync(string id);
    new Task AddAsync(ProdUnitConversion entity);
    new Task AddRangeAsync(IEnumerable<ProdUnitConversion> entities);
    new void Update(ProdUnitConversion entity);
}
```

### 4.2 Repository Class

**Location:** `erp-backend/ERP.Infrastructure/Repositories/ProductUnitConversionRepository.cs`

```csharp
public class ProductUnitConversionRepository : Repository<ProdUnitConversion>, IProductUnitConversionRepository
{
    public ProductUnitConversionRepository(ApplicationDbContext context) : base(context)
    {
    }

    // DRY: Extract common includes to helper method
    private IQueryable<ProdUnitConversion> WithIncludes()
    {
        return GetAllWithActiveFilter()
            .Include(p => p.FromUnit)
            .Include(p => p.ToUnit)
            .Include(p => p.Product);
    }

    // GetByIdAsync with navigation properties
    public new async Task<ProdUnitConversion?> GetByIdAsync(string id)
    {
        return await WithIncludes().FirstOrDefaultAsync(p => p.Id == id);
    }

    // Get all with navigation properties
    public override async Task<IEnumerable<ProdUnitConversion>> GetAllAsync()
    {
        return await WithIncludes().ToListAsync();
    }

    // Create with timestamp
    public new async Task AddAsync(ProdUnitConversion entity)
    {
        entity.CreatedAt = DateTime.UtcNow;
        await _dbSet.AddAsync(entity);
    }

    // Update with timestamp
    public new void Update(ProdUnitConversion entity)
    {
        entity.UpdatedAt = DateTime.UtcNow;
        _dbSet.Update(entity);
    }
}
```

---

## 5. Commands & Handlers

### 5.1 Create Command

```csharp
// Command - Input data
public class CreateProductUnitConversionCommand : IRequest<Result<GetProductUnitConversionByIdDto>>
{
    public string ProductId { get; set; }
    public string FromUnitId { get; set; }
    public string ToUnitId { get; set; }
    public decimal Factor { get; set; }
}

// Handler - Business logic
public class CreateProductUnitConversionCommandHandler : IRequestHandler<CreateProductUnitConversionCommand, Result<GetProductUnitConversionByIdDto>>
{
    private readonly IProductUnitConversionRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public async Task<Result<GetProductUnitConversionByIdDto>> Handle(
        CreateProductUnitConversionCommand request, 
        CancellationToken cancellationToken)
    {
        // Step 1: Check for duplicate
        var existing = await _repository.GetByProductAndUnitsAsync(
            request.ProductId, request.FromUnitId, request.ToUnitId);
        
        if (existing != null)
            return Result<GetProductUnitConversionByIdDto>.Failure(
                "Product unit conversion with these units already exists");

        // Step 2: Create entity
        var entity = new ProdUnitConversion
        {
            Id = Guid.NewGuid().ToString(),
            ProductId = request.ProductId,
            FromUnitId = request.FromUnitId,
            ToUnitId = request.ToUnitId,
            Factor = request.Factor,
            Active = true,
            LastAction = "CREATE",
            CreatedAt = DateTime.UtcNow,
            CreatedBy = "System"
        };

        // Step 3: Save to database
        await _repository.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync();

        // Step 4: Return DTO
        return Result<GetProductUnitConversionByIdDto>.Success(new GetProductUnitConversionByIdDto
        {
            Id = entity.Id,
            ProductId = entity.ProductId,
            FromUnitId = entity.FromUnitId,
            ToUnitId = entity.ToUnitId,
            Factor = entity.Factor,
            Active = entity.Active,
            CreatedAt = entity.CreatedAt,
            CreatedBy = entity.CreatedBy,
            LastAction = entity.LastAction
        });
    }
}
```

### 5.2 Update Command

```csharp
public class UpdateProductUnitConversionCommand : IRequest<Result<GetProductUnitConversionByIdDto>>
{
    public string Id { get; set; }
    public string ProductId { get; set; }
    public string FromUnitId { get; set; }
    public string ToUnitId { get; set; }
    public decimal Factor { get; set; }
    public bool Active { get; set; }
}

public class UpdateProductUnitConversionCommandHandler : IRequestHandler<UpdateProductUnitConversionCommand, Result<GetProductUnitConversionByIdDto>>
{
    // Steps: Check exists → Check duplicate → Update → Save → Return DTO
}
```

### 5.3 Delete Command (Soft Delete)

```csharp
public class DeleteProductUnitConversionCommand : IRequest<Result<int>>
{
    public string Id { get; set; }
}

public class DeleteProductUnitConversionCommandHandler : IRequestHandler<DeleteProductUnitConversionCommand, Result<int>>
{
    public async Task<Result<int>> Handle(DeleteProductUnitConversionCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(request.Id);
        if (entity == null)
            return Result<int>.Failure($"Product unit conversion with ID '{request.Id}' not found");

        // Soft delete: Set Active = false
        entity.Active = false;
        entity.LastAction = "DELETE";
        entity.UpdatedAt = DateTime.UtcNow;
        entity.UpdatedBy = "System";

        _repository.Update(entity);
        await _unitOfWork.SaveChangesAsync();

        return Result<int>.Success(1);
    }
}
```

---

## 6. Validators

### 6.1 Create Validator

```csharp
public class CreateProductUnitConversionCommandValidator : AbstractValidator<CreateProductUnitConversionCommand>
{
    public CreateProductUnitConversionCommandValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty().WithMessage("Product ID is required");

        RuleFor(x => x.FromUnitId)
            .NotEmpty().WithMessage("From Unit ID is required");

        RuleFor(x => x.ToUnitId)
            .NotEmpty().WithMessage("To Unit ID is required")
            .NotEqual(x => x.FromUnitId).WithMessage("To Unit must be different from From Unit");

        RuleFor(x => x.Factor)
            .GreaterThan(0).WithMessage("Factor must be greater than 0");
    }
}
```

### 6.2 Update Validator

Same rules as Create, plus:
```csharp
RuleFor(x => x.Id)
    .NotEmpty().WithMessage("ID is required");
```

---

## 7. API Controller

**Location:** `erp-backend/ERP.API/Controllers/ProductUnitConversionController.cs`

```csharp
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProductUnitConversionController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProductUnitConversionController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get all product unit conversions
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var query = new GetProductUnitConversionQuery();
        var result = await _mediator.Send(query);
        return result.IsSuccess ? Ok(result.Data) : BadRequest(result.ErrorMessage);
    }

    /// <summary>
    /// Get product unit conversion by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var query = new GetProductUnitConversionByIdQuery { Id = id };
        var result = await _mediator.Send(query);
        return result.IsSuccess ? Ok(result.Data) : NotFound(result.ErrorMessage);
    }

    /// <summary>
    /// Create a new product unit conversion
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateProductUnitConversionCommand command)
    {
        var result = await _mediator.Send(command);
        if (!result.IsSuccess) return BadRequest(result.ErrorMessage);
        return CreatedAtAction(nameof(GetById), new { id = result.Data!.Id }, result.Data);
    }

    /// <summary>
    /// Update an existing product unit conversion
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] UpdateProductUnitConversionCommand command)
    {
        if (id != command.Id) return BadRequest("ID in URL does not match ID in request body");
        var result = await _mediator.Send(command);
        return result.IsSuccess ? Ok(result.Data) : BadRequest(result.ErrorMessage);
    }

    /// <summary>
    /// Soft-delete a product unit conversion (sets Active = false)
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var command = new DeleteProductUnitConversionCommand { Id = id };
        var result = await _mediator.Send(command);
        return result.IsSuccess ? NoContent() : NotFound(result.ErrorMessage);
    }
}
```

### Controller Flow

```
HTTP Request
     │
     ▼
Controller (MediatR.Send)
     │
     ▼
Command/Query Handler
     │
     ▼
Repository (EF Core → Database)
     │
     ▼
Response (200 OK / 201 Created / 404 Not Found / 400 Bad Request)
```

### HTTP Status Codes Used

| Method | Status Code | Description |
|--------|-------------|-------------|
| GET | 200 OK | Success |
| GET | 404 Not Found | ID not found |
| POST | 201 Created | Created successfully |
| PUT | 200 OK | Updated successfully |
| PUT | 400 Bad Request | Validation error |
| DELETE | 204 No Content | Deleted successfully |
| DELETE | 404 Not Found | ID not found |

---

## 8. Test Cases

### 7.1 Handler Tests

```csharp
public class CreateProductUnitConversionCommandHandlerTests
{
    [Fact]
    public async Task Handle_ReturnsFailure_WhenConversionAlreadyExists()
    {
        // Arrange
        var existing = new ProdUnitConversion { Id = "1", ProductId = "P1", FromUnitId = "U1", ToUnitId = "U2" };
        _repositoryMock.Setup(r => r.GetByProductAndUnitsAsync("P1", "U1", "U2"))
            .ReturnsAsync(existing);

        var command = new CreateProductUnitConversionCommand
        {
            ProductId = "P1", FromUnitId = "U1", ToUnitId = "U2", Factor = 100
        };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().Contain("already exists");
    }

    [Fact]
    public async Task Handle_CreatesConversion_WhenDataIsValid()
    {
        // Arrange
        _repositoryMock.Setup(r => r.GetByProductAndUnitsAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync((ProdUnitConversion?)null);
        _repositoryMock.Setup(r => r.AddAsync(It.IsAny<ProdUnitConversion>())).Returns(Task.CompletedTask);
        _unitOfWorkMock.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);

        var command = new CreateProductUnitConversionCommand
        {
            ProductId = "P1", FromUnitId = "U1", ToUnitId = "U2", Factor = 100
        };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data!.Factor.Should().Be(100);
        result.Data.Active.Should().BeTrue();
        result.Data.LastAction.Should().Be("CREATE");
    }
}
```

### 7.2 Validator Tests

```csharp
public class CreateProductUnitConversionCommandValidatorTests
{
    private readonly CreateProductUnitConversionCommandValidator _validator = new();

    [Fact]
    public void Validate_Fails_WhenToUnitIdEqualsFromUnitId()
    {
        var command = new CreateProductUnitConversionCommand
        {
            ProductId = "P1", FromUnitId = "U1", ToUnitId = "U1", Factor = 100
        };

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "ToUnitId");
    }

    [Fact]
    public void Validate_Fails_WhenFactorIsZero()
    {
        var command = new CreateProductUnitConversionCommand
        {
            ProductId = "P1", FromUnitId = "U1", ToUnitId = "U2", Factor = 0
        };

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Factor");
    }
}
```

---

## 9. Key Patterns Used

| Pattern | Description | Usage |
|---------|-------------|-------|
| **Clean Architecture** | Layer separation | Domain → Application → Infrastructure → API |
| **CQRS** | Command/Query separation | Commands: Create, Update, Delete. Queries: GetAll, GetById |
| **Repository Pattern** | Data access abstraction | IProductUnitConversionRepository |
| **Soft Delete** | Logical deletion | Active = false instead of physical delete |
| **Active Filter** | Auto-filter inactive records | GetAllWithActiveFilter() |
| **Result Pattern** | Consistent response | Result<T>.Success() / Result<T>.Failure() |
| **FluentValidation** | Input validation | AbstractValidator<T> |
| **MediatR** | Request/Handler routing | IRequestHandler<TRequest, TResult> |

---

## 10. Running Tests

```bash
# Run all ProductUnitConversion tests
dotnet test erp-backend/ERP.Tests/ERP.Tests.csproj --filter "FullyQualifiedName~ProductUnitConversion"

# Test output
# Passed! - Failed: 0, Passed: 24, Skipped: 0
```

---

*Version: 2026-03-24 | Module: ProductUnitConversion*
