using CommonLayer.Model;
using Microsoft.Extensions.Configuration;
using RepoLayer.Context;
using RepoLayer.Entity;
using RepoLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RepoLayer.Service
{
    public class CollabRepo : ICollabRepo
    {
        private readonly FundooContext fundooContext;
        private readonly IConfiguration configuration;

        public CollabRepo(FundooContext fundooContext, IConfiguration configuration)
        {
            this.fundooContext = fundooContext;
            this.configuration = configuration;
        }   

        // Create Collab
        public CollabEntity CreateCollab(CreateCollabModel model, int noteId, long userId, string email)
        {
            try
            {
              

                CollabEntity collabEntity = fundooContext.Collabs.SingleOrDefault(k => k.NoteId == noteId && k.Email == model.Email); // For Checking Existing Collab
                if(email != model.Email && collabEntity == null) 
                {
                    CollabEntity collab = new CollabEntity();
                    collab.Email = model.Email;
                    collab.UserId = userId;
                    collab.NoteId = noteId;
                    fundooContext.Collabs.Add(collab);
                    fundooContext.SaveChanges();
                    return collab;
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

        public CollabEntity DeleteCollab(DeleteCollabModel model, long userId, int noteId)
        {
            try
            {
                CollabEntity collabEntity = fundooContext.Collabs.SingleOrDefault(u=> u.Email == model.Email && u.UserId == userId && u.NoteId == noteId);
                if(collabEntity != null)
                {
                    fundooContext.Collabs.Remove(collabEntity);
                    fundooContext.SaveChanges();
                    return collabEntity;
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

        public List<CollabEntity> GetAllCollabOfNote(long userId, int noteId)
        {
            try
            {
                var collabList = fundooContext.Collabs.Where(u => u.UserId == userId && u.NoteId == noteId).ToList();
                if(collabList.Count != 0)
                {
                    return collabList;
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
