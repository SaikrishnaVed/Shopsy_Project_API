using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shopsy_Project.Common;
using Shopsy_Project.Interfaces;
using Shopsy_Project.Models;

namespace Shopsy_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductController : ControllerBase
    {
        private readonly IBL_Product _blProduct;

        public ProductController(IBL_Product blProduct)
        {
            _blProduct = blProduct;
        }

        [HttpPost("GetAll/{userid}")]
        public IActionResult GetAllProducts([FromBody] PaginationFilter filter, int userId)
        {
            if (filter.PageNumber <= 0) filter.PageNumber = 1;
            if (filter.PageSize <= 0) filter.PageSize = 5;

            var pagedProducts = _blProduct.GetAllProducts(filter, userId);

            return Ok(pagedProducts);
        }

        [HttpGet("GetById/{id}/{userId}")]
        public IActionResult GetProductById(int id, int userId)
        {
            var product = _blProduct.GetProductById(id, userId);
            if (product == null)
            {
                return NotFound("Product not found.");
            }
            return Ok(product);
        }
        
        [HttpPost("Add")]
        public IActionResult AddProduct([FromBody] Products product)
        {
            if (ModelState.IsValid)
            {
                _blProduct.AddProduct(product);
                return Ok();
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpPost("Update")]
        //[Authorize(Policy = "AdminOnly")]
        public IActionResult UpdateProduct([FromBody] Products product)
        {
            if (ModelState.IsValid)
            {
                _blProduct.UpdateProduct(product);
                return Ok();
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpDelete("Delete/{id}")]
        //[Authorize(Policy = "AdminOnly")]
        public IActionResult DeleteProduct(int id)
        {
            _blProduct.DeleteProduct(id);
            return Ok();
        }

        //For dropdown.
        [HttpGet("GetAllCategories")]
        //[Authorize(Policy = "AdminOnly")]
        public IActionResult GetAllCategories()
        {
            var categories = _blProduct.GetAllCategories();

            return Ok(categories);
        }

        //For dropdown.
        [HttpGet("GetAllBrands")]
        //[Authorize(Policy = "AdminOnly")]
        public IActionResult GetAllBrands()
        {
            var brands = _blProduct.GetAllBrands();

            return Ok(brands);
        }
    }
}