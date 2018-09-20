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
        private ISportLogic _sportLogic;
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
            Sport sport = new Sport()
            {
                Name = "Test Sport"
            };
            _match = new Match()
            {
                Sport = sport,
                Local = localTeam,
                Visitor = visitorTeam,
                Date = DateTime.Now.AddDays(1)
            };
            var options = new DbContextOptionsBuilder<RepositoryContext>()
                .UseInMemoryDatabase<RepositoryContext>(databaseName: "MatchLogicTestDB")
                .Options;
            _repository = new RepositoryContext(options);
            _unit = new RepositoryUnitOfWork(_repository);
            _matchLogic = new MatchLogic(_unit);
            _sportLogic = new SportLogic(_unit);
            _sportLogic.AddSport(sport);
            _sportLogic.AddTeamToSport(sport,localTeam);
            _sportLogic.AddTeamToSport(sport, visitorTeam);
        }

        [TestCleanup]
        public void TearDown()
        {
            _repository.Matches.RemoveRange(_repository.Matches);
            _repository.Sports.RemoveRange(_repository.Sports);
            _repository.Teams.RemoveRange(_repository.Teams);
            _repository.SaveChanges();
        }

        [TestMethod]
        public void AddFullMatch()
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
        [ExpectedException(typeof(InvalidSportDataException))]
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
        [ExpectedException(typeof(InvalidSportDataException))]
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
            _matchLogic.ModifyMatch(_match.Id, _match);
            Assert.AreEqual(_matchLogic.GetMatchById(_match.Id).Date, _match.Date);
        }


        [TestMethod]
        [ExpectedException(typeof(InvalidMatchDataException))]
        public void ModifyInvalidMatch()
        {
            _matchLogic.ModifyMatch(_match.Id, _match);
        }
        
        [TestMethod]
        [ExpectedException(typeof(InvalidSportDataException))]
        public void AddMatchWithInvalidSport()
        {
            Sport sport = new Sport()
            {
                Name = "Test Sport"
            };
            _match.Sport = sport;
            _matchLogic.AddMatch(_match);
        }
        //add match without sport


        //modify sport
        //modify invalid sport
        //modify teams
        //add teams not of sport
        //delete match
    }
}
