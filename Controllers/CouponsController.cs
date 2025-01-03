using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shopsy_Project.Interfaces;
using Shopsy_Project.Models;

namespace Shopsy_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [System.Web.Http.Authorize]
    public class CouponsController : ControllerBase
    {
        private readonly IBL_Coupons _bL_Coupons;

        public CouponsController(IBL_Coupons bL_Coupons)
        {
            _bL_Coupons = bL_Coupons;
        }

        [HttpGet("GetCoupons")]
        public IActionResult GetCoupons()
        {
            var coupons = _bL_Coupons.GetCoupons();
            return Ok(coupons);
        }

        [HttpPost("AddToCouponUsage")]
        public IActionResult AddToCouponUsage(CouponUsage couponUsage)
        {
            _bL_Coupons.AddCouponUsage(couponUsage);
            return Ok();
        }
    }
}