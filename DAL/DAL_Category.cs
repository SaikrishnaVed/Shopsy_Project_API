using NHibernate;
using Shopsy_Project.Interfaces;
using Shopsy_Project.Models;

namespace Shopsy_Project.DAL
{
    public class DAL_Category : IDAL_Category
    {
        private readonly ISessionFactory _sessionFactory;
        private readonly HttpContextAccessor _httpContextAccessor;
        
        public DAL_Category()
        {
            var config = ConfigurationManager.SetConfiguration();
            _sessionFactory = config.BuildSessionFactory();
        }

        //Get All Categories
        public List<Categories> GetAllCategories()
        {
            using (var session = _sessionFactory.OpenSession())
            {
                return session.Query<Categories>().ToList();
            }
        }

        public void AddNewCategory(Categories category)
        {
            using (var session = _sessionFactory.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    try
                    {
                        //check whether category exists or not before add.
                        var isCategoryExists = session.Query<Categories>().Where(x=>x.category_Name == category.category_Name)?.FirstOrDefault() != null ? true : false;
                        if (!isCategoryExists)
                        {
                            session.Save(category);
                            transaction.Commit();
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception("An error occurred while adding the category.", ex);
                    }
                }
            }
        }
    }
}