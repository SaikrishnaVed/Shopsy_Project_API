using Shopsy_Project.DAL;
using Shopsy_Project.Interfaces;
using Shopsy_Project.Models;
using System.Collections.Generic;

namespace Shopsy_Project.BL
{
    public class BL_User : IBL_User
    {
        private readonly IDAL_User _dalUsers;

        public BL_User(IDAL_User dalUsers)
        {
            _dalUsers = dalUsers;
        }

        public List<Users> UserList()
        {
            return _dalUsers.UserList();
        }

        public Users GetUserById(int id)
        {
            return _dalUsers.GetUserById(id);
        }

        public void CreateUser(Users user)
        {
            _dalUsers.AddUser(user);
        }

        public void UpdateUser(int id, Users user)
        {
            _dalUsers.UpdateUser(id, user);
        }

        public void DeleteUser(int id)
        {
            _dalUsers.DeleteUser(id);
        }
    }
}
