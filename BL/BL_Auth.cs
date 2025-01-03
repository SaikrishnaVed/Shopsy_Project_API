using Microsoft.IdentityModel.Tokens;
using Shopsy_Project.Interfaces;
using Shopsy_Project.Models;
using Shopsy_Project.Models.RequestModels;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Shopsy_Project.BL
{
    public class BL_Auth : IBL_Auth
    {
        private readonly IDAL_Auth _dalAuth;
        private readonly JwtSettings _jwtSettings;

        public BL_Auth(IDAL_Auth dalAuth, JwtSettings jwtSettings)
        {
            _dalAuth = dalAuth;
            _jwtSettings = jwtSettings;
        }

        public void Register(AuthUsers user)
        {
            _dalAuth.Register(user);
        }

        public (int,string,string?) Login(string username, string password)
        {
            var user = _dalAuth.Authenticate(username, password);
            if (user == null) throw new UnauthorizedAccessException("Invalid credentials.");

            // Add role-based claims
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email != null ? user.Email : string.Empty),
                new Claim(ClaimTypes.Role, user.Role != null ? user.Role : string.Empty),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_jwtSettings.Secret != null ? _jwtSettings.Secret : string.Empty);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(authClaims),
                Expires = DateTime.UtcNow.AddHours(1),
                Issuer = _jwtSettings.ValidIssuer,
                Audience = _jwtSettings.ValidAudience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return (user.Id,tokenHandler.WriteToken(token), user != null ? user.Role?.ToString() : "user");
        }

        public string RefreshAccessToken(string refreshToken)
        {
            var userToken = _dalAuth.ValidateRefreshToken(refreshToken);
            if (userToken == null)
                throw new UnauthorizedAccessException("Invalid or expired refresh token.");

            // Generate a new access token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_jwtSettings.Secret != null ? _jwtSettings.Secret : string.Empty);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                new Claim(ClaimTypes.Name, userToken.UserId.ToString()),
            }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var newAccessToken = tokenHandler.WriteToken(token);

            // Save the new token in the database
            _dalAuth.SaveToken(new AuthUserTokens
            {
                UserId = userToken.UserId,
                Token = newAccessToken,
                Expiration = DateTime.UtcNow.AddHours(1),
                RefreshToken = Guid.NewGuid().ToString(),
                RefreshTokenExpiration = DateTime.UtcNow.AddDays(7)
            });

            return newAccessToken;
        }

        public List<UserRequest> GetAuthUsers()
        {
            return _dalAuth.GetAuthUsers();
        }

        public void UpdateUserRole(UpdateUserRequest updateUserRequest)
        {
            _dalAuth.UpdateUserRole(updateUserRequest);
        }

        public void UpdateUserProfile(UserProfile userProfile)
        {
            _dalAuth.UpdateUserProfile(userProfile);
        }

        public AuthUsers GetAuthUserById(int userId)
        {
            return _dalAuth.GetAuthUserById(userId);
        }
    }
}