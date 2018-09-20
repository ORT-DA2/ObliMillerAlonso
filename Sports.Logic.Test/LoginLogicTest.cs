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
    public class LoginLogicTest
    {
        private IRepositoryUnitOfWork _unitOfWork;
        private RepositoryContext _repository;
        private ILoginLogic _loginLogic;
        private IUserLogic _userLogic;

        [TestInitialize]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<RepositoryContext>()
                .UseInMemoryDatabase<RepositoryContext>(databaseName: "LoginLogicTestDB")
                .Options;
            _repository = new RepositoryContext(options);
            _unitOfWork = new RepositoryUnitOfWork(_repository);
            _loginLogic = new LoginLogic(_unitOfWork);
        }



    }
}
