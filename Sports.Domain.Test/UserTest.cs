﻿using System;
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
                Password = "root",
                IsAdmin = false
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
                Password = "root",
                IsAdmin = false
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
                Password = "",
                IsAdmin = false
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
                Password = "root",
                IsAdmin = false
            };
            invalidUser.IsValidUserEMail();
        }




    }
}
