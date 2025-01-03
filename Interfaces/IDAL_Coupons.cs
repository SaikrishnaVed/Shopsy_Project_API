using Shopsy_Project.Models;

namespace Shopsy_Project.Interfaces
{
    public interface IDAL_Coupons
    {
        List<Coupons> GetCoupons();
        void AddCouponUsage(CouponUsage couponUsage);
    }
}