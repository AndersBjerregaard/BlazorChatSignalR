using BlazorChatSignalR.Shared;
using Microsoft.AspNetCore.SignalR;

namespace BlazorChatSignalR.Server.Hubs
{
    public class DashboardHub : Hub
    {
        private static Dictionary<string, string> Dashboards = new();

        public IState State { get; set; }

        public DashboardHub(IState state)
        {
            State = state;
        }

        public override async Task OnConnectedAsync()
        {
            HttpContext? httpContext = Context.GetHttpContext();

            if (httpContext is null)
            {
                throw new Exception("Connection HttpContext was null");
            }

            State.Clients = Clients;
            State.Groups = Groups;

            string dashboardId = httpContext.Request.Query[$"{Dashboard.DASHBOARD_ID_ARGUMENT}"];

            Dashboards.Add(Context.ConnectionId, dashboardId); // Context here refers to the HubCallerContext type

            await Log("Dashboard, with the id '" + dashboardId + "' has connected.");

            await base.OnConnectedAsync(); // Returns Task.CompletedTask;
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            string dashboardId = Dashboards.FirstOrDefault(kvp => kvp.Key == Context.ConnectionId).Value;
           
            State.Clients = Clients;
            State.Groups = Groups;

            await Log("Dashboard, with the id '" + dashboardId + "' has disconnected!");
        }

        public async Task Log(string message)
        {
            if (State.Clients is null)
            {
                return;
            }
            await Clients.All.SendAsync(Dashboard.DASHBOARD_LOG_METHOD_NAME, arg1: message);
        }

        public async Task Update(string message)
        {
            if (State.Clients is null)
            {
                return;
            }
            await State.Clients.All.SendAsync(Dashboard.DASHBOARD_UPDATE_METHOD_NAME, arg1: message);
        }
    }
}
