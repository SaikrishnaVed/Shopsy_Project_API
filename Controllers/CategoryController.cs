using Microsoft.AspNetCore.Mvc;
using Shopsy_Project.Interfaces;
using Shopsy_Project.Models;

namespace Shopsy_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [System.Web.Http.Authorize]
    public class CategoryController : ControllerBase
    {
        private readonly IBL_Category _BL_Category;

        public CategoryController(IBL_Category BL_Category)
        {
            _BL_Category = BL_Category;
        }

        //For dropdown.
        [HttpGet("GetAllCategories")]
        public IActionResult GetAllCategories()
        {
            var brands = _BL_Category.GetAllCategories();

            return Ok(brands);
        }

        //for adding new category in the dropdown.
        [HttpPost("AddCategory")]
        public IActionResult AddNewCategory(Categories category)
        {
            if (ModelState.IsValid)
            {
                _BL_Category.AddNewCategory(category);
            }
            else
            {
                return BadRequest(ModelState);
            }
            return Ok(category);
        }
    }
}