using Microsoft.AspNetCore.SignalR;

public interface IState
{
    public IHubCallerClients Clients { get; set; }
    public IGroupManager Groups { get; set; }
}