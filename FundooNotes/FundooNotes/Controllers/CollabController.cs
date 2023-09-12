using BusinessLayer.Interface;
using CommonLayer.Model;
using Experimental.System.Messaging;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FundooNotes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CollabController : ControllerBase
    {
        private readonly ICollabBusiness collabBusiness;
        private readonly IBus bus;

        public CollabController(ICollabBusiness collabBusiness, IBus bus)
        {
            this.collabBusiness = collabBusiness;
            this.bus = bus;
        }

        [Authorize]
        [HttpPost]
        [Route("Create/{NoteId}")]
        public IActionResult CreateCollab(CreateCollabModel model, int noteId)
        {
            try
            {
                long userId = long.Parse(User.FindFirst("UserId").Value);
                string email = User.FindFirst(ClaimTypes.Email).Value.ToString();

                var result = collabBusiness.CreateCollab(model, noteId, userId, email);

                if (result != null)
                {
                    return Ok(new { success = true, Message = "CollabCreated", result = result });
                }
                else
                {
                    return BadRequest(new { success = false, Message = "CollabNotCreated" });
                }
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        [Authorize]
        [HttpPost]
        [Route("Create(mail)/{NoteId}")]
        public async Task<IActionResult> CreateCollabSendMail(CreateCollabModel model, int noteId)
        {
            try
            {
                long userId = long.Parse(User.FindFirst("UserId").Value);
                string email = User.FindFirst(ClaimTypes.Email).Value.ToString();

                var result = collabBusiness.CreateCollab(model, noteId, userId, email);

                if (result != null)
                {
                    Uri uri = new Uri("rabbitmq://localhost/ticketQueue");
                    var endpoint = await bus.GetSendEndpoint(uri); //ticketQueue
                    await endpoint.Send(model);// Send CreateCollabModel to endpoint of RabbitMq
                    //var data = model.Email; //
                    return Ok(new { success = true, Message = "Mail send to RabbitMQ", data = model});
                }
                else
                {
                    return BadRequest(new { success = false, Message = "Mail Not sent to RabbitMQ"});
                }
            }
            catch (System.Exception)
            {

                throw;
            }
        }

        [Authorize]
        [HttpDelete]
        [Route("Delete/{NoteId}")]
        public IActionResult DeleteCollab(DeleteCollabModel model, int noteId)
        {
            try
            {
                long userId = long.Parse(User.FindFirst("UserId").Value);
                var result = collabBusiness.DeleteCollab(model, userId, noteId);

                if (result != null)
                {
                    return Ok(new { success = true, Message = "Collaborator Deleted", result = result });
                }
                else
                {
                    return BadRequest(new { successs = false, Message = "Collaborator Not Deleted" });
                }
            }
            catch (System.Exception)
            {

                throw;
            }
        }

        [Authorize]
        [HttpGet]
        [Route("GetAll/{NoteId}")]
        public IActionResult GetAllCollabOfNote(int noteId)
        {
            try
            {
                long userId = long.Parse(User.FindFirst("UserId").Value);
                var result = collabBusiness.GetAllCollabOfNote(userId, noteId);
                if (result != null)
                {
                    return Ok(new { success = true, Message = "All Collab for Note", result = result });
                }
                else
                {
                    return BadRequest(new { success = false, Message = "Collab not found" });
                }
            }
            catch (System.Exception)
            {

                throw;
            }
        }


    }
}
