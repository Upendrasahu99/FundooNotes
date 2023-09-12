using CommonLayer.Model;
using Microsoft.AspNetCore.Http;
using RepoLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RepoLayer.Interface
{
    public interface INoteRepo
    {
        public NoteEntity CreateNote(CreateNoteModel createNoteModel, long userId);

        public NoteEntity UpdateNote(UpdateNoteModel updateNoteModel, long userId, long noteId);

        public List<NoteEntity> GetAllNotes(long userId);

        public NoteEntity DeletNote(long noteId, long userId);

        public NoteEntity ChangeArchive(long noteId, long userId);
        public NoteEntity ChangePinNote(long noteId, long userId);
        public NoteEntity ChangeTrashSection(long noteId, long userId);
        public NoteEntity ChangeBackgroundColor(string color, long noteId, long userId);
        public List<NoteEntity> FindNotes(string keyword, long userId);
         Task<Tuple<int, string>> Image(long noteId, long userId, IFormFile imageFile);

        public NoteEntity CopyNote(int noteId, long userId);
    }
}
