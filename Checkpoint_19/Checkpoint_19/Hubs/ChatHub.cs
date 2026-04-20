using Checkpoint_19.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Checkpoint_19.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        public async Task SendMessage(string message)
        {
            var userName = Context.User?.Identity?.Name ?? "Аноним";
            if (!string.IsNullOrEmpty(message))
            {
                await Clients.All.SendAsync("ReceiveMessage", userName, message);
            }
        }
    }
}
