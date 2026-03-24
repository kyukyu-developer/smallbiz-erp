using ERP.Application.Features.ProductUnitConversion.Commands;
using ERP.Domain.Interfaces;
using FluentAssertions;
using Moq;

namespace ERP.Tests.ProductUnitConversion.Commands;

public class CreateProductUnitConversionCommandHandlerTests
{
    private readonly Mock<IProductUnitConversionRepository> _repositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly CreateProductUnitConversionCommandHandler _handler;

    public CreateProductUnitConversionCommandHandlerTests()
    {
        _repositoryMock = new Mock<IProductUnitConversionRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();

        _handler = new CreateProductUnitConversionCommandHandler(
            _repositoryMock.Object,
            _unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_ReturnsFailure_WhenConversionAlreadyExists()
    {
        var existing = new Domain.Entities.ProdUnitConversion 
        { 
            Id = "1", 
            ProductId = "P1", 
            FromUnitId = "U1", 
            ToUnitId = "U2" 
        };

        _repositoryMock.Setup(r => r.GetByProductAndUnitsAsync("P1", "U1", "U2"))
            .ReturnsAsync(existing);

        var command = new CreateProductUnitConversionCommand
        {
            ProductId = "P1",
            FromUnitId = "U1",
            ToUnitId = "U2",
            Factor = 100
        };

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().Contain("already exists");
        _repositoryMock.Verify(r => r.AddAsync(It.IsAny<Domain.Entities.ProdUnitConversion>()), Times.Never);
    }

    [Fact]
    public async Task Handle_CreatesConversion_WhenDataIsValid()
    {
        _repositoryMock.Setup(r => r.GetByProductAndUnitsAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync((Domain.Entities.ProdUnitConversion?)null);

        _repositoryMock.Setup(r => r.AddAsync(It.IsAny<Domain.Entities.ProdUnitConversion>()))
            .Returns(Task.CompletedTask);

        _unitOfWorkMock.Setup(u => u.SaveChangesAsync())
            .ReturnsAsync(1);

        var command = new CreateProductUnitConversionCommand
        {
            ProductId = "P1",
            FromUnitId = "U1",
            ToUnitId = "U2",
            Factor = 100
        };

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data!.ProductId.Should().Be("P1");
        result.Data.FromUnitId.Should().Be("U1");
        result.Data.ToUnitId.Should().Be("U2");
        result.Data.Factor.Should().Be(100);
        result.Data.Active.Should().BeTrue();
        result.Data.LastAction.Should().Be("CREATE");

        _repositoryMock.Verify(r => r.AddAsync(It.IsAny<Domain.Entities.ProdUnitConversion>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_SetsDefaultProperties_Correctly()
    {
        _repositoryMock.Setup(r => r.GetByProductAndUnitsAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync((Domain.Entities.ProdUnitConversion?)null);

        _repositoryMock.Setup(r => r.AddAsync(It.IsAny<Domain.Entities.ProdUnitConversion>()))
            .Returns(Task.CompletedTask);

        _unitOfWorkMock.Setup(u => u.SaveChangesAsync())
            .ReturnsAsync(1);

        var command = new CreateProductUnitConversionCommand
        {
            ProductId = "P1",
            FromUnitId = "U1",
            ToUnitId = "U2",
            Factor = 50
        };

        var result = await _handler.Handle(command, CancellationToken.None);

        var dto = result.Data!;

        dto.Id.Should().NotBeNullOrEmpty();
        dto.Active.Should().BeTrue();
        dto.CreatedBy.Should().Be("System");
        dto.LastAction.Should().Be("CREATE");
        dto.CreatedAt.Should().NotBe(default);
    }
}
