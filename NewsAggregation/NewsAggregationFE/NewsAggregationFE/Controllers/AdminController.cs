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
                        await ShowExternalServersList();
                        return true;
                        
                    case "2":
                        await ShowExternalServersDetails();
                        return true;
                        
                    case "3":
                        await ShowUpdateServerDetails();
                        return true;
                        
                    case "4":
                        await ShowAddNewCategory();
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

        private async Task ShowExternalServersList()
        {
            try
            {
                _consoleView.ShowMessages("\nList of external servers:");
                List<ExternalServerList> externalServers = await _adminService.GetExternalServersList();
                string data = _adminService.ConcateServersDetails(externalServers);
                _consoleView.ShowMessages(data);
                
                // Add back button functionality
                _consoleView.ShowMessages("\nPress any key to return to Admin Menu...");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                _consoleView.ShowMessages($"Error loading external servers: {ex.Message}");
                _consoleView.ShowMessages("\nPress any key to return to Admin Menu...");
                Console.ReadKey();
            }
        }

        private async Task ShowExternalServersDetails()
        {
            try
            {
                _consoleView.ShowMessages("\nList of external server details:");
                List<ExternalServerDetail> externalServerDetails = await _adminService.GetExternalServersDetails();
                string data = _adminService.ConcateList(externalServerDetails);
                _consoleView.ShowMessages(data);
                
                // Add back button functionality
                _consoleView.ShowMessages("\nPress any key to return to Admin Menu...");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                _consoleView.ShowMessages($"Error loading external server details: {ex.Message}");
                _consoleView.ShowMessages("\nPress any key to return to Admin Menu...");
                Console.ReadKey();
            }
        }

        private async Task ShowUpdateServerDetails()
        {
            try
            {
                _consoleView.ShowMessages("\nUpdate/Edit the external server's details");
                _consoleView.ShowMessages("");
                string serverId = _consoleView.ReadInput("Enter the external server ID:");
                _consoleView.ShowMessages("");
                string apiKey = _consoleView.ReadInput("Enter the updated API key:");
                
                await _adminService.UpdateServer(serverId, apiKey);
                _consoleView.ShowMessages("\nServer updated successfully!");
                
                // Add back button functionality
                _consoleView.ShowMessages("\nPress any key to return to Admin Menu...");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                _consoleView.ShowMessages($"\nError updating server: {ex.Message}");
                _consoleView.ShowMessages("\nPress any key to return to Admin Menu...");
                Console.ReadKey();
            }
        }

        private async Task ShowAddNewCategory()
        {
            try
            {
                _consoleView.ShowMessages("\nAdd new News Category");
                _consoleView.ShowMessages("");
                string newCategory = _consoleView.ReadInput("Enter new Category:");
                
                await _adminService.AddNewCategory(newCategory);
                _consoleView.ShowMessages("\nCategory added successfully!");
                
                // Add back button functionality
                _consoleView.ShowMessages("\nPress any key to return to Admin Menu...");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                _consoleView.ShowMessages($"\nError adding category: {ex.Message}");
                _consoleView.ShowMessages("\nPress any key to return to Admin Menu...");
                Console.ReadKey();
            }
        }
    }
}