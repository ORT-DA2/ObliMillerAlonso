using System;
using System.Collections.Generic;
using System.Text;
using Sports.Exceptions;
using System.Net.Mail;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        public override int GetHashCode()
        {
            return this.UserName.GetHashCode();
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
                throw new InvalidUserDataException("Invalid Name");
        }

        private void IsValidLastName()
        {
            if (string.IsNullOrWhiteSpace(LastName))
                throw new InvalidUserDataException("Invalid LastName");
        }

        private void IsValidUserUserName()
        {
            if (string.IsNullOrWhiteSpace(UserName))
                throw new InvalidUserDataException("Invalid UserName");
        }

        private void IsValidUserPassword()
        {
            if (string.IsNullOrWhiteSpace(Password))
                throw new InvalidUserDataException("Invalid Password");
        }

        private void IsValidUserEMail()
        {
            if (string.IsNullOrWhiteSpace(Email))
                throw new InvalidUserDataException("Invalid EMail");
        }

        private void IsValidUserEmailFormat(string emailaddress)
        {
           if(!IsValidEmailAt(emailaddress) || !IsValidEmailDotCom(emailaddress))
                throw new InvalidUserDataException("Invalid EMail");
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
            this.FirstName = user.FirstName;
            this.LastName = user.LastName;
            this.Email = user.Email;
        }


    }
}
