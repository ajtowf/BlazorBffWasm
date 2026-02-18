using BFF;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System.Net;

namespace IntegrationTests.e2e.Setup;

public class FrontendServerFixture : WebHostServerFixture
{
    public string BackendUrl { get; set; } = default!;

    protected override IHost CreateWebHost()
    {
        var builder = WebApplication.CreateBuilder(new WebApplicationOptions
        {
            ApplicationName = typeof(Startup).Assembly.GetName().Name,
            EnvironmentName = "Development",
        });

        builder.Configuration["LocalBackendApiAddress"] = BackendUrl;

        builder.WebHost.ConfigureKestrel(options =>
        {
            options.Listen(IPAddress.Loopback, 0);
        });

        var startup = new FrontendTestStartup(builder.Configuration);
        startup.ConfigureServices(builder.Services);

        var app = builder.Build();
        startup.Configure(app, app.Environment);

        return app;
    }
}
