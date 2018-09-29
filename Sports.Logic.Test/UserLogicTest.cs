using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Sports.Domain;
using Sports.Logic.Interface;
using Sports.Repository;
using Sports.Repository.Interface;
using Sports.Repository.Context;
using System.Diagnostics.CodeAnalysis;
using Sports.Domain.Exceptions;
using Sports.Logic.Exceptions;

namespace Sports.Logic.Test
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class UserLogicTest
    {
        private IRepositoryUnitOfWork unitOfWork;
        private RepositoryContext repository;
        private IUserLogic userLogic;
        User user;

        [TestInitialize]
        public void SetUp()
        {
            SetUpRepositories();
            user = new User(true)
            {
                FirstName = "Itai",
                LastName = "Miller",
                Email = "itaimiller@gmail.com",
                UserName = "iMiller",
                Password = "root"
            };
        }

        private void SetUpRepositories()
        {
            var options = new DbContextOptionsBuilder<RepositoryContext>()
                .UseInMemoryDatabase<RepositoryContext>(databaseName: "UserLogicTestDB")
                .Options;
            repository = new RepositoryContext(options);
            unitOfWork = new RepositoryUnitOfWork(repository);
            userLogic = new UserLogic(unitOfWork);
        }

        [TestCleanup]
        public void TearDown()
        {
            repository.Users.RemoveRange(repository.Users);
            repository.SaveChanges();
        }

        [TestMethod]
        public void AddUser()
        {
            userLogic.AddUser(user);
            Assert.IsNotNull(userLogic.GetUserById(user.Id));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNullValueException))]
        public void AddNullUser()
        {
            userLogic.AddUser(null);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidEmptyTextFieldException))]
        public void AddUserWithInvalidData()
        {
            User invalidUser = new User()
            {
                FirstName = "",
                LastName = "",
                Email = "itaimil",
                UserName = "iMiller",
                Password = "root"
            };
            userLogic.AddUser(invalidUser);
        }
        
        [TestMethod]
        public void UpdateUserFirstName()
        {
            userLogic.AddUser(user);
            User userChanges = new User()
            {
                FirstName = "Pepe"
            };
            userLogic.UpdateUser(user.Id, userChanges);
            Assert.AreEqual<string>(userLogic.GetUserById(user.Id).FirstName,userChanges.FirstName);
        }

        [TestMethod]
        public void UpdateUserLastName()
        {
            userLogic.AddUser(user);
            User userChanges = new User()
            {
                LastName = "Alonso"
            };
            userLogic.UpdateUser(user.Id, userChanges);
            Assert.AreEqual<string>(userLogic.GetUserById(user.Id).LastName, userChanges.LastName);
        }

        [TestMethod]
        public void UpdateUserEmail()
        {
            userLogic.AddUser(user);
            User userChanges = new User()
            {
                Email = "pepealonso@gmail.com"
            };
            userLogic.UpdateUser(user.Id, userChanges);
            Assert.AreEqual<string>(userLogic.GetUserById(user.Id).Email, userChanges.Email);
        }

        [TestMethod]
        public void UpdateUserPassword()
        {
            userLogic.AddUser(user);
            User userChanges = new User()
            {
                Password = "abcd"
            };
            userLogic.UpdateUser(user.Id, userChanges);
            Assert.AreEqual<string>(userLogic.GetUserById(user.Id).Password, userChanges.Password);
        }

        [TestMethod]
        public void UpdateUserUserName()
        {
            userLogic.AddUser(user);
            User userChanges = new User()
            {
                UserName = "pepealonso"
            };
            userLogic.UpdateUser(user.Id, userChanges);
            Assert.AreEqual<string>(userLogic.GetUserById(user.Id).UserName, userChanges.UserName);
        }

        [TestMethod]
        public void UpdateIgnoreEmptyFields()
        {
            userLogic.AddUser(user);
            User userChanges = new User()
            {
                UserName = ""
            };
            userLogic.UpdateUser(user.Id, userChanges);
            Assert.AreNotEqual<string>(userLogic.GetUserById(user.Id).UserName, userChanges.UserName);
        }
        
        [TestMethod]
        [ExpectedException(typeof(InvalidUserDataFormatException))]
        public void UpdateInvalidData()
        {
            userLogic.AddUser(user);
            User userChanges = new User()
            {
                Email = "fakeEmail"
            };
            userLogic.UpdateUser(user.Id, userChanges);
        }

        [TestMethod]
        [ExpectedException(typeof(UserAlreadyExistException))]
        public void AddDuplicatedUsername()
        {
            userLogic.AddUser(user);
            User identicalUser = new User()
            {
                FirstName = "Itai",
                LastName = "Miller",
                Email = "itaimiller@gmail.com",
                UserName = "iMiller",
                Password = "root"
            };
            userLogic.AddUser(identicalUser);
        }

        [TestMethod]
        public void GetUserByUsername()
        {
            userLogic.AddUser(user);
            Assert.AreEqual<string>(userLogic.GetUserByUserName(user.UserName).UserName, user.UserName);
        }

        [TestMethod]
        public void DeleteUser()
        {
            userLogic.AddUser(user);
            userLogic.RemoveUser(user.Id);
            Assert.AreEqual(userLogic.GetAll().Count, 0);
        }


        [TestMethod]
        [ExpectedException(typeof(UserDoesNotExistException))]
        public void DeleteNonExistingUser()
        {
            userLogic.AddUser(user);
            userLogic.RemoveUser(user.Id+1);
        }
    }
}
