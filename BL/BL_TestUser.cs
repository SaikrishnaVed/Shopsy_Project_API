using Shopsy_Project.DAL;
using Shopsy_Project.Interfaces;
using Shopsy_Project.Models;
using System.Collections.Generic;

namespace Shopsy_Project.BL
{
    public class BL_TestUser : IBL_TestUser
    {
        private readonly IDAL_TestUser _dalTestUser;

        public BL_TestUser(IDAL_TestUser dalTestUser)
        {
            _dalTestUser = dalTestUser;
        }

        public List<TestUser> UserList()
        {
            return _dalTestUser.UserList();
        }

        public TestUser GetUserById(int id)
        {
            return _dalTestUser.GetUserById(id);
        }

        public void CreateUser(TestUser user)
        {
            _dalTestUser.AddUser(user);
        }

        public void UpdateUser(int id, TestUser user)
        {
            _dalTestUser.UpdateUser(id, user);
        }

        public void DeleteUser(int id)
        {
            _dalTestUser.DeleteUser(id);
        }
    }
}
