using ERP.Application.Features.ProductGroup.Queries;
using ERP.Application.Features.Products.Queries;
using ERP.Domain.Interfaces;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Tests.ProductGroup
{
    public class GetProductGroupQueryHandlerTests
    {
        private readonly Mock<IProductGroupRepository> _repositoryMock;
        private readonly GetProductGroupQueryHandler _handler;

        public GetProductGroupQueryHandlerTests()
        {
            _repositoryMock = new Mock<IProductGroupRepository>();
            _handler = new GetProductGroupQueryHandler(_repositoryMock.Object);
        }


        [Fact]
        public async Task Handle_ShouldReturnProductGroups_WhenDataExists()
        {
            // Arrange

            var productGroups = new List<Domain.Entities.ProdGroup>
        {
            new Domain.Entities.ProdGroup
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Electronics",
                Description = "Electronic items",
                Active = true,
                CreatedBy = "Admin"
            },
            new Domain.Entities.ProdGroup
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Beverages",
                Description = "Drinks",
                Active = true,
                CreatedBy = "Admin"
            }
        };

            _repositoryMock
            .Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(productGroups);

            var query = new GetProductGroupQuery();

            // Act

            var result = await _handler.Handle(query, CancellationToken.None);

            //Assert
            result.IsSuccess.Should().BeTrue();
            result.Data.Count.Should().Be(2);
            result.Data[0].Name.Should().Be("Electronics");

        }

            [Fact]

            public async Task Handle_ShouldReturnEmptyList_WhenNoDataExists()
        {
            // Arrange
        _repositoryMock
            .Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(new List<Domain.Entities.ProdGroup>());


            var query = new GetProductGroupQuery();

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Data.Should().BeEmpty();

        }
    }
}
