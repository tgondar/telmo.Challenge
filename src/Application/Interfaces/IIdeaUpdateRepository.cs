using Application.Dto;

namespace Application.Interfaces
{
    public interface IIdeaUpdateRepository
    {
        Task<List<Domain.Entities.IdeaUpdates>> GetAllAsync();
        Task<List<Domain.Entities.IdeaUpdates>> GetAllByUserIdAsync(int userId);
        Task<PagedResult<Domain.Entities.IdeaUpdates>> GetAllWithRelationsAsync(int userId, bool newUpdatesOnly, int pageNumber, int pageSize);
        Task SaveAsync(Domain.Entities.IdeaUpdates ideaUpdate);
        Task<Domain.Entities.IdeaUpdates> GetIdeaByIdAndUserIdAsync(string ravenId, int userId);
    }
}
