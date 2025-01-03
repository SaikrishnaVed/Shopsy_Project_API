using NHibernate;
using Shopsy_Project.Interfaces;
using Shopsy_Project.Models;
using System.Text;
using System.Security.Cryptography;
using Shopsy_Project.Models.RequestModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace Shopsy_Project.DAL
{
    public class DAL_Auth: IDAL_Auth
    {
        private readonly ISessionFactory _sessionFactory;

        public DAL_Auth()
        {
            var config = ConfigurationManager.SetConfiguration();
            _sessionFactory = config.BuildSessionFactory();
        }

        public void Register(AuthUsers user)
        {
            using (var session = _sessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                if (user.PasswordHash != null)
                {
                    user.PasswordHash = HashPassword(user.PasswordHash);
                    session.Save(user);
                    transaction.Commit();
                }
            }
        }

        public AuthUsers Authenticate(string username, string password)
        {
            using (var session = _sessionFactory.OpenSession())
            {
                try
                {
                    var user = session.Query<AuthUsers>().FirstOrDefault(u => u.UserName == username);
                    if (user != null && user.PasswordHash != null && VerifyPassword(user.PasswordHash, password))
                    {
                        return user;
                    }
                    else
                    {
                        throw new UnauthorizedAccessException("Invalid username or password");
                    }
                }
                catch (Exception ex)
                {
                    throw new UnauthorizedAccessException("Invalid username or password", ex);
                }
            }
        }

        //Added refresh token when token got expires, automatically calls token method.
        public void SaveToken(AuthUserTokens token)
        {
            using (var session = _sessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                // Generate a unique refresh token
                token.RefreshToken = Guid.NewGuid().ToString();
                token.RefreshTokenExpiration = DateTime.UtcNow.AddDays(7); // Set refresh token validity (e.g., 7 days)

                token.UserId = 1;//Hardcode as of now

                session.Save(token);
                transaction.Commit();
            }
        }

        public List<UserRequest> GetAuthUsers()
        {
            List<AuthUsers> userList = new List<AuthUsers>();
            List<UserRequest> users = new List<UserRequest>();
            using (var session = _sessionFactory.OpenSession())
            using (var tx = session.BeginTransaction())
            {
                try
                {
                    userList = session.Query<AuthUsers>().ToList();
                    foreach (var item in userList)
                    {
                        UserRequest user = new UserRequest()
                        {
                            Id = item.Id,
                            UserName = item.UserName,
                            Email = item.Email,
                            Role = item.Role,
                            DateOfBirth = item.DateOfBirth,
                            Gender = item.Gender,
                            isActive = item.isActive,
                            Phone = item.Phone,
                        };
                        users.Add(user);
                    }
                    return users;
                }
                catch
                {
                    tx.Rollback();
                    return new List<UserRequest>();
                }
            }
        }

        public AuthUserTokens ValidateRefreshToken(string refreshToken)
        {
            using (var session = _sessionFactory.OpenSession())
            {
                var userToken = session.Query<AuthUserTokens>()
                    .FirstOrDefault(t => t.RefreshToken == refreshToken && t.RefreshTokenExpiration > DateTime.UtcNow);

                return userToken != null ? userToken : new AuthUserTokens();
            }
        }


        public bool IsTokenValid(string token)
        {
            using (var session = _sessionFactory.OpenSession())
            {
                var userToken = session.Query<AuthUserTokens>().FirstOrDefault(t => t.Token == token && t.Expiration > DateTime.UtcNow);
                return userToken != null;
            }
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(bytes);
            }
        }

        private bool VerifyPassword(string hashedPassword, string plainPassword)
        {
            return HashPassword(plainPassword) == hashedPassword;
        }

        public void UpdateUserRole(UpdateUserRequest updateUserRequest)
        {
            using (var session = _sessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                AuthUsers user = session.Query<AuthUsers>().FirstOrDefault(x=>x.Id == updateUserRequest.userId);
                if (user != null) {
                    user.Role = updateUserRequest.role;
                    user.isActive = updateUserRequest.isActive;
                    session.Save(user);
                    transaction.Commit();
                }
            }
        }

        public void UpdateUserProfile(UserProfile userProfile)
        {
            using (var session = _sessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                AuthUsers user = session.Query<AuthUsers>().FirstOrDefault(x => x.Id == userProfile.Id);
                if (user != null)
                {
                    user.UserName = userProfile.UserName;
                    user.DateOfBirth = userProfile.DateOfBirth;
                    user.Gender = userProfile.Gender;
                    user.Phone = userProfile.Phone;
                    user.Email = userProfile.Email;
                    
                    session.Save(user);
                    transaction.Commit();
                }
            }
        }

        public AuthUsers GetAuthUserById(int userId) 
        {
            using (var session = _sessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                AuthUsers user = session.Query<AuthUsers>().FirstOrDefault(x => x.Id == userId);
                return user;
            }
        }
    }
}