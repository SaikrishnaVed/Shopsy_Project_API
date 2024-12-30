using Microsoft.AspNetCore.Mvc;
using Shopsy_Project.Interfaces;
using Shopsy_Project.Models;

namespace Shopsy_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WishItemController : ControllerBase
    {
        private readonly ILogger<WishItemController> _logger;
        private readonly IBL_WishItem _blwishItems;

        public WishItemController(IBL_WishItem blwishItems, ILogger<WishItemController> logger)
        {
            _blwishItems = blwishItems;
            _logger = logger;
        }

        // Get list of all users
        [HttpGet("WishItems")]
        public IActionResult GetWishItemList()
        {
            List<WishItem> wishItems = new List<WishItem>();
            try
            {
                wishItems = _blwishItems.WishItemList();
                return Ok(wishItems);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "LogError: An error occurred while processing the request.");
                return BadRequest(ex.Message);
            }
        }

        // Create a new user
        [HttpPost("AddWishItem")]
        public IActionResult CreatewishItem([FromBody] WishItem wishItem)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid user details..");
                }
                if (wishItem == null)
                {
                    return BadRequest("Invalid request to add.");
                }

                _blwishItems.AddWishItem(wishItem);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "LogError: An error occurred at {{actoo}}.");
            }
            return Ok(1);
        }

        // Delete a user by ID
        [HttpDelete("DeleteWishItems/{id}")]
        public IActionResult DeleteUser(int id)
        {
            try
            {
                _blwishItems.DeleteWishItem(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "LogError: An error occurred while processing the request.");
            }
            return NoContent();
        }

    }
}
