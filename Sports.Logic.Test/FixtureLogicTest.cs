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
            fixtureLogic = new FixtureLogic();
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
            AddTeam(sport, "ForthTeam Team");
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
            fixtureLogic.AddFixtureImplementations("C:/Users/Rafael/Documents/Diseno2/MillerAlonso/FixtureDlls");
            ICollection<Sport> sports = sportLogic.GetAll();
            ICollection<Match> matches = fixtureLogic.GenerateFixture(sports);
            Assert.AreEqual(12,matches.Count);
        }


        [TestMethod]
        [ExpectedException(typeof(NoFixturesImportedException))]
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
        [ExpectedException(typeof(FixtureImportingException))]
        public void FixtureImplementationUnknownException()
        {
            fixtureLogic.AddFixtureImplementations(null);
        }

    }
}
