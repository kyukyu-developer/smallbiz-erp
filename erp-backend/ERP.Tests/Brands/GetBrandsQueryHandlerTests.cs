
using Xunit;
using Moq;
using ERP.Application.Features.Brands.Queries;
using ERP.Domain.Interfaces;
using ERP.Domain.Entities;

namespace ERP.Tests.Brands;

public class GetBrandsQueryHandlerTests
{
    private readonly Mock<IBrandRepository> _brandRepositoryMock;
    private readonly GetBrandsQueryHandler _handler;

    public GetBrandsQueryHandlerTests()
    {
        _brandRepositoryMock = new Mock<IBrandRepository>();

        _handler = new GetBrandsQueryHandler(_brandRepositoryMock.Object);
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

        _brandRepositoryMock
            .Setup(x => x.GetAllAsync())
            .ReturnsAsync(brands);

        var query = new GetBrandsQuery();

        // Act
        var result = await _handler.Handle(query, default);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(2, result.Data.Count);

        Assert.Equal("Nike", result.Data[0].Name);
        Assert.Equal("Adidas", result.Data[1].Name);

        _brandRepositoryMock.Verify(x => x.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnEmptyList_WhenNoBrandsExist()
    {
        // Arrange
        _brandRepositoryMock
            .Setup(x => x.GetAllAsync())
            .ReturnsAsync(new List<Domain.Entities.ProdBrand>());

        var query = new GetBrandsQuery();

        // Act
        var result = await _handler.Handle(query, default);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Empty(result.Data);
    }
}