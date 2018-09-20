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
    public class LoginLogicTest
    {
        private IRepositoryUnitOfWork _unitOfWork;
        private RepositoryContext _repository;
        private ILoginLogic _loginLogic;
        private IUserLogic _userLogic;
        User _admin;

        [TestInitialize]
        public void SetUp()
        {
            _admin = new User(true)
            {
                Id = Guid.NewGuid().GetHashCode(),
                FirstName = "",
                UserName = "Admin",
                Password = ""
            };
            
            var options = new DbContextOptionsBuilder<RepositoryContext>()
                .UseInMemoryDatabase<RepositoryContext>(databaseName: "LoginLogicTestDB")
                .Options;
            _repository = new RepositoryContext(options);
            _unitOfWork = new RepositoryUnitOfWork(_repository);
            _loginLogic = new LoginLogic(_unitOfWork);
        }

        [TestCleanup]
        public void TearDown()
        {
            _repository.Logins.RemoveRange(_repository.Logins);
            _repository.SaveChanges();
        }

        [TestMethod]
        public void TestLoginUser()
        {
            User user = LogNewUser();
            Guid token = LoginLogic.LogInUser(user.UserName, user.Password);
            Guid tokenFromDb = _repository.Logins.FirstOrDefault(l => l.TokenId.Equals(token)).TokenId;
            Assert.AreEqual(token, tokenFromDb);
        }

        private User LogNewUser()
        {
            User user = new User
            {
                UserName = "User",
                Password = "test"
            };
            _userLogic.AddUser(user);
            return user;
        }

    }
}
