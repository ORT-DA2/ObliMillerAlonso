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
    public class SportLogicTest
    {
        private IRepositoryUnitOfWork _unitOfWork;
        private RepositoryContext _repository;
        private SportLogic _sportLogic;
        private TeamLogic _teamLogic;
        Sport _sport;
        
        [TestInitialize]
        public void SetUp()
        {
            _sport = new Sport()
            {
                Name = "Tennis"
            };


            var options = new DbContextOptionsBuilder<RepositoryContext>()
                .UseInMemoryDatabase<RepositoryContext>(databaseName: "SportLogicTestDB")
                .Options;
            _repository = new RepositoryContext(options);
            _unitOfWork = new RepositoryUnitOfWork(_repository);
            _sportLogic = new SportLogic(_unitOfWork);
            _teamLogic = new TeamLogic(_unitOfWork);
        }

        [TestCleanup]
        public void TearDown()
        {
            _repository.Sports.RemoveRange(_repository.Sports);
            _repository.SaveChanges();
        }

        [TestMethod]
        public void AddSport()
        {
            _sportLogic.AddSport(_sport);
            Assert.IsNotNull(_sportLogic.GetSportById(_sport.Id));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidSportDataException))]
        public void AddNullSport()
        {
            _sportLogic.AddSport(null);
        }

        [TestMethod]
        public void UpdateSportName()
        {
            _sportLogic.AddSport(_sport);
            Sport sportChanges = new Sport()
            {
                Name = "Basketball"
            };
            _sportLogic.UpdateSport(_sport.Id, sportChanges);
            Assert.AreEqual<string>(_sportLogic.GetSportById(_sport.Id).Name, sportChanges.Name);
        }

        
        [TestMethod]
        [ExpectedException(typeof(InvalidSportDataException))]
        public void UpdateSportNameInvalid()
        {
            _sportLogic.AddSport(_sport);
            _sport.Name = "";
            _sportLogic.Modify(_sport);
        }


        [TestMethod]
        public void UpdateIgnoreEmptyFields()
        {
            _sportLogic.AddSport(_sport);
            Sport sportChanges = new Sport()
            {
                Name = ""
            };
            _sportLogic.UpdateSport(_sport.Id, sportChanges);
            Assert.AreNotEqual<string>(_sportLogic.GetSportById(_sport.Id).Name, sportChanges.Name);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidSportDataException))]
        public void AddDuplicatedName()
        {
            _sportLogic.AddSport(_sport);
            Sport identicalSport = new Sport()
            {
                Name = "Tennis"
            };
            _sportLogic.AddSport(identicalSport);
        }

        [TestMethod]
        public void GetSportByName()
        {
            _sportLogic.AddSport(_sport);
            Assert.AreEqual<string>(_sportLogic.GetSportByName(_sport.Name).Name, _sport.Name);
        }

        [TestMethod]
        public void DeleteSport()
        {
            _sportLogic.AddSport(_sport);
            _sportLogic.RemoveSport(_sport.Id);
            Assert.AreEqual(_sportLogic.GetAll().Count, 0);
        }

        [TestMethod]
        public void DeleteTeamFromSport()
        {
            _sportLogic.AddSport(_sport);
            Team _team = new Team()
            {
                Name = "Barcelona"
            };
            _sportLogic.AddTeamToSport(_sport, _team);
            _sportLogic.DeleteTeamFromSport(_sport, _team);
            Assert.AreEqual(_sportLogic.GetSportById(_sport.Id).Teams.Count, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidSportDataException))]
        public void DeleteNonExistingSport()
        {
            _sportLogic.AddSport(_sport);
            _sportLogic.RemoveSport(_sport.Id + 1);
        }

        [TestMethod]
        public void AddTeamtoSport()
        {
            _sportLogic.AddSport(_sport);
            Team _team = new Team()
            {
                Name = "Barcelona"
            };
            _sportLogic.AddTeamToSport(_sport, _team);
            Assert.AreEqual(_sportLogic.GetSportById(_sport.Id).Teams.Count, 1);
        }

        
        [TestMethod]
        public void UpdateTeamSport()
        {
            _sportLogic.AddSport(_sport);
            Team team = new Team()
            {
                Name = "Barcelona"
            };
            Team teamChanges = new Team()
            {
                Name = "Villareal"
            };
            _sportLogic.AddTeamToSport(_sport, team);
            _sportLogic.UpdateTeamSport(_sport.Id, team, teamChanges);
            Assert.AreEqual<string>(_sportLogic.GetTeamFromSport(_sport, team).Name, teamChanges.Name);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidTeamDataException))]
        public void UpdateInvalidTeamSport()
        {
            _sportLogic.AddSport(_sport);
            Team team = new Team()
            {
                Name = "Barcelona"
            };
            Team teamChanges = new Team()
            {
                Name = ""
            };
            _sportLogic.AddTeamToSport(_sport, team);
            _sportLogic.UpdateTeamSport(_sport.Id, team, teamChanges);
        }


        [TestMethod]
        [ExpectedException(typeof(InvalidSportDataException))]
        public void AddTeamToInvalidSport()
        {
            Team _team = new Team()
            {
                Name = "Barcelona"
            };
            _sportLogic.AddTeamToSport(_sport, _team);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidTeamDataException))]
        public void AddInvalidTeam()
        {
            _sportLogic.AddSport(_sport);
            Team _team = new Team()
            {
                Name = ""
            };
            _sportLogic.AddTeamToSport(_sport, _team);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidSportDataException))]
        public void AddDuplicateTeamToSport()
        {
            _sportLogic.AddSport(_sport);
            Team _team = new Team()
            {
                Name = "Barcelona"
            };
            Team _identicalTeam = new Team()
            {
                Name = "Barcelona"
            };
            _sportLogic.AddTeamToSport(_sport, _team);
            _sportLogic.AddTeamToSport(_sport, _identicalTeam);
        }

        [TestMethod]
        public void GetTeamFromSport()
        {
            _sportLogic.AddSport(_sport);
            Team _team = new Team()
            {
                Name = "Barcelona"
            };
            _sportLogic.AddTeamToSport(_sport, _team);
            Team returnedTeam = _sportLogic.GetTeamFromSport(_sport, _team);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidTeamDataException))]
        public void GetInvalidTeamFromSport()
        {
            _sportLogic.AddSport(_sport);
            Team _team = new Team()
            {
                Name = ""
            };
            _sportLogic.AddTeamToSport(_sport, _team);
            Team returnedTeam = _sportLogic.GetTeamFromSport(_sport, _team);
        }

    }
}
