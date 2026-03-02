

using ERP.Application.Features.Units.Commands;
using ERP.Domain.Entities;
using ERP.Domain.Interfaces;
using FluentAssertions;
using Moq;

namespace ERP.Tests.Units;

public class CreateUnitCommandHandlerTests
{
    private readonly Mock<IUnitRepository> _repoMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly CreateUnitCommandHandler _handler;

    public CreateUnitCommandHandlerTests()
    {
        _repoMock = new Mock<IUnitRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();

        _handler = new CreateUnitCommandHandler(
            _repoMock.Object,
            _unitOfWorkMock.Object);
    }

    // ─────────────────────────────────────────────
    // Duplicate Name Test
    // ─────────────────────────────────────────────

    [Fact]
    public async Task Handle_ReturnsFailure_WhenUnitNameAlreadyExists()
    {
        var existingUnit = new Unit { Id = "1", Name = "Kilogram" };

        _repoMock.Setup(r => r.GetByName("Kilogram"))
            .ReturnsAsync(existingUnit);


    var command = new CreateUnitCommand
    {
        Name = "Kilogram",
        Symbol = "KG"
    };

    var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should()
            .Be("Unit with name 'Kilogram' already exists ");

        _repoMock.Verify(r => r.AddAsync(It.IsAny<Unit>()), Times.Never);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Never);
    }

    // ─────────────────────────────────────────────
    // Success Test
    // ─────────────────────────────────────────────

    [Fact]
    public async Task Handle_CreatesUnit_WhenNameIsUnique()
    {
        _repoMock.Setup(r => r.GetByName("Kilogram"))
            .ReturnsAsync((Unit?)null);

        _repoMock.Setup(r => r.AddAsync(It.IsAny<Unit>()))
            .Returns(Task.CompletedTask);

        _unitOfWorkMock.Setup(u => u.SaveChangesAsync())
            .ReturnsAsync(1);

        var command = new CreateUnitCommand
        {
            Name = "Kilogram",
            Symbol = "KG"
        };


        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data!.Name.Should().Be("Kilogram");
        result.Data.Symbol.Should().Be("KG");
        result.Data.Active.Should().BeTrue();
        result.Data.LastAction.Should().Be("CREATE");

        _repoMock.Verify(r => r.AddAsync(It.IsAny<Unit>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    // ─────────────────────────────────────────────
    // Verify Created Values
    // ─────────────────────────────────────────────

    [Fact]
    public async Task Handle_SetsDefaultProperties_Correctly()
    {
        _repoMock.Setup(r => r.GetByName(It.IsAny<string>()))
            .ReturnsAsync((Unit?)null);

        _repoMock.Setup(r => r.AddAsync(It.IsAny<Unit>()))
            .Returns(Task.CompletedTask);

        _unitOfWorkMock.Setup(u => u.SaveChangesAsync())
            .ReturnsAsync(1);

  

        var command = new CreateUnitCommand
        {
            Name = "Box",
            Symbol = "BX"
        };


        var result = await _handler.Handle(command, CancellationToken.None);

        var dto = result.Data!;

        dto.Active.Should().BeTrue();
        dto.CreatedBy.Should().Be("System");
        dto.LastAction.Should().Be("CREATE");
        dto.Id.Should().NotBeNullOrEmpty();
    }
}