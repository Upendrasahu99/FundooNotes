using CommonLayer.Model;
using RepoLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Interface
{
    public interface ICollabBusiness
    {
        public CollabEntity CreateCollab(CreateCollabModel model, int noteId, long userId, string email);

        public CollabEntity DeleteCollab(DeleteCollabModel model, long userId, int noteId);
        public List<CollabEntity> GetAllCollabOfNote(long userId, int noteId);
    }
}
