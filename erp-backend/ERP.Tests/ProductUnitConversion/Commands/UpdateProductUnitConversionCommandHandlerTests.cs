using ERP.Application.Features.ProductUnitConversion.Commands;
using ERP.Domain.Interfaces;
using FluentAssertions;
using Moq;

namespace ERP.Tests.ProductUnitConversion.Commands;

public class UpdateProductUnitConversionCommandHandlerTests
{
    private readonly Mock<IProductUnitConversionRepository> _repositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly UpdateProductUnitConversionCommandHandler _handler;

    public UpdateProductUnitConversionCommandHandlerTests()
    {
        _repositoryMock = new Mock<IProductUnitConversionRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();

        _handler = new UpdateProductUnitConversionCommandHandler(
            _repositoryMock.Object,
            _unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_ReturnsFailure_WhenConversionNotFound()
    {
        _repositoryMock.Setup(r => r.GetByIdAsync("999"))
            .ReturnsAsync((Domain.Entities.ProdUnitConversion?)null);

        var command = new UpdateProductUnitConversionCommand
        {
            Id = "999",
            ProductId = "P1",
            FromUnitId = "U1",
            ToUnitId = "U2",
            Factor = 100
        };

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().Contain("not found");
    }

    [Fact]
    public async Task Handle_ReturnsFailure_WhenDuplicateConversionExists()
    {
        var existing = new Domain.Entities.ProdUnitConversion
        {
            Id = "1",
            ProductId = "P1",
            FromUnitId = "U1",
            ToUnitId = "U2"
        };

        var duplicate = new Domain.Entities.ProdUnitConversion
        {
            Id = "2",
            ProductId = "P1",
            FromUnitId = "U1",
            ToUnitId = "U3"
        };

        _repositoryMock.Setup(r => r.GetByIdAsync("1"))
            .ReturnsAsync(existing);

        _repositoryMock.Setup(r => r.GetByProductAndUnitsAsync("P1", "U1", "U3"))
            .ReturnsAsync(duplicate);

        var command = new UpdateProductUnitConversionCommand
        {
            Id = "1",
            ProductId = "P1",
            FromUnitId = "U1",
            ToUnitId = "U3",
            Factor = 200
        };

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().Contain("already exists");
    }

    [Fact]
    public async Task Handle_UpdatesConversion_WhenDataIsValid()
    {
        var existing = new Domain.Entities.ProdUnitConversion
        {
            Id = "1",
            ProductId = "P1",
            FromUnitId = "U1",
            ToUnitId = "U2",
            Factor = 100,
            Active = true,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = "System"
        };

        _repositoryMock.Setup(r => r.GetByIdAsync("1"))
            .ReturnsAsync(existing);

        _repositoryMock.Setup(r => r.GetByProductAndUnitsAsync("P1", "U1", "U2"))
            .ReturnsAsync(existing);

        _unitOfWorkMock.Setup(u => u.SaveChangesAsync())
            .ReturnsAsync(1);

        var command = new UpdateProductUnitConversionCommand
        {
            Id = "1",
            ProductId = "P1",
            FromUnitId = "U1",
            ToUnitId = "U2",
            Factor = 200,
            Active = false
        };

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data!.Factor.Should().Be(200);
        result.Data.Active.Should().BeFalse();
        result.Data.LastAction.Should().Be("UPDATE");

        _repositoryMock.Verify(r => r.Update(It.IsAny<Domain.Entities.ProdUnitConversion>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }
}
