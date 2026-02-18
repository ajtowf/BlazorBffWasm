namespace IntegrationTests.e2e.Setup;

public class SharedFixture : IAsyncLifetime
{
    public BackendServerFixture BackendServerFixture { get; set; } = default!;
    public FrontendServerFixture FrontendServerFixture { get; set; } = default!;

    public ValueTask DisposeAsync()
    {
        BackendServerFixture?.Dispose();
        FrontendServerFixture?.Dispose();

        return ValueTask.CompletedTask;
    }

    public ValueTask InitializeAsync()
    {
        BackendServerFixture = new BackendServerFixture();
        var rootUriAbsoluteUri = BackendServerFixture.RootUri.AbsoluteUri;
        FrontendServerFixture = new FrontendServerFixture { BackendUrl = rootUriAbsoluteUri };
        _ = FrontendServerFixture.RootUri.AbsoluteUri; // Trigger lazy loading of front end services

        return ValueTask.CompletedTask;
    }
}

