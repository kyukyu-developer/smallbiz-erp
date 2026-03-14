using ERP.Application.Features.Products.Commands;
using ERP.Application.DTOs.Products;
using ERP.Domain.Interfaces;
using FluentAssertions;
using Moq;

namespace ERP.Tests.Products.Commands
{
    public class CreateProductCommandHandlerTests
    {
        private readonly Mock<IProductRepository> _productRepoMock;
        private readonly Mock<IUnitRepository> _unitRepoMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly CreateProductCommandHandler _handler;

        public CreateProductCommandHandlerTests()
        {
            _productRepoMock = new Mock<IProductRepository>();
            _unitRepoMock = new Mock<IUnitRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _handler = new CreateProductCommandHandler(
                _productRepoMock.Object,
                _unitRepoMock.Object,
                _unitOfWorkMock.Object);
        }

        private CreateProductCommand CreateValidCommand() => new()
        {
            Code = "P001",
            Name = "Test Product",
            Description = "Test Description",
            BaseUnitId = "unit-1",
            CategoryId = "cat-1",
            TrackType = 0,
            MinimumStock = 10,
            MaximumStock = 100,
            ReorderLevel = 20,
            Barcode = "123456789",
            Active = true
        };

        [Fact]
        public async Task Handle_ReturnsSuccess_WhenValidProduct()
        {
            var command = CreateValidCommand();
            _productRepoMock.Setup(r => r.GetByCodeAsync(command.Code))
                .ReturnsAsync((Domain.Entities.ProdItem?)null);

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsSuccess.Should().BeTrue();
            result.Data.Should().NotBeNull();
            result.Data!.Code.Should().Be(command.Code);
            result.Data.Name.Should().Be(command.Name);
        }

        [Fact]
        public async Task Handle_ReturnsFailure_WhenDuplicateCode()
        {
            var command = CreateValidCommand();
            _productRepoMock.Setup(r => r.GetByCodeAsync(command.Code))
                .ReturnsAsync(new Domain.Entities.ProdItem { Id = "existing-id", Code = command.Code });

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsSuccess.Should().BeFalse();
            result.ErrorMessage.Should().Contain("already exists");
        }

        [Fact]
        public async Task Handle_CallsAddAsync_WhenValid()
        {
            var command = CreateValidCommand();
            _productRepoMock.Setup(r => r.GetByCodeAsync(command.Code))
                .ReturnsAsync((Domain.Entities.ProdItem?)null);

            await _handler.Handle(command, CancellationToken.None);

            _productRepoMock.Verify(r => r.AddAsync(It.IsAny<Domain.Entities.ProdItem>()), Times.Once);
        }

        [Fact]
        public async Task Handle_CallsSaveChangesAsync_WhenValid()
        {
            var command = CreateValidCommand();
            _productRepoMock.Setup(r => r.GetByCodeAsync(command.Code))
                .ReturnsAsync((Domain.Entities.ProdItem?)null);

            await _handler.Handle(command, CancellationToken.None);

            _unitOfWorkMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task Handle_GeneratesNewId_WhenCreating()
        {
            var command = CreateValidCommand();
            _productRepoMock.Setup(r => r.GetByCodeAsync(command.Code))
                .ReturnsAsync((Domain.Entities.ProdItem?)null);

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsSuccess.Should().BeTrue();
            result.Data!.Id.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task Handle_SetsLastActionToCreate()
        {
            var command = CreateValidCommand();
            Domain.Entities.ProdItem? addedProduct = null;
            _productRepoMock.Setup(r => r.GetByCodeAsync(command.Code))
                .ReturnsAsync((Domain.Entities.ProdItem?)null);
            _productRepoMock.Setup(r => r.AddAsync(It.IsAny<Domain.Entities.ProdItem>()))
                .Callback<Domain.Entities.ProdItem>(p => addedProduct = p);

            await _handler.Handle(command, CancellationToken.None);

            addedProduct!.LastAction.Should().Be("CREATE");
        }
    }
}
