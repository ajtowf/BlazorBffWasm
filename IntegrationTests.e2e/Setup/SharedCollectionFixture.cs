namespace IntegrationTests.e2e.Setup;

[CollectionDefinition(Name, DisableParallelization = true)]
public class SharedCollectionFixture : ICollectionFixture<SharedFixture>
{
    public const string Name = "Shared collection";
}
