using Application.IdeaUpdates.Commands.AddIdeaUpdate;
using Domain.Entities;
using Infrastructure;
using Infrastructure.Repository;
using Raven.Client.Documents;
using Raven.TestDriver;
using Xunit;

namespace Tests.Functional.IdeaUpdates.Commands
{
    public class AddIdeaUpdateCommandTests : RavenTestDriver
    {
        private readonly IDocumentStore _store;

        public AddIdeaUpdateCommandTests()
        {
            _store = GetDocumentStore();
        }

        [Fact]
        public async Task Handle_ShouldAddIdeaUpdatesToDatabase()
        {
            // Arrange
            using (var session = _store.OpenAsyncSession())
            {
                await session.StoreAsync(new Idea { IdeaId = 1, Title = "Test Idea" });
                await session.StoreAsync(new Status { StatusId = 2, Name = "In Progress" });
                await session.SaveChangesAsync();
            }

            var dbContext = new RavenDbContext(_store.OpenAsyncSession());
            var repository = new IdeaUpdateRepository(dbContext);
            var handler = new AddIdeaUpdateCommandHandler(repository);

            var command = new AddIdeaUpdateCommand
            {
                IdeaId = 1,
                StatusId = 2
            };

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            using (var session = _store.OpenAsyncSession())
            {
                var ideaUpdates = await session.Query<Domain.Entities.IdeaUpdates>().ToListAsync();

                Assert.NotNull(ideaUpdates);
                Assert.Single(ideaUpdates);
                Assert.Equal(1, ideaUpdates[0].IdeaId);
                Assert.Equal(2, ideaUpdates[0].StatusId);
                Assert.False(ideaUpdates[0].IsRead);
                Assert.NotEqual(default, ideaUpdates[0].CreatedAt);
            }
        }

        [Fact]
        public async Task Handle_ShouldAddMultipleIdeaUpdates_WhenMultipleUsersExist()
        {
            // Arrange
            using (var session = _store.OpenAsyncSession())
            {
                await session.StoreAsync(new Idea { IdeaId = 1, Title = "Test Idea" });
                await session.StoreAsync(new Status { StatusId = 2, Name = "In Progress" });
                await session.SaveChangesAsync();
            }

            var dbContext = new RavenDbContext(_store.OpenAsyncSession());
            var repository = new IdeaUpdateRepository(dbContext);
            var handler = new AddIdeaUpdateCommandHandler(repository);

            var command = new AddIdeaUpdateCommand
            {
                IdeaId = 1,
                StatusId = 2
            };

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            using (var session = _store.OpenAsyncSession())
            {
                var ideaUpdates = await session.Query<Domain.Entities.IdeaUpdates>().ToListAsync();

                Assert.NotNull(ideaUpdates);
                Assert.Single(ideaUpdates);
                Assert.All(ideaUpdates, update =>
                {
                    Assert.Equal(1, update.IdeaId);
                    Assert.Equal(2, update.StatusId);
                    Assert.False(update.IsRead);
                    Assert.NotEqual(default, update.CreatedAt);
                });
            }
        }
    }
}
