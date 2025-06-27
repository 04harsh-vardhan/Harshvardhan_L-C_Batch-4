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
            try
            {
                string choice = _consoleView.ReadInput("\n--- Admin Menu ---\n1. View the list of external servers and status\n2. View the external server's details\n3. Update/Edit the external server's details\n4. Add new News Category\n5. Logout\nEnter your choice:");
                
                switch (choice)
                {
                    case "1":
                        _consoleView.ShowMessages("\nList of external servers:");
                        List<ExternalServerList> externalServers = await _adminService.GetExternalServersList();
                        string data = _adminService.ConcateServersDetails(externalServers);
                        _consoleView.ShowMessages(data);
                        return true;
                        
                    case "2":
                        _consoleView.ShowMessages("\nList of external server details:");
                        List<ExternalServerDetail> externalServerDetails = await _adminService.GetExternalServersDetails();
                        string sData = _adminService.ConcateList(externalServerDetails);
                        _consoleView.ShowMessages(sData);
                        return true;
                        
                    case "3":
                        try
                        {
                            _consoleView.ShowMessages("\nUpdate/Edit the external server's details");
                            string serverId = _consoleView.ReadInput("Enter the external server ID:");
                            string apiKey = _consoleView.ReadInput("Enter the updated API key:");
                            await _adminService.UpdateServer(serverId, apiKey);
                            _consoleView.ShowMessages("Server updated successfully!");
                        }
                        catch (Exception ex)
                        {
                            _consoleView.ShowMessages($"Error updating server: {ex.Message}");
                        }
                        return true;
                        
                    case "4":
                        try
                        {
                            string newCategory = _consoleView.ReadInput("Enter new Category:");
                            await _adminService.AddNewCategory(newCategory);
                            _consoleView.ShowMessages("Category added successfully!");
                        }
                        catch (Exception ex)
                        {
                            _consoleView.ShowMessages($"Error adding category: {ex.Message}");
                        }
                        return true;
                        
                    case "5":
                        _consoleView.ShowMessages("Logging out...");
                        return false;
                        
                    default:
                        _consoleView.ShowMessages("Invalid option. Please try again.");
                        return true;
                }
            }
            catch (Exception ex)
            {
                _consoleView.ShowMessages($"An error occurred: {ex.Message}");
                return true;
            }
        }
    }
}