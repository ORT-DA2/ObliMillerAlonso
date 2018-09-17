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
        private Match _match;

        [TestInitialize]
        public void SetUp()
        {
            Team localTeam = new Team()
            {
                Name = "Local team"
            };
            Team visitorTeam = new Team()
            {
                Name = "Visitor team",

            };
            _match = new Match()
            {
                Local = localTeam,
                Visitor = visitorTeam,
                Date = DateTime.Now.AddDays(1)
            };
            var options = new DbContextOptionsBuilder<RepositoryContext>()
                .UseInMemoryDatabase<RepositoryContext>(databaseName: "UserLogicTestDB")
                .Options;
            _repository = new RepositoryContext(options);
            _wrapper = new RepositoryWrapper(_repository);
            _matchLogic = new MatchLogic(_wrapper);
        }

        [TestCleanup]
        public void TearDown()
        {
            _repository.Matches.RemoveRange(_repository.Matches);
            _repository.SaveChanges();
        }

        [TestMethod]
        public void AddMatch()
        {
            _matchLogic.AddMatch(_match);
            Assert.IsNotNull(_matchLogic.GetMatchById(_match.Id));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidMatchDataException))]
        public void AddNullMatch()
        {
            _matchLogic.AddMatch(null);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidMatchDataException))]
        public void AddMatchInvalidDate()
        {
            _match.Date = DateTime.Now.AddDays(-1);
            _matchLogic.AddMatch(_match);
        }
    }
}
