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
using Sports.Domain.Exceptions;
using Sports.Logic.Exceptions;

namespace Sports.Logic.Test
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class TeamLogicTest
    {
        static string mypath = AppDomain.CurrentDomain.BaseDirectory;
        string testImagePath = mypath + "/TestImage/gun.png";
        private IRepositoryUnitOfWork unitOfWork;
        private RepositoryContext repository;
        private ITeamLogic teamLogic;
        private Team team;

        [TestInitialize]
        public void SetUp()
        {
            SetUpRepositories();
            team = new Team()
            {
                Name = "Team"
            };
        }

        private void SetUpRepositories()
        {
            var options = new DbContextOptionsBuilder<RepositoryContext>()
                .UseInMemoryDatabase<RepositoryContext>(databaseName: "TeamLogicTestDB")
                .Options;
            repository = new RepositoryContext(options);
            unitOfWork = new RepositoryUnitOfWork(repository);
            teamLogic = new TeamLogic(unitOfWork);
        }

        [TestCleanup]
        public void TearDown()
        {
            repository.Teams.RemoveRange(repository.Teams);
            repository.SaveChanges();
        }

        [TestMethod]
        public void AddTeam()
        {
            teamLogic.AddTeam(team);
            Assert.IsNotNull(teamLogic.GetTeamById(team.Id));
        }


        [TestMethod]
        [ExpectedException(typeof(InvalidNullValueException))]
        public void AddNullTeam()
        {
            teamLogic.AddTeam(null);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidEmptyTextFieldException))]
        public void AddInvalidTeam()
        {
            Team invalidNameTeam = new Team()
            {
                Name = ""
            };
            teamLogic.AddTeam(invalidNameTeam);
        }

        [TestMethod]
        public void AddTeamPicture()
        {
            teamLogic.AddTeam(team);
            teamLogic.SetPictureFromPath(team,testImagePath);
            Assert.IsNotNull(teamLogic.GetTeamById(team.Id).Picture);
        }


        [TestMethod]
        [ExpectedException(typeof(TeamDoesNotExistException))]
        public void AddPictureToInvalidTeam()
        {
            teamLogic.SetPictureFromPath(team, testImagePath);
            Assert.IsNotNull(teamLogic.GetTeamById(team.Id).Picture);
        }


        [TestMethod]
        public void ChangeTeamName()
        {
            teamLogic.AddTeam(team);
            Team changeTeam = new Team()
            {
                Name = "New Name"
            };
            teamLogic.Modify(team.Id, changeTeam);
            Assert.AreEqual<string>(teamLogic.GetTeamById(team.Id).Name,team.Name);
        }

        [TestMethod]
        public void ChangeTeamNameNull()
        {
            teamLogic.AddTeam(team);
            Team changeTeam = new Team()
            {
                Name = null
            };
            teamLogic.Modify(team.Id, changeTeam);
            Assert.AreNotEqual<string>(teamLogic.GetTeamById(team.Id).Name, changeTeam.Name);
        }

        [TestMethod]
        public void ChangeTeamNameInvalid()
        {
            teamLogic.AddTeam(team);
            Team changeTeam = new Team()
            {
                Name = ""
            };
            teamLogic.Modify(team.Id, changeTeam);
            Assert.AreNotEqual<string>(teamLogic.GetTeamById(team.Id).Name, changeTeam.Name);
        }

        [TestMethod]
        public void DeleteTeam()
        {
            teamLogic.AddTeam(team);
            teamLogic.Delete(team);
            Assert.AreEqual(teamLogic.GetAll().Count, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(TeamDoesNotExistException))]
        public void DeleteInvalidTeam()
        {
            teamLogic.Delete(team);
        }
        
    }
}
