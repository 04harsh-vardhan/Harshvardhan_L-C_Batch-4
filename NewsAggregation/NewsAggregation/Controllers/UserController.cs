using Microsoft.AspNetCore.Mvc;
using NewsAggregation.Models.DTO;
using NewsAggregation.Services.Interfaces;

namespace NewsAggregation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }
        /// <summary>
        /// {
        ///"email": "john.doe@example.com",
        ///"password": "qwerty"
        ///}
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost("signup")]
        public async Task<ActionResult> SignupUser(SignupUserDto user)
        {
            try
            {
                _logger.LogInformation("SignupUser request for email: {Email}", user.Email);
                var result = await _userService.SignupUser(user);
                _logger.LogInformation("SignupUser completed successfully for email: {Email}", user.Email);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "SignupUser failed for email {Email}: {Message}", user.Email, ex.Message);
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("login/user")]
        public async Task<ActionResult> LoginUser(LoginUserRequestBody loginUserRequest)
        {
            try
            {
                _logger.LogInformation("LoginUser request for email: {Email}", loginUserRequest.Email);
                var result = await _userService.LoginUser(loginUserRequest);
                _logger.LogInformation("LoginUser completed successfully for email: {Email}", loginUserRequest.Email);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogWarning("LoginUser failed for email {Email}: {Message}", loginUserRequest.Email, ex.Message);
                return Unauthorized(ex.Message);
            }
        }
    }
}
