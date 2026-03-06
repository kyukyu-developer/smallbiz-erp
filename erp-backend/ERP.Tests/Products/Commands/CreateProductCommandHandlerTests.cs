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
        private readonly Mock<IProductGroupRepository> _groupRepoMock;
        private readonly Mock<IUnitRepository> _unitRepoMock;
        private readonly CreateProductCommandHandler _handler;

        public CreateProductCommandHandlerTests()
        {
            _productRepoMock = new Mock<IProductRepository>();
            _groupRepoMock = new Mock<IProductGroupRepository>();
            _unitRepoMock = new Mock<IUnitRepository>();
            _handler = new CreateProductCommandHandler(
                _productRepoMock.Object,
                _groupRepoMock.Object,
                _unitRepoMock.Object);
        }

        private CreateProductCommand CreateValidCommand() => new()
        {
            Code = "P001",
            Name = "Test Product",
            Description = "Test Description",
            GroupId = "group-1",
            BaseUnitId = "unit-1",
            CategoryId = "cat-1",
            BrandId = "brand-1",
            TrackInventory = true,
            HasVariant = false,
            HasSerialNumber = false,
            HasBatchNumber = false,
            AllowNegativeStock = false,
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
                .ReturnsAsync((Domain.Entities.Products?)null);
            _groupRepoMock.Setup(r => r.ExistsAsync(command.GroupId))
                .ReturnsAsync(true);
            _unitRepoMock.Setup(r => r.ExistsAsync(command.BaseUnitId))
                .ReturnsAsync(true);

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
                .ReturnsAsync(new Domain.Entities.Products { Id = "existing-id", Code = command.Code });

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsSuccess.Should().BeFalse();
            result.ErrorMessage.Should().Contain("already exists");
        }

        [Fact]
        public async Task Handle_ReturnsFailure_WhenGroupNotExists()
        {
            var command = CreateValidCommand();
            _productRepoMock.Setup(r => r.GetByCodeAsync(command.Code))
                .ReturnsAsync((Domain.Entities.Products?)null);
            _groupRepoMock.Setup(r => r.ExistsAsync(command.GroupId))
                .ReturnsAsync(false);

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsSuccess.Should().BeFalse();
            result.ErrorMessage.Should().Be("Product Group does not exist");
        }

        [Fact]
        public async Task Handle_ReturnsFailure_WhenUnitNotExists()
        {
            var command = CreateValidCommand();
            _productRepoMock.Setup(r => r.GetByCodeAsync(command.Code))
                .ReturnsAsync((Domain.Entities.Products?)null);
            _groupRepoMock.Setup(r => r.ExistsAsync(command.GroupId))
                .ReturnsAsync(true);
            _unitRepoMock.Setup(r => r.ExistsAsync(command.BaseUnitId))
                .ReturnsAsync(false);

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsSuccess.Should().BeFalse();
            result.ErrorMessage.Should().Be("Base Unit does not exist");
        }

        [Fact]
        public async Task Handle_CallsAddAsync_WhenValid()
        {
            var command = CreateValidCommand();
            _productRepoMock.Setup(r => r.GetByCodeAsync(command.Code))
                .ReturnsAsync((Domain.Entities.Products?)null);
            _groupRepoMock.Setup(r => r.ExistsAsync(command.GroupId))
                .ReturnsAsync(true);
            _unitRepoMock.Setup(r => r.ExistsAsync(command.BaseUnitId))
                .ReturnsAsync(true);

            await _handler.Handle(command, CancellationToken.None);

            _productRepoMock.Verify(r => r.AddAsync(It.IsAny<Domain.Entities.Products>()), Times.Once);
        }

        [Fact]
        public async Task Handle_CallsSaveChangesAsync_WhenValid()
        {
            var command = CreateValidCommand();
            _productRepoMock.Setup(r => r.GetByCodeAsync(command.Code))
                .ReturnsAsync((Domain.Entities.Products?)null);
            _groupRepoMock.Setup(r => r.ExistsAsync(command.GroupId))
                .ReturnsAsync(true);
            _unitRepoMock.Setup(r => r.ExistsAsync(command.BaseUnitId))
                .ReturnsAsync(true);

            await _handler.Handle(command, CancellationToken.None);

            _productRepoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task Handle_SetsDefaultValues_WhenNotProvided()
        {
            var command = new CreateProductCommand
            {
                Code = "P002",
                Name = "Product 2",
                GroupId = "group-1",
                BaseUnitId = "unit-1"
            };
            _productRepoMock.Setup(r => r.GetByCodeAsync(command.Code))
                .ReturnsAsync((Domain.Entities.Products?)null);
            _groupRepoMock.Setup(r => r.ExistsAsync(command.GroupId))
                .ReturnsAsync(true);
            _unitRepoMock.Setup(r => r.ExistsAsync(command.BaseUnitId))
                .ReturnsAsync(true);

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsSuccess.Should().BeTrue();
            result.Data!.TrackInventory.Should().BeTrue();
            result.Data.Active.Should().BeTrue();
        }

        [Fact]
        public async Task Handle_GeneratesNewId_WhenCreating()
        {
            var command = CreateValidCommand();
            _productRepoMock.Setup(r => r.GetByCodeAsync(command.Code))
                .ReturnsAsync((Domain.Entities.Products?)null);
            _groupRepoMock.Setup(r => r.ExistsAsync(command.GroupId))
                .ReturnsAsync(true);
            _unitRepoMock.Setup(r => r.ExistsAsync(command.BaseUnitId))
                .ReturnsAsync(true);

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsSuccess.Should().BeTrue();
            result.Data!.Id.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task Handle_SetsLastActionToCreate()
        {
            var command = CreateValidCommand();
            Domain.Entities.Products? addedProduct = null;
            _productRepoMock.Setup(r => r.GetByCodeAsync(command.Code))
                .ReturnsAsync((Domain.Entities.Products?)null);
            _groupRepoMock.Setup(r => r.ExistsAsync(command.GroupId))
                .ReturnsAsync(true);
            _unitRepoMock.Setup(r => r.ExistsAsync(command.BaseUnitId))
                .ReturnsAsync(true);
            _productRepoMock.Setup(r => r.AddAsync(It.IsAny<Domain.Entities.Products>()))
                .Callback<Domain.Entities.Products>(p => addedProduct = p);

            await _handler.Handle(command, CancellationToken.None);

            addedProduct!.LastAction.Should().Be("CREATE");
        }
    }
}
