using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Sports.Domain;
using Sports.Logic;
using Sports.Repository;
using Sports.Repository.Interface;
using Sports.Repository.Context;
using Sports.Exceptions;
using System.Diagnostics.CodeAnalysis;

namespace Sports.Logic.Test
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class TeamLogicTest
    {
        private IRepositoryWrapper _wrapper;
        private RepositoryContext _repository;
        private TeamLogic _teamLogic;

        [TestInitialize]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<RepositoryContext>()
                .UseInMemoryDatabase<RepositoryContext>(databaseName: "TeamLogicTestDB")
                .Options;
            _repository = new RepositoryContext(options);
            _wrapper = new RepositoryWrapper(_repository);
            _teamLogic = new TeamLogic(_wrapper);
        }

        [TestMethod]
        public void AddTeam()
        {
            Team team = new Team()
            {
                Name = "name"
            };
            _teamLogic.AddTeam(team);
            Assert.IsNotNull(_teamLogic.GetTeamById(team.Id));
        }
    }
}
