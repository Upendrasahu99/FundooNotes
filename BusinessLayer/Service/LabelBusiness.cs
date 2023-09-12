using BusinessLayer.Interface;
using CommonLayer.Model;
using RepoLayer.Entity;
using RepoLayer.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Service
{
    public class LabelBusiness : ILabelBusiness
    {

        private readonly ILabelRepo labelRepo;

        public LabelBusiness (ILabelRepo labelRepo)
        {
            this.labelRepo = labelRepo;
        }

        public LabelEntity CreateLabel(CreateLabelModel model, long userId, int? noteId)
        {
            try
            {
                return labelRepo.CreateLabel(model, userId, noteId);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<LabelEntity> UpdateLabel(string labelName, string newLabelName, long userId)
        {
            try
            {
                return labelRepo.UpdateLabel(labelName, newLabelName, userId);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<LabelEntity> GetAllLabel(long userId)
        {
            try
            {
                return labelRepo.GetAllLabel(userId);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<LabelEntity> DeleteLabel(string labelName, long userId)
        {
            try
            {
                return labelRepo.DeleteLabel(labelName, userId);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public LabelEntity AddLabel(string labelName, long userId, int noteId)
        {
            try
            {
                return labelRepo.AddLabel(labelName, userId, noteId);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
