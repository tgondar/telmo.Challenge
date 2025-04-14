using Infrastructure;
using Infrastructure.Repository;
using Xunit;

namespace Tests.Unit.Repositories
{
    public class IdeaUpdateRepositoryTests : BaseRavenTest
    {
        [Fact]
        public async Task GetAllAsync_ShouldReturnAllIdeaUpdates()
        {
            using (var store = GetTestDocumentStore())
            {
                // Arrange
                using (var session = store.OpenAsyncSession())
                {
                    await session.StoreAsync(new Domain.Entities.IdeaUpdates { IdeaId = 1, UserId = 1, StatusId = 1, IsRead = false });
                    await session.StoreAsync(new Domain.Entities.IdeaUpdates { IdeaId = 2, UserId = 2, StatusId = 2, IsRead = true });
                    await session.SaveChangesAsync();
                }

                var dbContext = new RavenDbContext(store.OpenAsyncSession());
                var repository = new IdeaUpdateRepository(dbContext);

                // Act
                var result = await repository.GetAllAsync();

                // Assert
                Assert.Equal(2, result.Count);
                Assert.Contains(result, x => x.IdeaId == 1);
                Assert.Contains(result, x => x.IdeaId == 2);
            }
        }

        [Fact]
        public async Task GetAllByUserIdAsync_ShouldReturnFilteredIdeaUpdates()
        {
            using (var store = GetTestDocumentStore())
            {
                // Arrange
                using (var session = store.OpenAsyncSession())
                {
                    await session.StoreAsync(new Domain.Entities.IdeaUpdates { IdeaId = 1, UserId = 1, StatusId = 1, IsRead = false });
                    await session.StoreAsync(new Domain.Entities.IdeaUpdates { IdeaId = 2, UserId = 2, StatusId = 2, IsRead = true });
                    await session.SaveChangesAsync();
                }

                var dbContext = new RavenDbContext(store.OpenAsyncSession());
                var repository = new IdeaUpdateRepository(dbContext);

                // Act
                var result = await repository.GetAllByUserIdAsync(1);

                // Assert
                Assert.Single(result);
                Assert.Equal(1, result[0].UserId);
            }
        }

        [Fact]
        public async Task AddAsync_ShouldStoreIdeaUpdate()
        {
            using (var store = GetTestDocumentStore())
            {
                // Arrange
                var dbContext = new RavenDbContext(store.OpenAsyncSession());
                var repository = new IdeaUpdateRepository(dbContext);

                var ideaUpdate = new Domain.Entities.IdeaUpdates
                {
                    IdeaId = 1,
                    UserId = 1,
                    StatusId = 1,
                    IsRead = false,
                    CreatedAt = DateTime.UtcNow
                };

                // Act
                await repository.SaveAsync(ideaUpdate);

                // Assert
                using (var session = store.OpenAsyncSession())
                {
                    var storedIdeaUpdate = await session.LoadAsync<Domain.Entities.IdeaUpdates>(ideaUpdate.RavenId);
                    Assert.NotNull(storedIdeaUpdate);
                    Assert.Equal(ideaUpdate.IdeaId, storedIdeaUpdate.IdeaId);
                }
            }
        }
    }
}
