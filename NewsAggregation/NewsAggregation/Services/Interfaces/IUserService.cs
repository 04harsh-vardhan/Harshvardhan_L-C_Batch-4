using NewsAggregation.Models.DTO;

namespace NewsAggregation.Services.Interfaces
{
    public interface IUserService
    {
        public Task<bool> SignupUser(SignupUserDto user);
        public Task<string> LoginUser(LoginUserRequestBody loginUserRequest);
    }
}
