using BusinessLayer.Interface;
using CommonLayer.Model;
using Experimental.System.Messaging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using RepoLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FundooNotes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NoteController : ControllerBase
    {
        private readonly IDistributedCache distributedCache;
        private readonly INoteBusiness noteBusiness;
        public NoteController(INoteBusiness noteBusiness, IDistributedCache distributedCache)
        {
            this.distributedCache = distributedCache;
            this.noteBusiness = noteBusiness;
        }

        //CreateNote
        [Authorize]
        [HttpPost]
        [Route("Create")]
        public IActionResult CreateNote(CreateNoteModel createNoteModel)
        {
            try
            {
                long userId = long.Parse(User.FindFirst("UserId").Value);// long.Parse convert string to long formate, Value is property of Claim object which retrurn the value of claim.

                var result = noteBusiness.CreateNote(createNoteModel, userId);

                if (result != null)
                {
                    return Ok(new { success = true, Message = "Note Created", result = result });
                }
                else
                {
                    return BadRequest(new { success = false, Message = "Note Not Created " });
                }
            }
            catch (System.Exception)
            {

                throw;
            }
        }


        //Update Notes
        [Authorize]
        [HttpPut]
        [Route("Update/{noteId}")]

        public IActionResult UpdateNote(UpdateNoteModel updateNoteModel, long noteId)
        {
            try
            {
                long userId = long.Parse(User.FindFirst("UserId").Value);
                var result = noteBusiness.UpdateNote(updateNoteModel, userId, noteId);
                if (result != null)
                {
                    return Ok(new { success = true, Message = "Note Updated", result = result });
                }
                else
                {
                    return BadRequest(new { success = false, Message = "Note Not Updated" });
                }

            }
            catch (Exception)
            {

                throw;
            }
        }

        //Get All Notes
        [Authorize]
        [HttpGet]
        [Route("Get")]
        public IActionResult GetAllNotes()
        {
            try
            {
                long userId = long.Parse(User.FindFirst("userId").Value);
                var result = noteBusiness.GetAllNotes(userId);
                if (result != null)
                {
                    return Ok(new { success = true, Message = "All Notes List", result = result });
                }
                else
                {
                    return BadRequest(new { success = false, Message = "There is no Notes in " });
                }

            }
            catch (Exception)
            {

                throw;
            }
        }

        //Delete Note
        [Authorize]
        [HttpDelete]
        [Route("Delete/{noteId}")]
        public IActionResult DeleteNote(long noteId)
        {
            try
            {
                long userId = long.Parse(User.FindFirst("userId").Value);
                var result = noteBusiness.DeletNote(noteId, userId);
                if (result != null)
                {
                    return Ok(new { success = true, Message = "Note Deleted", result = result });
                }
                else
                {
                    return BadRequest(new { success = false, Message = "Don't have note" });
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        //Change Archive
        [Authorize]
        [HttpPut]
        [Route("Archive/{noteId}")]
        public IActionResult ChangeArchive(long noteId)
            {
            try
            {
                long userId = long.Parse(User.FindFirst("userId").Value);
                var result = noteBusiness.ChangeArchive(noteId, userId);
                if (result != null)
                {
                    return Ok(new { success = true, Message = "Archive Changed", result = result });
                }
                else
                {
                    return BadRequest(new { success = false, Message = "Archive Not change" });
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        //Change Pin
        [Authorize]
        [HttpPut]
        [Route("Pin/{noteId}")]
        public IActionResult ChangePinNote(long noteId)
        {
            try
            {
                long userId = long.Parse(User.FindFirst("userId").Value);
                var result = noteBusiness.ChangePinNote(noteId, userId);
                if (result != null)
                {
                    return Ok(new { success = true, Message = "Pin Changed", result = result });
                }
                else
                {
                    return BadRequest(new { success = false, Message = "Pin not Change" });
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        //Change Trash Section
        [Authorize]
        [HttpPut]
        [Route("Trash/{noteId}")]
        public IActionResult ChangeTrashSection(long noteId)
        {
            try
            {
                long userId = long.Parse(User.FindFirst("userId").Value);
                var result = noteBusiness.ChangeTrashSection(noteId, userId);

                if (result != null)
                {
                    return Ok(new { success = true, Message = "Trash Changed", result = result });
                }
                else
                {
                    return BadRequest(new { success = false, Message = "Trash Not Change" });
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        //Change Color
        [Authorize]
        [HttpPut]
        [Route("Background/{Color}/{noteId}")]
        public IActionResult ChangeBackgroundColor(string color, long noteId)
        {
            try
            {
                long userId = long.Parse(User.FindFirst("userId").Value);
                var result = noteBusiness.ChangeBackgroundColor(color, noteId, userId);
                if (result != null)
                {
                    return Ok(new { success = true, Message = "Color Changed", result = result });
                }
                else
                {
                    return BadRequest(new { success = false, Message = "Color Not Changed" });
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        //Find Note
        [Authorize]
        [HttpGet]
        [Route("Find/{Keyword}")]
        public IActionResult FindNote(string keyword)
        {
            try
            {
                long userId = long.Parse(User.FindFirst("UserId").Value);
                var result = noteBusiness.FindNotes(keyword, userId);
                if (result != null)
                {
                    return Ok(new { success = true, Message = "Notes which contain keyword " + keyword, result = result });
                }
                else
                {
                    return BadRequest(new { success = false, Message = "Not find Notes which contain " + keyword });
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        //Upload Image
        [Authorize]
        [HttpPost]
        [Route("Image/{noteId}")]
        public IActionResult UploadImage(long noteId, IFormFile imageFile)
        {
            try
            {
                long userId = long.Parse(User.FindFirst("UserId").Value);
                var result = noteBusiness.Image(noteId, userId, imageFile);
                if (result != null)
                {
                    return Ok(new { success = true, Message = "Image Uploaded", result = result });
                }
                else
                {
                    return BadRequest(new { success = false, Message = "Image Not Uploaded" });
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        //Get All Notes Using Redis
        [Authorize]
        [HttpGet]
        [Route("Redis")]
        public async Task<IActionResult> GetAllNotesUsingRedis()
        {
            try
            {
                var cachekey = "noteList"; //cachekey is key for access the cache data
                string serializedNoteList;
                var noteList = new List<NoteEntity>(); // List of NoteEntity which will store
                var redisNoteList = await distributedCache.GetAsync(cachekey);
                //redisNoteList is new variable(container) hold whatever the cache system gives us.(data form cache memory)
                // "await" is putting the task on hold and do something else.
                // distributedCache object represents a cache system, Where object store.
                //"GetAsync" method asking the cache system to give something, based on cache key
                if (redisNoteList != null)
                {
                    serializedNoteList = Encoding.UTF8.GetString(redisNoteList); // Translate the data which is likely stored as bytes  in string format.
                    noteList = JsonConvert.DeserializeObject<List<NoteEntity>>(serializedNoteList); // Data Convert in NoteEntity onbject.
                }
                else
                {
                    long userId = long.Parse(User.FindFirst("UserId").Value);
                    noteList = noteBusiness.GetAllNotes(userId); // Getting all the notes.
                    serializedNoteList = JsonConvert.SerializeObject(noteList); // It's converting this list into a format that cna be stored(serialized) as text.
                    redisNoteList = Encoding.UTF8.GetBytes(serializedNoteList);// Now the List is in text format now changed into binary format.
                    var options = new DistributedCacheEntryOptions().SetAbsoluteExpiration(DateTime.Now.AddMinutes(10)).SetSlidingExpiration(TimeSpan.FromMinutes(2)); // Set some option data should be stored for 10 minute and should expire after 2 minutes of inacivity.
                    await distributedCache.SetAsync(cachekey, redisNoteList, options); // This line store data (lines of notes) in cache using "cachekey" And set some option(rule) how long it will store.
                }
                if (noteList != null)
                {
                    return Ok(new { success = true, Message = "Got all the notes", result = noteList });
                }
                else
                {
                    return BadRequest(new { success = false, Message = "No Notes Found" });
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        //Copy Note
        [Authorize]
        [HttpPost]
        [Route("Copy/{noteId}")]
        public IActionResult CopyNote(int noteId)
        {
            try
            {
                long userId = long.Parse(User.FindFirst("UserId").Value);
                var result = noteBusiness.CopyNote(noteId, userId);
                if (result != null)
                {
                    return Ok(new { success = true, Message = "Copy Created", data = result });
                }
                else
                {
                    return BadRequest(new { success = false, Message = "Copy not create" });
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
