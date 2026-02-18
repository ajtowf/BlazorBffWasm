using IntegrationTests.e2e.Setup;
using Microsoft.Playwright;

namespace IntegrationTests.e2e.Support;

public class TestContext
{
    // Run-level (shared)
    public IPlaywright Playwright { get; set; } = default!;
    public IBrowser Browser { get; set; } = default!;
    public SharedFixture Fixture { get; set; } = default!;
    
    // Scenario-level
    public IBrowserContext BrowserContext { get; set; } = default!;
    public IPage Page { get; set; } = default!;
}
