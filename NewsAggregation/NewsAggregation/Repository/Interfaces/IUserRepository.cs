using NewsAggregation.Models;
using NewsAggregation.Models.DTO;

namespace NewsAggregation.Repository.Interfaces
{
    public interface IUserRepository
    {
        public Task<List<User>> GetAllUsers();
        public Task AddUser(SignupUserDto user);
        public Task<User?> GetSingleUser(string email);
        public Task<string> GetRoleById(int roleId);
    }
}
