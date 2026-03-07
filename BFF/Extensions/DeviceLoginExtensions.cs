using BFF.Options;
using Duende.IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace BFF.Extensions;

public static class DeviceLoginExtensions
{
    public static IApplicationBuilder UseDeviceAutoLogin(this IApplicationBuilder app, IConfiguration configuration)
    {
        app.Use(async (context, next) =>
        {
            if (!context.User.Identity?.IsAuthenticated ?? true)
            {
                var authOptions = new AuthOptions();
                configuration.GetSection(AuthOptions.SectionName).Bind(authOptions);

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

                await context.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    principal,
                    authProperties);
            }

            await next();
        });

        return app;
    }
}
