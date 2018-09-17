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
        private Team _team;

        [TestInitialize]
        public void SetUp()
        {
            _team = new Team()
            {
                Name = "Team"
            };
            var options = new DbContextOptionsBuilder<RepositoryContext>()
                .UseInMemoryDatabase<RepositoryContext>(databaseName: "TeamLogicTestDB")
                .Options;
            _repository = new RepositoryContext(options);
            _wrapper = new RepositoryWrapper(_repository);
            _teamLogic = new TeamLogic(_wrapper);
        }

        [TestCleanup]
        public void TearDown()
        {
            _repository.Teams.RemoveRange(_repository.Teams);
            _repository.SaveChanges();
        }

        [TestMethod]
        public void AddTeam()
        {
            _teamLogic.AddTeam(_team);
            Assert.IsNotNull(_teamLogic.GetTeamById(_team.Id));
        }


        [TestMethod]
        [ExpectedException(typeof(InvalidUserDataException))]
        public void AddNullTeam()
        {
            _teamLogic.AddTeam(null);
        }
    }
}
