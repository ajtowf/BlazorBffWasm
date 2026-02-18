using Duende.Bff;
using Duende.Bff.Yarp;
using Microsoft.AspNetCore.Authentication;

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
             .AddAuthentication(options =>
             {
                 options.DefaultScheme = "Cookies";
                 options.DefaultChallengeScheme = "oidc";
             })
            .AddCookie("Cookies")

            .AddOpenIdConnect("oidc", options =>
            {
                options.Authority = "https://demo.duendesoftware.com";
                options.ClientId = "interactive.confidential";
                options.ClientSecret = "secret";

                options.ResponseType = "code";
                options.ResponseMode = "query";

                options.Scope.Clear();
                options.Scope.Add("openid");
                options.Scope.Add("profile");
                options.Scope.Add("kvr-api");
                options.Scope.Add("offline_access");

                options.MapInboundClaims = false;
                options.ClaimActions.MapAll();
                options.GetClaimsFromUserInfoEndpoint = true;
                options.SaveTokens = true;

                options.TokenValidationParameters.NameClaimType = "name";
                options.TokenValidationParameters.RoleClaimType = "role";
            });

        services
            .AddBff()
            .AddRemoteApis();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseRouting();
        app.UseStaticFiles();

        app.UseAuthentication();
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
