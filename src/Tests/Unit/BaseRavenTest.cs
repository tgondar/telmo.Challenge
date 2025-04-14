using Raven.Client.Documents;
using Raven.TestDriver;

namespace Tests.Unit
{
    public abstract class BaseRavenTest : RavenTestDriver
    {
        protected IDocumentStore GetTestDocumentStore()
        {
            var store = GetDocumentStore();
            PreInitialize(store);
            return store;
        }

        protected override void PreInitialize(IDocumentStore documentStore)
        {
            documentStore.Conventions.FindCollectionName = t =>
            {
                return t.Name;
            };
        }
    }
}
