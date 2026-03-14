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
        private readonly UpdateProductCommandHandler _handler;

        public UpdateProductCommandHandlerTests()
        {
            _productRepoMock = new Mock<IProductRepository>();
            _handler = new UpdateProductCommandHandler(_productRepoMock.Object);
        }

        private Domain.Entities.ProdItem CreateExistingProduct() => new()
        {
            Id = "prod-1",
            Code = "P001",
            Name = "Old Name",
            Description = "Old Description",
            BaseUnitId = "unit-1",
            CategoryId = "cat-1",
            Active = true,
            CreatedAt = DateTime.UtcNow.AddDays(-10)
        };

        private UpdateProductCommand CreateValidCommand() => new()
        {
            Id = "prod-1",
            Code = "P001",
            Name = "Updated Product",
            Description = "Updated Description",
            BaseUnitId = "unit-1",
            CategoryId = "cat-1",
            TrackType = 1,
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

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsSuccess.Should().BeTrue();
            result.Data!.Name.Should().Be("Updated Product");
        }

        [Fact]
        public async Task Handle_ReturnsFailure_WhenNotFound()
        {
            var command = CreateValidCommand();
            _productRepoMock.Setup(r => r.GetByIdAsync(command.Id))
                .ReturnsAsync((Domain.Entities.ProdItem?)null);

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsSuccess.Should().BeFalse();
            result.ErrorMessage.Should().Contain("not found");
        }

        [Fact]
        public async Task Handle_ReturnsFailure_WhenDuplicateCode()
        {
            var existing = CreateExistingProduct();
            var otherProduct = new Domain.Entities.ProdItem { Id = "other-id", Code = "P002" };
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
        public async Task Handle_SetsLastActionToUpdate()
        {
            var existing = CreateExistingProduct();
            var command = CreateValidCommand();
            _productRepoMock.Setup(r => r.GetByIdAsync(command.Id))
                .ReturnsAsync(existing);
            _productRepoMock.Setup(r => r.GetByCodeAsync(command.Code))
                .ReturnsAsync(existing);

            await _handler.Handle(command, CancellationToken.None);

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

            await _handler.Handle(command, CancellationToken.None);

            _productRepoMock.Verify(r => r.Update(It.IsAny<Domain.Entities.ProdItem>()), Times.Once);
        }
    }
}
