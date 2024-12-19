using NHibernate;
using Shopsy_Project.Interfaces;
using Shopsy_Project.Models;
using NHibernate.Linq;
using System.Web.Http.ModelBinding;

namespace Shopsy_Project.DAL
{
    public class DAL_User : IDAL_User
    {
        private ISessionFactory sessionFactory;

        public DAL_User()
        {
            var config = ConfigurationManager.SetConfiguration();
            sessionFactory = config.BuildSessionFactory();
        }

        //This method is for Admin user only.
        public List<Users> UserList()
        {
            var userList = new List<Users>();
            using (var session = sessionFactory.OpenSession())
            {
                using (var tx = session.BeginTransaction())
                {
                    try
                    {
                        userList = session.Query<Users>().ToList();
                        tx.Commit();
                    }
                    catch
                    {
                        tx.Rollback();
                    }
                }
            }
            return userList;
        }

        // Get user by ID
        public Users GetUserById(int id)
        {
            Users user = new Users();
            using (var session = sessionFactory.OpenSession())
            {
                using (var tx = session.BeginTransaction())
                {
                    try
                    {
                        user = session.Query<Users>().FirstOrDefault(u => u.Id == id);
                        tx.Commit();
                    }
                    catch
                    {
                        tx.Rollback();
                    }
                }
            }
            return user ?? new Users();
        }

        // Add new user
        public void AddUser(Users user)
        {
            using (var session = sessionFactory.OpenSession())
            {
                using (var tx = session.BeginTransaction())
                {
                    try
                    {
                        session.Save(user);
                        tx.Commit();
                    }
                    catch
                    {
                        tx.Rollback();
                    }
                }
            }
        }

        // Update existing user
        public void UpdateUser(int id, Users user)
        {
            using (var session = sessionFactory.OpenSession())
            {
                using (var tx = session.BeginTransaction())
                {
                    try
                    {
                        var existingUser = session.Get<Users>(id);
                        if (existingUser != null)
                        {
                            existingUser.Name = user.Name;
                            existingUser.Description = user.Description;
                            existingUser.DateOfBirth = user.DateOfBirth;
                            existingUser.Email = user.Email;
                            existingUser.Phone = user.Phone;
                            existingUser.Gender = user.Gender;
                            existingUser.Password = user.Password;
                            session.Update(existingUser);
                        }
                        tx.Commit();
                    }
                    catch
                    {
                        tx.Rollback();
                    }
                }
            }
        }

        // Delete user
        public void DeleteUser(int id)
        {
            using (var session = sessionFactory.OpenSession())
            {
                using (var tx = session.BeginTransaction())
                {
                    try
                    {
                        var user = session.Get<Users>(id);
                        if (user != null)
                        {
                            //session.Delete(user);
                            user.isActive = false;
                            session.Update(user);
                        }
                        tx.Commit();
                    }
                    catch
                    {
                        tx.Rollback();
                    }
                }
            }
        }
    }
}