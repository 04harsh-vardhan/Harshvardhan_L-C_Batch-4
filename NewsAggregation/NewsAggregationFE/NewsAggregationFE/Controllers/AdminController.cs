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
                string choice = _consoleView.ReadInput("\n--- Admin Menu ---\n1. View the list of external servers and status\n2. View the external server's details\n3. Update/Edit the external server's details\n4. Add new News Category\n5. Manage Category Visibility\n6. Logout\nEnter your choice:");
                
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
                        await ShowCategoryManagement();
                        return true;
                        
                    case "6":
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

        private async Task ShowCategoryManagement()
        {
            try
            {
                _consoleView.ShowMessages("\n=== CATEGORY VISIBILITY MANAGEMENT ===");
                _consoleView.ShowMessages("Loading categories...");

                var categories = await _adminService.GetCategoriesStatusAsync();
                
                if (!categories.Any())
                {
                    _consoleView.ShowMessages("No categories found.");
                    _consoleView.ShowMessages("\nPress any key to return to Admin Menu...");
                    Console.ReadKey();
                    return;
                }

                _consoleView.ShowMessages("\nCategory Status:");
                _consoleView.ShowMessages(new string('-', 60));
                
                for (int i = 0; i < categories.Count; i++)
                {
                    var category = categories[i];
                    var status = category.IsEnabled ? "VISIBLE" : "HIDDEN";
                    var statusColor = category.IsEnabled ? "✓" : "✗";
                    
                    _consoleView.ShowMessages($"{i + 1}. {category.CategoryName} - {statusColor} {status}");
                }
                
                _consoleView.ShowMessages(new string('-', 60));
                _consoleView.ShowMessages($"{categories.Count + 1}. Back to Admin Menu");
                
                string choice = _consoleView.ReadInput("\nSelect category to toggle visibility:");
                
                if (int.TryParse(choice, out int categoryIndex) && categoryIndex > 0 && categoryIndex <= categories.Count)
                {
                    await ToggleCategoryVisibility(categories[categoryIndex - 1]);
                }
                else if (categoryIndex == categories.Count + 1)
                {
                    return;
                }
                else
                {
                    _consoleView.ShowMessages("Invalid selection.");
                    _consoleView.ShowMessages("\nPress any key to return...");
                    Console.ReadKey();
                }
            }
            catch (Exception ex)
            {
                _consoleView.ShowMessages($"Error loading category management: {ex.Message}");
                _consoleView.ShowMessages("\nPress any key to return to Admin Menu...");
                Console.ReadKey();
            }
        }

        private async Task ToggleCategoryVisibility(CategoryStatusDto category)
        {
            try
            {
                _consoleView.ShowMessages($"\n=== MANAGING: {category.CategoryName} ===");
                _consoleView.ShowMessages($"Current Status: {(category.IsEnabled ? "VISIBLE" : "HIDDEN")}");
                
                string action = category.IsEnabled ? "HIDE" : "SHOW";
                _consoleView.ShowMessages($"\nThis will {action} the category for all users.");
                
                string confirm = _consoleView.ReadInput($"Are you sure you want to {action} '{category.CategoryName}'? (y/n):");
                
                if (confirm.ToLower() == "y" || confirm.ToLower() == "yes")
                {
                    var updateDto = new UpdateCategoryStatusDto
                    {
                        CategoryId = category.CategoryId,
                        IsEnabled = !category.IsEnabled
                    };

                    bool success = await _adminService.UpdateCategoryStatusAsync(updateDto);
                    
                    if (success)
                    {
                        string newStatus = updateDto.IsEnabled ? "VISIBLE" : "HIDDEN";
                        _consoleView.ShowMessages($"\n✓ Category '{category.CategoryName}' is now {newStatus}!");
                    }
                    else
                    {
                        _consoleView.ShowMessages($"\n✗ Failed to update category '{category.CategoryName}' status.");
                    }
                }
                else
                {
                    _consoleView.ShowMessages("\nOperation cancelled.");
                }
                
                _consoleView.ShowMessages("\nPress any key to continue...");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                _consoleView.ShowMessages($"Error updating category: {ex.Message}");
                _consoleView.ShowMessages("\nPress any key to continue...");
                Console.ReadKey();
            }
        }
    }
}