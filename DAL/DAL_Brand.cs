using NHibernate;
using Shopsy_Project.Interfaces;
using Shopsy_Project.Models;

namespace Shopsy_Project.DAL
{
    public class DAL_Brand : IDAL_Brand
    {
        private readonly ISessionFactory _sessionFactory;
        private readonly HttpContextAccessor _httpContextAccessor;

        public DAL_Brand()
        {
            var config = ConfigurationManager.SetConfiguration();
            _sessionFactory = config.BuildSessionFactory();
        }

        //Get All Brands
        public List<Brands> GetAllBrands()
        {
            using (var session = _sessionFactory.OpenSession())
            {
                return session.Query<Brands>().ToList();
            }
        }

        public void AddNewBrand(Brands brand) 
        {
            using (var session = _sessionFactory.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    try
                    {
                        //check whether category exists or not before add.
                        var isBrandExists = session.Query<Brands>().Where(x => x.brand_Name == brand.brand_Name)?.FirstOrDefault() != null ? true : false;
                        if (!isBrandExists)
                        {
                            session.Save(brand);
                            transaction.Commit();
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception("An error occurred while adding the brand.", ex);
                    }
                }
            }
        }
    }
}
