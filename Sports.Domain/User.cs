using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Mail;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Sports.Domain.Exceptions;

namespace Sports.Domain
{
    [Table("User")]
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public bool IsAdmin { get; private set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get;  set; }
        public string UserName { get; set; }

        public User()
        {

        }
       
        public User(bool isAdmin)
        {
            this.IsAdmin = isAdmin;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !obj.GetType().Equals(this.GetType()))
            {
                return false;
            }
            else
            {
                return this.UserName.Equals(((User)obj).UserName);
            }
        }

        public override string ToString()
        {
            string tostring = "Name: " + FirstName + " LastName: " + LastName + " UserName: " + UserName;
            return tostring;
        }

        public void IsValid()
        {
            IsValidFirstName();
            IsValidLastName();
            IsValidUserUserName();
            IsValidUserPassword();
            IsValidUserEMail();
            IsValidUserEmailFormat(Email);
        }

        private void IsValidFirstName()
        {
            if (string.IsNullOrWhiteSpace(FirstName))
                throw new InvalidEmptyTextFieldException("Name cannot be empty");
        }

        private void IsValidLastName()
        {
            if (string.IsNullOrWhiteSpace(LastName))
                throw new InvalidEmptyTextFieldException("LastName cannot be empty");
        }

        private void IsValidUserUserName()
        {
            if (string.IsNullOrWhiteSpace(UserName))
                throw new InvalidEmptyTextFieldException("Username cannot be empty");
        }

        private void IsValidUserPassword()
        {
            if (string.IsNullOrWhiteSpace(Password))
                throw new InvalidEmptyTextFieldException("Password cannot be empty");
        }

        private void IsValidUserEMail()
        {
            if (string.IsNullOrWhiteSpace(Email))
                throw new InvalidEmptyTextFieldException("Email cannot be empty");
        }

        private void IsValidUserEmailFormat(string emailaddress)
        {
           if(!IsValidEmailAt(emailaddress) || !IsValidEmailDotCom(emailaddress))
                throw new InvalidUserDataFormatException("Invalid EMail. Format is not correct");
        }

        private bool IsValidEmailAt(string emailaddress)
        {
            string[] address = emailaddress.Split("@");
            if (address.Length != 2)
            {
                return false;
            }
            return true;
        }

        private bool IsValidEmailDotCom(string emailaddress)
        {
            string[] ending = emailaddress.Split(".com");
            if (ending.Length != 2 || ending[1] != "")
            {
                return false;
            }
            return true;
        }

        public void UpdateData(User user)
        {
            this.FirstName = IgnoreWhiteSpace(this.FirstName, user.FirstName);
            this.LastName = IgnoreWhiteSpace(this.LastName, user.LastName);
            this.Email = IgnoreWhiteSpace(this.Email, user.Email);
            this.Password = IgnoreWhiteSpace(this.Password, user.Password);
            this.UserName = IgnoreWhiteSpace(this.UserName,user.UserName);
        }

        private string IgnoreWhiteSpace(string originalText, string updatedText)
        {
            if (string.IsNullOrWhiteSpace(updatedText))
            {
                return originalText;
            }
            return updatedText;
        }

        public void ValidatePassword(string password)
        {
            if (!this.Password.Equals(password))
            {
                throw new InvalidAuthenticationException("Invalid password");
            }
        }

    }
}
