namespace NewsAggregationFE.Core.Interfaces
{
    public interface IAdminScreen
    {
        Task ShowExternalServers();
        Task ShowServerDetails();
        Task UpdateServerDetails();
        Task AddNewsCategory();
        Task<bool> ShowMenu();
    }
} 