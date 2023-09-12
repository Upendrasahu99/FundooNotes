using System;
using System.Collections.Generic;
using System.Text;

namespace CommonLayer.Model
{
    public class CreateNoteModel
    {
        public string Title { get; set; }
        public string Note { get; set; }
        public DateTime? Reminder { get; set; }
        public string BackgroundColor { get; set; }
        public string ImagePath { get; set; }
        public bool Archive { get; set; }
        public bool PinNote { get; set; }   
    }
}
