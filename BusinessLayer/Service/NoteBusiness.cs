using BusinessLayer.Interface;
using CommonLayer.Model;
using Microsoft.AspNetCore.Http;
using RepoLayer.Entity;
using RepoLayer.Interface;
using RepoLayer.Service;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Service
{
    public class NoteBusiness : INoteBusiness
    {
        private readonly INoteRepo noteRepo;
        public NoteBusiness(INoteRepo noteRepo)
        {
            this.noteRepo = noteRepo;
        }
        public NoteEntity CreateNote(CreateNoteModel createNoteModel, long userId)
        {
            try
            {
                return noteRepo.CreateNote(createNoteModel, userId);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public NoteEntity UpdateNote(UpdateNoteModel updateNoteModel, long userId, long noteId)
        {
            try
            {
                return noteRepo.UpdateNote(updateNoteModel, userId, noteId);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<NoteEntity> GetAllNotes(long userId)
        {
            try
            {
                return noteRepo.GetAllNotes(userId);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public NoteEntity DeletNote(long noteId, long userId)
        {
            try
            {
                return noteRepo.DeletNote(noteId, userId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public NoteEntity ChangeArchive(long noteId, long userId)
        {
            try
            {
                return noteRepo.ChangeArchive(noteId, userId);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public NoteEntity ChangePinNote(long noteId, long userId)
        {
            try
            {
                return noteRepo.ChangePinNote(noteId, userId);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public NoteEntity ChangeTrashSection(long noteId, long userId)
        {
            try
            {
                return noteRepo.ChangeTrashSection(noteId, userId);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public NoteEntity ChangeBackgroundColor(string color, long noteId, long userId)
        {
            try
            {
                return noteRepo.ChangeBackgroundColor(color, noteId, userId);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<NoteEntity> FindNotes(string keyword, long userId)
        {
            try
            {
                return noteRepo.FindNotes(keyword, userId);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<Tuple<int, string>> Image(long noteId, long userId, IFormFile imageFile)
        {
            try
            {
                return  await noteRepo.Image(noteId, userId, imageFile);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public NoteEntity CopyNote(int noteId, long userId)
        {
            try
            {
                 return  noteRepo.CopyNote(noteId, userId);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
