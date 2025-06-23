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
                    List<ExternalServerList> externalServers = await _adminService.GetExternalServersList();
                    string data = _adminService.ConcateServersDetails(externalServers);
                    _consoleView.ShowMessages(data);
                    break;
                case "2":
                    _consoleView.ShowMessages("List of external server details:");
                    List<ExternalServerDetail> externalServerDetails = await _adminService.GetExternalServersDetails();
                    string sData = _adminService.ConcateList(externalServerDetails);
                    _consoleView.ShowMessages(sData);
                    break;
                case "3":
                    try
                    {
                        _consoleView.ShowMessages("Update/Edit the external server’s details\r\nEnter the external server ID");
                        string serverId = _consoleView.ReadInput("");
                        string apiKey = _consoleView.ReadInput("Enter the updated API key");
                        await _adminService.UpdateServer(serverId, apiKey);
                    }
                    catch (Exception ex)
                    {

                    }
                    break;
                case "4":
                    try
                    {
                        string newCategory = _consoleView.ReadInput("Enter new Category");
                        await _adminService.AddNewCategory(newCategory);
                    }
                    catch (Exception ex)
                    { }
                    break;
                case "5":
                    // Logout
                    break;
                default:
                    break;
            }
            return true;
        }
    }
}
