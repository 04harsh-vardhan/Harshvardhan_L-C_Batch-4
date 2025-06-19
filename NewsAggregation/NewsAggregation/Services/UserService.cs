using System.Security.Cryptography;
using NewsAggregation.Models;
using NewsAggregation.Models.DTO;
using NewsAggregation.Repository.Interfaces;
using NewsAggregation.Services.Interfaces;
using NewsAggregation.Utils;

namespace NewsAggregation.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly ILogger<UserService> _logger;
        public UserService(IUserRepository userRepository, IJwtTokenGenerator jwtTokenGenerator, ILogger<UserService> logger)
        {
            _userRepository = userRepository;
            _jwtTokenGenerator = jwtTokenGenerator;
            _logger = logger;
        }
        public async Task<bool> SignupUser(SignupUserDto user)
        {
            List<User> allUsers = await _userRepository.GetAllUsers();
            bool isUserPresent = allUsers.Any(u => u.Email == user.Email);
            if (isUserPresent)
            {
                _logger.LogInformation("User email is Already in use");
                throw new Exception("User email is Already in use");
            }
            _logger.LogInformation("Encrypting the password");
            user.Password = EncryptPassword(user.Password);
            await _userRepository.AddUser(user);
            return true;
        }
        public async Task<string> LoginUser(LoginUserRequestBody loginUserRequest)
        {
            User? user = await _userRepository.GetSingleUser(loginUserRequest.Email);
            if (user == null)
            {
                _logger.LogInformation($"Login user {loginUserRequest.Email} Not Found");
                throw new Exception("No User Found with the given Email");
            }
            if (!VerifyPassword(loginUserRequest.Password, user.Password))
            {
                _logger.LogInformation($"{loginUserRequest.Email} user password is wrong");
                throw new Exception("UserPassword is Wrong");
            }
            //Todo: here I need to generate the JWT token for the user
            string role = await _userRepository.GetRoleById(user.RoleId);
            return GenerateJwtToken(user, role);
        }
        private string GenerateJwtToken<T>(T user, string role) where T : User
        {
            return _jwtTokenGenerator.GenerateJwtToken(user, role);
        }
        private static string EncryptPassword(string password)
        {
            byte[] salt = RandomNumberGenerator.GetBytes(16);
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations: 100, HashAlgorithmName.SHA256);
            byte[] hash = pbkdf2.GetBytes(32);
            byte[] hashBytes = new byte[48];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 32);

            return Convert.ToBase64String(hashBytes);
        }
        private static bool VerifyPassword(string password, string hashedPassword)
        {
            byte[] hashBytes = Convert.FromBase64String(hashedPassword);
            byte[] salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);

            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations: 100, HashAlgorithmName.SHA256);
            byte[] hash = pbkdf2.GetBytes(32);

            for (int i = 0; i < 32; i++)
            {
                if (hashBytes[i + 16] != hash[i])
                    return false;

            }
            return true;
        }

    }
}
