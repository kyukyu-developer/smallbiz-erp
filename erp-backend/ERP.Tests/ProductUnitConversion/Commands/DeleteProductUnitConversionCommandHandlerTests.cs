using ERP.Application.Features.ProductUnitConversion.Commands;
using ERP.Domain.Interfaces;
using FluentAssertions;
using Moq;

namespace ERP.Tests.ProductUnitConversion.Commands;

public class DeleteProductUnitConversionCommandHandlerTests
{
    private readonly Mock<IProductUnitConversionRepository> _repositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly DeleteProductUnitConversionCommandHandler _handler;

    public DeleteProductUnitConversionCommandHandlerTests()
    {
        _repositoryMock = new Mock<IProductUnitConversionRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();

        _handler = new DeleteProductUnitConversionCommandHandler(
            _repositoryMock.Object,
            _unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_ReturnsFailure_WhenConversionNotFound()
    {
        _repositoryMock.Setup(r => r.GetByIdAsync("999"))
            .ReturnsAsync((Domain.Entities.ProdUnitConversion?)null);

        var command = new DeleteProductUnitConversionCommand { Id = "999" };

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().Contain("not found");
    }

    [Fact]
    public async Task Handle_SoftDeletesConversion_WhenConversionExists()
    {
        var existing = new Domain.Entities.ProdUnitConversion
        {
            Id = "1",
            ProductId = "P1",
            FromUnitId = "U1",
            ToUnitId = "U2",
            Factor = 100,
            Active = true
        };

        _repositoryMock.Setup(r => r.GetByIdAsync("1"))
            .ReturnsAsync(existing);

        var command = new DeleteProductUnitConversionCommand { Id = "1" };

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        existing.Active.Should().BeFalse();
        existing.LastAction.Should().Be("DELETE");

        _repositoryMock.Verify(r => r.Update(existing), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }
}
