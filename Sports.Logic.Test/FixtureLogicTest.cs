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
        [TestMethod]
        public void GenerateFixture()
        {
            var options = new DbContextOptionsBuilder<RepositoryContext>()
               .UseInMemoryDatabase<RepositoryContext>(databaseName: "FixtureLogicTestDB")
               .Options;
            RepositoryContext _repository = new RepositoryContext(options);
            IRepositoryUnitOfWork _unit = new RepositoryUnitOfWork(_repository);
            IFixtureLogic _fixtureLogic = new FixtureLogic(_unit);
            ISportLogic _sportLogic = new SportLogic(_unit);
            IMatchLogic _matchLogic = new MatchLogic(_unit);
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
            _sportLogic.AddSport(sport);
            _sportLogic.AddTeamToSport(sport, localTeam);
            _sportLogic.AddTeamToSport(sport, visitorTeam);
            _fixtureLogic.AddFixtureImplementation("address");
            List<Sport> sports = new List<Sport>();
            sports.Add(sport);
            _fixtureLogic.GenerateFixture();
            Assert.Equals(_matchLogic.GetAllMatches().Count, 1);
        }
    }
}
