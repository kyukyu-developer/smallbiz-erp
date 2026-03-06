using ERP.Application.Features.ProductGroup.Queries;
using ERP.Domain.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Tests.ProductGroup.Queries
{
    public class GetProductGroupByIdQueryHandlerTests
    {
        private readonly Mock<IProductGroupRepository> _mockRepo;
        private readonly GetProductGroupByIdQueryHandler _handler;

        public GetProductGroupByIdQueryHandlerTests()
        {
            _mockRepo = new Mock<IProductGroupRepository>();
            _handler = new GetProductGroupByIdQueryHandler(_mockRepo.Object);
        }


        [Fact]
        public async Task Handle_ProductGroupExists_ReturnsSuccessResult()
        {
            // Arrange
            var productGroupId = "1";
            var productGroup = new Domain.Entities.ProductGroup
            {
                Id = productGroupId,
                Name = "Test Group",
                Description = "Test Desc",
                Active = true,
                CreatedAt = System.DateTime.UtcNow,
                UpdatedAt = System.DateTime.UtcNow,
                CreatedBy = "Tester",
                UpdatedBy = "Tester",
                LastAction = "Create"
            };

            _mockRepo.Setup(r => r.GetByIdAsync(productGroupId))
                     .ReturnsAsync(productGroup);

            var query = new GetProductGroupByIdQuery { Id = productGroupId };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Equal(productGroupId, result.Data.Id);
            Assert.Equal("Test Group", result.Data.Name);

            _mockRepo.Verify(r => r.GetByIdAsync(productGroupId), Times.Once);
        }

        [Fact]
        public async Task Handle_ProductGroupDoesNotExist_ReturnsFailureResult()
        {
            // Arrange
            var productGroupId = "999";
            _mockRepo.Setup(r => r.GetByIdAsync(productGroupId))
                     .ReturnsAsync((Domain.Entities.ProductGroup?)null);

            var query = new GetProductGroupByIdQuery { Id = productGroupId };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Null(result.Data);
            Assert.Contains(productGroupId.ToString(), result.ErrorMessage);

            _mockRepo.Verify(r => r.GetByIdAsync(productGroupId), Times.Once);
        }


    }
}
