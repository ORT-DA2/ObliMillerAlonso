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
using System.Linq;

namespace Sports.Logic.Test
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class FixtureLogicTest
    {
        IRepositoryUnitOfWork unit;
        IFixtureLogic fixtureLogic;
        ISportLogic sportLogic;
        IMatchLogic matchLogic;
        IUserLogic userLogic;
        ISessionLogic sessionLogic;
        RepositoryContext repository;
        private User user;
        ICollection<Sport> sports;

        //pasar a json
        string validImplementationsPath = "C:/Users/pepe1/Documentos/Diseno 2/ObliMillerAlonso/FixtureImplementations/bin/Debug/netcoreapp2.1";
        string failingImplementationsPath = "C:/Users/pepe1/Documentos/Diseno 2/ObliMillerAlonso/FailingFixtureImplementations/bin/Debug/netcoreapp2.1";

        [TestInitialize]
        public void SetUp()
        {
            SetUpRepositories();
            user = ValidUser();
            userLogic.AddUser(user);
            Guid token = sessionLogic.LogInUser(user.UserName, user.Password);
            fixtureLogic.SetSession(token);
            sportLogic.SetSession(token);
            matchLogic.SetSession(token);
            SetUpSportWithTeams();
        }

        private void SetUpRepositories()
        {
            var options = new DbContextOptionsBuilder<RepositoryContext>()
                            .UseInMemoryDatabase<RepositoryContext>(databaseName: "FixtureLogicTestDB")
                            .Options;
            repository = new RepositoryContext(options);
            unit = new RepositoryUnitOfWork(repository);
            fixtureLogic = new FixtureLogic(unit);
            sportLogic = new SportLogic(unit);
            matchLogic = new MatchLogic(unit);
            userLogic = new UserLogic(unit);
            sessionLogic = new SessionLogic(unit);
        }

        private void SetUpSportWithTeams()
        {
            Sport sport = new Sport()
            {
                Name = "Match Sport"
            };
            sportLogic.AddSport(sport);
            AddTeam(sport, "First Team");
            AddTeam(sport, "Second Team");
            AddTeam(sport, "Third Team");
            AddTeam(sport, "Forth Team");
            AddTeam(sport, "Fifth Team");
            AddTeam(sport, "Sixth Team");
        }

        private void AddTeam(Sport sport, string teamName)
        {
            Team team = new Team()
            {
                Name = teamName,

            };
            sportLogic.AddTeamToSport(sport, team);
        }

        [TestCleanup]
        public void TearDown()
        {
            repository.Users.RemoveRange(repository.Users);
            repository.Matches.RemoveRange(repository.Matches);
            repository.Sports.RemoveRange(repository.Sports);
            repository.Teams.RemoveRange(repository.Teams);
            repository.SaveChanges();
        }

        [TestMethod]
        public void GenerateFixture()
        {
            fixtureLogic.AddFixtureImplementations(validImplementationsPath);
            sports = sportLogic.GetAll();
            ICollection<Match> matches = fixtureLogic.GenerateFixture(sports);
            Assert.AreEqual(30,matches.Count);
        }


        [TestMethod]
        [ExpectedException(typeof(NoImportedFixtureStrategiesException))]
        public void GenerateWithoutFixtureImplementations()
        {
            sports = sportLogic.GetAll();
            ICollection<Match> matches = fixtureLogic.GenerateFixture(sports);
        }

        [TestMethod]
        [ExpectedException(typeof(FixtureImportingException))]
        public void AddInvalidPathFixtureImplementation()
        {
            fixtureLogic.AddFixtureImplementations("InvalidPath");
        }
        
        
        [TestMethod]
        [ExpectedException(typeof(InvalidNullValueException))]
        public void GenerateFixtureForNullSport()
        {
            fixtureLogic.AddFixtureImplementations(validImplementationsPath);
            ICollection<Match> matches = fixtureLogic.GenerateFixture(null);
        }

        [TestMethod]
        [ExpectedException(typeof(SportDoesNotExistException))]
        public void GenerateFixtureForInvalidSport()
        {
            sports = sportLogic.GetAll();
            fixtureLogic.AddFixtureImplementations(validImplementationsPath);
            Sport testSport = new Sport();
            sports.Add(testSport);
            ICollection<Match> matches = fixtureLogic.GenerateFixture(sports);
        }

        [TestMethod]
        [ExpectedException(typeof(MalfunctioningImplementationException))]
        public void GenerateWithMalfunctioningFixture()
        {
            fixtureLogic.AddFixtureImplementations(failingImplementationsPath);
            sports = sportLogic.GetAll();
            ICollection<Match> matches = fixtureLogic.GenerateFixture(sports);
        }

        [TestMethod]
        public void TestBackAndForthFixtureDailyNoMatchesOnSameDay()
        {
            fixtureLogic.AddFixtureImplementations(validImplementationsPath);
            sports = sportLogic.GetAll();
            ICollection<Match> matches = fixtureLogic.GenerateFixture(sports);
            int invalidMatches = 0;
            foreach(Match match in matches)
            {
                invalidMatches += MatchesWhereTeamPlaysTwice(matches, match).Count;
            }
            Assert.AreEqual(0, invalidMatches);
        }

        [TestMethod]
        public void ChangeFixtureImplementation()
        {
            fixtureLogic.AddFixtureImplementations(validImplementationsPath);
            fixtureLogic.ChangeFixtureImplementation();
            sports = sportLogic.GetAll();
            ICollection<Match> matches = fixtureLogic.GenerateFixture(sports);
            Assert.AreEqual(15, matches.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(NoImportedFixtureStrategiesException))]
        public void ChangeImplementationNoImports()
        {
            fixtureLogic.ChangeFixtureImplementation();
        }

        [TestMethod]
        public void TestFixtureWeekendMatchesOnlyOnWeekends()
        {
            fixtureLogic.AddFixtureImplementations(validImplementationsPath);
            fixtureLogic.ChangeFixtureImplementation();
            sports = sportLogic.GetAll();
            ICollection<Match> matches = fixtureLogic.GenerateFixture(sports);
            int invalidMatches = 0;
            foreach(Match match in matches)
            {
                invalidMatches += MatchesWhereTeamPlaysTwice(matches, match).Count;
            }
            invalidMatches += matches.Where(m => !IsWeekend(m.Date)).ToList().Count;
            Assert.AreEqual(0, invalidMatches);
        }

        private List<Match> MatchesWhereTeamPlaysTwice(ICollection<Match> matches, Match match)
        {
            return matches.Where(m => m.Date.Equals(match.Date)
                             && (IsInMatch(match.Visitor, m) || IsInMatch(match.Local, m))
                             && !m.Equals(match)).ToList();
        }

        private bool IsWeekend(DateTime date)
        {
            return date.DayOfWeek.Equals(DayOfWeek.Sunday) || date.DayOfWeek.Equals(DayOfWeek.Saturday);
        }


        private bool IsInMatch(Team team, Match match)
        {
            return match.Local.Equals(team) || match.Visitor.Equals(team);
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
        [ExpectedException(typeof(NonAdminException))]
        public void FixtureSetSessionNonAdminUser()
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
            fixtureLogic.SetSession(token);
            fixtureLogic.AddFixtureImplementations(validImplementationsPath);
            fixtureLogic.ChangeFixtureImplementation();
            fixtureLogic.GenerateFixture(sports);
           
        }

    }
}
