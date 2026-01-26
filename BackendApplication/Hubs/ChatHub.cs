using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace BackendApplication.Hubs;

[Authorize]
public class ChatHub(ILogger<ChatHub> logger) : Hub
{
    public override async Task OnConnectedAsync()
    {
        try
        {
            var claimsIdentity = (ClaimsIdentity?)Context.User?.Identity;
            var userDomainName = claimsIdentity?.DomainUsername();

            logger.LogInformation("{DomainUser} connected on {Machine} with connectionId {HubConnectionId}.", userDomainName, Environment.MachineName, Context.ConnectionId);
        }
        catch (Exception e)
        {
            logger.LogError(e, "{Message}", e.Message);
        }
        finally
        {
            await base.OnConnectedAsync();
        }
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        try
        {
            var claimsIdentity = (ClaimsIdentity?)Context.User?.Identity;
            var userDomainName = claimsIdentity?.DomainUsername();

            logger.LogInformation("{DomainUser} disconnected from {Machine} with connectionId {HubConnectionId}.", userDomainName, Environment.MachineName, Context.ConnectionId);
        }
        catch (Exception e)
        {
            logger.LogError(e, "{Message}", e.Message);
        }
        finally
        {
            await base.OnDisconnectedAsync(exception);
        }
    }

    public async Task Send(string message)
    {
        logger.LogInformation(message);

        await Clients.All.SendAsync("Receive", message);
    }
}

public static class ClaimsIdentityExtensions
{
    public static string DomainUsername(this ClaimsIdentity claimsIdentity)
    {
        var windowsAccountClaimTypes = new[] { "winaccountname", ClaimTypes.WindowsAccountName };

        var usernameClaim = claimsIdentity.Claims.FirstOrDefault(x => windowsAccountClaimTypes.Contains(x.Type));
        if (usernameClaim != null)
        {
            return usernameClaim.Value;
        }

        var clientIdClaim = claimsIdentity.Claims.FirstOrDefault(x => x.Type == "client_id");
        if (clientIdClaim != null)
        {
            return clientIdClaim.Value;
        }

        return string.Empty;
    }
}