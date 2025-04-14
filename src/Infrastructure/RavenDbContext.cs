using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using static System.Formats.Asn1.AsnWriter;

namespace Infrastructure
{
    public class RavenDbContext
    {
        private readonly IAsyncDocumentSession _session;

        public RavenDbContext(IAsyncDocumentSession session)
        {
            _session = session;
        }

        public IAsyncDocumentSession Session => _session;

        public Task<List<T>> GetAllAsync<T>()
        {
            return _session.Query<T>().ToListAsync();
        }

        //public async Task AddAsync<T>(T entity)
        //{
        //    using var session = _session.OpenAsyncSession();
        //    await session.StoreAsync(entity);
        //    await session.SaveChangesAsync();
        //}
    }
}
