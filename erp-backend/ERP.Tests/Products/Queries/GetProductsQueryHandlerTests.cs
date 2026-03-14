using ERP.Application.Features.Products.Queries;
using ERP.Application.DTOs.Products;
using ERP.Domain.Interfaces;
using ERP.Domain.Entities;
using FluentAssertions;
using Moq;

namespace ERP.Tests.Products.Queries
{
    public class GetProductsQueryHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IRepository<ProdItem>> _productsRepoMock;
        private readonly Mock<IRepository<ProdCategory>> _categoriesRepoMock;
        private readonly GetProductsQueryHandler _handler;

        public GetProductsQueryHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _productsRepoMock = new Mock<IRepository<ProdItem>>();
            _categoriesRepoMock = new Mock<IRepository<ProdCategory>>();
            _unitOfWorkMock.Setup(u => u.Products).Returns(_productsRepoMock.Object);
            _unitOfWorkMock.Setup(u => u.Categories).Returns(_categoriesRepoMock.Object);
            _categoriesRepoMock.Setup(r => r.GetAllAsync())
                .ReturnsAsync(new List<ProdCategory>());
            _handler = new GetProductsQueryHandler(_unitOfWorkMock.Object);
        }

        [Fact]
        public async Task Handle_ReturnsProductList_WhenProductsExist()
        {
            var products = new List<ProdItem>
            {
                new() { Id = "1", Code = "P001", Name = "Product 1", BaseUnitId = "u1", Active = true },
                new() { Id = "2", Code = "P002", Name = "Product 2", BaseUnitId = "u1", Active = true }
            };
            _productsRepoMock.Setup(r => r.GetAllAsync())
                .ReturnsAsync(products);

            var result = await _handler.Handle(new GetProductsQuery(), CancellationToken.None);

            result.IsSuccess.Should().BeTrue();
            result.Data.Should().HaveCount(2);
        }

        [Fact]
        public async Task Handle_ReturnsEmptyList_WhenNoProducts()
        {
            _productsRepoMock.Setup(r => r.GetAllAsync())
                .ReturnsAsync(new List<ProdItem>());

            var result = await _handler.Handle(new GetProductsQuery(), CancellationToken.None);

            result.IsSuccess.Should().BeTrue();
            result.Data.Should().BeEmpty();
        }

        [Fact]
        public async Task Handle_MapsPropertiesCorrectly()
        {
            var products = new List<ProdItem>
            {
                new()
                {
                    Id = "prod-1",
                    Code = "P001",
                    Name = "Test Product",
                    Description = "Description",
                    BaseUnitId = "unit-1",
                    TrackType = 1,
                    Active = true
                }
            };
            _productsRepoMock.Setup(r => r.GetAllAsync())
                .ReturnsAsync(products);

            var result = await _handler.Handle(new GetProductsQuery(), CancellationToken.None);

            result.Data![0].Id.Should().Be("prod-1");
            result.Data![0].Code.Should().Be("P001");
            result.Data![0].Name.Should().Be("Test Product");
        }

        [Fact]
        public async Task Handle_AlwaysReturnsSuccess()
        {
            _productsRepoMock.Setup(r => r.GetAllAsync())
                .ReturnsAsync(new List<ProdItem>());

            var result = await _handler.Handle(new GetProductsQuery(), CancellationToken.None);

            result.IsSuccess.Should().BeTrue();
        }
    }
}
