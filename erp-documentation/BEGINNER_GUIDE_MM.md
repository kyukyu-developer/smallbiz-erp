# ERP Backend - စတင်သူများအတွက် လမ်းညွှန်စာ (Myanmar)

> **ရည်ရွယ်ချက်:** ဤ document သည် ERP Backend ကို နားလည်ပြီး new form တစ်ခု ထပ်ထည့်နိုင်ရန် beginner developer များအတွက် ရေးထားသည်။

---

## မာတိကာ

1. [Project Structure နားလည်ခြင်း](#1-project-structure-နားလည်ခြင်း)
2. [Layer တစ်ခုချင်းစီ၏ အခန်းကဏ္ဍ](#2-layer-တစ်ခုချင်းစီ၏-အခန်းကဏ္ဍ)
3. [Request ၏ ခရီးစဉ် (Request Flow)](#3-request-ခရီးစဉ်)
4. [Test Cases နားလည်ခြင်း](#4-test-cases-နားလည်ခြင်း)
5. [New Form တစ်ခု ထပ်ထည့်နည်း (Step-by-Step)](#5-new-form-ထပ်ထည့်နည်း)
6. [အဓိက Patterns များ](#6-အဓိက-patterns-များ)

---

## 1. Project Structure နားလည်ခြင်း

```
erp-backend/
├── ERP.Domain/           ← Business Rule (ကုမ္ပဏီ၏ Policies)
├── ERP.Application/      ← Use Cases (လုပ်ဆောင်ချက်များ)
├── ERP.Infrastructure/   ← Database, Files (Storage)
├── ERP.API/              ← HTTP Endpoints (Internet နဲ့ ဆက်သွယ်မှု)
└── ERP.Tests/            ← Testing Code (စစ်ဆေးချက်)
```

### ဘာကြောင့် 4 Layer လဲ?

**ဥပမာ - စားသောက်ဆိုင် တစ်ဆိုင်ကို စဉ်းစားကြည့်:**

| Layer | ဆိုင်မှာ ဘာနဲ့ တူသလဲ | ဥပမာ |
|-------|----------------------|------|
| `ERP.Domain` | Menu Policy (ဘာကို ရောင်းမည်? စည်းမျဉ်း) | "Main Warehouse သာ supplier ဆီမှ stock လက်ခံနိုင်သည်" |
| `ERP.Application` | Waiter / Cashier (အမှာ handle လုပ်ခြင်း) | "Warehouse တစ်ခု create မည်" |
| `ERP.Infrastructure` | Kitchen / Fridge (data သိမ်းဆည်းခြင်း) | Database ထဲ save လုပ်ခြင်း |
| `ERP.API` | Front Door / Menu (customer နဲ့ ဆက်သွယ်) | HTTP API endpoint |

**အရေးကြီးသော Dependency Rule:**
- `Domain` → ဘာမှ depend မလုပ်
- `Application` → `Domain` ကိုသာ depend
- `Infrastructure` → `Application` + `Domain` depend
- `API` → အများဆုံး depend (ဒါပေမဲ့ Domain Rule ကို မပြောင်းနိုင်)

---

## 2. Layer တစ်ခုချင်းစီ၏ အခန်းကဏ္ဍ

### 2.1 ERP.Domain — Business Entities

ဤ layer တွင် **ဘာရှိသည်** နှင့် **ဘာ rule ရှိသည်** ကို သတ်မှတ်သည်။

**ဥပမာ — Warehouse Entity** ([ERP.Domain/Entities/Warehouse.cs](erp-backend/ERP.Domain/Entities/Warehouse.cs))

```csharp
public class Warehouse : AuditableEntity
{
    public string Id { get; set; }           // Warehouse ID (GUID)
    public string Name { get; set; }         // နာမည် (ဥပမာ: "HQ Warehouse")
    public string? City { get; set; }        // မြို့ (Optional)
    public BranchType BranchType { get; set; } // Main / Branch / Sub
    public bool IsMainWarehouse { get; set; }  // Main ဆိုရင် true
    public string? ParentWarehouseId { get; set; } // မိဘ Warehouse ID
    public bool Active { get; set; } = true; // Soft Delete အတွက်
    public string? LastAction { get; set; }  // "CREATE" / "UPDATE"

    // Navigation (မိဘ-သားသမီး ဆက်နွှယ်မှု)
    public Warehouse? ParentWarehouse { get; set; }
    public ICollection<Warehouse> ChildWarehouses { get; set; }
}
```

**BranchType Enum** — Warehouse ၏ အမျိုးအစား:
```csharp
// ERP.Domain/Enums/BranchType.cs (ဖန်တီးထားသော enum)
public enum BranchType
{
    Main = 1,   // ပင်မဂိုဒေါင် — supplier ဆီမှ stock လက်ခံနိုင်
    Branch = 2, // ဌာနခွဲ — Main ဆီမှ stock ရနိုင်
    Sub = 3     // တပ်ဖွဲ့ခွဲ — Main သို့မဟုတ် Branch ဆီမှ ရနိုင်
}
```

**AuditableEntity** — base class, tracking fields:
```csharp
// ERP.Domain/Common/AuditableEntity.cs
public abstract class AuditableEntity
{
    public DateTime CreatedAt { get; set; }   // ဖန်တီးသည့် ရက်စွဲ
    public DateTime? UpdatedAt { get; set; }  // ပြင်ဆင်သည့် ရက်စွဲ
    public string? CreatedBy { get; set; }    // ဖန်တီးသူ
    public string? UpdatedBy { get; set; }    // ပြင်ဆင်သူ
}
```

---

### 2.2 ERP.Application — Use Cases (CQRS Pattern)

ဤ layer သည် **ဘာလုပ်ဆောင်မည်** ကို ဆုံးဖြတ်သည်။
**CQRS** = Command Query Responsibility Segregation

```
Commands (data ပြောင်းသည်):    Queries (data ဖတ်သည်):
  CreateWarehouseCommand          GetWarehousesQuery
  UpdateWarehouseCommand          GetWarehouseByIdQuery
  DeleteWarehouseCommand
```

**Result Pattern** — Response wrapper ([ERP.Application/DTOs/Common/Result.cs](erp-backend/ERP.Application/DTOs/Common/Result.cs)):

```csharp
public class Result<T>
{
    public bool IsSuccess { get; set; }      // အောင်မြင်လားဆိုတာ
    public T? Data { get; set; }             // ပြန်လာသော data
    public string? ErrorMessage { get; set; } // Error message
    public List<string> Errors { get; set; }  // Errors များ

    // Static helper methods:
    public static Result<T> Success(T data) => new() { IsSuccess = true, Data = data };
    public static Result<T> Failure(string error) => new() { IsSuccess = false, ErrorMessage = error };
}
```

**Command ဥပမာ** — CreateWarehouseCommand ([erp-backend/ERP.Application/Features/Warehouses/Commands/CreateWarehouseCommand.cs](erp-backend/ERP.Application/Features/Warehouses/Commands/CreateWarehouseCommand.cs)):

```csharp
// Command = "ငါ warehouse တစ်ခု ဖန်တီးချင်တယ်" ဆိုတဲ့ message
public class CreateWarehouseCommand : IRequest<Result<WarehouseDto>>
{
    public string Name { get; set; }           // လိုအပ်သည်
    public string? City { get; set; }          // မလိုအပ်
    public BranchType BranchType { get; set; } // Main/Branch/Sub
    public string? ParentWarehouseId { get; set; } // Branch/Sub ဆိုရင် လိုသည်
    // ... အခြား fields
}
```

**Handler ဥပမာ** — CreateWarehouseCommandHandler ([erp-backend/ERP.Application/Features/Warehouses/Commands/CreateWarehouseCommandHandler.cs](erp-backend/ERP.Application/Features/Warehouses/Commands/CreateWarehouseCommandHandler.cs)):

```csharp
public class CreateWarehouseCommandHandler : IRequestHandler<CreateWarehouseCommand, Result<WarehouseDto>>
{
    // ① Field Declaration — "ငါ warehouse repository တစ်ခု လိုသည်" ဟု ကြေငြာသည်
    private readonly IWarehouseRepository _warehouseRepository;
    //  ↑ private    ↑ readonly           ↑ type (interface)   ↑ variable name
    private readonly IUnitOfWork _unitOfWork;

    // ② Constructor — DI system မှ object ထည့်ပေးသည်
    public CreateWarehouseCommandHandler(
        IWarehouseRepository warehouseRepository,
        IUnitOfWork unitOfWork)
    {
        _warehouseRepository = warehouseRepository; // inject လုပ်ပေးသည်
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<WarehouseDto>> Handle(CreateWarehouseCommand request, CancellationToken cancellationToken)
    {
        // Step 1: Parent warehouse ရှိမရှိ စစ်ဆေးသည်
        if (!string.IsNullOrEmpty(request.ParentWarehouseId))
        {
            var parentExists = await _warehouseRepository.ExistsAsync(request.ParentWarehouseId);
            if (!parentExists)
                return Result<WarehouseDto>.Failure("Parent warehouse does not exist");
        }

        // Step 2: အမည်တူ + မြို့တူ ရှိမရှိ စစ်ဆေးသည်
        var existing = await _warehouseRepository.GetByNameAndCityAsync(request.Name, request.City);
        if (existing != null)
            return Result<WarehouseDto>.Failure($"Warehouse '{request.Name}' already exists in {request.City}");

        // Step 3: Entity အသစ် ဖန်တီးသည်
        var warehouse = new Warehouse
        {
            Id = Guid.NewGuid().ToString(), // Auto-generate ID
            Name = request.Name,
            BranchType = request.BranchType,
            IsMainWarehouse = request.BranchType == BranchType.Main, // Main ဆိုရင် true
            LastAction = "CREATE",
            Active = true
            // ...
        };

        // Step 4: Database ထဲ သွင်းသည်
        await _warehouseRepository.AddAsync(warehouse);
        await _unitOfWork.SaveChangesAsync(); // Transaction commit

        // Step 5: DTO ပြန်ပေးသည်
        return Result<WarehouseDto>.Success(new WarehouseDto { ... });
    }
}
```

#### `_warehouseRepository` အကြောင်း အသေးစိတ်

**`private readonly` ဘာကြောင့် သုံးသလဲ?**
- `private` — class ပြင်ပမှ မမြင်နိုင်ရန်
- `readonly` — constructor ထဲတွင် inject ပြီးနောက် အခြား object နဲ့ မပြောင်းနိုင်ရန် (immutable)

**ဘာကြောင့် `IWarehouseRepository` (Interface) ကို သုံးသလဲ?**

```
IWarehouseRepository (Interface)        WarehouseRepository (Real Implementation)
      ↓                                           ↓
"ဘာ method ရှိသည်" ဟု                   "အဲ့ method တွေကို ဘယ်လို"
  သတ်မှတ်သည်                             SQL Server ဖြင့် လုပ်ဆောင်သည်
```

Application Layer သည် Interface ကိုသာ သိသည် — Real database code ကို မသိ။
ဒါကြောင့် Test မှာ **Fake (Mock) Repository** နဲ့ လဲနိုင်သည်:

```csharp
// Real Code → SQL Server နဲ့ ဆက်သွယ်
IWarehouseRepository repo = new WarehouseRepository(dbContext);

// Test Code → Fake/pretend, database မလို
IWarehouseRepository repo = new Mock<IWarehouseRepository>().Object;
```

**Handler ထဲတွင် `_warehouseRepository` မှ ဘာ methods တွေ ခေါ်သလဲ:**

| Method | ဘာစစ်သလဲ / ဘာလုပ်သလဲ | Step |
|--------|----------------------|------|
| `ExistsAsync(parentId)` | Parent warehouse ရှိမရှိ စစ်ဆေးသည် | Step 1 |
| `GetByNameAndCityAsync(name, city)` | Duplicate name+city စစ်ဆေးသည် | Step 2 |
| `AddAsync(warehouse)` | Database ထဲ record အသစ် သွင်းသည် | Step 4 |

**Data Flow:**

```
Handler._warehouseRepository.ExistsAsync("some-id")
              │
              │  (DI ဖြင့် inject ထားသော WarehouseRepository object)
              ▼
    WarehouseRepository.ExistsAsync()    ← Infrastructure Layer
              │
              ▼
    EF Core → SQL: SELECT COUNT(*) FROM Warehouses WHERE Id = 'some-id'
              │
              ▼
         SQL Server Database
```

**Validator ဥပမာ** — CreateWarehouseCommandValidator ([erp-backend/ERP.Application/Features/Warehouses/Commands/CreateWarehouseCommandValidator.cs](erp-backend/ERP.Application/Features/Warehouses/Commands/CreateWarehouseCommandValidator.cs)):

```csharp
public class CreateWarehouseCommandValidator : AbstractValidator<CreateWarehouseCommand>
{
    public CreateWarehouseCommandValidator()
    {
        // Name: လိုအပ်သည်, အများဆုံး 50 လုံး
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Warehouse name is required")
            .MaximumLength(50).WithMessage("Warehouse name cannot exceed 50 characters");

        // Branch/Sub ဆိုရင် Parent လိုသည်
        RuleFor(x => x.ParentWarehouseId)
            .NotEmpty().WithMessage("Parent warehouse is required for Branch and Sub warehouses")
            .When(x => x.BranchType == BranchType.Branch || x.BranchType == BranchType.Sub);

        // Main ဆိုရင် Parent မရှိရ
        RuleFor(x => x.ParentWarehouseId)
            .Empty().WithMessage("Main warehouse cannot have a parent warehouse")
            .When(x => x.BranchType == BranchType.Main);
    }
}
```

---

### 2.3 ERP.Infrastructure — Data Access

Database နဲ့ ဆက်သွယ်သည်။ Repository Pattern ကို အသုံးပြုသည်။

**Interface (Domain Layer တွင် သတ်မှတ်):**
```csharp
// Application layer မှ ဤ interface ကိုသာ သိသည် (Implementation မသိ)
public interface IWarehouseRepository
{
    Task<Warehouse?> GetByIdAsync(string id);
    Task<IEnumerable<Warehouse>> GetAllAsync();
    Task<Warehouse?> GetByNameAndCityAsync(string name, string? city);
    Task<bool> ExistsAsync(string id);
    Task AddAsync(Warehouse warehouse);
    // ...
}
```

**Implementation (Infrastructure Layer တွင်):**
```csharp
// ERP.Infrastructure/Repositories/WarehouseRepository.cs
public class WarehouseRepository : IWarehouseRepository
{
    private readonly ApplicationDbContext _context;

    public async Task<Warehouse?> GetByNameAndCityAsync(string name, string? city)
    {
        // EF Core ဖြင့် Database Query
        return await _context.Warehouses
            .FirstOrDefaultAsync(w => w.Name == name && w.City == city);
    }
    // ...
}
```

---

### 2.4 ERP.API — Controllers

HTTP Request ကို receive လုပ်ပြီး MediatR ဖြင့် handler ကို ဆက်လွှဲပေးသည်။

**ProductsController ဥပမာ** ([erp-backend/ERP.API/Controllers/ProductsController.cs](erp-backend/ERP.API/Controllers/ProductsController.cs)):

```csharp
[ApiController]
[Route("api/[controller]")]  // URL: /api/products
[Authorize]                   // JWT token လိုသည်
public class ProductsController : ControllerBase
{
    private readonly IMediator _mediator; // CQRS Handler ကို ခေါ်ရန်

    // GET /api/products?categoryId=1&search=phone
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int? categoryId, [FromQuery] string? search)
    {
        var query = new GetProductsQuery { CategoryId = categoryId, SearchTerm = search };
        var result = await _mediator.Send(query); // Handler ဆီ ဆက်လွှဲ

        if (!result.IsSuccess)
            return BadRequest(result.ErrorMessage); // 400 Error

        return Ok(result.Data); // 200 OK + Data
    }

    // POST /api/products
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateProductCommand command)
    {
        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
            return BadRequest(result.ErrorMessage);

        // 201 Created + Location Header
        return CreatedAtAction(nameof(GetById), new { id = result.Data!.Id }, result.Data);
    }
}
```

---

## 3. Request ခရီးစဉ်

### POST /api/warehouses (Create Warehouse) ဥပမာ

```
Client (Postman/Frontend)
       │
       │  POST /api/warehouses
       │  { "name": "HQ WH", "branchType": 1 }
       ▼
┌─────────────────────────────┐
│  ERP.API                    │
│  WarehousesController       │
│  → Create() method          │
│  → _mediator.Send(command)  │
└──────────────┬──────────────┘
               │  MediatR routing
               ▼
┌─────────────────────────────┐
│  ERP.Application            │
│  CreateWarehouseCommandHandler │
│  1. Validate parent exists  │
│  2. Check duplicate name    │
│  3. Create Warehouse object │
│  4. Call repository.Add()   │
│  5. SaveChanges()           │
│  6. Return DTO              │
└──────────────┬──────────────┘
               │
               ▼
┌─────────────────────────────┐
│  ERP.Infrastructure         │
│  WarehouseRepository        │
│  → EF Core → SQL Server     │
│  INSERT INTO Warehouses...  │
└─────────────────────────────┘
               │
               ▼
        Response: 201 Created
        { "id": "abc-123", "name": "HQ WH", ... }
```

---

## 4. Test Cases နားလည်ခြင်း

### 4.1 Test ဆိုတာ ဘာလဲ?

Test သည် "ဤ code ကို ဤ input ပေးလျှင် ဤ output ထွက်ရမည်" ဟု automatic စစ်ဆေးသည့် code ဖြစ်သည်။

**Tools ကိုသုံးသည်:**
- **xUnit** — Test framework (test ကို run လုပ်ပေးသည်)
- **Moq** — Mock objects (real database မသုံးပဲ fake ဖြင့် test)
- **FluentAssertions** — `.Should().Be()` ဖြင့် readable assertions

---

### 4.2 CreateWarehouseCommandHandlerTests ([erp-backend/ERP.Tests/Warehouses/CreateWarehouseCommandHandlerTests.cs](erp-backend/ERP.Tests/Warehouses/CreateWarehouseCommandHandlerTests.cs))

**Test Setup — Moq ဖြင့် fake dependencies ဖန်တီးခြင်း:**

```csharp
public class CreateWarehouseCommandHandlerTests
{
    private readonly Mock<IWarehouseRepository> _repoMock;  // Fake Repository
    private readonly Mock<IUnitOfWork> _uowMock;            // Fake UnitOfWork
    private readonly CreateWarehouseCommandHandler _handler;

    public CreateWarehouseCommandHandlerTests()
    {
        _repoMock = new Mock<IWarehouseRepository>();
        _uowMock = new Mock<IUnitOfWork>();
        // Handler ထဲ fake objects ထည့်ပေးသည် (real database မလိုဘူး!)
        _handler = new CreateWarehouseCommandHandler(_repoMock.Object, _uowMock.Object);
    }
}
```

**ဘာကြောင့် Mock လဲ?**
- Real database test မှာ ကြန့်ကြာသည်
- Test database setup မလိုဘဲ test ရေးနိုင်သည်
- Database error ကြောင့် test fail မဖြစ်

---

#### Test Case 1: Happy Path (အောင်မြင်သောကိစ္စ)

```csharp
[Fact]
public async Task Handle_CreatesWarehouse_AndReturnsDto_WhenValidMainWarehouse()
{
    // ARRANGE — ပြင်ဆင်ခြင်း
    var command = new CreateWarehouseCommand
    {
        Name = "HQ Warehouse",
        City = "Bangkok",
        BranchType = BranchType.Main
    };

    // Mock: "HQ Warehouse" + "Bangkok" မရှိဘူး ဆိုတဲ့ pretend
    _repoMock
        .Setup(r => r.GetByNameAndCityAsync("HQ Warehouse", "Bangkok"))
        .ReturnsAsync((Warehouse?)null); // null = မရှိဘူး

    // ACT — လုပ်ဆောင်ခြင်း
    var result = await _handler.Handle(command, CancellationToken.None);

    // ASSERT — စစ်ဆေးခြင်း
    result.IsSuccess.Should().BeTrue();           // အောင်မြင်ရမည်
    result.Data.Should().NotBeNull();             // Data ရှိရမည်
    result.Data!.Name.Should().Be("HQ Warehouse"); // Name မှန်ရမည်
    result.Data.IsMainWarehouse.Should().BeTrue(); // Main ဆိုရင် true
    result.Data.LastAction.Should().Be("CREATE"); // LastAction မှန်ရမည်
}
```

**AAA Pattern ကို မှတ်ထားပါ:**
- **A**rrange — Setup data, Mocks
- **A**ct — Handler ကို ခေါ်သည်
- **A**ssert — Result ကို စစ်ဆေးသည်

---

#### Test Case 2: Parent Warehouse မရှိသောကိစ္စ

```csharp
[Fact]
public async Task Handle_ReturnsFailure_WhenParentWarehouseDoesNotExist()
{
    // ARRANGE
    var command = new CreateWarehouseCommand
    {
        Name = "Branch WH",
        BranchType = BranchType.Branch,
        ParentWarehouseId = "nonexistent-parent" // မရှိသော ID
    };

    // Mock: "nonexistent-parent" ID မရှိဘူး ဆိုသည်
    _repoMock
        .Setup(r => r.ExistsAsync("nonexistent-parent"))
        .ReturnsAsync(false); // false = မရှိ

    // ACT
    var result = await _handler.Handle(command, CancellationToken.None);

    // ASSERT
    result.IsSuccess.Should().BeFalse(); // Fail ဖြစ်ရမည်
    result.ErrorMessage.Should().Be("Parent warehouse does not exist");

    // AddAsync ကို လုံးဝ မခေါ်ရ (save မလုပ်ရ)
    _repoMock.Verify(r => r.AddAsync(It.IsAny<Warehouse>()), Times.Never);
}
```

---

#### Test Case 3: Duplicate Name+City ရှိသောကိစ္စ

```csharp
[Fact]
public async Task Handle_ReturnsFailure_WhenDuplicateNameAndCityExist()
{
    // ARRANGE
    var command = new CreateWarehouseCommand
    {
        Name = "HQ Warehouse",
        City = "Bangkok",
        BranchType = BranchType.Main
    };

    // Mock: ဒီ name+city ရှိပြီးသား ဆိုသည်
    _repoMock
        .Setup(r => r.GetByNameAndCityAsync("HQ Warehouse", "Bangkok"))
        .ReturnsAsync(new Warehouse { Id = "existing-1", Name = "HQ Warehouse" });

    // ACT
    var result = await _handler.Handle(command, CancellationToken.None);

    // ASSERT
    result.IsSuccess.Should().BeFalse();
    result.ErrorMessage.Should().Be(
        "Warehouse with name 'HQ Warehouse' already exists in Bangkok");
    _repoMock.Verify(r => r.AddAsync(It.IsAny<Warehouse>()), Times.Never);
}
```

---

#### Test Case 4: Verify method calls (Mock Verify)

```csharp
[Fact]
public async Task Handle_CallsAddAsync_AndSaveChanges()
{
    // ARRANGE
    var command = new CreateWarehouseCommand { Name = "HQ WH", BranchType = BranchType.Main };
    _repoMock
        .Setup(r => r.GetByNameAndCityAsync(command.Name, command.City))
        .ReturnsAsync((Warehouse?)null);

    // ACT
    await _handler.Handle(command, CancellationToken.None);

    // ASSERT — method တွေကို စစ်ဆေးသည်
    _repoMock.Verify(r => r.AddAsync(It.IsAny<Warehouse>()), Times.Once);
    // ↑ AddAsync ကို တစ်ကြိမ်တိတိ ခေါ်ရမည်

    _uowMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    // ↑ SaveChangesAsync ကို တစ်ကြိမ်တိတိ ခေါ်ရမည်
}
```

---

### 4.3 CreateWarehouseCommandValidatorTests ([erp-backend/ERP.Tests/Warehouses/CreateWarehouseCommandValidatorTests.cs](erp-backend/ERP.Tests/Warehouses/CreateWarehouseCommandValidatorTests.cs))

**Validator Tests — Input Validation စစ်ဆေးခြင်း:**

```csharp
public class CreateWarehouseCommandValidatorTests
{
    private readonly CreateWarehouseCommandValidator _validator = new();

    // ─── Name Validation Tests ────────────────────────────────────────────

    [Fact]
    public void Validate_Fails_WhenNameIsEmpty()
    {
        var command = new CreateWarehouseCommand { Name = "", BranchType = BranchType.Main };

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
        // Error message မှန်ကန်ရမည်
        result.Errors.Should().Contain(e =>
            e.PropertyName == "Name" &&
            e.ErrorMessage == "Warehouse name is required");
    }

    [Fact]
    public void Validate_Fails_WhenNameExceeds50Characters()
    {
        var command = new CreateWarehouseCommand
        {
            Name = new string('A', 51), // 51 လုံး — limit ကျော်
            BranchType = BranchType.Main
        };

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e =>
            e.PropertyName == "Name" &&
            e.ErrorMessage == "Warehouse name cannot exceed 50 characters");
    }

    // ─── Business Rule: Parent Validation Tests ────────────────────────────

    [Fact]
    public void Validate_Fails_WhenBranchWarehouseHasNoParent()
    {
        var command = new CreateWarehouseCommand
        {
            Name = "Branch WH",
            BranchType = BranchType.Branch
            // ParentWarehouseId မပေးဘူး!
        };

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e =>
            e.PropertyName == "ParentWarehouseId" &&
            e.ErrorMessage == "Parent warehouse is required for Branch and Sub warehouses");
    }

    [Fact]
    public void Validate_Fails_WhenMainWarehouseHasParent()
    {
        var command = new CreateWarehouseCommand
        {
            Name = "Main WH",
            BranchType = BranchType.Main,
            ParentWarehouseId = "some-parent" // Main ဆိုရင် မရှိရ!
        };

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e =>
            e.PropertyName == "ParentWarehouseId" &&
            e.ErrorMessage == "Main warehouse cannot have a parent warehouse");
    }

    // ─── [Theory] — Parameter-driven tests ─────────────────────────────────

    // InlineData ဖြင့် test case များကို group လုပ်နိုင်သည်
    [Theory]
    [InlineData(101, "Location cannot exceed 100 characters", "Location")]
    [InlineData(256, "Address cannot exceed 255 characters", "Address")]
    public void Validate_Fails_WhenOptionalStringFieldExceedsLimit(
        int length, string expectedMessage, string propertyName)
    {
        var command = new CreateWarehouseCommand { Name = "WH", BranchType = BranchType.Main };

        if (propertyName == "Location") command.Location = new string('X', length);
        else if (propertyName == "Address") command.Address = new string('X', length);

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e =>
            e.PropertyName == propertyName && e.ErrorMessage == expectedMessage);
    }
}
```

---

### 4.4 GetWarehousesQueryHandlerTests ([erp-backend/ERP.Tests/Warehouses/GetWarehousesQueryHandlerTests.cs](erp-backend/ERP.Tests/Warehouses/GetWarehousesQueryHandlerTests.cs))

**Query Handler Tests — Data ဖတ်ခြင်း စစ်ဆေးသည်:**

```csharp
// ─── Filter Routing Tests ──────────────────────────────────────────────────

[Fact]
public async Task Handle_CallsGetAllAsync_WhenNoFiltersProvided()
{
    _repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Warehouse>());

    await _handler.Handle(new GetWarehousesQuery(), CancellationToken.None);

    // Filter မပေးရင် GetAllAsync ကိုသာ ခေါ်ရမည်
    _repoMock.Verify(r => r.GetAllAsync(), Times.Once);
    _repoMock.Verify(r => r.GetMainWarehousesAsync(), Times.Never);
}

// ─── Active/Inactive Filtering ─────────────────────────────────────────────

[Fact]
public async Task Handle_ReturnsOnlyActiveWarehouses_ByDefault()
{
    var warehouses = new List<Warehouse>
    {
        new Warehouse { Id="1", Name="Active WH", Active=true, BranchType=BranchType.Main },
        new Warehouse { Id="2", Name="Inactive WH", Active=false, BranchType=BranchType.Main }
    };
    _repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(warehouses);

    var result = await _handler.Handle(new GetWarehousesQuery(), CancellationToken.None);

    // Default ဆိုရင် Active warehouse ကိုသာ ပြရမည်
    result.Data.Should().HaveCount(1);
    result.Data![0].Name.Should().Be("Active WH");
}

[Fact]
public async Task Handle_ReturnsAllWarehouses_WhenIncludeInactiveIsTrue()
{
    // ... setup same as above ...
    var result = await _handler.Handle(
        new GetWarehousesQuery { IncludeInactive = true }, CancellationToken.None);

    // IncludeInactive=true ဆိုရင် ၂ ခုလုံး ပြရမည်
    result.Data.Should().HaveCount(2);
}
```

---

## 5. New Form ထပ်ထည့်နည်း

### 5.1 ဥပမာ — "Category" Module ကို ကြည့်ပြီး Pattern နားလည်ပါ

Category module တွင် files အောက်ပါအတိုင်း ရှိသည်:

```
ERP.Domain/
  Entities/Category.cs

ERP.Application/
  DTOs/Categories/CategoryDto.cs
  Features/Categories/
    Commands/
      CreateCategoryCommand.cs
      CreateCategoryCommandHandler.cs
      UpdateCategoryCommand.cs
      UpdateCategoryCommandHandler.cs
      DeleteCategoryCommand.cs
      DeleteCategoryCommandHandler.cs
    Queries/
      GetCategoriesQuery.cs
      GetCategoriesQueryHandler.cs
      GetCategoryByIdQuery.cs
      GetCategoryByIdQueryHandler.cs

ERP.Infrastructure/
  Repositories/CategoryRepository.cs

ERP.API/
  Controllers/CategoriesController.cs
```

### 5.2 New Form ("Supplier") တစ်ခု ထပ်ထည့်သော Checklist

```
[ ] 1. ERP.Domain/Entities/Supplier.cs          → Entity ဖန်တီး
[ ] 2. ERP.Domain/Interfaces/ISupplierRepository.cs → Interface ဖန်တီး
[ ] 3. ERP.Application/DTOs/Suppliers/SupplierDto.cs → DTO ဖန်တီး
[ ] 4. ERP.Application/Features/Suppliers/
         Commands/CreateSupplierCommand.cs       → Command ဖန်တီး
         Commands/CreateSupplierCommandHandler.cs → Handler ဖန်တီး
         Queries/GetSuppliersQuery.cs            → Query ဖန်တီး
         Queries/GetSuppliersQueryHandler.cs     → Query Handler ဖန်တီး
[ ] 5. ERP.Infrastructure/Repositories/SupplierRepository.cs → Repository ဖန်တီး
[ ] 6. ERP.Infrastructure/Data/ApplicationDbContext.cs → DbSet ထည့်
[ ] 7. ERP.Application/DependencyInjection.cs   → Register services
[ ] 8. ERP.API/Controllers/SuppliersController.cs → Controller ဖန်တီး
[ ] 9. Migration run (dotnet ef migrations add)
[ ] 10. ERP.Tests/Suppliers/ → Test ရေးသည်
```

### 5.3 Step-by-Step: New Module ဖန်တီးနည်း

**Step 1 — Entity ဖန်တီး** (`ERP.Domain/Entities/MyEntity.cs`):
```csharp
using ERP.Domain.Common;

namespace ERP.Domain.Entities
{
    public class MyEntity : AuditableEntity
    {
        // AuditableEntity မှ: Id, CreatedAt, UpdatedAt, CreatedBy, UpdatedBy
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
```

**Step 2 — DTO ဖန်တီး** (`ERP.Application/DTOs/MyEntities/MyEntityDto.cs`):
```csharp
namespace ERP.Application.DTOs.MyEntities
{
    public class MyEntityDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsActive { get; set; }
    }
}
```

**Step 3 — Command ဖန်တီး** (`ERP.Application/Features/MyEntities/Commands/CreateMyEntityCommand.cs`):
```csharp
using MediatR;
using ERP.Application.DTOs.MyEntities;
using ERP.Application.DTOs.Common;

namespace ERP.Application.Features.MyEntities.Commands
{
    public class CreateMyEntityCommand : IRequest<Result<MyEntityDto>>
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}
```

**Step 4 — Handler ဖန်တီး** (`ERP.Application/Features/MyEntities/Commands/CreateMyEntityCommandHandler.cs`):
```csharp
using MediatR;
using ERP.Application.DTOs.MyEntities;
using ERP.Application.DTOs.Common;
using ERP.Domain.Entities;
using ERP.Domain.Interfaces;

namespace ERP.Application.Features.MyEntities.Commands
{
    public class CreateMyEntityCommandHandler : IRequestHandler<CreateMyEntityCommand, Result<MyEntityDto>>
    {
        private readonly IRepository<MyEntity> _repository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateMyEntityCommandHandler(IRepository<MyEntity> repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<MyEntityDto>> Handle(CreateMyEntityCommand request, CancellationToken cancellationToken)
        {
            var entity = new MyEntity
            {
                Name = request.Name,
                Description = request.Description,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "System"
            };

            await _repository.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            return Result<MyEntityDto>.Success(new MyEntityDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                IsActive = entity.IsActive
            });
        }
    }
}
```

**Step 5 — Controller ဖန်တီး** (`ERP.API/Controllers/MyEntitiesController.cs`):
```csharp
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using ERP.Application.Features.MyEntities.Commands;
using ERP.Application.Features.MyEntities.Queries;

namespace ERP.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // → /api/myentities
    [Authorize]
    public class MyEntitiesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MyEntitiesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetMyEntitiesQuery());
            return result.IsSuccess ? Ok(result.Data) : BadRequest(result.ErrorMessage);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateMyEntityCommand command)
        {
            var result = await _mediator.Send(command);
            if (!result.IsSuccess) return BadRequest(result.ErrorMessage);
            return CreatedAtAction(nameof(GetById), new { id = result.Data!.Id }, result.Data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(new GetMyEntityByIdQuery { Id = id });
            return result.IsSuccess ? Ok(result.Data) : NotFound(result.ErrorMessage);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateMyEntityCommand command)
        {
            if (id != command.Id) return BadRequest("ID mismatch");
            var result = await _mediator.Send(command);
            return result.IsSuccess ? Ok(result.Data) : BadRequest(result.ErrorMessage);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteMyEntityCommand { Id = id });
            return result.IsSuccess ? NoContent() : NotFound(result.ErrorMessage);
        }
    }
}
```

---

### 5.4 New Module အတွက် Test ရေးနည်း

**Test File Template** (`ERP.Tests/MyEntities/CreateMyEntityCommandHandlerTests.cs`):

```csharp
using ERP.Application.Features.MyEntities.Commands;
using ERP.Domain.Entities;
using ERP.Domain.Interfaces;
using FluentAssertions;
using Moq;

namespace ERP.Tests.MyEntities;

public class CreateMyEntityCommandHandlerTests
{
    // 1. Mock objects ဖန်တီး
    private readonly Mock<IRepository<MyEntity>> _repoMock = new();
    private readonly Mock<IUnitOfWork> _uowMock = new();
    private readonly CreateMyEntityCommandHandler _handler;

    public CreateMyEntityCommandHandlerTests()
    {
        _handler = new CreateMyEntityCommandHandler(_repoMock.Object, _uowMock.Object);
    }

    // 2. Happy Path Test
    [Fact]
    public async Task Handle_ReturnsSuccess_WhenValidCommand()
    {
        // Arrange
        var command = new CreateMyEntityCommand { Name = "Test Entity" };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data!.Name.Should().Be("Test Entity");
        _repoMock.Verify(r => r.AddAsync(It.IsAny<MyEntity>()), Times.Once);
        _uowMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    // 3. Test run ပြုလုပ်ခြင်း:
    // Terminal ထဲ:  dotnet test
    // သို့မဟုတ် VS Code ထဲ Test Explorer ဖြင့်
}
```

---

## 6. အဓိက Patterns များ

### 6.1 Pattern Summary Table

| Pattern | ဘာကြောင့်သုံးသလဲ | ဘယ်မှာ ရှိသလဲ |
|---------|-----------------|--------------|
| **Clean Architecture** | Layer တစ်ခုချင်းစီ ကိုယ်ပိုင် တာဝန် | Project structure |
| **CQRS** | Read/Write ကို ခွဲခြားသည် | Commands vs Queries |
| **Repository Pattern** | Database logic ကို Application မှ ခွဲထားသည် | Infrastructure/Repositories |
| **MediatR** | Command→Handler routing | API Controller → Handler |
| **FluentValidation** | Input validation | CommandValidator classes |
| **Result Pattern** | Error handling ကို uniform ဖြစ်စေသည် | `Result<T>` class |
| **Soft Delete** | Data ကို physically မဖျက်ပဲ `Active=false` ထားသည် | `Active` field |
| **Audit Trail** | ဖန်တီးသူ/ပြင်ဆင်သူ track လုပ်သည် | `AuditableEntity` |

### 6.2 HTTP Status Code များ

| Code | အဓိပ္ပာယ် | ဘယ်အချိန် သုံးသလဲ |
|------|----------|-----------------|
| `200 OK` | အောင်မြင်သည် | GET, PUT |
| `201 Created` | ဖန်တီးပြီး | POST |
| `204 No Content` | ဖျက်ပြီး | DELETE |
| `400 Bad Request` | Input မှားသည် | Validation Error |
| `401 Unauthorized` | Token မပါ | JWT missing |
| `404 Not Found` | Data မရှိ | GET by ID |

### 6.3 Test ကို Run နည်း

```bash
# Project root ထဲ terminal ဖွင့်ပါ

# Test အားလုံး run
dotnet test erp-backend/ERP.Tests/ERP.Tests.csproj

# Test output ကြည့်ရင်
dotnet test --verbosity normal

# Specific test class တစ်ခုသာ run
dotnet test --filter "ClassName=CreateWarehouseCommandHandlerTests"
```

### 6.4 Swagger UI သုံးနည်း

1. API run ပါ: `dotnet run --project erp-backend/ERP.API`
2. Browser ဖွင့်ပါ: `https://localhost:5001/swagger`
3. `/api/auth/login` endpoint ခေါ်ပြီး JWT token ယူပါ
4. "Authorize" button နှိပ်ပြီး token ထည့်ပါ
5. Endpoint များကို test လုပ်နိုင်သည်

---

## Quick Reference — Warehouse Module Files

| File | Layer | အခန်းကဏ္ဍ |
|------|-------|---------|
| [Warehouse.cs](erp-backend/ERP.Domain/Entities/Warehouse.cs) | Domain | Entity definition |
| [CreateWarehouseCommand.cs](erp-backend/ERP.Application/Features/Warehouses/Commands/CreateWarehouseCommand.cs) | Application | Input data |
| [CreateWarehouseCommandHandler.cs](erp-backend/ERP.Application/Features/Warehouses/Commands/CreateWarehouseCommandHandler.cs) | Application | Business logic |
| [CreateWarehouseCommandValidator.cs](erp-backend/ERP.Application/Features/Warehouses/Commands/CreateWarehouseCommandValidator.cs) | Application | Input validation |
| [WarehouseRepository.cs](erp-backend/ERP.Infrastructure/Repositories/WarehouseRepository.cs) | Infrastructure | DB access |
| [WarehousesController.cs](erp-backend/ERP.API/Controllers/WarehousesController.cs) | API | HTTP endpoints |
| [CreateWarehouseCommandHandlerTests.cs](erp-backend/ERP.Tests/Warehouses/CreateWarehouseCommandHandlerTests.cs) | Tests | Handler tests |
| [CreateWarehouseCommandValidatorTests.cs](erp-backend/ERP.Tests/Warehouses/CreateWarehouseCommandValidatorTests.cs) | Tests | Validator tests |
| [GetWarehousesQueryHandlerTests.cs](erp-backend/ERP.Tests/Warehouses/GetWarehousesQueryHandlerTests.cs) | Tests | Query tests |

---

*ဤ documentation သည် ERP Backend project ၏ Warehouse module ကို အခြေခံ၍ ရေးထားသည်။*
*Version: 2026-02-19 | ဘာသာ: မြန်မာ*
