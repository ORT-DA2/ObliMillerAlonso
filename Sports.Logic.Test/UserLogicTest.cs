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

namespace Sports.Logic.Test
{
    [TestClass]
    public class UserLogicTest
    {
        private IRepositoryWrapper _wrapper;
        private RepositoryContext _repository;
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
        }

        [TestCleanup]
        public void TearDown() { }

        [TestMethod]
        public void AddUser()
        {
            UserLogic userLogic = new UserLogic(_wrapper);
            userLogic.AddUser(_user);
            Assert.IsNotNull(userLogic.GetUserById(_user.Id));
        }
    }
}
