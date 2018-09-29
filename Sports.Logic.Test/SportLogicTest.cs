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
        private SportLogic sportLogic;
        private TeamLogic teamLogic;
        Sport sport;
        
        [TestInitialize]
        public void SetUp()
        {
            SetUpRepositories();

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
        }

        [TestCleanup]
        public void TearDown()
        {
            repository.Sports.RemoveRange(repository.Sports);
            repository.SaveChanges();
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
            Team _team = new Team()
            {
                Name = "Barcelona"
            };
            sportLogic.AddTeamToSport(sport, _team);
            sportLogic.DeleteTeamFromSport(sport, _team);
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
            sportLogic.AddTeamToSport(sport, _team);
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
            sportLogic.AddTeamToSport(sport, team);
            sportLogic.UpdateTeamSport(sport.Id, team, teamChanges);
            Assert.AreEqual<string>(sportLogic.GetTeamFromSport(sport, team).Name, teamChanges.Name);
        }
        

        [TestMethod]
        [ExpectedException(typeof(SportDoesNotExistException))]
        public void AddTeamToInvalidSport()
        {
            Team _team = new Team()
            {
                Name = "Barcelona"
            };
            sportLogic.AddTeamToSport(sport, _team);
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
            sportLogic.AddTeamToSport(sport, _team);
        }

        [TestMethod]
        [ExpectedException(typeof(TeamAlreadyInSportException))]
        public void AddDuplicateTeamToSport()
        {
            sportLogic.AddSport(sport);
            Team _team = new Team()
            {
                Name = "Barcelona"
            };
            Team _identicalTeam = new Team()
            {
                Name = "Barcelona"
            };
            sportLogic.AddTeamToSport(sport, _team);
            sportLogic.AddTeamToSport(sport, _identicalTeam);
        }

        [TestMethod]
        public void GetTeamFromSport()
        {
            sportLogic.AddSport(sport);
            Team _team = new Team()
            {
                Name = "Barcelona"
            };
            sportLogic.AddTeamToSport(sport, _team);
            Team returnedTeam = sportLogic.GetTeamFromSport(sport, _team);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidEmptyTextFieldException))]
        public void GetInvalidTeamFromSport()
        {
            sportLogic.AddSport(sport);
            Team _team = new Team()
            {
                Name = ""
            };
            sportLogic.AddTeamToSport(sport, _team);
            Team returnedTeam = sportLogic.GetTeamFromSport(sport, _team);
        }

    }
}
