using Shopsy_Project.Models;
using Shopsy_Project.Models.RequestModels;

namespace Shopsy_Project.Interfaces
{
    public interface IBL_Auth
    {
        void Register(AuthUsers user);
        (int,string, string?) Login(string username, string password);
        string RefreshAccessToken(string refreshToken);
        List<UserRequest> GetAuthUsers();
        void UpdateUserRole(UpdateUserRequest updateUserRequest);
        void UpdateUserProfile(UserProfile userProfile);
        AuthUsers GetAuthUserById(int userId);
    }
}