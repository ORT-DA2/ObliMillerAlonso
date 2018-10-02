﻿using System;
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
        private IUserLogic userLogic;
        private ISessionLogic sessionLogic;
        private Team team;
        private User user;

        [TestInitialize]
        public void SetUp()
        {
            SetUpRepositories();
            team = new Team()
            {
                Name = "Team"
            };
            user = ValidUser();
            userLogic.AddUser(user);
            Guid token = sessionLogic.LogInUser(user.UserName, user.Password);
            teamLogic.SetSession(token);
        }

        private void SetUpRepositories()
        {
            var options = new DbContextOptionsBuilder<RepositoryContext>()
                .UseInMemoryDatabase<RepositoryContext>(databaseName: "TeamLogicTestDB")
                .Options;
            repository = new RepositoryContext(options);
            unitOfWork = new RepositoryUnitOfWork(repository);
            teamLogic = new TeamLogic(unitOfWork);
            userLogic = new UserLogic(unitOfWork);
            sessionLogic = new SessionLogic(unitOfWork);
        }

        [TestCleanup]
        public void TearDown()
        {
            repository.Teams.RemoveRange(repository.Teams);
            repository.Users.RemoveRange(repository.Users);
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

        private User ValidUser()
        {
            return new User(true)
            {
                FirstName = "Itai",
                LastName = "Miller",
                Email = "itaimiller@gmail.com",
                UserName = "iMiller",
                Password = "root"
            };
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

        [TestMethod]
        [ExpectedException(typeof(NonAdminException))]
        public void TeamSetSessionNonAdminUser()
        {
            User user = new User()
            {
                FirstName = "Itai",
                LastName = "Miller",
                Email = "itaimiller@gmail.com",
                UserName = "newUser",
                Password = "root"
            };
            userLogic.AddUser(user);
            Guid token = sessionLogic.LogInUser(user.UserName, user.Password);
            sessionLogic.GetUserFromToken(token);
            teamLogic.SetSession(token);
            teamLogic.AddTeam(team);
            teamLogic.Modify(team.Id, team);
            teamLogic.Delete(team);
        }

        [TestMethod]
        public void FilterOrderTeamName()
        {
            Team otherTeam = new Team()
            {
                Name = "TeamName"
            };
            teamLogic.AddTeam(otherTeam);
            ICollection<Team> filteredTeams = teamLogic.FilterOrderTeamName("TeamName");
            Assert.AreEqual(filteredTeams.Count, 1);
        }

        [TestMethod]
        public void FilterOrderTeamNameDesc()
        {
            Team otherTeam = new Team()
            {
                Name = "TeamName"
            };
            string order = "desc";
            teamLogic.AddTeam(otherTeam);
            ICollection<Team> filteredTeams = teamLogic.FilterOrderTeamName("TeamName", order);
            Assert.AreEqual(filteredTeams.Count, 1);
        }

        [TestMethod]
        public void InvalidFilterOrderTeamName()
        {
            Team otherTeam = new Team()
            {
                Name = "TeamName"
            };
            teamLogic.AddTeam(otherTeam);
            ICollection<Team> filteredTeams = teamLogic.FilterOrderTeamName(null);
            Assert.AreEqual(filteredTeams.Count, 1);
        }

    }
}
