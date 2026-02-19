using IntegrationTests.e2e.Support;
using Reqnroll;

namespace IntegrationTests.e2e.Steps;

[Binding]
public class HomePageSteps
{
    private readonly TestScenarioContext _context;

    public HomePageSteps(TestScenarioContext context)
    {
        _context = context;
    }

    [Given("the application is running")]
    public Task GivenTheApplicationIsRunning()
    {
        // Already handled in BeforeScenario
        return Task.CompletedTask;
    }

    [When("I click the Counter link")]
    public async Task WhenIClickCounter()
    {
        await _context.Page.ClickAsync("text=Counter");
        await _context.Page.WaitForURLAsync("**/counter");
    }

    [When(@"I click the increment button (.*) times")]
    public async Task WhenIClickIncrementButtonNTimes(int clicks)
    {
        for (int i = 0; i < clicks; i++)
        {
            await _context.Page.ClickAsync("text=Click me");
        }
    }

    [Then(@"the counter should be (.*)")]
    public async Task ThenTheCounterShouldBe(int expected)
    {
        var content = await _context.Page.TextContentAsync("text=Current count:");

        Assert.Equal($"Current count: {expected}", content?.Trim());
    }

    [When("I click the Weather link")]
    public async Task WhenIClickWeather()
    {
        await _context.Page.ClickAsync("text=Weather");
    }

    [Then("I should see the Weather page")]
    public async Task ThenIShouldSeeWeather()
    {
        await _context.Page.WaitForURLAsync("**/weather");
        Assert.Contains("/weather", _context.Page.Url);
    }

    [When("I click the Claims link")]
    public async Task WhenIClickClaims()
    {
        await _context.Page.ClickAsync("text=Claims");
    }

    [Then("I should see the Claims page")]
    public async Task ThenIShouldSeeClaims()
    {
        await _context.Page.WaitForURLAsync("**/claims");
        Assert.Contains("/claims", _context.Page.Url);
    }
}