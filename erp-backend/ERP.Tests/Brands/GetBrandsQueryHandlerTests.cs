
using Xunit;
using Moq;
using ERP.Application.Features.Brands.Queries;
using ERP.Domain.Interfaces;
using ERP.Domain.Entities;

namespace ERP.Tests.Brands;

public class GetBrandsQueryHandlerTests
{
    private readonly Mock<IBrandRepository> _brandRepositoryMock;
    private readonly Mock<ICacheService> _cacheMock;
    private readonly Mock<ICacheKeyBuilder> _keyBuilderMock;
    private readonly GetBrandsQueryHandler _handler;

    public GetBrandsQueryHandlerTests()
    {
        _brandRepositoryMock = new Mock<IBrandRepository>();
        _cacheMock = new Mock<ICacheService>();
        _keyBuilderMock = new Mock<ICacheKeyBuilder>();

        _keyBuilderMock.Setup(x => x.Brand_All).Returns("brand:all");

        _handler = new GetBrandsQueryHandler(
            _brandRepositoryMock.Object,
            _cacheMock.Object,
            _keyBuilderMock.Object
        );
    }

    [Fact]
    public async Task Handle_ShouldReturnBrandList_WhenBrandsExist()
    {
        // Arrange
        var brands = new List<ERP.Domain.Entities.ProdBrand>
        {
            new ERP.Domain.Entities.ProdBrand
            {
                Id = "1",
                Name = "Nike",
                Description = "Sports brand",
                Active = true
            },
            new ERP.Domain.Entities.ProdBrand
            {
                Id = "2",
                Name = "Adidas",
                Description = "Sports company",
                Active = true
            }
        };

        _cacheMock
            .Setup(x => x.GetOrSetAsync(
                It.IsAny<string>(),
                It.IsAny<Func<Task<IEnumerable<ProdBrand>>>>(),
                It.IsAny<TimeSpan?>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(brands);

        var query = new GetBrandsQuery();

        // Act
        var result = await _handler.Handle(query, default);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(2, result.Data.Count);

        Assert.Equal("Nike", result.Data[0].Name);
        Assert.Equal("Adidas", result.Data[1].Name);

        _cacheMock.Verify(x => x.GetOrSetAsync(
            It.IsAny<string>(),
            It.IsAny<Func<Task<IEnumerable<ProdBrand>>>>(),
            It.IsAny<TimeSpan?>(),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnEmptyList_WhenNoBrandsExist()
    {
        // Arrange
        var emptyBrands = new List<Domain.Entities.ProdBrand>();

        _cacheMock
            .Setup(x => x.GetOrSetAsync(
                It.IsAny<string>(),
                It.IsAny<Func<Task<IEnumerable<ProdBrand>>>>(),
                It.IsAny<TimeSpan?>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(emptyBrands);

        var query = new GetBrandsQuery();

        // Act
        var result = await _handler.Handle(query, default);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Empty(result.Data);
    }
}