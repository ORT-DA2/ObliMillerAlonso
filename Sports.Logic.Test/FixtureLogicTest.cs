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
using Sports.Repository.UnitOfWork;
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
        ICollection<Sport> sports;

        //pasar a json
        string validImplementationsPath = "C:/Users/pepe1/Documentos/Diseno 2/ObliMillerAlonso/FixtureImplementations/bin/Debug/netcoreapp2.1";
        string failingImplementationsPath = "C:/Users/pepe1/Documentos/Diseno 2/ObliMillerAlonso/FailingFixtureImplementations/bin/Debug/netcoreapp2.1";

        [TestInitialize]
        public void SetUp()
        {
            SetUpRepositories();
            SetUpAdminSession();
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
            fixtureLogic.SetSession(adminToken);
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
            fixtureLogic.GenerateFixture(sports,DateTime.Now);
            ICollection<Match> matches = matchLogic.GetAllMatches();
            Assert.AreEqual(30,matches.Count);
        }


        [TestMethod]
        [ExpectedException(typeof(NoImportedFixtureStrategiesException))]
        public void GenerateWithoutFixtureImplementations()
        {
            sports = sportLogic.GetAll();
            fixtureLogic.GenerateFixture(sports, DateTime.Now);
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
            fixtureLogic.GenerateFixture(null, DateTime.Now);
        }

        [TestMethod]
        [ExpectedException(typeof(SportDoesNotExistException))]
        public void GenerateFixtureForInvalidSport()
        {
            sports = sportLogic.GetAll();
            fixtureLogic.AddFixtureImplementations(validImplementationsPath);
            Sport testSport = new Sport();
            sports.Add(testSport);
            fixtureLogic.GenerateFixture(sports, DateTime.Now);
        }

        [TestMethod]
        [ExpectedException(typeof(MalfunctioningImplementationException))]
        public void GenerateWithMalfunctioningFixture()
        {
            fixtureLogic.AddFixtureImplementations(failingImplementationsPath);
            sports = sportLogic.GetAll();
            fixtureLogic.GenerateFixture(sports, DateTime.Now);
        }

        [TestMethod]
        public void TestBackAndForthFixtureDailyNoMatchesOnSameDay()
        {
            fixtureLogic.AddFixtureImplementations(validImplementationsPath);
            sports = sportLogic.GetAll();
            fixtureLogic.GenerateFixture(sports, DateTime.Now);
            ICollection<Match> matches = matchLogic.GetAllMatches();
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
            fixtureLogic.GenerateFixture(sports, DateTime.Now);
            ICollection<Match> matches = matchLogic.GetAllMatches();
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
            fixtureLogic.GenerateFixture(sports, DateTime.Now);
            ICollection<Match> matches = matchLogic.GetAllMatches();
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
            return new User()
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
            User user = ValidUser();
            userLogic.AddUser(user);
            Guid token = sessionLogic.LogInUser(user.UserName, user.Password);
            sessionLogic.GetUserFromToken(token);
            fixtureLogic.SetSession(token);
            fixtureLogic.AddFixtureImplementations(validImplementationsPath);
        }


        [TestMethod]
        [ExpectedException(typeof(InvalidMatchDateFormatException))]
        public void GenerateFixtureWithInvalidDate()
        {
            fixtureLogic.AddFixtureImplementations(validImplementationsPath);
            sports = sportLogic.GetAll();
            fixtureLogic.GenerateFixture(sports, DateTime.Now.AddDays(-1));
        }

    }
}
