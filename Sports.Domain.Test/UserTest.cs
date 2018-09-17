using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sports.Exceptions;
using Sports.Domain;



namespace Sports.Domain.Test
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class UserTest
    {
        const bool ADMINUSER = true;
        User user;

        [TestInitialize]
        public void SetUp()
        {
            user = new User()
            {
                FirstName = "Itai",
                LastName = "Miller",
                Email = "itaimiller@gmail.com",
                UserName = "iMiller",
                Password = "root"
            };
        }

        [TestMethod]
        public void NewUser()
        {
            Assert.IsNotNull(user);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidUserDataException))]
        public void InvalidUserName()
        {
            user.FirstName = "";
            user.IsValid();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidUserDataException))]
        public void InvalidUserLastName()
        {
            user.LastName = "";
            user.IsValid();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidUserDataException))]
        public void InvalidUserUserName()
        {
            user.UserName = "";
            user.IsValid();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidUserDataException))]
        public void InvalidUserPassword()
        {
            user.Password = "";
            user.IsValid();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidUserDataException))]
        public void InvalidUserEMail()
        {
            user.Email = "";
            user.IsValid();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidUserDataException))]
        public void InvalidUserEMailFormat()
        {
            user.Email = "itaimillergmail";
            user.IsValid();
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
