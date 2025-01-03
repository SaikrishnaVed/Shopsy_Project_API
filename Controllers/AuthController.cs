using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shopsy_Project.Helpers;
using Shopsy_Project.Interfaces;
using Shopsy_Project.Models;
using Shopsy_Project.Models.RequestModels;

namespace Shopsy_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IBL_Auth _blAuth;
        private readonly JwtSettings _jwtSettings;
        private readonly EmailSender _emailSender;

        public AuthController(IBL_Auth blAuth, JwtSettings jwtSettings, EmailSender emailSender)
        {
            _blAuth = blAuth;
            _jwtSettings = jwtSettings;
            _emailSender = emailSender;
        }

        [HttpPost("Register")]
        public IActionResult Register([FromBody] AuthUsers user)
        {
            _blAuth.Register(user);
            return Ok(new { message = "Registration successful" });
        }

        [HttpPost("Login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            var (userId,token, role) = _blAuth.Login(!string.IsNullOrEmpty(request.UserName) ? request.UserName : string.Empty, !string.IsNullOrEmpty(request.Password) ? request.Password : string.Empty);
            string userName = request.UserName != null ? request.UserName : string.Empty;

            var message = new Message(new string[] { "saikrishnavedagiri567@mailinator.com" }, "Test email", "Hi, saikrishna here. I just got logged in.");
            _emailSender.SendEmail(message);

            return Ok(new { Token = token, Role = role?.ToLower(), UserId = userId, Username = userName });
        }

        [HttpPost("RefreshToken")]
        public IActionResult RefreshToken([FromBody] RefreshTokenRequest request)
        {
            var newAccessToken = _blAuth.RefreshAccessToken(request.RefreshToken != null ? request.RefreshToken : string.Empty);
            return Ok(new { Token = newAccessToken });
        }

        // Get list of all users
        [HttpGet("AuthUserList")]
        //[Authorize(Policy = "AdminOnly")]
        public IActionResult GetUserList()
        {
            List<UserRequest> userlist = new List<UserRequest>();
            try
            {
                userlist = _blAuth.GetAuthUsers();
                return Ok(userlist);
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "LogError: An error occurred while processing the request.");
                return BadRequest(ex.Message);
            }
        }

        // Update role of user.
        [HttpPost("UpdateUserRole")]
        //[Authorize(Policy = "AdminOnly")]
        public IActionResult UpdateUserRole(UpdateUserRequest updateUserRequest)
        {
            try
            {
                _blAuth.UpdateUserRole(updateUserRequest);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Update profile of user.
        [HttpPost("UpdateUserProfile")]
        public IActionResult UpdateUserProfile(UserProfile userProfile)
        {
            try
            {
                _blAuth.UpdateUserProfile(userProfile);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Update profile of user.
        [HttpGet("GetAuthUserById/{userId}")]
        public IActionResult GetAuthUserById(int userId)
        {
            try
            {
                var user = _blAuth.GetAuthUserById(userId);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

public class LoginRequest
{
    public string? UserName { get; set; }
    public string? Password { get; set; }
}

public class RefreshTokenRequest
{
    public string? RefreshToken { get; set; }
}