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

        [TestInitialize]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<RepositoryContext>()
                .UseInMemoryDatabase<RepositoryContext>(databaseName: "FixtureLogicTestDB")
                .Options;
            repository = new RepositoryContext(options);
            unit = new RepositoryUnitOfWork(repository);
            fixtureLogic = new FixtureLogic(unit);
            sportLogic = new SportLogic(unit);
            matchLogic = new MatchLogic(unit);
            Sport sport = new Sport()
            {
                Name = "Match Sport"
            };
            sportLogic.AddSport(sport);
            AddTeam(sport,"First Team");
            AddTeam(sport, "Second Team");
            AddTeam(sport, "Third Team");
            AddTeam(sport, "Forth Team");
            AddTeam(sport, "Fifth Team");
            AddTeam(sport, "Sixth Team");
            AddTeam(sport, "Seventh Team");
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
            fixtureLogic.AddFixtureImplementations("C:/Users/Rafael/Documents/Diseno2/MillerAlonso/FixtureImplementations/bin/Debug/netcoreapp2.1");
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
            fixtureLogic.AddFixtureImplementations("C:/Users/Rafael/Documents/Diseno2/MillerAlonso/FixtureDlls");
            ICollection<Match> matches = fixtureLogic.GenerateFixture(null);
        }

        [TestMethod]
        [ExpectedException(typeof(SportDoesNotExistException))]
        public void GenerateFixtureForInvalidSport()
        {
            ICollection<Sport> sports = sportLogic.GetAll();
            fixtureLogic.AddFixtureImplementations("C:/Users/Rafael/Documents/Diseno2/MillerAlonso/FixtureDlls");
            Sport testSport = new Sport();
            sports.Add(testSport);
            ICollection<Match> matches = fixtureLogic.GenerateFixture(sports);
        }

        [TestMethod]
        [ExpectedException(typeof(MalfunctioningImplementationException))]
        public void GenerateWithMalfunctioningFixture()
        {
            fixtureLogic.AddFixtureImplementations("C:/Users/Rafael/Documents/Diseno2/MillerAlonso/FailingFixtureDll");
            ICollection<Sport> sports = sportLogic.GetAll();
            ICollection<Match> matches = fixtureLogic.GenerateFixture(sports);
        }

        [TestMethod]
        public void TestBackAndForthFixtureDailyNoMatchesOnSameDay()
        {
            fixtureLogic.AddFixtureImplementations("C:/Users/Rafael/Documents/Diseno2/MillerAlonso/FixtureImplementations/bin/Debug/netcoreapp2.1");
            ICollection<Sport> sports = sportLogic.GetAll();
            ICollection<Match> matches = fixtureLogic.GenerateFixture(sports);
            int invalidMatches = 0;
            foreach(Match match in matches)
            {
                invalidMatches+= matches.Where(m => m.Date.Equals(match.Date) 
                &&!m.Visitor.Equals(match.Visitor)&&!m.Local.Equals(match.Local))
                .ToList().Count;
            }
            Assert.AreEqual(0, invalidMatches);
        }

        [TestMethod]
        public void ChangeFixture()
        {
            fixtureLogic.AddFixtureImplementations("C:/Users/Rafael/Documents/Diseno2/MillerAlonso/FixtureImplementations/bin/Debug/netcoreapp2.1");
            fixtureLogic.ChangeFixtureImplementation();
            ICollection<Sport> sports = sportLogic.GetAll();
            ICollection<Match> matches = fixtureLogic.GenerateFixture(sports);
            Assert.AreEqual(21, matches.Count);
        }

    }
}
