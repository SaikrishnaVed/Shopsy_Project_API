using NHibernate;
using Shopsy_Project.Interfaces;
using Shopsy_Project.Models;
using NHibernate.Linq;

namespace Shopsy_Project.DAL
{
    public class DAL_TestUser : IDAL_TestUser
    {
        private ISessionFactory sessionFactory;

        public DAL_TestUser()
        {
            var config = ConfigurationManager.SetConfiguration();
            sessionFactory = config.BuildSessionFactory();
        }

        public List<TestUser> UserList()
        {
            var userList = new List<TestUser>();
            using (var session = sessionFactory.OpenSession())
            {
                using (var tx = session.BeginTransaction())
                {
                    try
                    {
                        userList = session.Query<TestUser>().ToList();
                        tx.Commit();
                    }
                    catch { 
                        tx.Rollback();
                    }
                }
            }
            return userList;
        }

        // Get user by ID
        public TestUser GetUserById(int id)
        {
            TestUser user = new TestUser();
            using (var session = sessionFactory.OpenSession())
            {
                using (var tx = session.BeginTransaction())
                {
                    try
                    {
                        user = session.Query<TestUser>().FirstOrDefault(u => u.User_Id == id);
                        tx.Commit();
                    }
                    catch {
                        tx.Rollback();
                    }
                }
            }
            return user ?? new TestUser();
        }

        // Add new user
        public void AddUser(TestUser user)
        {
            using (var session = sessionFactory.OpenSession())
            {
                using (var tx = session.BeginTransaction())
                {
                    try
                    {
                        session.Save(user); // This automatically handles the ID assignment if configured in NHibernate
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
        public void UpdateUser(int id, TestUser user)
        {
            using (var session = sessionFactory.OpenSession())
            {
                using (var tx = session.BeginTransaction())
                {
                    try
                    {
                        var existingUser = session.Get<TestUser>(id);
                        if (existingUser != null)
                        {
                            existingUser.FirstName = user.FirstName;
                            existingUser.SecondName = user.SecondName;
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
                        var user = session.Get<TestUser>(id);
                        if (user != null)
                        {
                            session.Delete(user);
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