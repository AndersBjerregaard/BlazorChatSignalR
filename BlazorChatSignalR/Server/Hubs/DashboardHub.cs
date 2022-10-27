using BlazorChatSignalR.Shared;
using Microsoft.AspNetCore.SignalR;

namespace BlazorChatSignalR.Server.Hubs
{
    public class DashboardHub : Hub
    {
        private static Dictionary<string, string> Dashboards = new();

        public override async Task OnConnectedAsync()
        {
            HttpContext? httpContext = Context.GetHttpContext();

            if (httpContext is null)
            {
                throw new Exception("Connection HttpContext was null");
            }

            string dashboardId = httpContext.Request.Query[$"{Dashboard.DASHBOARD_ID_ARGUMENT}"];

            Dashboards.Add(Context.ConnectionId, dashboardId); // Context here refers to the HubCallerContext type

            await Log(dashboardId + " has connected.");

            await base.OnConnectedAsync(); // Returns Task.CompletedTask;
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            string dashboardId = Dashboards.FirstOrDefault(kvp => kvp.Key == Context.ConnectionId).Value;
           

            await Log(dashboardId + " has disconnected!");
        }

        public async Task Log(string message)
        {
            await Clients.All.SendAsync("LogToDashboard", message);
        }

        public async Task Update(string message)
        {
            await Clients.All.SendAsync("UpdateDashboard", message);
        }
    }
}
