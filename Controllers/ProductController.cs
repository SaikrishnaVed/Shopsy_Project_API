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

        [HttpPost("GetAll")]
        //[Authorize(Policy = "AdminOnly")] commented it must access for all & Added for testing purpose only.
        public IActionResult GetAllProducts([FromBody] PaginationFilter filter)
        {
            if (filter.PageNumber <= 0) filter.PageNumber = 1;
            if (filter.PageSize <= 0) filter.PageSize = 10;

            var pagedProducts = _blProduct.GetAllProducts(filter);

            return Ok(pagedProducts);
        }

        [HttpGet("GetById/{id}")]
        public IActionResult GetProductById(int id)
        {
            var product = _blProduct.GetProductById(id);
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
                return Ok("Product added successfully.");
            }
            return BadRequest(ModelState);
        }

        [HttpPut("Update")]
        public IActionResult UpdateProduct([FromBody] Products product)
        {
            if (ModelState.IsValid)
            {
                _blProduct.UpdateProduct(product);
                return Ok("Product updated successfully.");
            }
            return BadRequest(ModelState);
        }

        [HttpDelete("Delete/{id}")]
        public IActionResult DeleteProduct(int id)
        {
            _blProduct.DeleteProduct(id);
            return Ok("Product deleted successfully.");
        }
    }
}