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
using Sports.Repository.UnitOfWork;
using Sports.Logic.Exceptions;
using Sports.Domain.Exceptions;
using System.Diagnostics.CodeAnalysis;

namespace Sports.Logic.Test
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class SportLogicTest
    {
        private IRepositoryUnitOfWork unitOfWork;
        private RepositoryContext repository;
        private ISportLogic sportLogic;
        private ITeamLogic teamLogic;
        private IUserLogic userLogic;
        private ISessionLogic sessionLogic;
        Sport sport;
        
        
        [TestInitialize]
        public void SetUp()
        {
            SetUpRepositories();
            SetUpAdminSession();
            sport = new Sport()
            {
                Name = "Tennis"
            };
        }

        private void SetUpRepositories()
        {
            var options = new DbContextOptionsBuilder<RepositoryContext>()
                .UseInMemoryDatabase<RepositoryContext>(databaseName: "SportLogicTestDB")
                .Options;
            repository = new RepositoryContext(options);
            unitOfWork = new RepositoryUnitOfWork(repository);
            sportLogic = new SportLogic(unitOfWork);
            teamLogic = new TeamLogic(unitOfWork);
            userLogic = new UserLogic(unitOfWork);
            sessionLogic = new SessionLogic(unitOfWork);
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
            sportLogic.SetSession(adminToken);
            teamLogic.SetSession(adminToken);
        }

        [TestCleanup]
        public void TearDown()
        {
            repository.Users.RemoveRange(repository.Users);
            repository.Sports.RemoveRange(repository.Sports);
            repository.SaveChanges();
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
        public void AddSport()
        {
            sportLogic.AddSport(sport);
            Assert.IsNotNull(sportLogic.GetSportById(sport.Id));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNullValueException))]
        public void AddNullSport()
        {
            sportLogic.AddSport(null);
        }

        [TestMethod]
        public void UpdateSportName()
        {
            sportLogic.AddSport(sport);
            Sport sportChanges = new Sport()
            {
                Name = "Basketball"
            };
            sportLogic.ModifySport(sport.Id, sportChanges);
            Assert.AreEqual<string>(sportLogic.GetSportById(sport.Id).Name, sportChanges.Name);
        }

        
        [TestMethod]
        [ExpectedException(typeof(InvalidEmptyTextFieldException))]
        public void UpdateSportNameInvalid()
        {
            sportLogic.AddSport(sport);
            sport.Name = "";
            sportLogic.ModifySport(sport.Id, sport);
        }


        [TestMethod]
        public void UpdateIgnoreEmptyFields()
        {
            sportLogic.AddSport(sport);
            Sport sportChanges = new Sport()
            {
                Name = ""
            };
            sportLogic.ModifySport(sport.Id, sportChanges);
            Assert.AreNotEqual<string>(sportLogic.GetSportById(sport.Id).Name, sportChanges.Name);
        }

        [TestMethod]
        [ExpectedException(typeof(SportAlreadyExistsException))]
        public void AddDuplicatedName()
        {
            sportLogic.AddSport(sport);
            Sport identicalSport = new Sport()
            {
                Name = "Tennis"
            };
            sportLogic.AddSport(identicalSport);
        }

        [TestMethod]
        public void GetSportByName()
        {
            sportLogic.AddSport(sport);
            Assert.AreEqual<string>(sportLogic.GetSportByName(sport.Name).Name, sport.Name);
        }

        [TestMethod]
        [ExpectedException(typeof(SportDoesNotExistException))]
        public void GetSportByInvalidName()
        {
            sportLogic.GetSportByName("fakeName");
        }

        [TestMethod]
        public void DeleteSport()
        {
            sportLogic.AddSport(sport);
            sportLogic.RemoveSport(sport.Id);
            Assert.AreEqual(sportLogic.GetAll().Count, 0);
        }

        [TestMethod]
        public void DeleteTeamFromSport()
        {
            sportLogic.AddSport(sport);
            Team team = new Team()
            {
                Name = "Barcelona"
            };
            sportLogic.AddTeamToSport(sport.Id, team);
            sportLogic.DeleteTeamFromSport(sport.Id, team.Id);
            Assert.AreEqual(sportLogic.GetSportById(sport.Id).Teams.Count, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(SportDoesNotExistException))]
        public void DeleteNonExistingSport()
        {
            sportLogic.AddSport(sport);
            sportLogic.RemoveSport(sport.Id + 1);
        }

        [TestMethod]
        public void AddTeamtoSport()
        {
            sportLogic.AddSport(sport);
            Team _team = new Team()
            {
                Name = "Barcelona"
            };
            sportLogic.AddTeamToSport(sport.Id, _team);
            Assert.AreEqual(sportLogic.GetSportById(sport.Id).Teams.Count, 1);
        }

        
        [TestMethod]
        public void UpdateTeamSport()
        {
            sportLogic.AddSport(sport);
            Team team = new Team()
            {
                Name = "Barcelona"
            };
            Team teamChanges = new Team()
            {
                Name = "Villareal"
            };
            sportLogic.AddTeamToSport(sport.Id, team);
            sportLogic.UpdateTeamSport(sport.Id, team.Id, teamChanges);
            Assert.AreEqual<string>(sportLogic.GetTeamFromSport(sport.Id, team.Id).Name, teamChanges.Name);
        }
        

        [TestMethod]
        [ExpectedException(typeof(SportDoesNotExistException))]
        public void AddTeamToInvalidSport()
        {
            Team _team = new Team()
            {
                Name = "Barcelona"
            };
            sportLogic.AddTeamToSport(sport.Id, _team);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidEmptyTextFieldException))]
        public void AddInvalidTeam()
        {
            sportLogic.AddSport(sport);
            Team _team = new Team()
            {
                Name = ""
            };
            sportLogic.AddTeamToSport(sport.Id, _team);
        }

        [TestMethod]
        [ExpectedException(typeof(TeamAlreadyInSportException))]
        public void AddDuplicateTeamToSport()
        {
            sportLogic.AddSport(sport);
            Team team = new Team()
            {
                Name = "Barcelona"
            };
            Team identicalTeam = new Team()
            {
                Name = "Barcelona"
            };
            sportLogic.AddTeamToSport(sport.Id, team);
            sportLogic.AddTeamToSport(sport.Id, identicalTeam);
        }

        [TestMethod]
        public void GetTeamFromSport()
        {
            sportLogic.AddSport(sport);
            Team team = new Team()
            {
                Name = "Barcelona"
            };
            sportLogic.AddTeamToSport(sport.Id, team);
            Team returnedTeam = sportLogic.GetTeamFromSport(sport.Id, team.Id);
            Assert.AreEqual(returnedTeam, team);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidEmptyTextFieldException))]
        public void GetInvalidTeamFromSport()
        {
            sportLogic.AddSport(sport);
            Team team = new Team()
            {
                Name = ""
            };
            sportLogic.AddTeamToSport(sport.Id, team);
            Team returnedTeam = sportLogic.GetTeamFromSport(sport.Id, team.Id);
        }

        [TestMethod]
        public void CascadeDeleteTeams()
        {
            sportLogic.AddSport(sport);
            Team team = new Team()
            {
                Name = "Team"
            };
            sportLogic.AddTeamToSport(sport.Id, team);
            sportLogic.RemoveSport(sport.Id);
            Assert.AreEqual(teamLogic.GetAll().Count, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(NonAdminException))]
        public void SportSetSessionNonAdminUser()
        {
            User user = new User()
            {
                FirstName = "Itai",
                LastName = "Miller",
                Email = "itaimiller@gmail.com",
                UserName = "newUser",
                Password = "root"
            };
            Team team = new Team()
            {
                Name = "Team"
            };
            Team teamChanges = new Team()
            {
                Name = "TeamChanges"
            };
            userLogic.AddUser(user);
            Guid token = sessionLogic.LogInUser(user.UserName, user.Password);
            sessionLogic.GetUserFromToken(token);
            sportLogic.SetSession(token);
            sportLogic.AddSport(sport);
            sportLogic.ModifySport(sport.Id, sport);
            sportLogic.AddTeamToSport(sport.Id, team);
            sportLogic.UpdateTeamSport(sport.Id, team.Id, teamChanges);
            sportLogic.DeleteTeamFromSport(sport.Id, team.Id);
            sportLogic.RemoveSport(sport.Id);
        }

    }
}
