using Shopsy_Project.Models;

namespace Shopsy_Project.Interfaces
{
    public interface IDAL_Auth
    {
        void Register(AuthUsers user);
        AuthUsers Authenticate(string username, string password);
        void SaveToken(AuthUserTokens token);
        bool IsTokenValid(string token);
        AuthUserTokens ValidateRefreshToken(string refreshToken);
    }
}