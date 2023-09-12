using CommonLayer.Model;
using RepoLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Interface
{
    public interface ILabelBusiness
    {
        public LabelEntity CreateLabel(CreateLabelModel model, long userId, int? noteId);

        public List<LabelEntity> UpdateLabel(string labelName, string newLabelName, long userId);
        public List<LabelEntity> GetAllLabel(long userId);
        public List<LabelEntity> DeleteLabel(string labelName, long userId);
        public LabelEntity AddLabel(string labelName, long userId, int noteId);
    }
}
