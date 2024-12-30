using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shopsy_Project.Interfaces;
using Shopsy_Project.Models;

namespace Shopsy_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IBL_Auth _blAuth;
        private readonly JwtSettings _jwtSettings;

        public AuthController(IBL_Auth blAuth, JwtSettings jwtSettings)
        {
            _blAuth = blAuth;
            _jwtSettings = jwtSettings;
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
            return Ok(new { Token = token, Role = role?.ToLower(), UserId = userId, Username = userName });
        }

        [HttpPost("RefreshToken")]
        public IActionResult RefreshToken([FromBody] RefreshTokenRequest request)
        {
            var newAccessToken = _blAuth.RefreshAccessToken(request.RefreshToken != null ? request.RefreshToken : string.Empty);
            return Ok(new { Token = newAccessToken });
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