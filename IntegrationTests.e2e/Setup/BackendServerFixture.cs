using BackendApplication;
using IntegrationTests.e2e.Setup.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace IntegrationTests.e2e.Setup;

public class BackendServerFixture : WebHostServerFixture
{
    protected override IHost CreateWebHost()
    {
        return Microsoft.Extensions.Hosting.Host
            .CreateDefaultBuilder()
            .ConfigureServices(services =>
            {
                services.AddSingleton<IAuthorizationHandler, TestAuthorizationHandler>();
                services.AddSingleton<IAuthorizationPolicyProvider, TestAuthorizationPolicyProvider>();
                services
                    .AddAuthentication(o =>
                    {
                        o.DefaultAuthenticateScheme = TestAuthenticationHandler.SchemeName;
                        o.DefaultChallengeScheme = TestAuthenticationHandler.SchemeName;
                    }).AddTestAuth(_ => { });
            })
            .ConfigureWebHost(webHostBuilder => webHostBuilder
                .UseKestrel()
                .UseStartup<Startup>()
                .UseUrls("http://127.0.0.1:0")) // :0 allows to choose a port automatically
            .ConfigureLogging((ctx, builder) =>
            {
                builder.AddConfiguration(ctx.Configuration);
                builder.ClearProviders();
                builder.AddDebug();
                builder.AddConsole();
            })
            .UseEnvironment("Development")
            .Build();
    }
}
