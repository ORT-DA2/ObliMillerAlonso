using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Sports.Domain.Exceptions;
using Sports.Domain.Constants;

namespace Sports.Domain
{
    public class Comment
    {
        [Key]
        public int Id { get;  set; }
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
            IsNotNullUser();
        }

        private void IsNotNullUser()
        {
            if (this.User == null)
            {
                throw new InvalidEmptyUserException(EmptyUser.EMPTY_USER);
            }
        }

        private void IsValidCommentText()
        {
            if (string.IsNullOrWhiteSpace(Text))
            {
                throw new InvalidEmptyTextFieldException(EmptyField.EMPTY_TEXT_MESSAGE);
            }
        }
    }
}
