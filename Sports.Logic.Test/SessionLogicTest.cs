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
using System.Linq;

namespace Sports.Logic.Test
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class SessionLogicTest
    {
        private IRepositoryUnitOfWork _unitOfWork;
        private RepositoryContext _repository;
        private ISessionLogic _sessionLogic;
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
                .UseInMemoryDatabase<RepositoryContext>(databaseName: "SessionLogicTestDB")
                .Options;
            _repository = new RepositoryContext(options);
            _unitOfWork = new RepositoryUnitOfWork(_repository);
            _sessionLogic = new SessionLogic(_unitOfWork);
            _userLogic = new UserLogic(_unitOfWork);
            _userLogic.AddUser(_user);
        }

        [TestCleanup]
        public void TearDown()
        {
            _repository.Logins.RemoveRange(_repository.Logins);
            _repository.Users.RemoveRange(_repository.Users);
            _repository.SaveChanges();
        }

        [TestMethod]
        public void TestLoginUser()
        {
            Guid token = _sessionLogic.LogInUser(_user.UserName, _user.Password);
            User sessionUser = _sessionLogic.GetUserFromToken(token);
            Assert.AreEqual(sessionUser, _user);
        }

        [TestMethod]
        public void TestUserLogin()
        {
            Guid newToken = _sessionLogic.CreateSession(_user);
            Guid token = _sessionLogic.LogInUser(_user.UserName, _user.Password);
            Assert.AreEqual(_user, _sessionLogic.GetUserFromToken(token));
        }

        [ExpectedException(typeof(InvalidUserDataException))]
        [TestMethod]
        public void TestLoginNonExistingUser()
        {
            User user = new User
            {
                UserName = "User",
                Password = "test"
            };
            Guid token = _sessionLogic.LogInUser(user.UserName, user.Password);
        }

        [TestMethod]
        public void TestGetUserLogin()
        {
            Guid token = _sessionLogic.LogInUser(_user.UserName, _user.Password);
            Assert.AreEqual(_user, _sessionLogic.GetUserFromToken(token));
        }

        [ExpectedException(typeof(InvalidUserDataException))]
        [TestMethod]
        public void TestGeUserNonExistingLogin()
        {
            User user = new User(true)
            {
                FirstName = "sergio",
                LastName = "Miller",
                Email = "sergio@gmail.com",
                UserName = "sergiom",
                Password = "abcd"
            };
            Guid token = _sessionLogic.LogInUser(user.UserName, user.Password);
            _sessionLogic.GetUserFromToken(Guid.NewGuid());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidUserDataException))]
        public void TestLoginUserAlreadyExist()
        {
            User identicalUser = new User(true)
            {
                FirstName = "Itai",
                LastName = "Miller",
                Email = "itaimiller@gmail.com",
                UserName = "iMiller",
                Password = "root"
            };
            _userLogic.AddUser(identicalUser);
            Guid token = _sessionLogic.LogInUser(_user.UserName, _user.Password);
            Guid anotherGuid = _sessionLogic.LogInUser(identicalUser.UserName, identicalUser.Password);
            Assert.AreEqual(token, anotherGuid);
        }

        [TestMethod]
        public void TestLogoutUser()
        {
            _sessionLogic.Logout(_user);
            Assert.IsNull(_user);
        }


    }
}
