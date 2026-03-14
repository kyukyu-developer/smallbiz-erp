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


    public class CreateProductGroupCommandHandlerTests
    {
        private readonly Mock<IProductGroupRepository> _mockRepo;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly CreateProductGroupCommandHandler _handler;

        public CreateProductGroupCommandHandlerTests()
        {
            _mockRepo = new Mock<IProductGroupRepository>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _handler = new CreateProductGroupCommandHandler(_mockRepo.Object, _mockUnitOfWork.Object);
        }

        [Fact]
        public async Task Handle_DuplicateName_ReturnsFailure()
        {
            // Arrange
            var command = new CreateProductGroupCommand
            {
                Name = "Existing Group",
                Description = "Some description"
            };

            // Mock repository to return an existing group
            _mockRepo.Setup(r => r.GetByName(command.Name))
                     .ReturnsAsync(new Domain.Entities.ProdGroup { Id = "1", Name = "Existing Group" });

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Null(result.Data);
            Assert.Contains("already exists", result.ErrorMessage);

            // Ensure AddAsync & SaveChangesAsync are NOT called
            _mockRepo.Verify(r => r.AddAsync(It.IsAny<Domain.Entities.ProdGroup>()), Times.Never);
            _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task Handle_NewProductGroup_ReturnsSuccess()
        {
            // Arrange
            var command = new CreateProductGroupCommand
            {
                Name = "New Group",
                Description = "Some description"
            };

            // Mock repository to return null (no duplicates)
            _mockRepo.Setup(r => r.GetByName(command.Name))
                     .ReturnsAsync((Domain.Entities.ProdGroup?)null);

            // Mock AddAsync to just complete
            _mockRepo.Setup(r => r.AddAsync(It.IsAny<Domain.Entities.ProdGroup>()))
                     .Returns(Task.CompletedTask);

            //// Mock SaveChangesAsync to just complete
            //_mockUnitOfWork.Setup(u => u.SaveChangesAsync())
            //               .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Equal("New Group", result.Data.Name);
            Assert.Equal("Some description", result.Data.Description);
            Assert.True(result.Data.Active);

            // Verify AddAsync and SaveChangesAsync called exactly once
            _mockRepo.Verify(r => r.AddAsync(It.IsAny<Domain.Entities.ProdGroup>()), Times.Once);
            _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
        }
    }
}
