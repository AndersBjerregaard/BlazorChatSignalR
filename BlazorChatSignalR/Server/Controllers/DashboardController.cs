using BlazorChatSignalR.Server.Hubs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlazorChatSignalR.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DashboardController : ControllerBase
    {
        private readonly DashboardHub _dashboardHub;

        public DashboardController(DashboardHub dashboardHub)
        {
            _dashboardHub = dashboardHub;
        }

        // POST: Dashboard/SendMessageAsync
        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult> Message(string message)
        {
            if (string.IsNullOrEmpty(message))
            {
                return BadRequest("message cannot be null or empty.");
            }
            await _dashboardHub.Update(message);

            return Ok(message);
        }
    }
}
