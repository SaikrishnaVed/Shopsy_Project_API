using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using NHibernate;
using Shopsy_Project.Models;

namespace Shopsy_Project.Controllers
{
    [System.Web.Http.Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private static ISessionFactory _sessionFactory;

        public ChatController(ISessionFactory sessionFactory) { 
            _sessionFactory = sessionFactory;
        }

        [HttpPost("SaveConversation")]
        public IActionResult SaveConversation([FromBody] Conversation conversation)
        {
            using (var session = _sessionFactory.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    try
                    {
                        session.Save(conversation);
                        transaction.Commit();
                        return Ok("Conversation saved successfully.");
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        return BadRequest("Error saving conversation: " + ex.Message);
                    }
                }
            }
        }

    }
}
