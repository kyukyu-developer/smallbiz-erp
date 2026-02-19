using ERP.Application.Features.Warehouses.Commands;
using ERP.Domain.Entities;
using ERP.Domain.Enums;
using ERP.Domain.Interfaces;
using FluentAssertions;
using Moq;

namespace ERP.Tests.Warehouses;

public class UpdateWarehouseCommandHandlerTests
{
    private readonly Mock<IWarehouseRepository> _repoMock;
    private readonly Mock<IUnitOfWork> _uowMock;
    private readonly UpdateWarehouseCommandHandler _handler;

    public UpdateWarehouseCommandHandlerTests()
    {
        _repoMock = new Mock<IWarehouseRepository>();
        _uowMock = new Mock<IUnitOfWork>();
        _handler = new UpdateWarehouseCommandHandler(_repoMock.Object, _uowMock.Object);
    }

    private static Warehouse ExistingWarehouse(string id = "wh-1") =>
        new()
        {
            Id = id,
            Name = "Old Name",
            City = "Old City",
            BranchType = BranchType.Main,
            IsMainWarehouse = true,
            Active = true
        };

    private static UpdateWarehouseCommand ValidUpdateCommand(string id = "wh-1") =>
        new()
        {
            Id = id,
            Name = "Updated Name",
            City = "Bangkok",
            BranchType = BranchType.Main,
            Active = true
        };

    // ─── Happy paths ──────────────────────────────────────────────────────────

    [Fact]
    public async Task Handle_ReturnsSuccess_WhenWarehouseUpdated()
    {
        var existing = ExistingWarehouse();
        var command = ValidUpdateCommand();
        _repoMock.Setup(r => r.GetByIdAsync("wh-1")).ReturnsAsync(existing);
        _repoMock.Setup(r => r.GetByNameAndCityAsync(command.Name, command.City))
            .ReturnsAsync((Warehouse?)null);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Data!.Name.Should().Be("Updated Name");
        result.Data.City.Should().Be("Bangkok");
        result.Data.LastAction.Should().Be("UPDATE");
    }

    [Fact]
    public async Task Handle_SetsIsMainWarehouse_True_WhenBranchTypeChangedToMain()
    {
        var existing = ExistingWarehouse();
        existing.BranchType = BranchType.Branch;
        existing.IsMainWarehouse = false;

        var command = ValidUpdateCommand();
        command.BranchType = BranchType.Main;

        _repoMock.Setup(r => r.GetByIdAsync("wh-1")).ReturnsAsync(existing);
        _repoMock.Setup(r => r.GetByNameAndCityAsync(command.Name, command.City))
            .ReturnsAsync((Warehouse?)null);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Data!.IsMainWarehouse.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_SetsIsMainWarehouse_False_WhenBranchTypeChangedToBranch()
    {
        var existing = ExistingWarehouse();
        var parentId = "parent-1";
        var command = ValidUpdateCommand();
        command.BranchType = BranchType.Branch;
        command.ParentWarehouseId = parentId;

        _repoMock.Setup(r => r.GetByIdAsync("wh-1")).ReturnsAsync(existing);
        _repoMock.Setup(r => r.ExistsAsync(parentId)).ReturnsAsync(true);
        _repoMock.Setup(r => r.GetByNameAndCityAsync(command.Name, command.City))
            .ReturnsAsync((Warehouse?)null);
        _repoMock.Setup(r => r.GetByIdAsync(parentId))
            .ReturnsAsync(new Warehouse { Id = parentId, Name = "Parent WH" });

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Data!.IsMainWarehouse.Should().BeFalse();
    }

    [Fact]
    public async Task Handle_AllowsSameName_IfBelongsToCurrentWarehouse()
    {
        var existing = ExistingWarehouse("wh-1");
        var command = ValidUpdateCommand("wh-1");
        // Simulate duplicate check returns the SAME warehouse
        _repoMock.Setup(r => r.GetByIdAsync("wh-1")).ReturnsAsync(existing);
        _repoMock.Setup(r => r.GetByNameAndCityAsync(command.Name, command.City))
            .ReturnsAsync(existing);   // same id -> should be allowed

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_CallsRepositoryUpdate_AndSaveChanges()
    {
        var existing = ExistingWarehouse();
        var command = ValidUpdateCommand();
        _repoMock.Setup(r => r.GetByIdAsync("wh-1")).ReturnsAsync(existing);
        _repoMock.Setup(r => r.GetByNameAndCityAsync(command.Name, command.City))
            .ReturnsAsync((Warehouse?)null);

        await _handler.Handle(command, CancellationToken.None);

        _repoMock.Verify(r => r.Update(It.IsAny<Warehouse>()), Times.Once);
        _uowMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    // ─── Failure paths ────────────────────────────────────────────────────────

    [Fact]
    public async Task Handle_ReturnsFailure_WhenWarehouseDoesNotExist()
    {
        _repoMock.Setup(r => r.GetByIdAsync("wh-missing")).ReturnsAsync((Warehouse?)null);

        var result = await _handler.Handle(
            new UpdateWarehouseCommand { Id = "wh-missing", Name = "X", BranchType = BranchType.Main },
            CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().Be("Warehouse with ID 'wh-missing' not found");
        _repoMock.Verify(r => r.Update(It.IsAny<Warehouse>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ReturnsFailure_WhenParentWarehouseDoesNotExist()
    {
        var existing = ExistingWarehouse();
        var command = ValidUpdateCommand();
        command.ParentWarehouseId = "nonexistent-parent";
        command.BranchType = BranchType.Branch;

        _repoMock.Setup(r => r.GetByIdAsync("wh-1")).ReturnsAsync(existing);
        _repoMock.Setup(r => r.ExistsAsync("nonexistent-parent")).ReturnsAsync(false);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().Be("Parent warehouse does not exist");
    }

    [Fact]
    public async Task Handle_ReturnsFailure_WhenCircularReferenceDetected()
    {
        var existing = ExistingWarehouse("wh-1");
        var command = ValidUpdateCommand("wh-1");
        command.ParentWarehouseId = "wh-1";   // pointing to itself
        command.BranchType = BranchType.Branch;

        _repoMock.Setup(r => r.GetByIdAsync("wh-1")).ReturnsAsync(existing);
        // No ExistsAsync stub — self-reference must be caught before any DB call

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().Be("Warehouse cannot be its own parent");
        _repoMock.Verify(r => r.ExistsAsync(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ReturnsFailure_WhenDuplicateNameAndCityWithDifferentWarehouse()
    {
        var existing = ExistingWarehouse("wh-1");
        var command = ValidUpdateCommand("wh-1");
        command.Name = "Duplicate WH";
        command.City = "Phuket";

        _repoMock.Setup(r => r.GetByIdAsync("wh-1")).ReturnsAsync(existing);
        _repoMock.Setup(r => r.GetByNameAndCityAsync("Duplicate WH", "Phuket"))
            .ReturnsAsync(new Warehouse { Id = "wh-other", Name = "Duplicate WH", City = "Phuket" });

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().Be("Warehouse with name 'Duplicate WH' already exists in Phuket");
    }

    [Fact]
    public async Task Handle_UpdatesActiveFlag_WhenDeactivatingWarehouse()
    {
        var existing = ExistingWarehouse();    // Active = true
        var command = ValidUpdateCommand();
        command.Active = false;

        _repoMock.Setup(r => r.GetByIdAsync("wh-1")).ReturnsAsync(existing);
        _repoMock.Setup(r => r.GetByNameAndCityAsync(command.Name, command.City))
            .ReturnsAsync((Warehouse?)null);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Data!.Active.Should().BeFalse();
        result.Data.LastAction.Should().Be("UPDATE");
    }
}
