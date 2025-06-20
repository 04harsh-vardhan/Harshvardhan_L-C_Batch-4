using NewsAggregationFE.Controllers.Interfaces;
using NewsAggregationFE.Core.Interfaces;
using NewsAggregationFE.Core.Models;
using NewsAggregationFE.Services.Interfaces;

namespace NewsAggregationFE.Controllers
{
    public class AdminController : IAdminController
    {
        private readonly IConsoleView _consoleView;
        private readonly IAdminService _adminService;
        public AdminController(IConsoleView consoleView, IAdminService adminService)
        {
            _consoleView = consoleView;
            _adminService = adminService;
        }
        public async Task<bool> AdminMenu()
        {
            string choice = _consoleView.ReadInput("1. View the list of external servers and status\r\n2. View the external server’s details\r\n3. Update/Edit the external server’s details\r\n4. Add new News Category\r\n5. Logout");
            switch (choice)
            {
                case "1":
                    _consoleView.ShowMessages("List of external servers:");
                    List<ExternalServerDetails> externalServers = await _adminService.GetExternalServersDetails();
                    string data = _adminService.ConcateServersDetails(externalServers);
                    _consoleView.ShowMessages(data);
                    break;
            }
            return true;
        }
    }
}
