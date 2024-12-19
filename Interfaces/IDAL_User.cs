using Shopsy_Project.Models;

namespace Shopsy_Project.Interfaces
{
    public interface IDAL_User
    {
        public List<Users> UserList();
        Users GetUserById(int id);
        void AddUser(Users user);
        void UpdateUser(int id, Users user);
        void DeleteUser(int id);
    }
}
