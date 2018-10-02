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
using Sports.Logic.Constants;

namespace Sports.Logic.Test
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class SessionLogicTest
    {
        private IRepositoryUnitOfWork unitOfWork;
        private RepositoryContext repository;
        private ISessionLogic sessionLogic;
        private IUserLogic userLogic;
        User user;
        Session session;

        [TestInitialize]
        public void SetUp()
        {
            SetUpRepositories();
            SetUpAdminSession();
            SetUpTestData();
        }

        private void SetUpRepositories()
        {
            var options = new DbContextOptionsBuilder<RepositoryContext>()
                            .UseInMemoryDatabase<RepositoryContext>(databaseName: "SessionLogicTestDB")
                            .Options;
            repository = new RepositoryContext(options);
            unitOfWork = new RepositoryUnitOfWork(repository);
            sessionLogic = new SessionLogic(unitOfWork);
            userLogic = new UserLogic(unitOfWork);
        }

        private void SetUpAdminSession()
        {
            User admin = new User(true)
            {
                FirstName = "Rafael",
                LastName = "Alonso",
                Email = "ralonso@gmail.com",
                UserName = "rAlonso",
                Password = "pass"
            };
            IUserRepository repo = unitOfWork.User;
            repo.Create(admin);
            repo.Save();
            Guid adminToken = sessionLogic.LogInUser(admin.UserName, admin.Password);
            sessionLogic.GetUserFromToken(adminToken);
            userLogic.SetSession(adminToken);
        }

        private void SetUpTestData()
        {
            user = new User(true)
            {
                FirstName = "Itai",
                LastName = "Miller",
                Email = "itaimiller@gmail.com",
                UserName = "iMiller",
                Password = "root"
            };
            userLogic.AddUser(user);
            session = new Session()
            {
                User = user,
                Token = Guid.NewGuid()
            };
        }

        [TestCleanup]
        public void TearDown()
        {
            repository.Logins.RemoveRange(repository.Logins);
            repository.Users.RemoveRange(repository.Users);
            repository.SaveChanges();
        }

        [TestMethod]
        public void TestLoginUser()
        {
            Guid token = sessionLogic.LogInUser(user.UserName, user.Password);
            User sessionUser = sessionLogic.GetUserFromToken(token);
            Assert.AreEqual(sessionUser, user);
        }

        [TestMethod]
        public void TestUserLogin()
        {
            Guid newToken = sessionLogic.LogInUser(user.UserName,user.Password);
            Guid token = sessionLogic.LogInUser(user.UserName, user.Password);
            Assert.AreEqual(user, sessionLogic.GetUserFromToken(token));
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
            Guid token = sessionLogic.LogInUser(user.UserName, user.Password);
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
            Guid token = sessionLogic.LogInUser(user.UserName, user.Password);
        }

        [TestMethod]
        public void TestGetUserLogin()
        {
            Guid token = sessionLogic.LogInUser(user.UserName, user.Password);
            Assert.AreEqual(user, sessionLogic.GetUserFromToken(token));
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
            Guid token = sessionLogic.LogInUser(user.UserName, user.Password);
            sessionLogic.GetUserFromToken(Guid.NewGuid());
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
            Guid token = sessionLogic.LogInUser(user.UserName, user.Password);
            Guid anotherGuid = sessionLogic.LogInUser(identicalUser.UserName, identicalUser.Password);
            Assert.AreNotEqual(token, anotherGuid);
        }

        [TestMethod]
        [ExpectedException(typeof(SessionDoesNotExistException))]
        public void TestLogoutUser()
        {
            Guid token = sessionLogic.LogInUser(user.UserName, user.Password);
            sessionLogic.LogoutByUser(user);
           sessionLogic.GetUserFromToken(token);
        }


        [TestMethod]
        [ExpectedException(typeof(InvalidNullValueException))]
        public void TestPassNullToken()
        {
            Guid token = Guid.Empty;
            sessionLogic.GetUserFromToken(token);
        }
    }
}
