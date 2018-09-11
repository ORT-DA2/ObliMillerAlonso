using System;
using System.Collections.Generic;
using System.Text;
using Sports.Exceptions;

namespace Sports.Domain
{
    public class Comment
    {
        public int Id { get; private set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public User Author { get; set; }
        

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
