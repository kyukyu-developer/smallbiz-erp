using ERP.Application.Features.Warehouses.Queries;
using ERP.Domain.Entities;
using ERP.Domain.Enums;
using ERP.Domain.Interfaces;
using FluentAssertions;
using Moq;

namespace ERP.Tests.Warehouses;

public class GetWarehousesQueryHandlerTests
{
    private readonly Mock<IWarehouseRepository> _repoMock;
    private readonly GetWarehousesQueryHandler _handler;

    public GetWarehousesQueryHandlerTests()
    {
        _repoMock = new Mock<IWarehouseRepository>();
        _handler = new GetWarehousesQueryHandler(_repoMock.Object);
    }

    private static Warehouse MakeWarehouse(string id, string name, BranchType type, bool active = true) =>
        new()
        {
            Id = id,
            Name = name,
            BranchType = type,
            IsMainWarehouse = type == BranchType.Main,
            Active = active
        };

    // ─── Routing ──────────────────────────────────────────────────────────────

    [Fact]
    public async Task Handle_CallsGetAllAsync_WhenNoFiltersProvided()
    {
        _repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Warehouse>());

        await _handler.Handle(new GetWarehousesQuery(), CancellationToken.None);

        _repoMock.Verify(r => r.GetAllAsync(), Times.Once);
        _repoMock.Verify(r => r.GetMainWarehousesAsync(), Times.Never);
        _repoMock.Verify(r => r.GetByBranchTypeAsync(It.IsAny<BranchType>()), Times.Never);
    }

    [Fact]
    public async Task Handle_CallsGetMainWarehousesAsync_WhenMainWarehousesOnlyIsTrue()
    {
        _repoMock.Setup(r => r.GetMainWarehousesAsync()).ReturnsAsync(new List<Warehouse>());

        await _handler.Handle(new GetWarehousesQuery { MainWarehousesOnly = true }, CancellationToken.None);

        _repoMock.Verify(r => r.GetMainWarehousesAsync(), Times.Once);
        _repoMock.Verify(r => r.GetAllAsync(), Times.Never);
    }

    [Fact]
    public async Task Handle_CallsGetByBranchTypeAsync_WhenBranchTypeSpecified()
    {
        _repoMock.Setup(r => r.GetByBranchTypeAsync(BranchType.Branch))
            .ReturnsAsync(new List<Warehouse>());

        await _handler.Handle(
            new GetWarehousesQuery { BranchType = BranchType.Branch },
            CancellationToken.None);

        _repoMock.Verify(r => r.GetByBranchTypeAsync(BranchType.Branch), Times.Once);
    }

    // ─── Active filtering ─────────────────────────────────────────────────────

    [Fact]
    public async Task Handle_ReturnsOnlyActiveWarehouses_ByDefault()
    {
        var warehouses = new List<Warehouse>
        {
            MakeWarehouse("1", "Active WH", BranchType.Main, active: true),
            MakeWarehouse("2", "Inactive WH", BranchType.Main, active: false)
        };
        _repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(warehouses);

        var result = await _handler.Handle(new GetWarehousesQuery(), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Data.Should().HaveCount(1);
        result.Data![0].Name.Should().Be("Active WH");
    }

    [Fact]
    public async Task Handle_ReturnsAllWarehouses_WhenIncludeInactiveIsTrue()
    {
        var warehouses = new List<Warehouse>
        {
            MakeWarehouse("1", "Active WH", BranchType.Main, active: true),
            MakeWarehouse("2", "Inactive WH", BranchType.Main, active: false)
        };
        _repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(warehouses);

        var result = await _handler.Handle(
            new GetWarehousesQuery { IncludeInactive = true },
            CancellationToken.None);

        result.Data.Should().HaveCount(2);
    }

    [Fact]
    public async Task Handle_ReturnsEmptyList_WhenNoWarehousesExist()
    {
        _repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Warehouse>());

        var result = await _handler.Handle(new GetWarehousesQuery(), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Data.Should().BeEmpty();
    }

    // ─── DTO mapping ──────────────────────────────────────────────────────────

    [Fact]
    public async Task Handle_MapsWarehouseToDto_Correctly()
    {
        var parent = MakeWarehouse("p-1", "Parent WH", BranchType.Main);
        var child = new Warehouse
        {
            Id = "c-1",
            Name = "Child WH",
            City = "Phuket",
            BranchType = BranchType.Branch,
            IsMainWarehouse = false,
            Active = true,
            ParentWarehouseId = "p-1",
            ParentWarehouse = parent,
            Location = "Zone A",
            Address = "123 St",
            Country = "Thailand",
            ContactPerson = "John",
            Phone = "0812345678",
            LastAction = "CREATE"
        };
        _repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Warehouse> { child });

        var result = await _handler.Handle(new GetWarehousesQuery(), CancellationToken.None);

        var dto = result.Data![0];
        dto.Id.Should().Be("c-1");
        dto.Name.Should().Be("Child WH");
        dto.City.Should().Be("Phuket");
        dto.BranchType.Should().Be("Branch");
        dto.IsMainWarehouse.Should().BeFalse();
        dto.ParentWarehouseId.Should().Be("p-1");
        dto.ParentWarehouseName.Should().Be("Parent WH");
        dto.Location.Should().Be("Zone A");
        dto.Country.Should().Be("Thailand");
        dto.ContactPerson.Should().Be("John");
        dto.Phone.Should().Be("0812345678");
    }

    [Fact]
    public async Task Handle_PrioritizesMainWarehousesOnly_WhenBothFiltersSpecified()
    {
        _repoMock.Setup(r => r.GetMainWarehousesAsync()).ReturnsAsync(new List<Warehouse>());

        await _handler.Handle(
            new GetWarehousesQuery { MainWarehousesOnly = true, BranchType = BranchType.Branch },
            CancellationToken.None);

        _repoMock.Verify(r => r.GetMainWarehousesAsync(), Times.Once);
        _repoMock.Verify(r => r.GetByBranchTypeAsync(It.IsAny<BranchType>()), Times.Never);
    }

    // ─── Always success ───────────────────────────────────────────────────────

    [Fact]
    public async Task Handle_AlwaysReturnsSuccess()
    {
        _repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Warehouse>());

        var result = await _handler.Handle(new GetWarehousesQuery(), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.ErrorMessage.Should().BeNull();
    }
}
