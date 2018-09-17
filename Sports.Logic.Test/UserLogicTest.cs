using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Sports.Domain;
using Sports.Logic;
using Sports.Repository;
using Sports.Repository.Interface;
using Sports.Repository.Context;
using Sports.Exceptions;

namespace Sports.Logic.Test
{
    [TestClass]
    public class UserLogicTest
    {
        private IRepositoryWrapper _wrapper;
        private RepositoryContext _repository;
        private UserLogic _userLogic;
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
        public void TearDown() { }

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
            _user.FirstName = "Juan";
            _userLogic.UpdateUser(_user.Id,_user);
            Assert.Equals(_userLogic.GetUserById(_user.Id).FirstName,_user.FirstName);
        }
        

    }
}
