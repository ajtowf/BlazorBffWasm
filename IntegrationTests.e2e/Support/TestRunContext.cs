using IntegrationTests.e2e.Setup;
using Microsoft.Playwright;

namespace IntegrationTests.e2e.Support;

public class TestRunContext
{
    public IPlaywright Playwright { get; set; } = default!;
    public IBrowser Browser { get; set; } = default!;
    public SharedFixture Fixture { get; set; } = default!;
}
