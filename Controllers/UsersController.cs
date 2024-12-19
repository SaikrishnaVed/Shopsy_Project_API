using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shopsy_Project.Interfaces;
using Shopsy_Project.Models;

namespace Shopsy_Project.Controllers
{
    [Route("api/[controller]")]
    //[System.Web.Http.Authorize]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;
        private readonly IBL_User _blUser;

        public UsersController(IBL_User blUser, ILogger<UsersController> logger)
        {
            _blUser = blUser;
            _logger = logger;
        }

        // Get list of all users
        [HttpGet("UserList")]
        [Authorize(Policy = "AdminOnly")]
        public IActionResult GetUserList()
        {
            List<Users> userlist = new List<Users>();
            try
            {
                userlist = _blUser.UserList();
                return Ok(userlist);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "LogError: An error occurred while processing the request.");
                return BadRequest(ex.Message);
            }
        }

        // Get a single user by ID
        [HttpGet("User/{id}")]
        [Authorize(Policy = "AdminOnly")]
        public IActionResult GetUserById(int id)
        {
            Users user = new Users();
            try
            {
                user = _blUser.GetUserById(id);
                if (user == null)
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "LogError: An error occurred while processing the request.");
            }
            return Ok(user);
        }

        // Create a new user
        [HttpPost("CreateUser")]
        [Authorize(Policy = "AdminOnly")]
        public IActionResult CreateUser([FromBody] Users user)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid user details..");
                }
                if (user == null)
                {
                    return BadRequest();
                }

                _blUser.CreateUser(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "LogError: An error occurred at {{actoo}}.");
            }
            return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
        }

        // Update an existing user
        [HttpPut("UpdateUser/{id}")]
        [Authorize(Policy = "AdminOnly")]
        public IActionResult UpdateUser(int id, [FromBody] Users user)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid user details..");
                }
                if (user == null || id != user.Id)
                {
                    return BadRequest();
                }

                var existingUser = _blUser.GetUserById(id);
                if (existingUser == null)
                {
                    return NotFound();
                }

                _blUser.UpdateUser(id, user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "LogError: An error occurred while processing the request.");
            }

            return NoContent();
        }

        // Delete a user by ID
        [HttpDelete("DeleteUser/{id}")]
        [Authorize(Policy = "AdminOnly")]
        public IActionResult DeleteUser(int id)
        {
            try
            {
                var existingUser = _blUser.GetUserById(id);
                if (existingUser == null)
                {
                    return NotFound();
                }

                _blUser.DeleteUser(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "LogError: An error occurred while processing the request.");
            }
            return NoContent();
        }
    }
}
