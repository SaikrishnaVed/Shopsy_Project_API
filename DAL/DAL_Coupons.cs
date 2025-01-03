using NHibernate;
using Shopsy_Project.Interfaces;
using Shopsy_Project.Models;

namespace Shopsy_Project.DAL
{
    public class DAL_Coupons : IDAL_Coupons
    {
        private readonly ISessionFactory _sessionFactory;
        //private readonly HttpContextAccessor _httpContextAccessor;

        public DAL_Coupons()
        {
            var config = ConfigurationManager.SetConfiguration();
            _sessionFactory = config.BuildSessionFactory();
        }

        public void AddCouponUsage(CouponUsage couponUsage)
        {
            using (var session = _sessionFactory.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    try
                    {
                        //check whether coupon exists or not before adding to coupon usage table.
                        var isCouponExists = session.Query<Coupons>().Where(x => x.Id == couponUsage.Coupon_Id)?.FirstOrDefault() != null ? true : false;
                        if (isCouponExists)
                        {
                            session.Save(couponUsage);
                            transaction.Commit();
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception("An error occurred while adding the coupon.", ex);
                    }
                }
            }
        }

        public List<Coupons> GetCoupons()
        {
            using (var session = _sessionFactory.OpenSession())
            {
                return session.Query<Coupons>().ToList();
            }
        }
    }
}