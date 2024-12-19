using NHibernate;
using Shopsy_Project.Interfaces;
using Shopsy_Project.Models;
using System.Text;
using System.Security.Cryptography;

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
    }
}