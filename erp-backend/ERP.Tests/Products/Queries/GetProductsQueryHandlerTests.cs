using ERP.Application.Features.Products.Queries;
using ERP.Application.DTOs.Products;
using ERP.Domain.Interfaces;
using FluentAssertions;
using Moq;

namespace ERP.Tests.Products.Queries
{
    public class GetProductsQueryHandlerTests
    {
        private readonly Mock<IProductRepository> _productRepoMock;
        private readonly GetProductsQueryHandler _handler;

        public GetProductsQueryHandlerTests()
        {
            _productRepoMock = new Mock<IProductRepository>();
            _handler = new GetProductsQueryHandler(_productRepoMock.Object);
        }

        [Fact]
        public async Task Handle_ReturnsProductList_WhenProductsExist()
        {
            var products = new List<Domain.Entities.Products>
            {
                new() { Id = "1", Code = "P001", Name = "Product 1", GroupId = "g1", BaseUnitId = "u1", Active = true },
                new() { Id = "2", Code = "P002", Name = "Product 2", GroupId = "g1", BaseUnitId = "u1", Active = true }
            };
            _productRepoMock.Setup(r => r.GetAllAsync())
                .ReturnsAsync(products);

            var result = await _handler.Handle(new GetProductsQuery(), CancellationToken.None);

            result.IsSuccess.Should().BeTrue();
            result.Data.Should().HaveCount(2);
        }

        [Fact]
        public async Task Handle_ReturnsEmptyList_WhenNoProducts()
        {
            _productRepoMock.Setup(r => r.GetAllAsync())
                .ReturnsAsync(new List<Domain.Entities.Products>());

            var result = await _handler.Handle(new GetProductsQuery(), CancellationToken.None);

            result.IsSuccess.Should().BeTrue();
            result.Data.Should().BeEmpty();
        }

        [Fact]
        public async Task Handle_MapsPropertiesCorrectly()
        {
            var products = new List<Domain.Entities.Products>
            {
                new()
                {
                    Id = "prod-1",
                    Code = "P001",
                    Name = "Test Product",
                    Description = "Description",
                    GroupId = "group-1",
                    BaseUnitId = "unit-1",
                    TrackInventory = true,
                    HasVariant = false,
                    HasSerialNumber = false,
                    HasBatchNumber = false,
                    AllowNegativeStock = false,
                    Active = true
                }
            };
            _productRepoMock.Setup(r => r.GetAllAsync())
                .ReturnsAsync(products);

            var result = await _handler.Handle(new GetProductsQuery(), CancellationToken.None);

            result.Data![0].Id.Should().Be("prod-1");
            result.Data![0].Code.Should().Be("P001");
            result.Data![0].Name.Should().Be("Test Product");
        }

        [Fact]
        public async Task Handle_AlwaysReturnsSuccess()
        {
            _productRepoMock.Setup(r => r.GetAllAsync())
                .ReturnsAsync(new List<Domain.Entities.Products>());

            var result = await _handler.Handle(new GetProductsQuery(), CancellationToken.None);

            result.IsSuccess.Should().BeTrue();
        }
    }
}
