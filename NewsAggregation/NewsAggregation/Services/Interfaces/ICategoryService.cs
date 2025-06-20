namespace NewsAggregation.Services.Interfaces
{
    public interface ICategoryService
    {
        public Task<bool> AddCategory(string category);
    }
}
