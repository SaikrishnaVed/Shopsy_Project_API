using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shopsy_Project.Interfaces;
using Shopsy_Project.Models;
using Shopsy_Project.Models.RequestModels;

namespace Shopsy_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ILogger<CartController> _logger;
        private readonly IBL_Cart _blCart;

        public CartController(IBL_Cart blCart, ILogger<CartController> logger)
        {
            _blCart = blCart;
            _logger = logger;
        }

        // Get list of all cart items.
        [HttpGet("CartList")]
        public IActionResult GetUserList()
        {
            List<Cart> cartlist = new List<Cart>();
            try
            {
                cartlist = _blCart.GetAllCartItems();
                return Ok(cartlist);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "LogError: An error occurred while processing the request.");
                return BadRequest(ex.Message);
            }
        }

        // Create a new cart
        [HttpPost("addCartItem")]
        public IActionResult CreateCart([FromBody] AddCartRequest cart)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid cart details..");
                }
                if (cart == null)
                {
                    return BadRequest();
                }

                _blCart.AddCartItems(cart);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "LogError: An error occurred at {{actoo}}.");
            }
            return Ok("Item added successfully.");
        }

        // Update an existing cart
        [HttpPut("UpdateCart/{id}")]
        public IActionResult UpdateCart(int id, [FromBody] Cart cart)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid user details..");
                }
                if (cart == null || id != cart.Cart_Id)
                {
                    return BadRequest();
                }

                var existingCart = _blCart.GetCartById(id);
                if (existingCart == null)
                {
                    return NotFound();
                }

                _blCart.UpdateCartItems(cart);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "LogError: An error occurred while processing the request.");
            }

            return NoContent();
        }

        // Delete a cartItem by ID
        [HttpDelete("DeleteCart/{id}")]
        public IActionResult DeleteUser(int id)
        {
            try
            {
                var existingCart = _blCart.GetCartById(id);
                if (existingCart == null)
                {
                    return NotFound();
                }

                _blCart.DeleteCartItems(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "LogError: An error occurred while processing the request.");
            }
            return NoContent();
        }
    }
}
