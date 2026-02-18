using BFF.Options;
using Duende.Bff;
using Duende.Bff.Yarp;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;

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
                 options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                 options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
             })
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
            {
                var authOptions = new AuthOptions();
                Configuration.GetSection(AuthOptions.SectionName).Bind(authOptions);
                
                options.Authority = authOptions.Authority;
                options.ClientId = authOptions.ClientId;
                options.ClientSecret = authOptions.ClientSecret;

                options.ResponseType = authOptions.ResponseType;
                options.ResponseMode = authOptions.ResponseMode;

                options.Scope.Clear();
                foreach (var scope in authOptions.Scope)
                {
                    options.Scope.Add(scope);
                }

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
