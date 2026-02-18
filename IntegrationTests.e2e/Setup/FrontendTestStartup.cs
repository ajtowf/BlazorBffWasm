using BFF;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;

namespace IntegrationTests.e2e.Setup;

public class FrontendTestStartup(IConfiguration configuration) : Startup(configuration)
{
    protected override void ExtendConfiguration(IApplicationBuilder app)
    {
        base.ExtendConfiguration(app);

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapGet("/test-login", async (HttpContext ctx) =>
            {
                var claims = new[]
                {
                new Claim("sub", "123"),
                new("name", "John Doe"),
                new("upn", "john@doe.se"),
                new("winaccountname", "DOMAIN\\jodo"),
                new("primarysid", "2887B4BD-4019-4ECA-AE7E-183E4D6F2674"),
                new("winaccountname", "DOMAIN\\jodo"),
                new("given_name", "John"),
                new("family_name", "Doe"),
                new(ClaimTypes.Email, "john.doe@gmail.com")
        };

                var identity = new ClaimsIdentity(claims, "Cookies");
                var principal = new ClaimsPrincipal(identity);

                var props = new AuthenticationProperties();
                props.StoreTokens([
                    new AuthenticationToken
                {
                    Name = "access_token",
                    Value = "fake-access-token"
                }
                ]);

                await ctx.SignInAsync("Cookies", principal, props);

                return Results.Ok();
            });
        });
    }
}
