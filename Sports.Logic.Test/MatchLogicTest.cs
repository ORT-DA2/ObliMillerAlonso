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
using Sports.Repository.UnitOfWork;
using Sports.Logic.Exceptions;
using Sports.Domain.Exceptions;
using System.Linq;

namespace Sports.Logic.Test
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class MatchLogicTest
    {
        private IRepositoryUnitOfWork unit;
        private RepositoryContext repository;
        private IMatchLogic matchLogic;
        private ISportLogic sportLogic;
        private IUserLogic userLogic;
        private ICommentLogic commentLogic;
        private ISessionLogic sessionLogic;
        private Match match;
        private User user;

        [TestInitialize]
        public void SetUp()
        {
            SetupRepositories();
            SetUpAdminSession();
            CreateBaseDataForTests();
            user = ValidUser();
            userLogic.AddUser(user);
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
            IUserRepository repo = unit.User;
            repo.Create(admin);
            repo.Save();
            Guid adminToken = sessionLogic.LogInUser(admin.UserName, admin.Password);
            sessionLogic.GetUserFromToken(adminToken);
            userLogic.SetSession(adminToken);
            matchLogic.SetSession(adminToken);
            sportLogic.SetSession(adminToken);
            commentLogic.SetSession(adminToken);
        }

        private void CreateBaseDataForTests()
        {
            Sport sport = new Sport()
            {
                Name = "Match Sport"
            };
            sportLogic.AddSport(sport);
            Team localTeam = AddTeamToSport(sport, "Local team");
            Team visitorTeam = AddTeamToSport(sport, "Visitor team");
            match = new Match()
            {
                Sport = sport,
                Local = localTeam,
                Visitor = visitorTeam,
                Date = DateTime.Now.AddDays(1)
            };
        }

        private void SetupRepositories()
        {
            var options = new DbContextOptionsBuilder<RepositoryContext>()
                            .UseInMemoryDatabase<RepositoryContext>(databaseName: "MatchLogicTestDB")
                            .Options;
            repository = new RepositoryContext(options);
            unit = new RepositoryUnitOfWork(repository);
            matchLogic = new MatchLogic(unit);
            sportLogic = new SportLogic(unit);
            userLogic = new UserLogic(unit);
            commentLogic = new CommentLogic(unit);
            sessionLogic = new SessionLogic(unit);
        }

        private Team AddTeamToSport(Sport sport, string teamName)
        {
            Team team = new Team()
            {
                Name = teamName,
            };
            sportLogic.AddTeamToSport(sport, team);
            return team;
        }

        [TestCleanup]
        public void TearDown()
        {
            repository.Matches.RemoveRange(repository.Matches);
            repository.Teams.RemoveRange(repository.Teams);
            repository.Sports.RemoveRange(repository.Sports);
            repository.Users.RemoveRange(repository.Users);
            repository.Comments.RemoveRange(repository.Comments);
            repository.SaveChanges();
        }

        [TestMethod]
        public void AddFullMatch()
        {
            matchLogic.AddMatch(match);
            Assert.IsNotNull(matchLogic.GetMatchById(match.Id));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNullValueException))]
        public void AddNullMatch()
        {
            matchLogic.AddMatch(null);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidMatchDateFormatException))]
        public void AddMatchInvalidDate()
        {
            match.Date = DateTime.Now.AddDays(-1);
            matchLogic.AddMatch(match);
        }

        [TestMethod]
        [ExpectedException(typeof(TeamDoesNotExistInSportException))]
        public void AddMatchInvalidLocalTeam()
        {
            Team invalidTeam = new Team()
            {
                Name = "Unregistered team"
            };
            match.Local = invalidTeam;
            matchLogic.AddMatch(match);
        }

        [TestMethod]
        [ExpectedException(typeof(TeamDoesNotExistInSportException))]
        public void AddMatchInvalidVisitorTeam()
        {
            Team invalidTeam = new Team()
            {
                Name = "Unregistered team"
            };
            match.Visitor = invalidTeam;
            matchLogic.AddMatch(match);
        }


        [TestMethod]
        public void ModifyDate()
        {
            matchLogic.AddMatch(match);
            match.Date = DateTime.Now.AddDays(+2);
            matchLogic.ModifyMatch(match.Id, match);
            Assert.AreEqual(matchLogic.GetMatchById(match.Id).Date, match.Date);
        }


        [TestMethod]
        [ExpectedException(typeof(MatchDoesNotExistException))]
        public void ModifyInvalidMatch()
        {
            matchLogic.ModifyMatch(match.Id, match);
        }
        
        [TestMethod]
        [ExpectedException(typeof(SportDoesNotExistException))]
        public void AddMatchWithInvalidSport()
        {
            Sport sport = new Sport()
            {
                Name = "Test Sport"
            };
            match.Sport = sport;
            matchLogic.AddMatch(match);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidSportIsEmptyException))]
        public void AddMatchWithoutSport()
        {
            match.Sport = null;
            matchLogic.AddMatch(match);
        }
        
        [TestMethod]
        public void ModifySportAndTeams()
        {
            matchLogic.AddMatch(match);
            Sport sport = new Sport()
            {
                Name = "New Sport"
            };
            sportLogic.AddSport(sport);
            Team localTeam = AddTeamToSport(sport, "New Local team");
            Team visitorTeam = AddTeamToSport(sport, "New Visitor team");
            match.Sport = sport;
            match.Local = localTeam;
            match.Visitor = visitorTeam;
            matchLogic.ModifyMatch(match.Id, match);
            Assert.AreEqual(matchLogic.GetMatchById(match.Id).Sport, sport);
        }
        
        [TestMethod]
        [ExpectedException(typeof(SportDoesNotExistException))]
        public void ModifyInvalidSport()
        {
            matchLogic.AddMatch(match);
            Sport sport = new Sport()
            {
                Name = "Test Sport"
            };
            
            match.Sport = sport;
            matchLogic.ModifyMatch(match.Id, match);
        }

        [TestMethod]
        public void ModifyVisitorTeam()
        {
            matchLogic.AddMatch(match);
            Team visitorTeam = new Team()
            {
                Name = "New Visitor team",

            };
            sportLogic.AddTeamToSport(match.Sport, visitorTeam);
            match.Visitor = visitorTeam;
            matchLogic.ModifyMatch(match.Id, match);
            Assert.AreEqual(matchLogic.GetMatchById(match.Id).Visitor, visitorTeam);
        }

        [TestMethod]
        public void ModifyLocalTeam()
        {
            matchLogic.AddMatch(match);
            Team localTeam = new Team()
            {
                Name = "New Local team",

            };
            sportLogic.AddTeamToSport(match.Sport, localTeam);
            match.Local = localTeam;
            matchLogic.ModifyMatch(match.Id, match);
            Assert.AreEqual(matchLogic.GetMatchById(match.Id).Local, localTeam);
        }
        
        [TestMethod]
        [ExpectedException(typeof(TeamDoesNotExistInSportException))]
        public void ModifyInvalidVisitorTeam()
        {
            matchLogic.AddMatch(match);
            Team visitorTeam = new Team()
            {
                Name = "New Visitor team",

            };
            match.Visitor = visitorTeam;
            matchLogic.ModifyMatch(match.Id, match);
        }

        [TestMethod]
        [ExpectedException(typeof(TeamDoesNotExistInSportException))]
        public void ModifyInvalidLocalTeam()
        {
            matchLogic.AddMatch(match);
            Team localTeam = new Team()
            {
                Name = "New Local team",

            };
            match.Local = localTeam;
            matchLogic.ModifyMatch(match.Id, match);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidTeamIsEmptyException))]
        public void AddWithoutLocalTeam()
        {
            match.Local = null;
            matchLogic.AddMatch(match);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidTeamIsEmptyException))]
        public void AddWithoutVisitorTeam()
        {
            match.Visitor = null;
            matchLogic.AddMatch(match);
        }
        
        [TestMethod]
        public void ModifyIgnoresNullFields()
        {
            matchLogic.AddMatch(match);
            Team original = match.Local;
            Match updatedMatch = new Match()
            {
            };
            matchLogic.ModifyMatch(match.Id, updatedMatch);
            Assert.AreEqual(matchLogic.GetMatchById(match.Id).Local, original);
        }
        
        [TestMethod]
        public void DeleteMatch()
        {
            matchLogic.AddMatch(match);
            matchLogic.DeleteMatch(match);
            Assert.AreEqual(matchLogic.GetAllMatches().Count, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(MatchDoesNotExistException))]
        public void DeleteNonExistingMatch()
        {
            matchLogic.DeleteMatch(match);
        }


        [TestMethod]
        public void AddCommentToMatch()
        {
            matchLogic.AddMatch(match);
            Comment comment = new Comment
            {
                Text = "Text",
                User = user
            };
            matchLogic.AddCommentToMatch(match.Id, comment);
            Comment commentInMatchStored = matchLogic.GetAllComments(match.Id).FirstOrDefault();
            Assert.AreEqual(commentInMatchStored.Id, comment.Id);
        }

        private User ValidUser()
        {
            return new User(true)
            {
                FirstName = "Itai",
                LastName = "Miller",
                Email = "itaimiller@gmail.com",
                UserName = "iMiller",
                Password = "root"
            };
        }

        [TestMethod]
        [ExpectedException(typeof(MatchDoesNotExistException))]
        public void AddCommentToInexistentMatch()
        {
            Comment comment = new Comment
            {
                Text = "Text",
                User = user
            };
            matchLogic.AddCommentToMatch(match.Id, comment);
        }
        

        [TestMethod]
        public void CheckSportIsNotDuplicated()
        {
            matchLogic.AddMatch(match);
            Assert.AreEqual(sportLogic.GetAll().Count,1);
        }

        [TestMethod]
        [ExpectedException(typeof(MatchDoesNotExistException))]
        public void GetMatchesForTeamThatDidntPlay()
        {
            Sport sport = new Sport()
            {
                Name = "Unplayed Sport"
            };
            sportLogic.AddSport(sport);

            Team unplayedTeam = new Team()
            {
                Name = "New Local team"
            };
            sportLogic.AddTeamToSport(sport, unplayedTeam);
            matchLogic.GetAllMatchesForTeam(unplayedTeam);
        }


        [TestMethod]
        public void CascadeDeleteMatchFromSport()
        { 
            matchLogic.AddMatch(match);
            sportLogic.RemoveSport(match.Sport.Id);
            Assert.AreEqual(matchLogic.GetAllMatches().Count,0);
        }

        [TestMethod]
        public void CascadeDeleteMatchFromLocalTeam()
        {
            matchLogic.AddMatch(match);
            sportLogic.DeleteTeamFromSport(match.Sport,match.Local);
            Assert.AreEqual(matchLogic.GetAllMatches().Count, 0);
        }

        [TestMethod]
        public void CascadeDeleteMatchFromVisitorTeam()
        {
            matchLogic.AddMatch(match);
            sportLogic.DeleteTeamFromSport(match.Sport, match.Visitor);
            Assert.AreEqual(matchLogic.GetAllMatches().Count, 0);
        }

        [TestMethod]
        public void CascadeDeleteCommentsFromMatch()
        {
            matchLogic.AddMatch(match);
            Comment comment = new Comment
            {
                Text = "Text",
                User = user
            };
            matchLogic.AddCommentToMatch(match.Id, comment);
            matchLogic.DeleteMatch(match);
            Assert.AreEqual(commentLogic.GetAll().Count, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(NonAdminException))]
        public void MatchSetSessionNonAdminUser()
        {
            User user = new User()
            {
                FirstName = "Itai",
                LastName = "Miller",
                Email = "itaimiller@gmail.com",
                UserName = "newUser",
                Password = "root"
            };
            userLogic.AddUser(user);
            Guid token = sessionLogic.LogInUser(user.UserName, user.Password);
            sessionLogic.GetUserFromToken(token);
            matchLogic.SetSession(token);
            matchLogic.AddMatch(match);
        }


    }
}
