using Microsoft.AspNetCore.Authorization;

namespace IntegrationTests.e2e.Setup.Auth;

public class TestAuthorizationPolicyProvider : IAuthorizationPolicyProvider
{
    public Task<AuthorizationPolicy> GetDefaultPolicyAsync()
    {
        return Task.FromResult(new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build());
    }

    public Task<AuthorizationPolicy?> GetFallbackPolicyAsync()
    {
        return Task.FromResult(new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build() ?? null);
    }

    public Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        return Task.FromResult(new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build() ?? null);
    }
}
