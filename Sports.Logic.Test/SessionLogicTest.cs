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
using System.Linq;
using Sports.Logic.Exceptions;
using Sports.Domain.Exceptions;

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
        Session _session;

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
            _session = new Session()
            {
                User = _user,
                Token = Guid.NewGuid()
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
            Guid newToken = _sessionLogic.LogInUser(_user.UserName,_user.Password);
            Guid token = _sessionLogic.LogInUser(_user.UserName, _user.Password);
            Assert.AreEqual(_user, _sessionLogic.GetUserFromToken(token));
        }

        [ExpectedException(typeof(UserDoesNotExistException))]
        [TestMethod]
        public void TestLoginNonExistingUsernameLogin()
        {
            User user = new User
            {
                UserName = "pepe",
                Password = "root"
            };
            Guid token = _sessionLogic.LogInUser(user.UserName, user.Password);
        }

        [ExpectedException(typeof(InvalidAuthenticationException))]
        [TestMethod]
        public void TestLoginNonExistingPasswordLogin()
        {
            User user = new User
            {
                UserName = "iMiller",
                Password = "abcd"
            };
            Guid token = _sessionLogic.LogInUser(user.UserName, user.Password);
        }

        [TestMethod]
        public void TestGetUserLogin()
        {
            Guid token = _sessionLogic.LogInUser(_user.UserName, _user.Password);
            Assert.AreEqual(_user, _sessionLogic.GetUserFromToken(token));
        }

        [ExpectedException(typeof(UserDoesNotExistException))]
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
            Guid token = _sessionLogic.LogInUser(_user.UserName, _user.Password);
            Guid anotherGuid = _sessionLogic.LogInUser(identicalUser.UserName, identicalUser.Password);
            Assert.AreNotEqual(token, anotherGuid);
        }

        [TestMethod]
        [ExpectedException(typeof(SessionDoesNotExistException))]
        public void TestLogoutUser()
        {
            Guid token = _sessionLogic.LogInUser(_user.UserName, _user.Password);
            _sessionLogic.LogoutByUser(_user);
           _sessionLogic.GetUserFromToken(token);
        }
        


    }
}
