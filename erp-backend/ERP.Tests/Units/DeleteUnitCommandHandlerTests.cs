using ERP.Application.Features.Units.Commands;
using ERP.Domain.Interfaces;
using MediatR;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Tests.Units
{
    public class DeleteUnitCommandHandlerTests
    {
        private readonly Mock<IUnitRepository> _unitRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly DeleteUnitCommandHandler _handler;

        public DeleteUnitCommandHandlerTests()
        {
            _unitRepositoryMock = new Mock<IUnitRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();

            _handler = new DeleteUnitCommandHandler(
                _unitRepositoryMock.Object,
                _unitOfWorkMock.Object
            );
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenUnitNotFound()
        {
            // Arrange
            var command = new DeleteUnitCommand { Id = "1" };

            _unitRepositoryMock
                .Setup(x => x.GetByIdAsync(command.Id))
                .ReturnsAsync((Domain.Entities.Units)null);

            // Act
            var result = await _handler.Handle(command, default);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("not found", result.ErrorMessage);
        }

        [Fact]
        public async Task Handle_ShouldSoftDeleteUnit_WhenUnitExists()
        {
            // Arrange
            var unit = new Domain.Entities.Units
            {
                Id = "1",
                Name = "Kilogram",
                Symbol = "kg",
                Active = true
            };

            var command = new DeleteUnitCommand { Id = "1" };

            _unitRepositoryMock
                .Setup(x => x.GetByIdAsync(command.Id))
                .ReturnsAsync(unit);

            // Act
            var result = await _handler.Handle(command, default);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.False(unit.Active);
            Assert.Equal("DELETE", unit.LastAction);

            _unitRepositoryMock.Verify(x => x.Update(unit), Times.Once);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }
    }
}
