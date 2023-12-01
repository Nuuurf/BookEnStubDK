using Microsoft.AspNetCore.SignalR;


namespace RestfulApi.Utilities
{
    public class DateHub : Hub
    {
        public async Task JoinDateGroup(string date)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, date);
        }

        public async Task LeaveDateGroup(string date)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, date);
        }

        public Task SendUpdateToGroup(string date, string message)
        {
            return Clients.Group(date).SendAsync("ReceiveUpdate", message);
        }
    }
}
