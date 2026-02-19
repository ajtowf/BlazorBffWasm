using IntegrationTests.e2e.Setup;
using Microsoft.Playwright;
using Reqnroll;

namespace IntegrationTests.e2e.Support;

[Binding]
public class Hooks
{
    private static TestRunContext _runContext = new();

    private readonly TestScenarioContext _scenarioContext;

    public Hooks(TestScenarioContext scenarioContext)
    {
        _scenarioContext = scenarioContext;
    }

    [BeforeTestRun]
    public static async Task BeforeTestRun()
    {
        _runContext.Fixture = new SharedFixture();
        await _runContext.Fixture.InitializeAsync();

        var visual = false;
        _runContext.Playwright = await Playwright.CreateAsync();
        _runContext.Browser = await _runContext.Playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = !visual,
            SlowMo = visual? 200: 0,
            //Channel = "msedge"
        });

        // Sign in once and store auth state
        var tempContext = await _runContext.Browser.NewContextAsync();
        var tempPage = await tempContext.NewPageAsync();
        await tempPage.SetExtraHTTPHeadersAsync(new Dictionary<string, string>
        {
            ["X-CSRF"] = "1"
        });

        var serverAddress = _runContext.Fixture.FrontendServerFixture.RootUri.ToString();
        await tempPage.GotoAsync(serverAddress + "test-login");

        // Save cookies + localStorage
        await tempContext.StorageStateAsync(new BrowserContextStorageStateOptions
        {
            Path = "storageState.json"
        });

        await tempContext.CloseAsync();
    }

    [BeforeScenario]
    public async Task BeforeScenario()
    {
        _scenarioContext.BrowserContext = await _runContext.Browser.NewContextAsync(new() { StorageStatePath = "storageState.json" });
        _scenarioContext.Page = await _scenarioContext.BrowserContext.NewPageAsync();
        await _scenarioContext.Page.SetExtraHTTPHeadersAsync(new Dictionary<string, string>
        {
            ["X-CSRF"] = "1"
        });

        var serverAddress = _runContext.Fixture.FrontendServerFixture.RootUri.ToString();
        await _scenarioContext.Page.GotoAsync(serverAddress);        
    }

    [AfterScenario]
    public async Task AfterScenario()
    {
        await _scenarioContext.BrowserContext.CloseAsync();
    }

    [AfterTestRun]
    public static async Task AfterTestRun()
    {
        await _runContext.Browser.CloseAsync();
        _runContext.Playwright.Dispose();
        await _runContext.Fixture.DisposeAsync();
    }
}