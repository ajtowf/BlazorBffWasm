using Microsoft.Playwright;

namespace IntegrationTests.e2e.Support;

public class TestScenarioContext
{
    public IBrowserContext BrowserContext { get; set; } = default!;
    public IPage Page { get; set; } = default!;
}
