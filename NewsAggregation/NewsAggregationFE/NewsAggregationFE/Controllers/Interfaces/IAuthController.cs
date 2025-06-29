namespace NewsAggregationFE.Controllers.Interfaces
{
    public interface IAuthController
    {
        public Task<bool> Login();
        public Task<bool> SignUp();
    }
}
