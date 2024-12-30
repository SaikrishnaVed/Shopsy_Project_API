using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shopsy_Project.BL;
using Shopsy_Project.Interfaces;
using Shopsy_Project.Models;

namespace Shopsy_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [System.Web.Http.Authorize]
    public class FeedbackController : ControllerBase
    {
        private readonly IBL_Feedback _bL_Feedback;

        public FeedbackController(IBL_Feedback bL_Feedback)
        {
            _bL_Feedback = bL_Feedback;
        }

        [HttpGet("GetAllFeedbacks/{product_Id}")]
        public IActionResult GetAllFeedbacks(int product_Id)
        {
            var feedbacks = _bL_Feedback.GetAllFeedbacks(product_Id);

            return Ok(feedbacks);
        }

        //For adding new Feedback in the dropdown.
        [HttpPost("AddEditFeedback")]
        public IActionResult AddEditFeedback(Feedback feedback)
        {
            if (ModelState.IsValid)
            {
                _bL_Feedback.AddOrUpdateFeedback(feedback);
            }
            else
            {
                return BadRequest(ModelState);
            }
            return Ok(feedback);
        }
    }
}