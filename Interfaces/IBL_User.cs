using Shopsy_Project.Models;
using Shopsy_Project.Models.RequestModels;

namespace Shopsy_Project.Interfaces
{
    public interface IBL_User
    {
        public List<Users> UserList();
        Users GetUserById(int id);
        void CreateUser(Users user);
        void UpdateUser(int id, Users user);
        void DeleteUser(int id);
    }
}
