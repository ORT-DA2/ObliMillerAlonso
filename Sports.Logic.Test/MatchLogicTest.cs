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
using Sports.Logic.Exceptions;
using Sports.Domain.Exceptions;

namespace Sports.Logic.Test
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class MatchLogicTest
    {
        private IRepositoryUnitOfWork _unit;
        private RepositoryContext _repository;
        private IMatchLogic _matchLogic;
        private ISportLogic _sportLogic;
        private IUserLogic _userLogic;
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
            _userLogic = new UserLogic(_unit);
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
        [ExpectedException(typeof(InvalidNullValueException))]
        public void AddNullMatch()
        {
            _matchLogic.AddMatch(null);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidMatchDateFormatException))]
        public void AddMatchInvalidDate()
        {
            _match.Date = DateTime.Now.AddDays(-1);
            _matchLogic.AddMatch(_match);
        }

        [TestMethod]
        [ExpectedException(typeof(TeamDoesNotExistInSportException))]
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
        [ExpectedException(typeof(TeamDoesNotExistInSportException))]
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
        [ExpectedException(typeof(MatchDoesNotExistException))]
        public void ModifyInvalidMatch()
        {
            _matchLogic.ModifyMatch(_match.Id, _match);
        }
        
        [TestMethod]
        [ExpectedException(typeof(SportDoesNotExistException))]
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
        [ExpectedException(typeof(InvalidSportIsEmptyException))]
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
        [ExpectedException(typeof(SportDoesNotExistException))]
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
        [ExpectedException(typeof(TeamDoesNotExistInSportException))]
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
        [ExpectedException(typeof(TeamDoesNotExistInSportException))]
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

        [TestMethod]
        [ExpectedException(typeof(InvalidTeamIsEmptyException))]
        public void AddWithoutLocalTeam()
        {
            _match.Local = null;
            _matchLogic.AddMatch(_match);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidTeamIsEmptyException))]
        public void AddWithoutVisitorTeam()
        {
            _match.Visitor = null;
            _matchLogic.AddMatch(_match);
        }
        
        [TestMethod]
        public void ModifyIgnoresNullFields()
        {
            _matchLogic.AddMatch(_match);
            Team original = _match.Local;
            Match updatedMatch = new Match()
            {
            };
            _matchLogic.ModifyMatch(_match.Id, updatedMatch);
            Assert.AreEqual(_matchLogic.GetMatchById(_match.Id).Local, original);
        }
        
        [TestMethod]
        public void DeleteMatch()
        {
            _matchLogic.AddMatch(_match);
            _matchLogic.DeleteMatch(_match);
            Assert.AreEqual(_matchLogic.GetAllMatches().Count, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(MatchDoesNotExistException))]
        public void DeleteNonExistingMatch()
        {
            _matchLogic.DeleteMatch(_match);
        }


        [TestMethod]
        public void AddCommentToMatch()
        {
            _matchLogic.AddMatch(_match);
            User user = new User()
            {
                FirstName = "Itai",
                LastName = "Miller",
                Email = "itaimiller@gmail.com",
                UserName = "iMiller",
                Password = "root"
            };
            _userLogic.AddUser(user);
            Comment comment = new Comment
            {
                Text = "Text",
                User = user
            };
            _matchLogic.AddCommentToMatch(_match.Id, comment);
            Assert.AreEqual(_matchLogic.GetAllComments(_match.Id).Count, 1);
        }

        [TestMethod]
        [ExpectedException(typeof(MatchDoesNotExistException))]
        public void AddCommentToInexistentMatch()
        {
            User user = new User()
            {
                FirstName = "Itai",
                LastName = "Miller",
                Email = "itaimiller@gmail.com",
                UserName = "iMiller",
                Password = "root"
            };
            _userLogic.AddUser(user);
            Comment comment = new Comment
            {
                Text = "Text",
                User = user
            };
            _matchLogic.AddCommentToMatch(_match.Id, comment);
        }

        [TestMethod]
        [ExpectedException(typeof(UserDoesNotExistException))]
        public void AddCommentWithInexistentUserToMatch()
        {
            _matchLogic.AddMatch(_match);
            User user = new User()
            {
                FirstName = "Itai",
                LastName = "Miller",
                Email = "itaimiller@gmail.com",
                UserName = "iMiller",
                Password = "root"
            };
            Comment comment = new Comment
            {
                Text = "Text",
                User = user
            };
            _matchLogic.AddCommentToMatch(_match.Id, comment);
        }

        //verify add team/ sport/ comment no duplica datos
    }
}
