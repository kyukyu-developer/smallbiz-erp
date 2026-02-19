using ERP.API.Controllers;
using ERP.Application.DTOs.Common;
using ERP.Application.DTOs.Warehouses;
using ERP.Application.Features.Warehouses.Commands;
using ERP.Application.Features.Warehouses.Queries;
using ERP.Domain.Enums;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace ERP.Tests.Warehouses;

public class WarehousesControllerTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly WarehousesController _controller;

    public WarehousesControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new WarehousesController(_mediatorMock.Object);
    }

    // ─── GetAll ───────────────────────────────────────────────────────────────

    [Fact]
    public async Task GetAll_ReturnsOk_WithWarehouseList_WhenSuccess()
    {
        var warehouses = new List<WarehouseDto>
        {
            new() { Id = "1", Name = "Main WH", BranchType = "Main", Active = true }
        };
        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetWarehousesQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<List<WarehouseDto>>.Success(warehouses));

        var result = await _controller.GetAll(null, null, null);

        var ok = result.Should().BeOfType<OkObjectResult>().Subject;
        ok.Value.Should().BeEquivalentTo(warehouses);
    }

    [Fact]
    public async Task GetAll_PassesQueryParameters_ToMediator()
    {
        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetWarehousesQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<List<WarehouseDto>>.Success(new List<WarehouseDto>()));

        await _controller.GetAll(includeInactive: true, branchType: BranchType.Branch, mainWarehousesOnly: false);

        _mediatorMock.Verify(m => m.Send(
            It.Is<GetWarehousesQuery>(q =>
                q.IncludeInactive == true &&
                q.BranchType == BranchType.Branch &&
                q.MainWarehousesOnly == false),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetAll_ReturnsBadRequest_WhenFailure()
    {
        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetWarehousesQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<List<WarehouseDto>>.Failure("Query failed"));

        var result = await _controller.GetAll(null, null, null);

        var bad = result.Should().BeOfType<BadRequestObjectResult>().Subject;
        bad.Value.Should().Be("Query failed");
    }

    // ─── GetById ──────────────────────────────────────────────────────────────

    [Fact]
    public async Task GetById_ReturnsOk_WhenWarehouseFound()
    {
        var dto = new WarehouseDto { Id = "wh-1", Name = "Main Warehouse" };
        _mediatorMock
            .Setup(m => m.Send(It.Is<GetWarehouseByIdQuery>(q => q.Id == "wh-1"), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<WarehouseDto>.Success(dto));

        var result = await _controller.GetById("wh-1");

        var ok = result.Should().BeOfType<OkObjectResult>().Subject;
        ok.Value.Should().BeEquivalentTo(dto);
    }

    [Fact]
    public async Task GetById_ReturnsNotFound_WhenWarehouseDoesNotExist()
    {
        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetWarehouseByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<WarehouseDto>.Failure("Warehouse with ID 'bad-id' not found"));

        var result = await _controller.GetById("bad-id");

        var notFound = result.Should().BeOfType<NotFoundObjectResult>().Subject;
        notFound.Value.Should().Be("Warehouse with ID 'bad-id' not found");
    }

    // ─── Create ───────────────────────────────────────────────────────────────

    [Fact]
    public async Task Create_ReturnsCreatedAtAction_WhenSuccess()
    {
        var dto = new WarehouseDto { Id = "new-wh", Name = "New Warehouse" };
        var command = new CreateWarehouseCommand { Name = "New Warehouse", BranchType = BranchType.Main };

        _mediatorMock
            .Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<WarehouseDto>.Success(dto));

        var result = await _controller.Create(command);

        var created = result.Should().BeOfType<CreatedAtActionResult>().Subject;
        created.ActionName.Should().Be(nameof(_controller.GetById));
        created.RouteValues!["id"].Should().Be("new-wh");
        created.Value.Should().BeEquivalentTo(dto);
    }

    [Fact]
    public async Task Create_ReturnsBadRequest_WhenFailure()
    {
        var command = new CreateWarehouseCommand { Name = "Duplicate WH", BranchType = BranchType.Main };
        _mediatorMock
            .Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<WarehouseDto>.Failure("Warehouse with name 'Duplicate WH' already exists in this location"));

        var result = await _controller.Create(command);

        result.Should().BeOfType<BadRequestObjectResult>();
    }

    // ─── Update ───────────────────────────────────────────────────────────────

    [Fact]
    public async Task Update_ReturnsOk_WhenSuccess()
    {
        var dto = new WarehouseDto { Id = "wh-1", Name = "Updated Warehouse" };
        var command = new UpdateWarehouseCommand { Id = "wh-1", Name = "Updated Warehouse", BranchType = BranchType.Main };

        _mediatorMock
            .Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<WarehouseDto>.Success(dto));

        var result = await _controller.Update("wh-1", command);

        var ok = result.Should().BeOfType<OkObjectResult>().Subject;
        ok.Value.Should().BeEquivalentTo(dto);
    }

    [Fact]
    public async Task Update_ReturnsBadRequest_WhenIdMismatch()
    {
        var command = new UpdateWarehouseCommand { Id = "wh-1", Name = "WH", BranchType = BranchType.Main };

        var result = await _controller.Update("different-id", command);

        var bad = result.Should().BeOfType<BadRequestObjectResult>().Subject;
        bad.Value.Should().Be("ID in URL does not match ID in request body");
    }

    [Fact]
    public async Task Update_ReturnsBadRequest_WhenHandlerFails()
    {
        var command = new UpdateWarehouseCommand { Id = "wh-1", Name = "WH", BranchType = BranchType.Main };
        _mediatorMock
            .Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<WarehouseDto>.Failure("Warehouse not found"));

        var result = await _controller.Update("wh-1", command);

        result.Should().BeOfType<BadRequestObjectResult>();
    }
}
