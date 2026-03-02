

using ERP.Application.DTOs.Units;
using ERP.Application.Features.Units.Queries;
using ERP.Domain.Entities;
using ERP.Domain.Interfaces;
using FluentAssertions;
using Moq;

namespace ERP.Tests.Units;

public class GetUnitByIdQueryHandlerTests
{
    private readonly Mock<IUnitRepository> _repoMock;
    private readonly GetUnitByIdQueryHandler _handler;

    public GetUnitByIdQueryHandlerTests()
    {
        _repoMock = new Mock<IUnitRepository>();
        _handler = new GetUnitByIdQueryHandler(_repoMock.Object);
    }

    private static Unit MakeUnit(string id, string name)
    {
        return new Unit
        {
            Id = id,
            Name = name,
            Symbol = name.Substring(0, 1),
            CreatedAt = DateTime.UtcNow.AddDays(-1),
            UpdatedAt = DateTime.UtcNow,
            CreatedBy = "system",
            UpdatedBy = "admin",
            LastAction = "CREATE"
        };
    }

    // ─────────────────────────────────────────────
    // Repository Call Test
    // ─────────────────────────────────────────────

    [Fact]
    public async Task Handle_CallsGetByIdAsync()
    {
        var unit = MakeUnit("1", "Kilogram");

        _repoMock.Setup(r => r.GetByIdAsync("1"))
            .ReturnsAsync(unit);

        await _handler.Handle(new GetUnitByIdQuery { Id = "1" }, CancellationToken.None);

        _repoMock.Verify(r => r.GetByIdAsync("1"), Times.Once);
    }

    // ─────────────────────────────────────────────
    // Success Case
    // ─────────────────────────────────────────────

    [Fact]
    public async Task Handle_ReturnsSuccess_WhenUnitExists()
    {
        var unit = MakeUnit("1", "Kilogram");

        _repoMock.Setup(r => r.GetByIdAsync("1"))
            .ReturnsAsync(unit);

        var result = await _handler.Handle(
            new GetUnitByIdQuery { Id = "1" },
            CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data!.Id.Should().Be("1");
        result.Data.Name.Should().Be("Kilogram");
    }

    // ─────────────────────────────────────────────
    // Failure Case
    // ─────────────────────────────────────────────

    [Fact]
    public async Task Handle_ReturnsFailure_WhenUnitDoesNotExist()
    {
        _repoMock.Setup(r => r.GetByIdAsync("99"))
            .ReturnsAsync((Unit?)null);

        var result = await _handler.Handle(
            new GetUnitByIdQuery { Id = "99" },
            CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Data.Should().BeNull();
        result.ErrorMessage.Should().Be("Unit with ID '99' not found");
    }

    // ─────────────────────────────────────────────
    // DTO Mapping Test
    // ─────────────────────────────────────────────

    [Fact]
    public async Task Handle_MapsUnitToDto_Correctly()
    {
        var unit = MakeUnit("1", "Kilogram");

        _repoMock.Setup(r => r.GetByIdAsync("1"))
            .ReturnsAsync(unit);

        var result = await _handler.Handle(
            new GetUnitByIdQuery { Id = "1" },
            CancellationToken.None);

        var dto = result.Data!;

        dto.Id.Should().Be(unit.Id);
        dto.Name.Should().Be(unit.Name);
        dto.Symbol.Should().Be(unit.Symbol);
        dto.CreatedBy.Should().Be(unit.CreatedBy);
        dto.ModifiedBy.Should().Be(unit.UpdatedBy);
        dto.LastAction.Should().Be(unit.LastAction);
    }
}