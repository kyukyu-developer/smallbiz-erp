using ERP.Application.Features.Products.Commands;
using ERP.Application.DTOs.Common;
using ERP.Domain.Interfaces;
using FluentAssertions;
using Moq;

namespace ERP.Tests.Products.Commands
{
    public class DeleteProductCommandHandlerTests
    {
        private readonly Mock<IProductRepository> _productRepoMock;
        private readonly DeleteProductCommandHandler _handler;

        public DeleteProductCommandHandlerTests()
        {
            _productRepoMock = new Mock<IProductRepository>();
            _handler = new DeleteProductCommandHandler(_productRepoMock.Object);
        }

        [Fact]
        public async Task Handle_ReturnsSuccess_WhenProductExists()
        {
            var product = new Domain.Entities.ProdItem
            {
                Id = "prod-1",
                Code = "P001",
                Name = "Test Product",
                Active = true
            };
            _productRepoMock.Setup(r => r.GetByIdAsync("prod-1"))
                .ReturnsAsync(product);

            var result = await _handler.Handle(new DeleteProductCommand { Id = "prod-1" }, CancellationToken.None);

            result.IsSuccess.Should().BeTrue();
            result.Data.Should().BeTrue();
        }

        [Fact]
        public async Task Handle_ReturnsFailure_WhenProductNotFound()
        {
            _productRepoMock.Setup(r => r.GetByIdAsync("missing"))
                .ReturnsAsync((Domain.Entities.ProdItem?)null);

            var result = await _handler.Handle(new DeleteProductCommand { Id = "missing" }, CancellationToken.None);

            result.IsSuccess.Should().BeFalse();
            result.ErrorMessage.Should().Contain("not found");
        }

        [Fact]
        public async Task Handle_SetsActiveToFalse()
        {
            var product = new Domain.Entities.ProdItem
            {
                Id = "prod-1",
                Active = true
            };
            _productRepoMock.Setup(r => r.GetByIdAsync("prod-1"))
                .ReturnsAsync(product);

            await _handler.Handle(new DeleteProductCommand { Id = "prod-1" }, CancellationToken.None);

            product.Active.Should().BeFalse();
        }

        [Fact]
        public async Task Handle_SetsLastActionToDelete()
        {
            var product = new Domain.Entities.ProdItem
            {
                Id = "prod-1",
                Active = true
            };
            _productRepoMock.Setup(r => r.GetByIdAsync("prod-1"))
                .ReturnsAsync(product);

            await _handler.Handle(new DeleteProductCommand { Id = "prod-1" }, CancellationToken.None);

            product.LastAction.Should().Be("DELETE");
        }

        [Fact]
        public async Task Handle_CallsUpdate()
        {
            var product = new Domain.Entities.ProdItem { Id = "prod-1", Active = true };
            _productRepoMock.Setup(r => r.GetByIdAsync("prod-1"))
                .ReturnsAsync(product);

            await _handler.Handle(new DeleteProductCommand { Id = "prod-1" }, CancellationToken.None);

            _productRepoMock.Verify(r => r.Update(It.IsAny<Domain.Entities.ProdItem>()), Times.Once);
        }
    }
}
