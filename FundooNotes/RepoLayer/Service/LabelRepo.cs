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
    public class LabelRepo : ILabelRepo
    {
        private readonly FundooContext fundooContext;

        public LabelRepo(FundooContext fundooContext)
        {
            this.fundooContext = fundooContext;
        }

        // Create Label with note or empty label and also for add new label in note
        public LabelEntity CreateLabel(CreateLabelModel model, long userId, int? noteId)
        {
            try
            { 
                NoteEntity note = fundooContext.Notes.SingleOrDefault(u => u.NoteId == noteId && u.UserId == userId);
                LabelEntity checkLabel = fundooContext.Labels.FirstOrDefault(u =>u.LabelName == model.LabelName); // For not adding duplicate note in same label
                if(note != null && checkLabel == null)
                {
                    if (noteId != null || noteId == null)
                    {
                        LabelEntity label = new LabelEntity();
                        label.LabelName = model.LabelName;
                        label.UserId = userId;
                        label.NoteId = noteId;
                        fundooContext.Labels.Add(label);
                        fundooContext.SaveChanges();
                        return label;
                    }
                    else
                    {
                        return null;
                    }
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

        // Change All label name which is same 
        public List<LabelEntity> UpdateLabel(string labelName,string newLabelName, long userId)
        {
            try
            {
                var labels = fundooContext.Labels.Where(u => u.LabelName == labelName && u.UserId == userId).ToList();
                if (labels != null)
                {
                    foreach(var label in labels)
                    {
                        label.LabelName = newLabelName;
                    }
                    fundooContext.SaveChanges();
                    return labels;  
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

        public List<LabelEntity> GetAllLabel(long userId)
        {
            try
            {
                var labels = fundooContext.Labels.Where(u => u.UserId == userId).ToList();
                if (labels != null)
                {
                    return labels;
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

        public List<LabelEntity> DeleteLabel(string labelName, long userId) 
        {
            try
            {
                var labels = fundooContext.Labels.Where(u => u.LabelName == labelName && u.UserId == userId).ToList();
                if (labels != null)
                {
                    foreach(var label in labels)
                    {
                        fundooContext.Remove(label);
                        fundooContext.SaveChanges() ;
                    }
                    return labels;
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


        // Adding in existing Label
        public LabelEntity AddLabel(string labelName, long userId, int noteId)
        {
            try
            {
                LabelEntity checkExistLabel = fundooContext.Labels.FirstOrDefault(u => u.LabelName == labelName && u.UserId == userId);
                if (checkExistLabel != null && checkExistLabel.NoteId == null)
                {
                    checkExistLabel.NoteId = noteId;
                    fundooContext.SaveChanges();
                    return checkExistLabel;
                }
                else if(checkExistLabel != null)
                {
                    LabelEntity label = new LabelEntity();
                    label.LabelName = labelName;
                    label.UserId = userId;
                    label.NoteId = noteId;
                    fundooContext.Add(label);
                    fundooContext.SaveChanges();
                    return label;
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
