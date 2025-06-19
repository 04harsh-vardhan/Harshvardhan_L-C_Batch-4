namespace NewsAggregationFE.Services.Interfaces
{
    public interface IAuthService
    {
        public Task<bool> Login(string email, string password);
    }
}
