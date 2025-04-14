using Domain.Entities;
using Raven.Client.Documents;
using Raven.Client.Documents.Operations.Revisions;
using Raven.TestDriver;
using Xunit;

namespace Tests.Unit
{
    public class BasicTest : RavenTestDriver
    {
        protected override void PreInitialize(IDocumentStore documentStore)
        {
            documentStore.Conventions.FindCollectionName = t =>
            {
                return "IdeaUpdates";
            };
        }
        protected override void SetupDatabase(IDocumentStore documentStore)
        {
            documentStore.Maintenance.Send(new ConfigureRevisionsOperation(new RevisionsConfiguration
            {
                Default = new RevisionsCollectionConfiguration
                {
                    Disabled = false,
                    PurgeOnDelete = true,
                    MinimumRevisionsToKeep = 1,
                    MinimumRevisionAgeToKeep = TimeSpan.FromDays(14),
                }
            }));
        }

        [Fact]
        public void TestBasicContractor()
        {
            using (var store = GetDocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    session.Store(new Domain.Entities.IdeaUpdates
                    {
                        IdeaId = 1,
                        UserId = 1,
                        StatusId = 1,
                        IsRead = false,
                        CreatedAt = DateTime.UtcNow
                    });
                    session.SaveChanges();
                }
                // the rest of your test 
            }
        }
    }
}
