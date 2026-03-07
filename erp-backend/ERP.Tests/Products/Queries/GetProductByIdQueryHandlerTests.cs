using ERP.Application.Features.Products.Queries;
using ERP.Application.DTOs.Products;
using ERP.Domain.Interfaces;
using FluentAssertions;
using Moq;

namespace ERP.Tests.Products.Queries
{
    public class GetProductByIdQueryHandlerTests
    {
        private readonly Mock<IProductRepository> _productRepoMock;
        private readonly GetProductByIdQueryHandler _handler;

        public GetProductByIdQueryHandlerTests()
        {
            _productRepoMock = new Mock<IProductRepository>();
            _handler = new GetProductByIdQueryHandler(_productRepoMock.Object);
        }

        [Fact]
        public async Task Handle_ReturnsSuccess_WhenProductExists()
        {
            var product = new Domain.Entities.ProdItem
            {
                Id = "prod-1",
                Code = "P001",
                Name = "Test Product",
                BaseUnitId = "unit-1",
                Active = true
            };
            _productRepoMock.Setup(r => r.GetByIdAsync("prod-1"))
                .ReturnsAsync(product);

            var result = await _handler.Handle(new GetProductByIdQuery { Id = "prod-1" }, CancellationToken.None);

            result.IsSuccess.Should().BeTrue();
            result.Data.Should().NotBeNull();
            result.Data!.Id.Should().Be("prod-1");
        }

        [Fact]
        public async Task Handle_ReturnsFailure_WhenProductNotFound()
        {
            _productRepoMock.Setup(r => r.GetByIdAsync("missing"))
                .ReturnsAsync((Domain.Entities.ProdItem?)null);

            var result = await _handler.Handle(new GetProductByIdQuery { Id = "missing" }, CancellationToken.None);

            result.IsSuccess.Should().BeFalse();
            result.ErrorMessage.Should().Contain("not found");
        }

        [Fact]
        public async Task Handle_MapsAllProperties()
        {
            var product = new Domain.Entities.ProdItem
            {
                Id = "prod-1",
                Code = "P001",
                Name = "Full Product",
                Description = "Description",
                CategoryId = "cat-1",
                BaseUnitId = "unit-1",
                MinimumStock = 10,
                MaximumStock = 100,
                ReorderLevel = 20,
                Barcode = "123456",
                TrackType = 2,
                Active = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            _productRepoMock.Setup(r => r.GetByIdAsync("prod-1"))
                .ReturnsAsync(product);

            var result = await _handler.Handle(new GetProductByIdQuery { Id = "prod-1" }, CancellationToken.None);

            result.Data!.Code.Should().Be("P001");
            result.Data.Name.Should().Be("Full Product");
            result.Data.Description.Should().Be("Description");
            result.Data.TrackType.Should().Be(2);
        }

        [Fact]
        public async Task Handle_SetsDataToNull_WhenNotFound()
        {
            _productRepoMock.Setup(r => r.GetByIdAsync("missing"))
                .ReturnsAsync((Domain.Entities.ProdItem?)null);

            var result = await _handler.Handle(new GetProductByIdQuery { Id = "missing" }, CancellationToken.None);

            result.Data.Should().BeNull();
        }
    }
}
