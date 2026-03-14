using ERP.Application.Features.ProductGroup.Commands;
using ERP.Domain.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Tests.ProductGroup.Commands
{

    public class UpdateProductGroupCommandHandlerTests
    {
        private readonly Mock<IProductGroupRepository> _mockRepo;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly UpdateProductGroupCommandHandler _handler;

        public UpdateProductGroupCommandHandlerTests()
        {
            _mockRepo = new Mock<IProductGroupRepository>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _handler = new UpdateProductGroupCommandHandler(_mockRepo.Object, _mockUnitOfWork.Object);
        }

        [Fact]
        public async Task Handle_ProductGroupNotFound_ReturnsFailure()
        {
            // Arrange
            var command = new UpdateProductGroupCommand
            {
                Id = "non-existing-id",
                Name = "Updated Name",
                Description = "Updated Desc",
                Active = true
            };

            _mockRepo.Setup(r => r.GetByIdAsync(command.Id))
                     .ReturnsAsync((Domain.Entities.ProdGroup?)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Null(result.Data);
            Assert.Contains("not found", result.ErrorMessage);

            _mockRepo.Verify(r => r.Update(It.IsAny<Domain.Entities.ProdGroup>()), Times.Never);
            _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task Handle_DuplicateName_ReturnsFailure()
        {
            // Arrange
            var command = new UpdateProductGroupCommand
            {
                Id = "1",
                Name = "Existing Name",
                Description = "Updated Desc",
                Active = true
            };

            var existingProduct = new Domain.Entities.ProdGroup { Id = "2", Name = "Existing Name" };

            _mockRepo.Setup(r => r.GetByIdAsync(command.Id))
                     .ReturnsAsync(new  Domain.Entities.ProdGroup { Id = command.Id, Name = "Old Name" });

            _mockRepo.Setup(r => r.GetByName(command.Name))
                     .ReturnsAsync(existingProduct);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Null(result.Data);
            Assert.Contains("already exists", result.ErrorMessage);

            _mockRepo.Verify(r => r.Update(It.IsAny<Domain.Entities.ProdGroup>()), Times.Never);
            _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task Handle_ValidUpdate_ReturnsSuccess()
        {
            // Arrange
            var command = new UpdateProductGroupCommand
            {
                Id = "1",
                Name = "Updated Name",
                Description = "Updated Desc",
                Active = true
            };

            var productGroup = new Domain.Entities.ProdGroup
            {
                Id = command.Id,
                Name = "Old Name",
                Description = "Old Desc",
                Active = true,
                CreatedAt = DateTime.UtcNow.AddDays(-1),
                CreatedBy = "Tester"
            };

            _mockRepo.Setup(r => r.GetByIdAsync(command.Id))
                     .ReturnsAsync(productGroup);

            // No duplicate
            _mockRepo.Setup(r => r.GetByName(command.Name))
                     .ReturnsAsync((Domain.Entities.ProdGroup?)null);

            //_mockUnitOfWork.Setup(u => u.SaveChangesAsync())
            //               .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Equal(command.Name, result.Data.Name);
            Assert.Equal(command.Active, result.Data.Active);
            Assert.Equal("UPDATE", result.Data.LastAction);
            Assert.NotNull(result.Data.UpdatedAt);
            Assert.Equal("System", result.Data.UpdatedBy);

            _mockRepo.Verify(r => r.Update(It.IsAny<Domain.Entities.ProdGroup>()), Times.Once);
            _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
        }
    }
}
