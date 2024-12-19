using Shopsy_Project.Models;

namespace Shopsy_Project.Interfaces
{
    public interface IDAL_TestUser
    {
        public List<TestUser> UserList();
        TestUser GetUserById(int id);
        void AddUser(TestUser user);
        void UpdateUser(int id, TestUser user);
        void DeleteUser(int id);
    }
}
