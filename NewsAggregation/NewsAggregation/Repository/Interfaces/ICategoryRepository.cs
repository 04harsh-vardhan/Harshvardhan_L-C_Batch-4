namespace NewsAggregation.Repository.Interfaces
{
    public interface ICategoryRepository
    {
        public Task<int> GetCategoryIdByName(string categoryName);
    }
}
