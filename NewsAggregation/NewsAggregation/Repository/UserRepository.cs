using Microsoft.EntityFrameworkCore;
using NewsAggregation.Models;
using NewsAggregation.Models.DTO;
using NewsAggregation.Repository.Interfaces;


namespace NewsAggregation.Repository
{
    public class UserRepository : IUserRepository
    {
        private NewsAggDBContext _dbContext;
        public UserRepository(NewsAggDBContext newsAggDBContext)
        {
            _dbContext = newsAggDBContext;
        }
        public async Task<List<User>> GetAllUsers()
        {
            return await _dbContext.Users.ToListAsync();
        }

        public async Task AddUser(SignupUserDto user)
        {
            try
            {
                await _dbContext.AddAsync(new User { Username = user.UserName, Email = user.Email, Password = user.Password, RoleId = user.RoleId });
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
                throw;
            }
        }
        public async Task<User?> GetSingleUser(string email)
        {
            User? user = await _dbContext.Users.Where(u => u.Email == email).FirstOrDefaultAsync();
            return user;
        }
        public async Task<string> GetRoleById(int roleId)
        {
            Role? role = await _dbContext.Roles.FirstOrDefaultAsync(r => r.RoleId == roleId);
            if (role == null)
            {
                return "user";
            }
            return role.UserRole;
        }

        public async Task<List<User>> GetAllAdminUsersAsync()
        {
            return await _dbContext.Users
                .Include(u => u.Role)
                .Where(u => u.Role.UserRole.ToLower() == "admin")
                .ToListAsync();
        }
    }
}
