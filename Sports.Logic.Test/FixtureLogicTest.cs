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
        RepositoryContext repository;

        //pasar a json
        string validImplementationsPath = "C:/Users/pepe1/Documentos/Diseno 2/ObliMillerAlonso/FixtureImplementations/bin/Debug/netcoreapp2.1";
        string failingImplementationsPath = "C:/Users/pepe1/Documentos/Diseno 2/ObliMillerAlonso/FailingFixtureImplementations/bin/Debug/netcoreapp2.1";

        [TestInitialize]
        public void SetUp()
        {
            SetUpRepositories();
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
            AddTeam(sport, "Seventh Team");
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
            repository.Matches.RemoveRange(repository.Matches);
            repository.Sports.RemoveRange(repository.Sports);
            repository.Teams.RemoveRange(repository.Teams);
            repository.SaveChanges();
        }

        [TestMethod]
        public void GenerateFixture()
        {
            fixtureLogic.AddFixtureImplementations(validImplementationsPath);
            ICollection<Sport> sports = sportLogic.GetAll();
            ICollection<Match> matches = fixtureLogic.GenerateFixture(sports);
            Assert.AreEqual(42,matches.Count);
        }


        [TestMethod]
        [ExpectedException(typeof(NoImportedFixtureStrategiesException))]
        public void GenerateWithoutFixtureImplementations()
        {
            ICollection<Sport> sports = sportLogic.GetAll();
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
            ICollection<Sport> sports = sportLogic.GetAll();
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
            ICollection<Sport> sports = sportLogic.GetAll();
            ICollection<Match> matches = fixtureLogic.GenerateFixture(sports);
        }

        [TestMethod]
        public void TestBackAndForthFixtureDailyNoMatchesOnSameDay()
        {
            fixtureLogic.AddFixtureImplementations(validImplementationsPath);
            ICollection<Sport> sports = sportLogic.GetAll();
            ICollection<Match> matches = fixtureLogic.GenerateFixture(sports);
            int invalidMatches = 0;
            foreach(Match match in matches)
            {
                invalidMatches += matches.Where(m => m.Date.Equals(match.Date)
                 && (IsInMatch(match.Visitor, m) || IsInMatch(match.Local, m)) && (!m.Local.Equals(match.Local) || !m.Visitor.Equals(match.Visitor)))
                .ToList().Count;
            }
            Assert.AreEqual(0, invalidMatches);
        }

        [TestMethod]
        public void ChangeFixtureImplementation()
        {
            fixtureLogic.AddFixtureImplementations(validImplementationsPath);
            fixtureLogic.ChangeFixtureImplementation();
            ICollection<Sport> sports = sportLogic.GetAll();
            ICollection<Match> matches = fixtureLogic.GenerateFixture(sports);
            Assert.AreEqual(21, matches.Count);
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
            ICollection<Sport> sports = sportLogic.GetAll();
            ICollection<Match> matches = fixtureLogic.GenerateFixture(sports);
            int invalidMatches = 0;
            foreach(Match match in matches)
            {
                invalidMatches += matches.Where(m => m.Date.Equals(match.Date)
                 && (IsInMatch(match.Visitor,m) || IsInMatch(match.Local, m))&&(!m.Local.Equals(match.Local)||!m.Visitor.Equals(match.Visitor)))
                .ToList().Count;
            }
            invalidMatches+= matches.Where(m => !IsWeekend(m.Date))
                .ToList().Count;
            Assert.AreEqual(0, invalidMatches);
        }

        private bool IsWeekend(DateTime date)
        {
            return date.DayOfWeek.Equals(DayOfWeek.Sunday) || date.DayOfWeek.Equals(DayOfWeek.Saturday);
        }


        private static bool IsInMatch(Team team, Match match)
        {
            return match.Local.Equals(team) || match.Visitor.Equals(team);
        }
    }
}
