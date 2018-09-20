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
    //agregar sport despues
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class MatchLogicTest
    {
        private IRepositoryUnitOfWork _unit;
        private RepositoryContext _repository;
        private IMatchLogic _matchLogic;
        private ITeamLogic _teamLogic;
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
            _unit = new RepositoryUnitOfWork(_repository);
            _matchLogic = new MatchLogic(_unit);
            _teamLogic = new TeamLogic(_unit);
            _teamLogic.AddTeam(localTeam);
            _teamLogic.AddTeam(visitorTeam);
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

        [TestMethod]
        [ExpectedException(typeof(InvalidTeamDataException))]
        public void AddMatchInvalidLocalTeam()
        {
            Team invalidTeam = new Team()
            {
                Name = "Unregistered team"
            };
            _match.Local = invalidTeam;
            _matchLogic.AddMatch(_match);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidTeamDataException))]
        public void AddMatchInvalidVisitorTeam()
        {
            Team invalidTeam = new Team()
            {
                Name = "Unregistered team"
            };
            _match.Visitor = invalidTeam;
            _matchLogic.AddMatch(_match);
        }


        [TestMethod]
        public void ModifyDate()
        {
            _matchLogic.AddMatch(_match);
            _match.Date = DateTime.Now.AddDays(+2);
            _matchLogic.ModifyMatch(_match);
            Assert.AreEqual(_matchLogic.GetMatchById(_match.Id).Date, _match.Date);
        }


        [TestMethod]
        [ExpectedException(typeof(InvalidMatchDataException))]
        public void ModifyInvalidMatch()
        {
            _matchLogic.ModifyMatch(_match);
        }
    }
}
