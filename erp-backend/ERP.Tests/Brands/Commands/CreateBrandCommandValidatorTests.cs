using ERP.Application.Features.Brands.Commands;
using FluentAssertions;
using FluentValidation.TestHelper;
using Moq;
using ERP.Domain.Interfaces;
using ERP.Domain.Entities;

namespace ERP.Tests.Brands.Commands
{
    public class CreateBrandCommandValidatorTests
    {
        private readonly CreateBrandCommandValidators _validator;
        private readonly Mock<IBrandRepository> _brandRepositoryMock;

        public CreateBrandCommandValidatorTests()
        {
            _brandRepositoryMock = new Mock<IBrandRepository>();
            _validator = new CreateBrandCommandValidators(_brandRepositoryMock.Object);
        }

        [Fact]
        public void Validate_HasError_WhenNameIsEmpty()
        {
            var command = new CreateBrandCommand { Name = "" };
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(c => c.Name);
        }

        [Fact]
        public void Validate_HasError_WhenNameExceedsMaxLength()
        {
            var command = new CreateBrandCommand { Name = new string('A', 51) };
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(c => c.Name);
        }

        [Fact]
        public void Validate_NoError_WhenNameIsValid()
        {
            var command = new CreateBrandCommand { Name = "Valid Brand" };
            var result = _validator.TestValidate(command);
            result.ShouldNotHaveValidationErrorFor(c => c.Name);
        }

        [Fact]
        public async Task Validate_HasError_WhenDuplicateNameExists()
        {
            // Arrange
            var existingBrand = new ProdBrand { Id = "existing-id", Name = "Duplicate Name" };
            _brandRepositoryMock.Setup(r => r.GetByNameAsync("Duplicate Name"))
                .ReturnsAsync(existingBrand);

            var command = new CreateBrandCommand { Name = "Duplicate Name" };

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.Name)
                .WithErrorMessage("A brand with the same name already exists");
        }

        [Fact]
        public async Task Validate_NoError_WhenNameIsUnique()
        {
            // Arrange
            _brandRepositoryMock.Setup(r => r.GetByNameAsync(It.IsAny<string>()))
                .ReturnsAsync((ProdBrand?)null);

            var command = new CreateBrandCommand { Name = "Unique Brand" };

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldNotHaveValidationErrorFor(c => c.Name);
        }
    }
}