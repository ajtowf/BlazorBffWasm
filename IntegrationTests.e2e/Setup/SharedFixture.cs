namespace IntegrationTests.e2e.Setup;

public class SharedFixture : IAsyncLifetime, IDisposable
{
    public BackendServerFixture BackendServerFixture { get; set; }
    public FrontendServerFixture FrontendServerFixture { get; set; }

    public SharedFixture()
    {
        BackendServerFixture = new BackendServerFixture();
        var rootUriAbsoluteUri = BackendServerFixture.RootUri.AbsoluteUri;
        FrontendServerFixture = new FrontendServerFixture { BackendUrl = rootUriAbsoluteUri };
        _ = FrontendServerFixture.RootUri.AbsoluteUri; // Trigger lazy loading of front end services
    }

    public void Dispose()
    {
    }

    public Task DisposeAsync()
    {
        BackendServerFixture?.Dispose();
        FrontendServerFixture?.Dispose();
        return Task.CompletedTask;
    }

    public Task InitializeAsync()
    {
        return Task.CompletedTask;
    }
}

