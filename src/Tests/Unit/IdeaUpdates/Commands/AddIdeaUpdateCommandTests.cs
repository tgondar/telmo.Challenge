using Application.IdeaUpdates.Commands.AddIdeaUpdate;
using Application.Interfaces;
using Moq;
using Xunit;

namespace Tests.Unit.IdeaUpdates.Commands
{
    public class AddIdeaUpdateCommandHandlerTests
    {
        private readonly Mock<IIdeaUpdateRepository> _mockRepository;
        private readonly AddIdeaUpdateCommandHandler _handler;

        public AddIdeaUpdateCommandHandlerTests()
        {
            _mockRepository = new Mock<IIdeaUpdateRepository>();
            _handler = new AddIdeaUpdateCommandHandler(_mockRepository.Object);
        }

        [Fact]
        public async Task Handle_ShouldAddIdeaUpdate_ForEachUser()
        {
            // Arrange
            var command = new AddIdeaUpdateCommand
            {
                IdeaId = 1,
                StatusId = 2
            };

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockRepository.Verify(
                repo => repo.SaveAsync(It.Is<Domain.Entities.IdeaUpdates>(ideaUpdate =>
                    ideaUpdate.IdeaId == command.IdeaId &&
                    ideaUpdate.StatusId == command.StatusId &&
                    ideaUpdate.UserId == 1 && // Emulated user ID
                    ideaUpdate.IsRead == false &&
                    ideaUpdate.CreatedAt != default)),
                Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldCallRepositoryAddAsync_ForEachUser()
        {
            // Arrange
            var command = new AddIdeaUpdateCommand
            {
                IdeaId = 1,
                StatusId = 2
            };

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockRepository.Verify(repo => repo.SaveAsync(It.IsAny<Domain.Entities.IdeaUpdates>()), Times.Exactly(1));
        }
    }
}
