using Microsoft.AspNetCore.Authentication;

namespace IntegrationTests.e2e.Setup.Auth;

public static class TestAutheticationExtensions
{
    public static AuthenticationBuilder AddTestAuth(this AuthenticationBuilder builder, Action<AuthenticationSchemeOptions> configureOptions)
    {
        return builder.AddScheme<AuthenticationSchemeOptions, TestAuthenticationHandler>(TestAuthenticationHandler.SchemeName, "Test auth", configureOptions);
    }
}
