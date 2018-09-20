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
                Name = "Match Sport"
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

        [TestMethod]
        [ExpectedException(typeof(InvalidMatchDataException))]
        public void AddMatchWithoutSport()
        {
            _match.Sport = null;
            _matchLogic.AddMatch(_match);
        }
        
        [TestMethod]
        public void ModifySportAndTeams()
        {
            _matchLogic.AddMatch(_match);
            Sport sport = new Sport()
            {
                Name = "Test Sport"
            };
            _sportLogic.AddSport(sport);

            Team localTeam = new Team()
            {
                Name = "New Local team"
            };
            Team visitorTeam = new Team()
            {
                Name = "New Visitor team",

            };
            _sportLogic.AddTeamToSport(sport, localTeam);
            _sportLogic.AddTeamToSport(sport, visitorTeam);
            _match.Sport = sport;
            _match.Local = localTeam;
            _match.Visitor = visitorTeam;
            _matchLogic.ModifyMatch(_match.Id, _match);
            Assert.AreEqual(_matchLogic.GetMatchById(_match.Id).Sport, sport);
        }
        
        [TestMethod]
        [ExpectedException(typeof(InvalidSportDataException))]
        public void ModifyInvalidSport()
        {
            _matchLogic.AddMatch(_match);
            Sport sport = new Sport()
            {
                Name = "Test Sport"
            };
            
            _match.Sport = sport;
            _matchLogic.ModifyMatch(_match.Id, _match);
        }

        [TestMethod]
        public void ModifyVisitorTeam()
        {
            _matchLogic.AddMatch(_match);
            Team visitorTeam = new Team()
            {
                Name = "New Visitor team",

            };
            _sportLogic.AddTeamToSport(_match.Sport, visitorTeam);
            _match.Visitor = visitorTeam;
            _matchLogic.ModifyMatch(_match.Id, _match);
            Assert.AreEqual(_matchLogic.GetMatchById(_match.Id).Visitor, visitorTeam);
        }

        [TestMethod]
        public void ModifyLocalTeam()
        {
            _matchLogic.AddMatch(_match);
            Team localTeam = new Team()
            {
                Name = "New Local team",

            };
            _sportLogic.AddTeamToSport(_match.Sport, localTeam);
            _match.Local = localTeam;
            _matchLogic.ModifyMatch(_match.Id, _match);
            Assert.AreEqual(_matchLogic.GetMatchById(_match.Id).Local, localTeam);
        }
        
        [TestMethod]
        [ExpectedException(typeof(InvalidSportDataException))]
        public void ModifyInvalidVisitorTeam()
        {
            _matchLogic.AddMatch(_match);
            Team visitorTeam = new Team()
            {
                Name = "New Visitor team",

            };
            _match.Visitor = visitorTeam;
            _matchLogic.ModifyMatch(_match.Id, _match);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidSportDataException))]
        public void ModifyInvalidLocalTeam()
        {
            _matchLogic.AddMatch(_match);
            Team localTeam = new Team()
            {
                Name = "New Local team",

            };
            _match.Local = localTeam;
            _matchLogic.ModifyMatch(_match.Id, _match);
        }

        //modify teams not of sport
        //add without local
        //add without visitor
        //delete match
    }
}
