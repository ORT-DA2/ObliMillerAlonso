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
using Sports.Exceptions;
using System.Diagnostics.CodeAnalysis;

namespace Sports.Logic.Test
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class UserLogicTest
    {
        private IRepositoryWrapper _wrapper;
        private RepositoryContext _repository;
        private IUserLogic _userLogic;
        User _user;

        [TestInitialize]
        public void SetUp()
        {
            _user = new User(true)
            {
                FirstName = "Itai",
                LastName = "Miller",
                Email = "itaimiller@gmail.com",
                UserName = "iMiller",
                Password = "root"
            };
            var options = new DbContextOptionsBuilder<RepositoryContext>()
                .UseInMemoryDatabase<RepositoryContext>(databaseName: "UserLogicTestDB")
                .Options;
            _repository = new RepositoryContext(options);
            _wrapper = new RepositoryWrapper(_repository);
            _userLogic = new UserLogic(_wrapper);
        }

        [TestCleanup]
        public void TearDown()
        {
            _repository.Users.RemoveRange(_repository.Users);
            _repository.SaveChanges();
        }

        [TestMethod]
        public void AddUser()
        {
            _userLogic.AddUser(_user);
            Assert.IsNotNull(_userLogic.GetUserById(_user.Id));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidUserDataException))]
        public void AddNullUser()
        {
            _userLogic.AddUser(null);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidUserDataException))]
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
            _userLogic.AddUser(invalidUser);
        }
        
        [TestMethod]
        public void UpdateUserFirstName()
        {
            _userLogic.AddUser(_user);
            User userChanges = new User()
            {
                FirstName = "Pepe"
            };
            _userLogic.UpdateUser(_user.Id, userChanges);
            Assert.AreEqual<string>(_userLogic.GetUserById(_user.Id).FirstName,userChanges.FirstName);
        }

        [TestMethod]
        public void UpdateUserLastName()
        {
            _userLogic.AddUser(_user);
            User userChanges = new User()
            {
                LastName = "Alonso"
            };
            _userLogic.UpdateUser(_user.Id, userChanges);
            Assert.AreEqual<string>(_userLogic.GetUserById(_user.Id).LastName, userChanges.LastName);
        }

        [TestMethod]
        public void UpdateUserEmail()
        {
            _userLogic.AddUser(_user);
            User userChanges = new User()
            {
                Email = "pepealonso@gmail.com"
            };
            _userLogic.UpdateUser(_user.Id, userChanges);
            Assert.AreEqual<string>(_userLogic.GetUserById(_user.Id).Email, userChanges.Email);
        }

        [TestMethod]
        public void UpdateUserPassword()
        {
            _userLogic.AddUser(_user);
            User userChanges = new User()
            {
                Password = "abcd"
            };
            _userLogic.UpdateUser(_user.Id, userChanges);
            Assert.AreEqual<string>(_userLogic.GetUserById(_user.Id).Password, userChanges.Password);
        }

        [TestMethod]
        public void UpdateUserUserName()
        {
            _userLogic.AddUser(_user);
            User userChanges = new User()
            {
                UserName = "pepealonso"
            };
            _userLogic.UpdateUser(_user.Id, userChanges);
            Assert.AreEqual<string>(_userLogic.GetUserById(_user.Id).UserName, userChanges.UserName);
        }

        [TestMethod]
        public void UpdateIgnoreEmptyFields()
        {
            _userLogic.AddUser(_user);
            User userChanges = new User()
            {
                UserName = ""
            };
            _userLogic.UpdateUser(_user.Id, userChanges);
            Assert.AreNotEqual<string>(_userLogic.GetUserById(_user.Id).UserName, userChanges.UserName);
        }
        
        [TestMethod]
        [ExpectedException(typeof(InvalidUserDataException))]
        public void UpdateInvalidData()
        {
            _userLogic.AddUser(_user);
            User userChanges = new User()
            {
                Email = "fakeEmail"
            };
            _userLogic.UpdateUser(_user.Id, userChanges);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidUserDataException))]
        public void AddDuplicatedUsername()
        {
            _userLogic.AddUser(_user);
            User identicalUser = new User()
            {
                FirstName = "Itai",
                LastName = "Miller",
                Email = "itaimiller@gmail.com",
                UserName = "iMiller",
                Password = "root"
            };
            _userLogic.AddUser(identicalUser);
        }

        [TestMethod]
        public void GetUserByUsername()
        {
            _userLogic.AddUser(_user);
            Assert.AreEqual<string>(_userLogic.GetUserByUserName(_user.UserName).UserName, _user.UserName);
        }

        [TestMethod]
        public void DeleteUser()
        {
            _userLogic.AddUser(_user);
            _userLogic.RemoveUser(_user.Id);
            Assert.AreEqual(_userLogic.GetAll().Count, 0);
        }


        [TestMethod]
        [ExpectedException(typeof(InvalidUserDataException))]
        public void DeleteNonExistingUser()
        {
            _userLogic.AddUser(_user);
            _userLogic.RemoveUser(_user.Id+1);
        }
    }
}
