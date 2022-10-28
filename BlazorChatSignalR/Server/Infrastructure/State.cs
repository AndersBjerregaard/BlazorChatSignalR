using Microsoft.AspNetCore.SignalR;

public class State : IState
{
    public IHubCallerClients Clients { get; set; }
    public IGroupManager Groups { get; set; }
}