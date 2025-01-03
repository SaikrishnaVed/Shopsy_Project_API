using Shopsy_Project.Interfaces;
using Shopsy_Project.Models;

namespace Shopsy_Project.BL
{
    public class BL_Coupons : IBL_Coupons
    {
        private readonly IDAL_Coupons _dAL_Coupons;

        public BL_Coupons(IDAL_Coupons dAL_Coupons)
        {
            _dAL_Coupons = dAL_Coupons;
        }

        public void AddCouponUsage(CouponUsage couponUsage)
        {
            _dAL_Coupons.AddCouponUsage(couponUsage);
        }

        public List<Coupons> GetCoupons()
        {
            return _dAL_Coupons.GetCoupons();
        }
    }
}