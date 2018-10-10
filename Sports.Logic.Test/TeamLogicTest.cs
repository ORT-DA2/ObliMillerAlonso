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
using Sports.Repository.UnitOfWork;
using Newtonsoft.Json.Linq;
using System.IO;

namespace Sports.Logic.Test
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class TeamLogicTest
    {
        string testImagePath;
        private IRepositoryUnitOfWork unitOfWork;
        private RepositoryContext repository;
        private ITeamLogic teamLogic;
        private IUserLogic userLogic;
        private ISportLogic sportLogic;
        private ISessionLogic sessionLogic;
        private Team team;
        private Sport sport;

        [TestInitialize]
        public void SetUp()
        {
            SetUpRepositories();
            SetUpAdminSession();
            sport = new Sport()
            {
                Name = "SportName"
            };
            sportLogic.AddSport(sport);
            team = new Team()
            {
                Name = "Team",
                Sport = sport
            };
            JObject jsonPaths = JObject.Parse(File.ReadAllText(@"testFilesPaths.json"));
            testImagePath = jsonPaths.SelectToken("TestImagePath").ToString();
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
            IUserRepository repo = unitOfWork.User;
            repo.Create(admin);
            repo.Save();
            Guid adminToken = sessionLogic.LogInUser(admin.UserName, admin.Password);
            sessionLogic.GetUserFromToken(adminToken);
            userLogic.SetSession(adminToken);
            teamLogic.SetSession(adminToken);
            sportLogic.SetSession(adminToken);
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
            sportLogic = new SportLogic(unitOfWork);
        }

        [TestCleanup]
        public void TearDown()
        {
            repository.Teams.RemoveRange(repository.Teams);
            repository.Sports.RemoveRange(repository.Sports);
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
            teamLogic.SetPictureFromPath(team.Id,testImagePath);
            Assert.IsNotNull(teamLogic.GetTeamById(team.Id).Picture);
        }


        [TestMethod]
        [ExpectedException(typeof(TeamDoesNotExistException))]
        public void AddPictureToInvalidTeam()
        {
            teamLogic.SetPictureFromPath(team.Id, testImagePath);
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
                Name = "New Name",
                Sport = sport
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
                Name = null,
                Sport = sport
            };
            teamLogic.Modify(team.Id, changeTeam);
            Assert.AreNotEqual<string>(teamLogic.GetTeamById(team.Id).Name, changeTeam.Name);
        }

        [TestMethod]
        [ExpectedException(typeof(TeamAlreadyInSportException))]
        public void ChangeTeamNameInvalid()
        {
            teamLogic.AddTeam(team);
            Team changeTeam = new Team()
            {
                Name = "Name",
                Sport = sport
            };
            teamLogic.AddTeam(changeTeam);
            teamLogic.Modify(team.Id, changeTeam);
        }

        [TestMethod]
        public void DeleteTeam()
        {
            teamLogic.AddTeam(team);
            teamLogic.Delete(team.Id);
            Assert.AreEqual(teamLogic.GetAll().Count, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(TeamDoesNotExistException))]
        public void DeleteInvalidTeam()
        {
            teamLogic.Delete(team.Id);
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
        }

        [TestMethod]
        public void FilterOrderTeamName()
        {
            Team otherTeam = new Team()
            {
                Name = "TeamName"
            };
            teamLogic.AddTeam(otherTeam);
            ICollection<Team> filteredTeams = teamLogic.GetFilteredTeams("TeamName","asc");
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
            ICollection<Team> filteredTeams = teamLogic.GetFilteredTeams("TeamName", order);
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
            ICollection<Team> filteredTeams = teamLogic.GetFilteredTeams(null,null);
            Assert.AreEqual(filteredTeams.Count, 1);
        }

    }
}
