using ERP.Application.Features.Units.Commands;
using ERP.Domain.Interfaces;
using MediatR;
using Moq;
using ERP.Domain.Entities;
using ERP.Application.DTOs.Units;


namespace ERP.Tests.Units
{


    public class UpdateUnitCommandHandlerTests
    {
        private readonly Mock<IUnitRepository> _unitRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly UpdateUnitCommandHandler _handler;

        public UpdateUnitCommandHandlerTests()
        {
            _unitRepositoryMock = new Mock<IUnitRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _handler = new UpdateUnitCommandHandler(_unitRepositoryMock.Object, _unitOfWorkMock.Object);
        }

        [Fact]

        public async Task Handle_ShouldReturnFailure_WhenUnitNotFound()
        {
            //Arrange
            var command = new UpdateUnitCommand
            {
                Id = "1",
                Name = "Kilogram",
                Symbol = "kg",
                Active = true
            };

            _unitRepositoryMock.Setup(repo => repo.GetByIdAsync(command.Id)).ReturnsAsync((ERP.Domain.Entities.ProdUnit)null);

            //Act

            var result = await _handler.Handle(command, default);

            //Assert

            Assert.False(result.IsSuccess);
            Assert.Contains("not found", result.ErrorMessage);


        }



        [Fact]

        public async Task Handle_ShouldReturnFailure_WhenDuplicateNameExists()
        {
            //Arrange
            var unit = new Domain.Entities.ProdUnit { Id="1",Name = "Kilogram",Symbol ="kg" };

            var duplicate = new Domain.Entities.ProdUnit { Id = "2", Name = "Kilogram", Symbol = "kg" };

            var command = new UpdateUnitCommand
            {
                Id = "1",
                Name = "Kilogram",
                Symbol = "kg",
                Active = true
            };  


            _unitRepositoryMock.Setup(repo => repo.GetByIdAsync(command.Id)).ReturnsAsync(unit);

           _unitRepositoryMock.Setup(repo => repo.GetByName(command.Name)).ReturnsAsync(duplicate);

            //Act
            var result = await _handler.Handle(command, default);

            //Assert
            Assert.False(result.IsSuccess);

            Assert.Contains("already exists", result.ErrorMessage);


        }

        [Fact]

        public async Task Handle_ShouldUpdateUnit_WhenValidRequest()
        {
            //Arrange
            var unit = new Domain.Entities.ProdUnit { Id = "1", Name = "Gram", Symbol = "g" };


            var command = new UpdateUnitCommand
            {
                Id = "1",
                Name = "Kilogram",
                Symbol = "kg",
                Active = true
            };


            _unitRepositoryMock.Setup(repo => repo.GetByIdAsync(command.Id)).ReturnsAsync(unit);

            _unitRepositoryMock.Setup(repo => repo.GetByName(command.Name)).ReturnsAsync((Domain.Entities.ProdUnit)null);

            //Act
            var result = await _handler.Handle(command, default);

            //Assert
            Assert.True(result.IsSuccess);

            Assert.Equal("Kilogram", unit.Name);

            _unitRepositoryMock.Verify(x => x.Update(unit),Times.Once);

            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(), Times.Once);




        }

    }
}
