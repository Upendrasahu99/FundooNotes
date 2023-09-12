using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using CommonLayer.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RepoLayer.Context;
using RepoLayer.Entity;
using RepoLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepoLayer.Service
{
    public class NoteRepo : INoteRepo
    {
        private readonly FundooContext fundooContext;
        private readonly Cloudinary cloudinary;
        private readonly FileService fileService;
        private readonly IConfiguration configuration;
        public NoteRepo(FundooContext fundooContext,Cloudinary cloudinary, FileService fileService, IConfiguration configuration)
        {
            this.fundooContext = fundooContext;// ony intilize once
            this.cloudinary = cloudinary;
            this.fileService = fileService;
            this.configuration = configuration;
        }

        // Create Note
        public NoteEntity CreateNote(CreateNoteModel createNoteModel,long userId)
        {
            try
            {
                NoteEntity noteEntity = new NoteEntity();
                noteEntity.Title = createNoteModel.Title;
                noteEntity.Note = createNoteModel.Note;
                noteEntity.EditedTime = DateTime.Now;
                noteEntity.Reminder = createNoteModel.Reminder;
                noteEntity.BackgroundColor = createNoteModel.BackgroundColor;
                noteEntity.ImagePath = createNoteModel.ImagePath;
                noteEntity.Archive = createNoteModel.Archive;
                noteEntity.PinNote = createNoteModel.PinNote;
                noteEntity.UserId = userId;

                fundooContext.Notes.Add(noteEntity); 
                fundooContext.SaveChanges();

                if(noteEntity != null)
                {
                    return noteEntity;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {

                throw (ex);
            }
        }
        //UpdateNote
        public NoteEntity UpdateNote(UpdateNoteModel updateNoteModel, long userId, long noteId)
        {
            try
            {               
                NoteEntity note = fundooContext.Notes.FirstOrDefault(u => u.NoteId == noteId && u.UserId == userId);
                if  (note != null)
                {                  
                    note.Title = updateNoteModel.Title;
                    note.Note = updateNoteModel.Note;
                    note.EditedTime = DateTime.Now;
                    note.BackgroundColor = updateNoteModel.BackgroundColor;
                    note.ImagePath = updateNoteModel.ImagePath;
                    note.Archive = updateNoteModel.Archive;
                    note.PinNote = updateNoteModel.PinNote;
                    note.Trash = updateNoteModel.Trash;
                    fundooContext.SaveChanges();
                    return note;
                }                
                else { return null; }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // List of note for perticular user using 'UserId' form Claim
        public List<NoteEntity> GetAllNotes(long userId)
        {
            try
            {               
                var result  = fundooContext.Notes.Where(u => u.UserId == userId).ToList(); // WHere is used for collection of data 
                if(result != null)
                {
                    return result;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        
        // Delete Note
        public NoteEntity DeletNote(long noteId, long userId)
        {
            try
            {
                NoteEntity note = fundooContext.Notes.SingleOrDefault(u => u.NoteId == noteId && u.UserId == userId);
                if (note != null)
                {
                    fundooContext.Remove(note);
                    fundooContext.SaveChanges();
                    return note;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        // Change Archive
        public NoteEntity ChangeArchive(long noteId, long userId)
        {
            try
            {
                NoteEntity note = fundooContext.Notes.SingleOrDefault(u => u.NoteId ==noteId && u.UserId == userId);

                if (note != null && note.Archive == true)
                {
                    note.Archive = false;
                    fundooContext.SaveChanges();
                    return note;
                }
                else if (note != null && note.Archive == false)
                {
                    note.Archive = true;
                    fundooContext.SaveChanges();
                    return note;
                }
                else 
                {
                    return null;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
       
        //Change PinNote
        public NoteEntity ChangePinNote(long noteId, long userId)
        {
            try
            {
                NoteEntity note = fundooContext.Notes.SingleOrDefault(u => u.NoteId == noteId && u.UserId == userId);
                if(note != null && note.PinNote == true)
                {
                    note.PinNote = false;
                    fundooContext.SaveChanges();
                    return note;
                } 
                else if(note != null && note.PinNote == false)
                {
                    note.PinNote = true;
                    fundooContext.SaveChanges();
                    return note;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            { 
                throw;
            }
        }

        //Change Trash Section
        public NoteEntity ChangeTrashSection(long noteId, long userId)
        {
            try
            {
                NoteEntity note = fundooContext.Notes.SingleOrDefault(u => u.NoteId == noteId && u.UserId == userId);
                if(note != null && note.Trash == false)
                {
                    note.Trash = true;
                    fundooContext.SaveChanges();
                    return note;
                }
                else if( note != null && note.Trash == true)
                {
                    note.Trash = false;
                    fundooContext.SaveChanges();
                    return note;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        //Change BackgroundColor
        public NoteEntity ChangeBackgroundColor(string color, long noteId, long userId)
        {
            try
            {
                NoteEntity note = fundooContext.Notes.SingleOrDefault(u => u.NoteId == noteId && u.UserId == userId);
                if(note != null)
                {
                    note.BackgroundColor = color;
                    fundooContext.SaveChanges();
                    return note;
                }
                else 
                { 
                    return null;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        /*
         * your tasks is to develop an API that enables users to find notes based on keywords or phrases. 
             The API should accept a search query parameter and return the search results
         */

        // Find Notes
        public List<NoteEntity> FindNotes(string keyword, long userId)
        {
            try
            {
                var notes = fundooContext.Notes.Where(u => u.Note.Contains(keyword)).ToList();
                if(notes != null)
                {
                    return notes;
                }
                else
                {
                    return notes;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        //Image Upload on Cloudinary
        public async Task<Tuple<int, string>> Image(long noteId, long userId, IFormFile imageFile)  // Tuple is a data structure that allows us to store collection of elements of different types.
        {
            try
            {
                NoteEntity note = fundooContext.Notes.SingleOrDefault(u => u.NoteId == noteId && u.UserId == userId);
                if(note != null)
                {
                    var data = await fileService.UploadImage(imageFile); // Upload the image using a file service . This is an asynchronous operation. await keyword is use asynchronous programing it will stop particular method when perticular operation is completed.
                    if (data.Item1 == 0)// Check the result of the image upload.
                    {
                        return new Tuple<int, string>(0, data.Item2); // If the image upload was successful, return the tuple indicatin success.
                    }
                    var UploadImage = new ImageUploadParams
                    {
                        File = new CloudinaryDotNet.FileDescription(imageFile.FileName, imageFile.OpenReadStream())
                    };  // Configurw the pEmwrwea doe uploading the image to a cloud service(e.g. cloudinar).

                    ImageUploadResult uploadResult = await cloudinary.UploadAsync(UploadImage);// Uplaod image to cloud service
                    string imagUrl = uploadResult.SecureUrl.AbsoluteUri; // Get the URL of the uploaded image.
                    note.ImagePath = imagUrl; // Aded imagepath 

                    fundooContext.Notes.Update(note);
                    fundooContext.SaveChanges();

                    return new Tuple<int, string>(1, "Image Uploaded Successfully"); // If everything was successful, return a tuple indiacatin sucess.
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        //your task is to develop an API that enables users to make copy of notes. The API should accept a Noteid and create the copy of notes.

        public NoteEntity CopyNote(int noteId , long userId)
        {
            try
            {
                NoteEntity note = fundooContext.Notes.SingleOrDefault(u => u.NoteId == noteId && u.UserId == userId);
                if (note != null) 
                {
                    NoteEntity newNote = new NoteEntity();
                    newNote.Title = note.Title;
                    newNote.Note = note.Note;
                    newNote.EditedTime = DateTime.Now;
                    newNote.BackgroundColor = note.BackgroundColor;
                    newNote.ImagePath = note.ImagePath;
                    newNote.Archive = false;
                    newNote.PinNote = false;
                    newNote.Trash = false;
                    newNote.UserId = userId;
                    fundooContext.Add(newNote);
                    fundooContext.SaveChanges();
                    return newNote;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
