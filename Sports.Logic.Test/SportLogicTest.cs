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
        private IRepositoryWrapper _wrapper;
        private RepositoryContext _repository;
        private SportLogic _sportLogic;
        private TeamLogic _teamLogic;
        Sport _sport;
        Team _team;
        

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
            _wrapper = new RepositoryWrapper(_repository);
            _sportLogic = new SportLogic(_wrapper);
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
        public void GetSportById()
        {
            _sportLogic.AddSport(_sport);
            Assert.AreEqual<string>(_sportLogic.GetSportById(_sport.Id).Id, _sport.Id);
        }

    }
}
