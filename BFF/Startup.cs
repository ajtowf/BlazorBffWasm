using BFF.Options;
using Duende.Bff;
using Duende.Bff.Yarp;
using Duende.IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

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
        app.UseBff();
        app.UseAuthorization();

        app.UseAntiforgery();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapStaticAssets();

            endpoints.MapGet("/device-login", async (HttpContext ctx, IConfiguration config) =>
            {
                var authOptions = new AuthOptions();
                config.GetSection(AuthOptions.SectionName).Bind(authOptions);

                var client = new HttpClient();

                var disco = await client.GetDiscoveryDocumentAsync(authOptions.Authority);

                var tokenResponse = await client.RequestClientCredentialsTokenAsync(
                    new ClientCredentialsTokenRequest
                    {
                        Address = disco.TokenEndpoint,
                        ClientId = authOptions.ClientId,
                        ClientSecret = authOptions.ClientSecret,
                        Scope = string.Join(" ", authOptions.Scope)
                    });

                if (tokenResponse.IsError)
                    throw new Exception(tokenResponse.Error);

                var claims = new List<Claim>
                {
                    new("sub", authOptions.ClientId),
                    new("name", "device"),
                    new("client_id", authOptions.ClientId)
                };

                var identity = new ClaimsIdentity(
                    claims,
                    CookieAuthenticationDefaults.AuthenticationScheme);

                var principal = new ClaimsPrincipal(identity);

                var authProperties = new AuthenticationProperties();
                authProperties.StoreTokens(
                [
                    new AuthenticationToken
                    {
                        Name = "access_token",
                        Value = tokenResponse.AccessToken!
                    },
                    new AuthenticationToken
                    {
                        Name = "expires_at",
                        Value = DateTime.UtcNow
                            .AddSeconds(tokenResponse.ExpiresIn)
                            .ToString("o")
                    }
                ]);

                await ctx.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    principal,
                    authProperties);

                return Results.Redirect("/");
            });

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
