using NewsAggregation.Models.DTO;

namespace NewsAggregation.Services.Interfaces
{
    public interface IUserService
    {
        public Task<bool> SignupUser(SignupUserDto user);
        public Task<LoginResponseDto> LoginUser(LoginUserRequestBody loginUserRequest);
    }
}
