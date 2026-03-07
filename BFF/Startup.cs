using BFF.Extensions;
using Duende.Bff;
using Duende.Bff.Yarp;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace BFF;

public class Startup(IConfiguration configuration)
{
    public IConfiguration Configuration { get; } = configuration;

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddRouting();
        services.AddAntiforgery();
        services.AddAuthorization();

        services
            .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme);

        services
            .AddBff()
            .AddRemoteApis();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseRouting();
        app.UseStaticFiles();

        app.UseAuthentication();

        app.UseDeviceAutoLogin(Configuration);

        app.UseBff();
        app.UseAuthorization();

        app.UseAntiforgery();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapStaticAssets();

            endpoints.MapGet("/api/data", async () =>
            {
                var json = await File.ReadAllTextAsync("weather.json");
                return Results.Content(json, "application/json");
            }).RequireAuthorization().AsBffApiEndpoint();

            var backendAddress = Configuration.GetValue<string>("LocalBackendApiAddress");

            endpoints.MapRemoteBffApiEndpoint("/hubapi", new Uri(backendAddress!))
                .WithAccessToken()
                .SkipAntiforgery();

            endpoints.MapRemoteBffApiEndpoint("/remoteapi", new Uri(backendAddress!))
                .WithAccessToken();

            endpoints.MapFallbackToFile("index.html");
        });

        ExtendConfiguration(app);
    }

    protected virtual void ExtendConfiguration(IApplicationBuilder app)
    {
    }
}
