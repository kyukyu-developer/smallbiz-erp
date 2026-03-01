using ERP.Application.Features.Units.Queries;
using ERP.Domain.Entities;
using ERP.Domain.Interfaces;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Tests.Units
{

    


    public class GetUnitsQueryHandlerTests
    {
        private readonly Mock<IUnitRepository> _repoMock;
        private readonly GetUnitsQueryHandler _handler;

        public GetUnitsQueryHandlerTests()
        {
            _repoMock = new Mock<IUnitRepository>();
            _handler = new GetUnitsQueryHandler(_repoMock.Object);
        }

        private static Unit MakeUnit(
            string id,
            string name,
            bool active = true)
        {
            return new Unit
            {
                Id = id,
                Name = name,
                Symbol = name.Substring(0, 1),
                Active = active,
                CreatedAt = DateTime.UtcNow.AddDays(-1),
                UpdatedAt = DateTime.UtcNow,
                CreatedBy = "system",
                UpdatedBy = "admin",
                LastAction = "CREATE"
            };
        }

        // ─────────────────────────────────────────────────────────────
        // Repository Call Test
        // ─────────────────────────────────────────────────────────────

        [Fact]
        public async Task Handle_CallsGetAllAsync()
        {
            _repoMock.Setup(r => r.GetAllAsync())
                .ReturnsAsync(new List<Unit>());

            await _handler.Handle(new GetUnitsQuery(), CancellationToken.None);

            _repoMock.Verify(r => r.GetAllAsync(), Times.Once);
        }

        // ─────────────────────────────────────────────────────────────
        // Active Filtering Tests
        // ─────────────────────────────────────────────────────────────

        [Fact]
        public async Task Handle_ReturnsOnlyActiveUnits_ByDefault()
        {
            var units = new List<Unit>
        {
            MakeUnit("1", "Kilogram", true),
            MakeUnit("2", "Box", false)
        };

            _repoMock.Setup(r => r.GetAllAsync())
                .ReturnsAsync(units);

            var result = await _handler.Handle(
                new GetUnitsQuery(),
                CancellationToken.None);

            result.IsSuccess.Should().BeTrue();
            result.Data.Should().HaveCount(1);
            result.Data![0].Name.Should().Be("Kilogram");
        }

        [Fact]
        public async Task Handle_ReturnsAllUnits_WhenIncludeInactiveIsTrue()
        {
            var units = new List<Unit>
        {
            MakeUnit("1", "Kilogram", true),
            MakeUnit("2", "Box", false)
        };

            _repoMock.Setup(r => r.GetAllAsync())
                .ReturnsAsync(units);

            var result = await _handler.Handle(
                new GetUnitsQuery { IncludeInactive = true },
                CancellationToken.None);

            result.Data.Should().HaveCount(2);
        }

        // ─────────────────────────────────────────────────────────────
        // Empty List Test
        // ─────────────────────────────────────────────────────────────

        [Fact]
        public async Task Handle_ReturnsEmptyList_WhenNoUnitsExist()
        {
            _repoMock.Setup(r => r.GetAllAsync())
                .ReturnsAsync(new List<Unit>());

            var result = await _handler.Handle(
                new GetUnitsQuery(),
                CancellationToken.None);

            result.IsSuccess.Should().BeTrue();
            result.Data.Should().BeEmpty();
        }

        // ─────────────────────────────────────────────────────────────
        // DTO Mapping Test
        // ─────────────────────────────────────────────────────────────

        [Fact]
        public async Task Handle_MapsUnitToDto_Correctly()
        {
            var unit = MakeUnit("1", "Kilogram", true);

            _repoMock.Setup(r => r.GetAllAsync())
                .ReturnsAsync(new List<Unit> { unit });

            var result = await _handler.Handle(
                new GetUnitsQuery(),
                CancellationToken.None);

            var dto = result.Data![0];

            dto.Id.Should().Be(unit.Id);
            dto.Name.Should().Be(unit.Name);
            dto.Symbol.Should().Be(unit.Symbol);
            dto.Active.Should().BeTrue();
            dto.CreatedBy.Should().Be("system");
            dto.ModifiedBy.Should().Be("admin");
            dto.LastAction.Should().Be("CREATE");
        }

        // ─────────────────────────────────────────────────────────────
        // Always Success Test
        // ─────────────────────────────────────────────────────────────

        [Fact]
        public async Task Handle_AlwaysReturnsSuccess()
        {
            _repoMock.Setup(r => r.GetAllAsync())
                .ReturnsAsync(new List<Unit>());

            var result = await _handler.Handle(
                new GetUnitsQuery(),
                CancellationToken.None);

            result.IsSuccess.Should().BeTrue();
            result.ErrorMessage.Should().BeNull();
        }
    }

}
