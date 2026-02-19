using ERP.Application.DTOs.Warehouses;
using ERP.Application.Features.Warehouses.Commands;
using ERP.Domain.Entities;
using ERP.Domain.Enums;
using ERP.Domain.Interfaces;
using FluentAssertions;
using Moq;

namespace ERP.Tests.Warehouses;

public class CreateWarehouseCommandHandlerTests
{
    private readonly Mock<IWarehouseRepository> _repoMock;
    private readonly Mock<IUnitOfWork> _uowMock;
    private readonly CreateWarehouseCommandHandler _handler;

    public CreateWarehouseCommandHandlerTests()
    {
        _repoMock = new Mock<IWarehouseRepository>();
        _uowMock = new Mock<IUnitOfWork>();
        _handler = new CreateWarehouseCommandHandler(_repoMock.Object, _uowMock.Object);
    }

    private CreateWarehouseCommand BuildMainWarehouseCommand(string name = "HQ Warehouse") =>
        new()
        {
            Name = name,
            City = "Bangkok",
            BranchType = BranchType.Main,
            Country = "Thailand"
        };

    private CreateWarehouseCommand BuildBranchWarehouseCommand(string parentId = "parent-1") =>
        new()
        {
            Name = "Branch WH",
            City = "Chiang Mai",
            BranchType = BranchType.Branch,
            ParentWarehouseId = parentId
        };

    // ─── Happy paths ──────────────────────────────────────────────────────────

    [Fact]
    public async Task Handle_CreatesWarehouse_AndReturnsDto_WhenValidMainWarehouse()
    {
        var command = BuildMainWarehouseCommand();
        _repoMock.Setup(r => r.GetByNameAndCityAsync(command.Name, command.City))
            .ReturnsAsync((Warehouse?)null);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data!.Name.Should().Be(command.Name);
        result.Data.City.Should().Be(command.City);
        result.Data.BranchType.Should().Be(BranchType.Main.ToString());
        result.Data.IsMainWarehouse.Should().BeTrue();
        result.Data.Active.Should().BeTrue();
        result.Data.LastAction.Should().Be("CREATE");
    }

    [Fact]
    public async Task Handle_SetsIsMainWarehouse_True_WhenBranchTypeIsMain()
    {
        var command = BuildMainWarehouseCommand();
        _repoMock.Setup(r => r.GetByNameAndCityAsync(command.Name, command.City))
            .ReturnsAsync((Warehouse?)null);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Data!.IsMainWarehouse.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_SetsIsMainWarehouse_False_WhenBranchTypeIsBranch()
    {
        var parentId = "parent-1";
        var command = BuildBranchWarehouseCommand(parentId);
        _repoMock.Setup(r => r.ExistsAsync(parentId)).ReturnsAsync(true);
        _repoMock.Setup(r => r.GetByNameAndCityAsync(command.Name, command.City))
            .ReturnsAsync((Warehouse?)null);
        _repoMock.Setup(r => r.GetByIdAsync(parentId))
            .ReturnsAsync(new Warehouse { Id = parentId, Name = "Main WH" });

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Data!.IsMainWarehouse.Should().BeFalse();
    }

    [Fact]
    public async Task Handle_IncludesParentWarehouseName_WhenParentProvided()
    {
        var parentId = "parent-1";
        var command = BuildBranchWarehouseCommand(parentId);
        _repoMock.Setup(r => r.ExistsAsync(parentId)).ReturnsAsync(true);
        _repoMock.Setup(r => r.GetByNameAndCityAsync(command.Name, command.City))
            .ReturnsAsync((Warehouse?)null);
        _repoMock.Setup(r => r.GetByIdAsync(parentId))
            .ReturnsAsync(new Warehouse { Id = parentId, Name = "Main Warehouse" });

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Data!.ParentWarehouseName.Should().Be("Main Warehouse");
        result.Data.ParentWarehouseId.Should().Be(parentId);
    }

    [Fact]
    public async Task Handle_CallsAddAsync_AndSaveChanges()
    {
        var command = BuildMainWarehouseCommand();
        _repoMock.Setup(r => r.GetByNameAndCityAsync(command.Name, command.City))
            .ReturnsAsync((Warehouse?)null);

        await _handler.Handle(command, CancellationToken.None);

        _repoMock.Verify(r => r.AddAsync(It.IsAny<Warehouse>()), Times.Once);
        _uowMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    // ─── Failure paths ────────────────────────────────────────────────────────

    [Fact]
    public async Task Handle_ReturnsFailure_WhenParentWarehouseDoesNotExist()
    {
        var command = BuildBranchWarehouseCommand("nonexistent-parent");
        _repoMock.Setup(r => r.ExistsAsync("nonexistent-parent")).ReturnsAsync(false);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().Be("Parent warehouse does not exist");
        _repoMock.Verify(r => r.AddAsync(It.IsAny<Warehouse>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ReturnsFailure_WhenDuplicateNameAndCityExist()
    {
        var command = BuildMainWarehouseCommand("HQ Warehouse");
        _repoMock.Setup(r => r.GetByNameAndCityAsync(command.Name, command.City))
            .ReturnsAsync(new Warehouse { Id = "existing-1", Name = "HQ Warehouse", City = "Bangkok" });

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().Be("Warehouse with name 'HQ Warehouse' already exists in Bangkok");
        _repoMock.Verify(r => r.AddAsync(It.IsAny<Warehouse>()), Times.Never);
    }

    [Fact]
    public async Task Handle_SkipsParentValidation_WhenParentWarehouseIdIsNull()
    {
        var command = BuildMainWarehouseCommand();  // no ParentWarehouseId
        _repoMock.Setup(r => r.GetByNameAndCityAsync(command.Name, command.City))
            .ReturnsAsync((Warehouse?)null);

        await _handler.Handle(command, CancellationToken.None);

        _repoMock.Verify(r => r.ExistsAsync(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task Handle_GeneratesNewId_ForCreatedWarehouse()
    {
        var command = BuildMainWarehouseCommand();
        _repoMock.Setup(r => r.GetByNameAndCityAsync(command.Name, command.City))
            .ReturnsAsync((Warehouse?)null);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Data!.Id.Should().NotBeNullOrEmpty();
        Guid.TryParse(result.Data.Id, out _).Should().BeTrue();
    }

    [Fact]
    public async Task Handle_ReturnsFailure_WithFallbackLocation_WhenCityIsNull()
    {
        var command = new CreateWarehouseCommand
        {
            Name = "No City WH",
            BranchType = BranchType.Main,
            City = null
        };
        _repoMock.Setup(r => r.GetByNameAndCityAsync(command.Name, null))
            .ReturnsAsync(new Warehouse { Id = "existing-1", Name = "No City WH", City = null });

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().Be("Warehouse with name 'No City WH' already exists in this location");
        _repoMock.Verify(r => r.AddAsync(It.IsAny<Warehouse>()), Times.Never);
    }
}
