using Microsoft.AspNetCore.Mvc;
using NewsAggregation.Models.DTO;
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
        [HttpPost("AddServer")]
        public async Task<IActionResult> AddServer(ExternalServerDto externalServerDto)
        {
            return Ok(await _serverService.AddServer(externalServerDto));
        }

        [HttpPut("UpdateServer")]
        public async Task<IActionResult> UpdateServerDetails([FromBody] ServerDetailRequestBody serverDetail)
        {
            return Ok(await _serverService.UpdateServerDetails(serverDetail));
        }
    }
}
