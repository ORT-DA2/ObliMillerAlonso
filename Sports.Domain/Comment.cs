using System;
using System.Collections.Generic;
using System.Text;
using Sports.Exceptions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sports.Domain
{
    public class Comment
    {
        [Key]
        public int Id { get; private set; }
        public string Text { get; set; }
        public DateTime Date { get; private set; }
        public User User { get; set; }
        
        public Comment()
        {
            Date = DateTime.Now;
        }
        public void IsValid()
        {
            IsValidCommentText();
        }
        private void IsValidCommentText()
        {
            if (string.IsNullOrWhiteSpace(Text))
                throw new InvalidCommentDataException("Invalid Text");
        }
    }
}
