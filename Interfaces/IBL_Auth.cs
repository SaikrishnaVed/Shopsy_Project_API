using Shopsy_Project.Models;

namespace Shopsy_Project.Interfaces
{
    public interface IBL_Auth
    {
        void Register(AuthUsers user);
        (int,string, string?) Login(string username, string password);
        string RefreshAccessToken(string refreshToken);
    }
}