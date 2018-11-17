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
using Newtonsoft.Json.Linq;
using System.IO;

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
        string validImplementationsPath;
        string failingImplementationsPath;

        [TestInitialize]
        public void SetUp()
        {
            SetUpRepositories();
            SetUpAdminSession();
            fixtureLogic.ResetFixtureStrategies();
            SetUpSportWithCompetitors();
            JObject jsonPaths = JObject.Parse(File.ReadAllText(@"testFilesPaths.json"));
            failingImplementationsPath = jsonPaths.SelectToken("FailingFixtureDlls").ToString();
            validImplementationsPath = jsonPaths.SelectToken("FixtureDlls").ToString();
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

        private void SetUpSportWithCompetitors()
        {
            Sport sport = new Sport()
            {
                Name = "Match Sport",
                Amount = 2
            };
            sportLogic.AddSport(sport);
            AddCompetitor(sport, "First Competitor");
            AddCompetitor(sport, "Second Competitor");
            AddCompetitor(sport, "Third Competitor");
            AddCompetitor(sport, "Forth Competitor");
            AddCompetitor(sport, "Fifth Competitor");
            AddCompetitor(sport, "Sixth Competitor");
        }

        private void AddCompetitor(Sport sport, string competitorName)
        {
            Competitor competitor = new Competitor()
            {
                Name = competitorName,

            };
            sportLogic.AddCompetitorToSport(sport.Id, competitor);
        }

        [TestCleanup]
        public void TearDown()
        {
            repository.Users.RemoveRange(repository.Users);
            repository.Matches.RemoveRange(repository.Matches);
            repository.Sports.RemoveRange(repository.Sports);
            repository.Competitors.RemoveRange(repository.Competitors);
            repository.SaveChanges();
        }
        [TestMethod]
        public void GenerateFixture()
        {
            fixtureLogic.RefreshFixtureImplementations();
            sports = sportLogic.GetAll();
            fixtureLogic.GenerateFixture(1, sports,DateTime.Now);
            ICollection<Match> matches = matchLogic.GetAllMatches();
            Assert.AreNotEqual(0, matches.Count);
        }


        [TestMethod]
        [ExpectedException(typeof(NoImportedFixtureStrategiesException))]
        public void GenerateWithoutFixtureImplementations()
        {
            sports = sportLogic.GetAll();
            fixtureLogic.GenerateFixture(1, sports, DateTime.Now);
        }
        
        [TestMethod]
        [ExpectedException(typeof(InvalidNullValueException))]
        public void GenerateFixtureForNullSport()
        {
            fixtureLogic.RefreshFixtureImplementations();
            fixtureLogic.GenerateFixture(1, null, DateTime.Now);
        }

        [TestMethod]
        [ExpectedException(typeof(SportDoesNotExistException))]
        public void GenerateFixtureForInvalidSport()
        {
            sports = sportLogic.GetAll();
            fixtureLogic.RefreshFixtureImplementations();
            Sport testSport = new Sport();
            sports.Add(testSport);
            fixtureLogic.GenerateFixture(1, sports, DateTime.Now);
        }

        [TestMethod]
        [ExpectedException(typeof(MalfunctioningImplementationException))]
        public void GenerateWithMalfunctioningFixture()
        {
            fixtureLogic.RefreshFixtureImplementations();
            sports = sportLogic.GetAll();
            fixtureLogic.GenerateFixture(1, sports, DateTime.Now);
        }

        [TestMethod]
        public void TestBackAndForthFixtureDailyNoMatchesOnSameDay()
        {
            fixtureLogic.RefreshFixtureImplementations();
            sports = sportLogic.GetAll();
            fixtureLogic.GenerateFixture(1, sports, DateTime.Now);
            ICollection<Match> matches = matchLogic.GetAllMatches();
            int invalidMatches = 0;
            foreach(Match match in matches)
            {
                invalidMatches += MatchesWhereCompetitorPlaysTwice(matches, match).Count;
            }
            Assert.AreEqual(0, invalidMatches);
        }

        [TestMethod]
        public void ChangeFixtureImplementation()
        {
            fixtureLogic.RefreshFixtureImplementations();
            sports = sportLogic.GetAll();
            fixtureLogic.GenerateFixture(2, sports, DateTime.Now);
            ICollection<Match> matches = matchLogic.GetAllMatches();
            Assert.AreEqual(15, matches.Count);
        }
        
        [TestMethod]
        [ExpectedException(typeof(NoImportedFixtureStrategiesException))]
        public void ChangeImplementationNoImports()
        {
            fixtureLogic.GenerateFixture(2, sports, DateTime.Now);
        }

        [TestMethod]
        public void TestFixtureWeekendMatchesOnlyOnWeekends()
        {
            fixtureLogic.RefreshFixtureImplementations();
            sports = sportLogic.GetAll();
            fixtureLogic.GenerateFixture(2, sports, DateTime.Now);
            ICollection<Match> matches = matchLogic.GetAllMatches();
            int invalidMatches = 0;
            foreach(Match match in matches)
            {
                invalidMatches += MatchesWhereCompetitorPlaysTwice(matches, match).Count;
            }
            invalidMatches += matches.Where(m => !IsWeekend(m.Date)).ToList().Count;
            Assert.AreEqual(0, invalidMatches);
        }

        private List<Match> MatchesWhereCompetitorPlaysTwice(ICollection<Match> matches, Match match)
        {
            return matches.Where(m => m.Date.Equals(match.Date)
                             && (m.Competitors.Intersect(match.Competitors).Count() > 0)
                             && !m.Equals(match)).ToList();
        }

        private bool IsWeekend(DateTime date)
        {
            return date.DayOfWeek.Equals(DayOfWeek.Sunday) || date.DayOfWeek.Equals(DayOfWeek.Saturday);
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
            fixtureLogic.RefreshFixtureImplementations();
        }


        [TestMethod]
        [ExpectedException(typeof(InvalidMatchDateFormatException))]
        public void GenerateFixtureWithInvalidDate()
        {
            fixtureLogic.RefreshFixtureImplementations();
            sports = sportLogic.GetAll();
            fixtureLogic.GenerateFixture(1, sports, DateTime.Now.AddDays(-1));
        }

    }
}
