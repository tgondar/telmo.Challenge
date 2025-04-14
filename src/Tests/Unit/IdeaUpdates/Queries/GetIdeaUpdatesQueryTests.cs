using Application.Dto;
using Application.IdeaUpdates.Queries.GetIdeaUpdates;
using Application.Interfaces;
using Ardalis.GuardClauses;
using AutoMapper;
using FluentValidation.TestHelper;
using Moq;
using Xunit;

namespace Tests.Unit.IdeaUpdates.Queries
{
    public class GetIdeaUpdatesQueryHandlerTests
    {
        private readonly Mock<IIdeaUpdateRepository> _mockRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly GetIdeaUpdatesQuery.GetIdeaUpdatesQueryHandler _handler;
        private readonly GetIdeaUpdatesQueryValidator _validator;

        public GetIdeaUpdatesQueryHandlerTests()
        {
            _mockRepository = new Mock<IIdeaUpdateRepository>();
            _mockMapper = new Mock<IMapper>();
            _handler = new GetIdeaUpdatesQuery.GetIdeaUpdatesQueryHandler(_mockRepository.Object, _mockMapper.Object);
            _validator = new GetIdeaUpdatesQueryValidator();
        }

        [Fact]
        public async Task Handle_ShouldReturnPagedResult_WhenDataExists()
        {
            // Arrange
            var query = new GetIdeaUpdatesQuery(1, false, 1, 10);

            var pagedResult = new PagedResult<Domain.Entities.IdeaUpdates>
            {
                Items = new List<Domain.Entities.IdeaUpdates>
                {
                    new Domain.Entities.IdeaUpdates { IdeaId = 1, UserId = 1, StatusId = 1, IsRead = false },
                    new Domain.Entities.IdeaUpdates { IdeaId = 2, UserId = 1, StatusId = 2, IsRead = true }
                },
                PageNumber = 1,
                PageSize = 10,
                RowCount = 2
            };

            var mappedResult = new PagedResult<IdeaUpdatesDto>
            {
                Items = new List<IdeaUpdatesDto>
                {
                    new IdeaUpdatesDto { IdeaId = 1, UserId = 1, StatusId = 1, IsRead = false },
                    new IdeaUpdatesDto { IdeaId = 2, UserId = 1, StatusId = 2, IsRead = true }
                },
                PageNumber = 1,
                PageSize = 10,
                RowCount = 2
            };

            _mockRepository
                .Setup(repo => repo.GetAllWithRelationsAsync(query.UserId, query.NewUpdatesOnly, query.PageNumber, query.PageSize))
                .ReturnsAsync(pagedResult);

            _mockMapper
                .Setup(mapper => mapper.Map<PagedResult<IdeaUpdatesDto>>(pagedResult))
                .Returns(mappedResult);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Items.Count);
            Assert.Equal(1, result.Items[0].IdeaId);
            Assert.Equal(2, result.Items[1].IdeaId);
        }

        [Fact]
        public Task Handle_ShouldThrowNotFoundException_WhenNoDataExists()
        {
            // Arrange
            var query = new GetIdeaUpdatesQuery(1, false, 1, 10);

            _mockRepository
                .Setup(repo => repo.GetAllWithRelationsAsync(query.UserId, query.NewUpdatesOnly, query.PageNumber, query.PageSize))
                .ReturnsAsync(new PagedResult<Domain.Entities.IdeaUpdates>
                {
                    Items = new List<Domain.Entities.IdeaUpdates>(),
                    PageNumber = 1,
                    PageSize = 10,
                    RowCount = 0
                });

            // Act & Assert
            return Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(query, CancellationToken.None));
        }

        [Fact]
        public void Validator_ShouldHaveError_WhenUserIdIsZeroOrNegative()
        {
            // Arrange
            var queryWithZeroUserId = new GetIdeaUpdatesQuery(0, false, 1, 10);
            var queryWithNegativeUserId = new GetIdeaUpdatesQuery(-1, false, 1, 10);

            // Act & Assert
            var resultZero = _validator.TestValidate(queryWithZeroUserId);
            resultZero.ShouldHaveValidationErrorFor(x => x.UserId)
                .WithErrorMessage("User Id must be greater than 0.");

            var resultNegative = _validator.TestValidate(queryWithNegativeUserId);
            resultNegative.ShouldHaveValidationErrorFor(x => x.UserId)
                .WithErrorMessage("User Id must be greater than 0.");
        }

        [Fact]
        public void Validator_ShouldNotHaveError_WhenUserIdIsValid()
        {
            // Arrange
            var validQuery = new GetIdeaUpdatesQuery(1, false, 1, 10);

            // Act & Assert
            var result = _validator.TestValidate(validQuery);
            result.ShouldNotHaveValidationErrorFor(x => x.UserId);
        }
    }
}
