

using Xunit;
using Moq;
using ERP.Application.Features.Brands.Queries;
using ERP.Domain.Interfaces;
using ERP.Domain.Entities;
using ERP.Application.DTOs.Brands;

namespace ERP.Tests.Brands;


public class GetBrandByIdQueryHandlerTests
{
    private readonly Mock<IBrandRepository> _brandRepositoryMock;
    private readonly GetBrandByIdQueryHandler _handler;

    public GetBrandByIdQueryHandlerTests()
    {
        _brandRepositoryMock = new Mock<IBrandRepository>();
        _handler = new GetBrandByIdQueryHandler(_brandRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnBrand_WhenBrandExists()
    {
        // Arrange
        var brandId = "1";

        var brand = new Domain.Entities.ProdBrand
        {
            Id = brandId,
            Name = "Nike",
            Description = "Sports Brand",
            Active = true
        };

        _brandRepositoryMock
            .Setup(x => x.GetByIdAsync(brandId))
            .ReturnsAsync(brand);

        var query = new GetBrandByIdQuery { Id = brandId };

        // Act
        var result = await _handler.Handle(query, default);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Data);
        Assert.Equal("Nike", result.Data.Name);
        Assert.Equal("Sports Brand", result.Data.Description);

        _brandRepositoryMock.Verify(x => x.GetByIdAsync(brandId), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenBrandNotFound()
    {
        // Arrange
        var brandId = "99";

        _brandRepositoryMock
            .Setup(x => x.GetByIdAsync(brandId))
            .ReturnsAsync((Domain.Entities.ProdBrand)null);

        var query = new GetBrandByIdQuery { Id = brandId };

        // Act
        var result = await _handler.Handle(query, default);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Brand not found", result.ErrorMessage);
    }
}