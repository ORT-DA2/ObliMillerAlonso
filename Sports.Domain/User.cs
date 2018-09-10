using System;
using System.Collections.Generic;
using System.Text;
using Sports.Exceptions;

namespace Sports.Domain
{
    public class User
    {
        public int Id { get; private set; }
        public string FirstName { get; set; }
        public bool IsAdmin { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
        public string Token { get; set; }

        public User()
        {
            Token = "";
        }

        public void IsValid()
        {
            IsValidFirstName();
            IsValidLastName();
            IsValidUserUserName();
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

    }
}
