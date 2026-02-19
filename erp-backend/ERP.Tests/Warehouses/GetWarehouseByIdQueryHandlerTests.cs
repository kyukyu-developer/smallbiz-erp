using ERP.Application.Features.Warehouses.Queries;
using ERP.Domain.Entities;
using ERP.Domain.Enums;
using ERP.Domain.Interfaces;
using FluentAssertions;
using Moq;

namespace ERP.Tests.Warehouses;

public class GetWarehouseByIdQueryHandlerTests
{
    private readonly Mock<IWarehouseRepository> _repoMock;
    private readonly GetWarehouseByIdQueryHandler _handler;

    public GetWarehouseByIdQueryHandlerTests()
    {
        _repoMock = new Mock<IWarehouseRepository>();
        _handler = new GetWarehouseByIdQueryHandler(_repoMock.Object);
    }

    // ─── Found ────────────────────────────────────────────────────────────────

    [Fact]
    public async Task Handle_ReturnsSuccess_WhenWarehouseExists()
    {
        var warehouse = new Warehouse
        {
            Id = "wh-1",
            Name = "Main Warehouse",
            City = "Bangkok",
            BranchType = BranchType.Main,
            IsMainWarehouse = true,
            Active = true
        };
        _repoMock.Setup(r => r.GetByIdAsync("wh-1")).ReturnsAsync(warehouse);

        var result = await _handler.Handle(new GetWarehouseByIdQuery { Id = "wh-1" }, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data!.Id.Should().Be("wh-1");
        result.Data.Name.Should().Be("Main Warehouse");
    }

    [Fact]
    public async Task Handle_MapsDtoFields_Correctly()
    {
        var warehouse = new Warehouse
        {
            Id = "wh-1",
            Name = "Branch WH",
            City = "Chiang Mai",
            BranchType = BranchType.Branch,
            IsMainWarehouse = false,
            Active = true,
            ParentWarehouseId = "parent-1",
            Location = "Zone B",
            Address = "456 Road",
            Country = "Thailand",
            ContactPerson = "Jane",
            Phone = "0899876543",
            LastAction = "UPDATE"
        };
        _repoMock.Setup(r => r.GetByIdAsync("wh-1")).ReturnsAsync(warehouse);
        _repoMock.Setup(r => r.GetByIdAsync("parent-1"))
            .ReturnsAsync(new Warehouse { Id = "parent-1", Name = "HQ" });

        var result = await _handler.Handle(new GetWarehouseByIdQuery { Id = "wh-1" }, CancellationToken.None);

        var dto = result.Data!;
        dto.BranchType.Should().Be("Branch");
        dto.IsMainWarehouse.Should().BeFalse();
        dto.ParentWarehouseId.Should().Be("parent-1");
        dto.ParentWarehouseName.Should().Be("HQ");
        dto.Location.Should().Be("Zone B");
        dto.Address.Should().Be("456 Road");
        dto.Country.Should().Be("Thailand");
        dto.ContactPerson.Should().Be("Jane");
        dto.Phone.Should().Be("0899876543");
        dto.LastAction.Should().Be("UPDATE");
    }

    [Fact]
    public async Task Handle_LooksUpParentWarehouseName_WhenParentIdExists()
    {
        var warehouse = new Warehouse
        {
            Id = "wh-1",
            Name = "Sub WH",
            BranchType = BranchType.Sub,
            ParentWarehouseId = "parent-1",
            Active = true
        };
        _repoMock.Setup(r => r.GetByIdAsync("wh-1")).ReturnsAsync(warehouse);
        _repoMock.Setup(r => r.GetByIdAsync("parent-1"))
            .ReturnsAsync(new Warehouse { Id = "parent-1", Name = "Branch WH" });

        var result = await _handler.Handle(new GetWarehouseByIdQuery { Id = "wh-1" }, CancellationToken.None);

        result.Data!.ParentWarehouseName.Should().Be("Branch WH");
        _repoMock.Verify(r => r.GetByIdAsync("parent-1"), Times.Once);
    }

    [Fact]
    public async Task Handle_DoesNotLookUpParent_WhenNoParentId()
    {
        var warehouse = new Warehouse
        {
            Id = "wh-1",
            Name = "Main WH",
            BranchType = BranchType.Main,
            Active = true
        };
        _repoMock.Setup(r => r.GetByIdAsync("wh-1")).ReturnsAsync(warehouse);

        var result = await _handler.Handle(new GetWarehouseByIdQuery { Id = "wh-1" }, CancellationToken.None);

        result.Data!.ParentWarehouseName.Should().BeNull();
        _repoMock.Verify(r => r.GetByIdAsync(It.IsNotIn("wh-1")), Times.Never);
    }

    // ─── Not Found ────────────────────────────────────────────────────────────

    [Fact]
    public async Task Handle_ReturnsFailure_WhenWarehouseDoesNotExist()
    {
        _repoMock.Setup(r => r.GetByIdAsync("missing-id")).ReturnsAsync((Warehouse?)null);

        var result = await _handler.Handle(new GetWarehouseByIdQuery { Id = "missing-id" }, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().Contain("missing-id");
        result.Data.Should().BeNull();
    }
}
