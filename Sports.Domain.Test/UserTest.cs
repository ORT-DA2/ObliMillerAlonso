using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sports.Exceptions;



namespace Sports.Domain.Test
{
    [TestClass]
    public class UserTest
    {
        User user;

        [TestInitialize]
        public void SetUp()
        {
            user = new User();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidUserDataException))]
        public void InvalidUserName()
        {
            User invalidUser = new User()
            {
                FirstName = "",
                LastName = "Miller",
                Email = "itaimiller@gmail.com",
                UserName = "iMiller",
                Token = "12345678",
                Password = "root",
                IsAdmin = false
            };
            invalidUser.IsValid();
        }


    }
}
