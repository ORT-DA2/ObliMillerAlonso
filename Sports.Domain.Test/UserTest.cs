using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sports.Exceptions;
using Sports.Domain;



namespace Sports.Domain.Test
{
    [TestClass]
    public class UserTest
    {
        const bool ADMINUSER = true;
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
                Password = "root"
            };
            invalidUser.IsValid();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidUserDataException))]
        public void InvalidUserLastName()
        {
            User invalidUser = new User()
            {
                FirstName = "Itai",
                LastName = "",
                Email = "itai@gmail.com",
                UserName = "iMiller",
                Token = "12345678",
                Password = "root"
            };
            invalidUser.IsValid();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidUserDataException))]
        public void InvalidUserUserName()
        {
            User invalidUser = new User()
            {
                FirstName = "Itai",
                LastName = "Miller",
                Email = "itai@gmail.com",
                UserName = "",
                Token = "12345678",
                Password = "root"
            };
            invalidUser.IsValid();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidUserDataException))]
        public void InvalidUserPassword()
        {
            User invalidUser = new User()
            {
                FirstName = "Itai",
                LastName = "Miller",
                Email = "itai@gmail.com",
                UserName = "IMiller",
                Token = "12345678",
                Password = ""
            };
            invalidUser.IsValid();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidUserDataException))]
        public void InvalidUserEMail()
        {
            User invalidUser = new User()
            {
                FirstName = "Itai",
                LastName = "Miller",
                Email = "",
                UserName = "IMiller",
                Token = "12345678",
                Password = "root"
            };
            invalidUser.IsValid();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidUserDataException))]
        public void InvalidUserEMailFormat()
        {
            User invalidFormat = new User()
            {
                Email = "itaimillergmail"
            };
            invalidFormat.IsValidEmailFormat();
        }



        [TestMethod]
        public void CreateAdmin()
        {
            User admin = new User(ADMINUSER)
            {
                FirstName = "Itai",
                LastName = "Miller",
                Email = "itai@gmail.com",
                UserName = "IMiller",
                Token = "12345678",
                Password = "root"
            };
            Assert.IsTrue(admin.IsAdmin);
        }
        

        [TestMethod]
        public void EqualsIsTrue()
        {
            User firstUser = new User
            {
                UserName = "Pablo"
            };
            User secondUser = new User
            {
                UserName = "Pablo"
            };
            Assert.AreEqual(firstUser, secondUser);
        }

        [TestMethod]
        public void EqualsIsFalse()
        {
            User firstUser = new User
            {
                UserName = "Pablo"
            };
            User secondUser = new User
            {
                UserName = "Diego"
            };
            Assert.AreNotEqual(firstUser, secondUser);
        }

        [TestMethod]
        public void ToStringIsOk()
        {
            User user = new User
            {
                UserName = "Pablito",
                FirstName = "Pablo",
                LastName = "Garcia"
            };
            String expectedToString = string.Format("Name: {0} LastName: {1} UserName: {2}", user.FirstName, user.LastName, user.UserName);
            Assert.AreEqual(expectedToString, user.ToString());
        }





    }
}
