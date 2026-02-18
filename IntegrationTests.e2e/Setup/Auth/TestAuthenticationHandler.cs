using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace IntegrationTests.e2e.Setup.Auth;

public class TestAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder) : 
    AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
{
    public const string SchemeName = "Test Scheme";

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var identity = new ClaimsIdentity([
            new("aud", "api"),
            new("sub", "123"),
            new("name", "John Doe"),
            new("upn", "john@doe.se"),
            new("winaccountname", "DOMAIN\\jodo"),
            new("primarysid", "2887B4BD-4019-4ECA-AE7E-183E4D6F2674"),
            new("winaccountname", "DOMAIN\\jodo"),
            new("given_name", "John"),
            new("family_name", "Doe"),
            new(ClaimTypes.Email, "john.doe@gmail.com")
        ]);
        var authTicket = new AuthenticationTicket(
            new ClaimsPrincipal(identity),
            new AuthenticationProperties(),
            SchemeName
        );

        return Task.FromResult(AuthenticateResult.Success(authTicket));
    }
}
