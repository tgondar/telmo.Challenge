using Application.Dto;
using Application.Interfaces;
using Domain.Entities;
using Raven.Client.Documents;

namespace Infrastructure.Repository
{
    public class IdeaUpdateRepository : IIdeaUpdateRepository
    {
        private readonly RavenDbContext _dbContext;

        public IdeaUpdateRepository(RavenDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<List<IdeaUpdates>> GetAllAsync()
        {
            return _dbContext.GetAllAsync<IdeaUpdates>();
        }

        public async Task<List<IdeaUpdates>> GetAllByUserIdAsync(int userId)
        {
            var entities = await _dbContext.GetAllAsync<IdeaUpdates>();
            return entities.Where(p => p.UserId == userId).ToList();
        }

        public async Task<PagedResult<IdeaUpdates>> GetAllWithRelationsAsync(
            int userId,
            bool newUpdatesOnly,
            int pageNumber,
            int pageSize)
        {
            using var session = _dbContext.Session;

            var ideaUpdates = await session.Query<IdeaUpdates>()
                .ToListAsync();

            ideaUpdates = ideaUpdates
                .Where(p => p.UserId == userId)
                .ToList();

            //Filters
            if (newUpdatesOnly)
            {
                ideaUpdates = ideaUpdates
                    .Where(p => !p.IsRead)
                    .ToList();
            }

            //Paging
            var rowCount = ideaUpdates.Count;
            var pagedItems = ideaUpdates
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            //TODO: Fix this
            foreach (var update in ideaUpdates)
            {
                var idea = await session.Query<Idea>().ToListAsync();
                update.Idea = idea.FirstOrDefault(i => i.IdeaId == 1);

                var status = await session.Query<Status>().ToListAsync();
                update.Status = status.FirstOrDefault(s => s.StatusId == update.StatusId);

                var user = await session.Query<User>().ToListAsync();
                update.User = user.FirstOrDefault(u => u.UserId == update.UserId);
            }

            return new PagedResult<IdeaUpdates>
            {
                Items = pagedItems,
                RowCount = rowCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task SaveAsync(IdeaUpdates ideaUpdate)
        {
            using var session = _dbContext.Session;
            await session.StoreAsync(ideaUpdate);
            await session.SaveChangesAsync();
        }

        public async Task<IdeaUpdates> GetIdeaByIdAndUserIdAsync(string ravenId, int userId)
        {
            using var session = _dbContext.Session;
            var ideaUpdates = await session.Query<IdeaUpdates>()
                .ToListAsync();

            var output = ideaUpdates
                .FirstOrDefault(p => p.RavenId == ravenId);

            return output;
        }
    }
}