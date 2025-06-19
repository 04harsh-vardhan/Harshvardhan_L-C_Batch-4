using Microsoft.AspNetCore.Mvc;
using NewsAggregation.Services.Interfaces;

namespace NewsAggregation.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class ServerController : ControllerBase
    {
        private readonly IServerService _serverService;
        public ServerController(IServerService serverService)
        {
            _serverService = serverService;
        }

        [HttpGet("GetAllServers")]
        public async Task<IActionResult> GetAllServers()
        {
            return Ok(await _serverService.GetAllServers());
        }

        [HttpPost("UpdateServer")]
        public async Task<IActionResult> UpdateServerDetails()
        {
            return Ok();
        }
    }
}
