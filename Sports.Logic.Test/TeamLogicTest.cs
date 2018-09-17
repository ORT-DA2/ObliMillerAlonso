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
using Sports.Exceptions;
using System.Diagnostics.CodeAnalysis;

namespace Sports.Logic.Test
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class TeamLogicTest
    {
        static string mypath = AppDomain.CurrentDomain.BaseDirectory;
        string _testImagePath = mypath + "/TestImage/gun.png";
        private IRepositoryWrapper _wrapper;
        private RepositoryContext _repository;
        private ITeamLogic _teamLogic;
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
        [ExpectedException(typeof(InvalidTeamDataException))]
        public void AddNullTeam()
        {
            _teamLogic.AddTeam(null);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidTeamDataException))]
        public void AddInvalidTeam()
        {
            Team invalidNameTeam = new Team()
            {
                Name = ""
            };
            _teamLogic.AddTeam(invalidNameTeam);
        }

        [TestMethod]
        public void AddTeamPicture()
        {
            _teamLogic.AddTeam(_team);
            _teamLogic.SetPictureFromPath(_team,_testImagePath);
            Assert.IsNotNull(_teamLogic.GetTeamById(_team.Id).Picture);
        }


        [TestMethod]
        [ExpectedException(typeof(InvalidTeamDataException))]
        public void AddPictureToInvalidTeam()
        {
            _teamLogic.SetPictureFromPath(_team, _testImagePath);
            Assert.IsNotNull(_teamLogic.GetTeamById(_team.Id).Picture);
        }


        [TestMethod]
        public void ChangeTeamName()
        {
            _teamLogic.AddTeam(_team);
            _team.Name = "new name";
            _teamLogic.Modify(_team);
            Assert.AreEqual<string>(_teamLogic.GetTeamById(_team.Id).Name,_team.Name);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidTeamDataException))]
        public void ChangeTeamNameInvalid()
        {
            _teamLogic.AddTeam(_team);
            _team.Name = "";
            _teamLogic.Modify(_team);
        }

        [TestMethod]
        public void DeleteTeam()
        {
            _teamLogic.AddTeam(_team);
            _teamLogic.Delete(_team);
            Assert.AreEqual(_teamLogic.GetAll().Count, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidTeamDataException))]
        public void DeleteInvalidTeam()
        {
            _teamLogic.Delete(_team);
        }
        
    }
}
