using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shopsy_Project.Interfaces;
using Shopsy_Project.Models;

namespace Shopsy_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestUserController : ControllerBase
    {
        private readonly ILogger<TestUserController> _logger;
        private readonly IBL_TestUser _blUser;

        public TestUserController(IBL_TestUser blUser, ILogger<TestUserController> logger)
        {
            _blUser = blUser;
            _logger = logger;
        }

        // Get list of all users
        [HttpGet("UserList")]
        public IActionResult GetUserList()
        {
            List<TestUser> userlist = new List<TestUser>();
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
        public IActionResult GetUserById(int id)
        {
            TestUser user = new TestUser();
            try
            {
                user = _blUser.GetUserById(id);
                if (user == null)
                {
                    return NotFound();
                }
            }
            catch (Exception ex) {
                _logger.LogError(ex, "LogError: An error occurred while processing the request.");
            }
                return Ok(user);
        }

        // Create a new user
        [HttpPost("CreateUser")]
        public IActionResult CreateUser([FromBody] TestUser user)
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
            return CreatedAtAction(nameof(GetUserById), new { id = user.User_Id }, user);
        }

        // Update an existing user
        [HttpPut("UpdateUser/{id}")]
        public IActionResult UpdateUser(int id, [FromBody] TestUser user)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid user details..");
                }
                if (user == null || id != user.User_Id)
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
            catch (Exception ex) {
               _logger.LogError(ex, "LogError: An error occurred while processing the request.");
            }

            return NoContent();
        }

        // Delete a user by ID
        [HttpDelete("DeleteUser/{id}")]
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
