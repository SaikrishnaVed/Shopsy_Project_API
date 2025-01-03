using Shopsy_Project.Models;

namespace Shopsy_Project.Interfaces
{
    public interface IBL_Coupons
    {
        List<Coupons> GetCoupons();
        void AddCouponUsage(CouponUsage couponUsage);
    }
}