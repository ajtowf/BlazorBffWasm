using Microsoft.AspNetCore.Authorization;

namespace IntegrationTests.e2e.Setup.Auth;

public class TestAuthorizationHandler : AuthorizationHandler<IAuthorizationRequirement>
{
    public override async Task HandleAsync(AuthorizationHandlerContext context)
    {
        foreach (var requirement in context.Requirements)
        {
            await HandleRequirementAsync(context, requirement);
        }
    }

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IAuthorizationRequirement requirement)
    {
        return Task.Run(() => context.Succeed(requirement));
    }
}
