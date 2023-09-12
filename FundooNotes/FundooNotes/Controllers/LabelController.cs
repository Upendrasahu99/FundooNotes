using BusinessLayer.Interface;
using CommonLayer.Model;
using Experimental.System.Messaging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FundooNotes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LabelController : ControllerBase
    {
        private readonly ILabelBusiness labelBusiness;
        
        public LabelController(ILabelBusiness labelBusiness)
        {
            this.labelBusiness = labelBusiness;
        }

        [Authorize]
        [HttpPost]
        [Route ("Create/{NoteId}")]
        public IActionResult CreateLabel(CreateLabelModel model, int? noteId)
        {
            try
            {
                long userId = long.Parse(User.FindFirst("UserId").Value);
                var result = labelBusiness.CreateLabel(model, userId, noteId);

                if (result != null)
                {
                    return Ok(new { success = true, Message = "Label Created", data = result });
                }
                else
                {
                    return BadRequest(new { success = false, Message = "Label not created" });
                }
            }
            catch (System.Exception)
            {

                throw;
            }
        }

        [Authorize]
        [HttpPut]
        [Route ("Update/{LabelName}/{NewLabelName}")]
        public IActionResult UpdateLabel(string labelName, string newLabelName)
        {
            try
            {
                long userId = long.Parse (User.FindFirst("UserId").Value);
                var result = labelBusiness.UpdateLabel(labelName, newLabelName, userId);

                if(result != null)
                {
                    return Ok(new { success = true, Message = "Label name changed", data = result });
                }
                else
                {
                    return BadRequest(new { success = false, Message = "Label name not Changed" });
                }
            }
            catch (System.Exception)
            {

                throw;
            }
        }

        [Authorize]
        [HttpGet]
        [Route ("GetAll")]
        public IActionResult GetAllLabel()
        {
            try
            {
                long userId = long.Parse(User.FindFirst("UserId").Value);
                var result = labelBusiness.GetAllLabel(userId);
                if (result != null)
                {
                    return Ok(new { success = true, Message = "All Labels of User", data = result });
                }
                else
                {
                    return BadRequest(new { success = false, Message = "No Label found" });
                }
            }
            catch (System.Exception)
            {

                throw;
            }
        }
      
        [Authorize]
        [HttpDelete]
        [Route ("Delete/{LabelName}")]  
        public IActionResult DeleteLabel(string labelName)
        {
            try
            {
                long userId = long.Parse(User.FindFirst("UserId").Value);
                var result = labelBusiness.DeleteLabel(labelName, userId);
                if (result != null)
                {
                    return Ok(new { success = true, Message = "Label Deleted", data = result });
                }
                else
                {
                    return BadRequest(new { success = false, Message = "Label not delete0" });
                }
            }
            catch (System.Exception)
            {

                throw;
            }
        }

        // Add Label in 
        [Authorize]
        [HttpPut]
        [Route ("Add/{LabelName}/{NoteId}")]
        public IActionResult AddLabel(string labelName, int noteId)
        {
            try
            {
                long userId = long.Parse(User.FindFirst("UserId").Value);
                var result = labelBusiness.AddLabel(labelName, userId, noteId);
                if(result != null)
                {
                    return Ok(new { success = true, Message = "Label Added", data = result });
                }
                else
                {
                    return BadRequest(new { success = false, Message = "Label Not Add" });
                }
            }
            catch (System.Exception)
            {

                throw;
            }
        }
    }
}
