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
                string choice = _consoleView.ReadInput("\n--- Admin Menu ---\n1. View the list of external servers and status\n2. View the external server's details\n3. Update/Edit the external server's details\n4. Add new News Category\n5. Manage Category Visibility\n6. Manage Moderated Keywords\n7. Logout\nEnter your choice:");
                
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
                        await ShowModeratedKeywordManagement();
                        return true;
                        
                    case "7":
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

        private async Task ShowModeratedKeywordManagement()
        {
            try
            {
                _consoleView.ShowMessages("\n=== MODERATED KEYWORD MANAGEMENT ===");
                _consoleView.ShowMessages("1. View All Moderated Keywords");
                _consoleView.ShowMessages("2. Add New Moderated Keyword");
                _consoleView.ShowMessages("3. Back to Admin Menu");
                
                string choice = _consoleView.ReadInput("\nEnter your choice:");
                
                switch (choice)
                {
                    case "1":
                        await ShowAllModeratedKeywords();
                        break;
                    case "2":
                        await ShowAddModeratedKeyword();
                        break;
                    case "3":
                        return;
                    default:
                        _consoleView.ShowMessages("Invalid choice. Please try again.");
                        _consoleView.ShowMessages("\nPress any key to continue...");
                        Console.ReadKey();
                        break;
                }
            }
            catch (Exception ex)
            {
                _consoleView.ShowMessages($"Error in moderated keyword management: {ex.Message}");
                _consoleView.ShowMessages("\nPress any key to return to Admin Menu...");
                Console.ReadKey();
            }
        }

        private async Task ShowAllModeratedKeywords()
        {
            try
            {
                _consoleView.ShowMessages("\n=== ALL MODERATED KEYWORDS ===");
                _consoleView.ShowMessages("Loading moderated keywords...");

                var keywords = await _adminService.GetAllModeratedKeywordsAsync();
                
                if (!keywords.Any())
                {
                    _consoleView.ShowMessages("\nNo moderated keywords found.");
                }
                else
                {
                    _consoleView.ShowMessages($"\nFound {keywords.Count} moderated keyword(s):");
                    _consoleView.ShowMessages(new string('-', 50));
                    
                    for (int i = 0; i < keywords.Count; i++)
                    {
                        var keyword = keywords[i];
                        _consoleView.ShowMessages($"{i + 1}. {keyword.Keyword} (ID: {keyword.Id})");
                    }
                    
                    _consoleView.ShowMessages(new string('-', 50));
                }
                
                _consoleView.ShowMessages("\nNote: These keywords are used to filter inappropriate content in articles and notifications.");
                _consoleView.ShowMessages("\nPress any key to continue...");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                _consoleView.ShowMessages($"Error loading moderated keywords: {ex.Message}");
                _consoleView.ShowMessages("\nPress any key to continue...");
                Console.ReadKey();
            }
        }

        private async Task ShowAddModeratedKeyword()
        {
            try
            {
                _consoleView.ShowMessages("\n=== ADD MODERATED KEYWORD ===");
                _consoleView.ShowMessages("Enter a keyword that should be moderated/filtered from content.");
                _consoleView.ShowMessages("This keyword will be used to identify inappropriate articles and notifications.");
                
                string keyword = _consoleView.ReadInput("\nEnter keyword to moderate:");
                
                if (string.IsNullOrWhiteSpace(keyword))
                {
                    _consoleView.ShowMessages("\nKeyword cannot be empty. Operation cancelled.");
                    _consoleView.ShowMessages("\nPress any key to continue...");
                    Console.ReadKey();
                    return;
                }

                keyword = keyword.Trim();
                
                _consoleView.ShowMessages($"\nYou are about to add '{keyword}' as a moderated keyword.");
                _consoleView.ShowMessages("This will affect content filtering across the entire application.");
                
                string confirm = _consoleView.ReadInput($"\nAre you sure you want to add '{keyword}' to moderated keywords? (y/n):");
                
                if (confirm.ToLower() == "y" || confirm.ToLower() == "yes")
                {
                    bool success = await _adminService.AddModeratedKeywordAsync(keyword);
                    
                    if (success)
                    {
                        _consoleView.ShowMessages($"\n✓ Keyword '{keyword}' has been successfully added to moderated keywords!");
                        _consoleView.ShowMessages("It will now be used for content filtering.");
                    }
                    else
                    {
                        _consoleView.ShowMessages($"\n✗ Failed to add keyword '{keyword}'. It may already exist or there was an error.");
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
                _consoleView.ShowMessages($"Error adding moderated keyword: {ex.Message}");
                _consoleView.ShowMessages("\nPress any key to continue...");
                Console.ReadKey();
            }
        }
    }
}