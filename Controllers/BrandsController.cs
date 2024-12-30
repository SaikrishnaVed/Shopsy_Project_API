using Microsoft.AspNetCore.Mvc;
using Shopsy_Project.Interfaces;
using Shopsy_Project.Models;

namespace Shopsy_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [System.Web.Http.Authorize]
    public class BrandsController : ControllerBase
    {
        private readonly IBL_Brand _IbL_Brand;

        public BrandsController(IBL_Brand IbL_Brand)
        {
            _IbL_Brand = IbL_Brand;
        }

        //For dropdown.
        [HttpGet("GetAllBrands")]
        //[Authorize(Policy = "AdminOnly")]
        public IActionResult GetAllBrands()
        {
            var brands = _IbL_Brand.GetAllBrands();

            return Ok(brands);
        }

        //For adding new brand in the dropdown.
        [HttpPost("AddBrand")]
        public IActionResult AddNewBrand(Brands brand)
        {
            if (ModelState.IsValid)
            {
                _IbL_Brand.AddNewBrand(brand);
            }
            else
            {
                return BadRequest(ModelState);
            }
            return Ok(brand);
        }
    }
}