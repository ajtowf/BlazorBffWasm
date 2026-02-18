using IntegrationTests.e2e.Setup;
using Microsoft.Playwright;
using static Microsoft.Playwright.Assertions;

namespace IntegrationTests.e2e;

[Collection(SharedCollectionFixture.Name)]
public class HomePageTests
{
    private string _serverAddress;

    public HomePageTests(SharedFixture sharedFixture)
    {
        _serverAddress = sharedFixture.FrontendServerFixture.RootUri.ToString();
    }

    [Fact]
    public async Task HomePage_Should_Load()
    {
        using var playwright = await Playwright.CreateAsync();
        await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = false,
            SlowMo = 1000,
            Channel = "msedge"
        });

        await using var context = await browser.NewContextAsync(new BrowserNewContextOptions() { ViewportSize = ViewportSize.NoViewport });

        var page = await context.NewPageAsync();
        
        await page.SetExtraHTTPHeadersAsync(new Dictionary<string, string>
        {
            ["X-CSRF"] = "1"
        });

        await page.GotoAsync(_serverAddress + "test-login");
        await context.StorageStateAsync(new()
        {
            Path = "auth.json"
        });

        await page.GotoAsync(_serverAddress);

        await page.ClickAsync("[class='nav-link']");
        await page.ClickAsync("[class='btn btn-primary']");

        var counter = page.GetByRole(AriaRole.Status);
        await Expect(counter).ToHaveTextAsync("Current count: 1");

        var weather = page.GetByRole(AriaRole.Link, new() { Name = "weather" });
        await weather.ClickAsync();

        var claims = page.GetByRole(AriaRole.Link, new() { Name = "claims" });
        await claims.ClickAsync();
    }
}