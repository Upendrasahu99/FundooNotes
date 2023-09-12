using BusinessLayer.Interface;
using CommonLayer.Model;
using RepoLayer.Entity;
using RepoLayer.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Service
{
    public class CollabBusiness : ICollabBusiness
    {
        private readonly ICollabRepo collabRepo;
        public CollabBusiness(ICollabRepo collabRepo)
        {
            this.collabRepo = collabRepo;
        }

        public CollabEntity CreateCollab(CreateCollabModel model, int noteId, long userId, string email)
        {
            try
            {
                return collabRepo.CreateCollab(model, noteId, userId, email);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public CollabEntity DeleteCollab(DeleteCollabModel model, long userId, int noteId)
        {
            try
            {
                return collabRepo.DeleteCollab(model, userId, noteId);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<CollabEntity> GetAllCollabOfNote(long userId, int noteId)
        {
            try
            {
                return collabRepo.GetAllCollabOfNote(userId, noteId);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
