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
    //chequear que no se dupliquen en la bd
    //agregar sport despues
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class MatchLogicTest
    {
        private IRepositoryWrapper _wrapper;
        private RepositoryContext _repository;
        private IMatchLogic _matchLogic;

        [TestInitialize]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<RepositoryContext>()
                .UseInMemoryDatabase<RepositoryContext>(databaseName: "UserLogicTestDB")
                .Options;
            _repository = new RepositoryContext(options);
            _wrapper = new RepositoryWrapper(_repository);
            _matchLogic = new MatchLogic(_wrapper);
        }

        [TestMethod]
        public void AddMatch()
        {
            Match match = new Match()
            {

            };
            _matchLogic.AddMatch(match);
            Assert.IsNotNull(_matchLogic.GetMatchById(match.Id));
        }
    }
}
