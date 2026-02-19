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

        _runContext.Playwright = await Playwright.CreateAsync();
        _runContext.Browser = await _runContext.Playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = true,
            SlowMo = 0,
            Channel = "msedge"
        });
    }

    [BeforeScenario]
    public async Task BeforeScenario()
    {
        _scenarioContext.BrowserContext = await _runContext.Browser.NewContextAsync();
        _scenarioContext.Page = await _scenarioContext.BrowserContext.NewPageAsync();
        await _scenarioContext.Page.SetExtraHTTPHeadersAsync(new Dictionary<string, string>
        {
            ["X-CSRF"] = "1"
        });

        var serverAddress = _runContext.Fixture.FrontendServerFixture.RootUri.ToString();
        await _scenarioContext.Page.GotoAsync(serverAddress);
        await _scenarioContext.Page.GotoAsync(serverAddress + "test-login");
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