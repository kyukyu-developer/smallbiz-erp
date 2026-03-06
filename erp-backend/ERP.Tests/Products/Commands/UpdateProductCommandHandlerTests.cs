using ERP.Application.Features.Products.Commands;
using ERP.Application.DTOs.Products;
using ERP.Domain.Interfaces;
using FluentAssertions;
using Moq;

namespace ERP.Tests.Products.Commands
{
    public class UpdateProductCommandHandlerTests
    {
        private readonly Mock<IProductRepository> _productRepoMock;
        private readonly Mock<IProductGroupRepository> _groupRepoMock;
        private readonly Mock<IUnitRepository> _unitRepoMock;
        private readonly UpdateProductCommandHandler _handler;

        public UpdateProductCommandHandlerTests()
        {
            _productRepoMock = new Mock<IProductRepository>();
            _groupRepoMock = new Mock<IProductGroupRepository>();
            _unitRepoMock = new Mock<IUnitRepository>();
            _handler = new UpdateProductCommandHandler(
                _productRepoMock.Object,
                _groupRepoMock.Object,
                _unitRepoMock.Object);
        }

        private Domain.Entities.Products CreateExistingProduct() => new()
        {
            Id = "prod-1",
            Code = "P001",
            Name = "Old Name",
            Description = "Old Description",
            GroupId = "group-1",
            BaseUnitId = "unit-1",
            CategoryId = "cat-1",
            TrackInventory = true,
            HasVariant = false,
            HasSerialNumber = false,
            HasBatchNumber = false,
            AllowNegativeStock = false,
            Active = true,
            CreatedAt = DateTime.UtcNow.AddDays(-10)
        };

        private UpdateProductCommand CreateValidCommand() => new()
        {
            Id = "prod-1",
            Code = "P001",
            Name = "Updated Product",
            Description = "Updated Description",
            GroupId = "group-1",
            BaseUnitId = "unit-1",
            CategoryId = "cat-1",
            TrackInventory = true,
            HasVariant = true,
            HasSerialNumber = false,
            HasBatchNumber = true,
            AllowNegativeStock = true,
            Active = true
        };

        [Fact]
        public async Task Handle_ReturnsSuccess_WhenValidUpdate()
        {
            var existing = CreateExistingProduct();
            var command = CreateValidCommand();
            _productRepoMock.Setup(r => r.GetByIdAsync(command.Id))
                .ReturnsAsync(existing);
            _productRepoMock.Setup(r => r.GetByCodeAsync(command.Code))
                .ReturnsAsync(existing);
            _groupRepoMock.Setup(r => r.ExistsAsync(command.GroupId))
                .ReturnsAsync(true);
            _unitRepoMock.Setup(r => r.ExistsAsync(command.BaseUnitId))
                .ReturnsAsync(true);

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsSuccess.Should().BeTrue();
            result.Data!.Name.Should().Be("Updated Product");
        }

        [Fact]
        public async Task Handle_ReturnsFailure_WhenNotFound()
        {
            var command = CreateValidCommand();
            _productRepoMock.Setup(r => r.GetByIdAsync(command.Id))
                .ReturnsAsync((Domain.Entities.Products?)null);

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsSuccess.Should().BeFalse();
            result.ErrorMessage.Should().Contain("not found");
        }

        [Fact]
        public async Task Handle_ReturnsFailure_WhenDuplicateCode()
        {
            var existing = CreateExistingProduct();
            var otherProduct = new Domain.Entities.Products { Id = "other-id", Code = "P002" };
            var command = CreateValidCommand();
            command.Code = "P002";

            _productRepoMock.Setup(r => r.GetByIdAsync(command.Id))
                .ReturnsAsync(existing);
            _productRepoMock.Setup(r => r.GetByCodeAsync(command.Code))
                .ReturnsAsync(otherProduct);

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsSuccess.Should().BeFalse();
            result.ErrorMessage.Should().Contain("already exists");
        }

        [Fact]
        public async Task Handle_ReturnsFailure_WhenGroupNotExists()
        {
            var existing = CreateExistingProduct();
            var command = CreateValidCommand();

            _productRepoMock.Setup(r => r.GetByIdAsync(command.Id))
                .ReturnsAsync(existing);
            _productRepoMock.Setup(r => r.GetByCodeAsync(command.Code))
                .ReturnsAsync(existing);
            _groupRepoMock.Setup(r => r.ExistsAsync(command.GroupId))
                .ReturnsAsync(false);

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsSuccess.Should().BeFalse();
            result.ErrorMessage.Should().Be("Product Group does not exist");
        }

        [Fact]
        public async Task Handle_SetsLastActionToUpdate()
        {
            var existing = CreateExistingProduct();
            var command = CreateValidCommand();
            _productRepoMock.Setup(r => r.GetByIdAsync(command.Id))
                .ReturnsAsync(existing);
            _productRepoMock.Setup(r => r.GetByCodeAsync(command.Code))
                .ReturnsAsync(existing);
            _groupRepoMock.Setup(r => r.ExistsAsync(command.GroupId))
                .ReturnsAsync(true);
            _unitRepoMock.Setup(r => r.ExistsAsync(command.BaseUnitId))
                .ReturnsAsync(true);

            var result = await _handler.Handle(command, CancellationToken.None);

            existing.LastAction.Should().Be("UPDATE");
        }

        [Fact]
        public async Task Handle_CallsUpdateMethod()
        {
            var existing = CreateExistingProduct();
            var command = CreateValidCommand();
            _productRepoMock.Setup(r => r.GetByIdAsync(command.Id))
                .ReturnsAsync(existing);
            _productRepoMock.Setup(r => r.GetByCodeAsync(command.Code))
                .ReturnsAsync(existing);
            _groupRepoMock.Setup(r => r.ExistsAsync(command.GroupId))
                .ReturnsAsync(true);
            _unitRepoMock.Setup(r => r.ExistsAsync(command.BaseUnitId))
                .ReturnsAsync(true);

            await _handler.Handle(command, CancellationToken.None);

            _productRepoMock.Verify(r => r.Update(It.IsAny<Domain.Entities.Products>()), Times.Once);
        }
    }
}
