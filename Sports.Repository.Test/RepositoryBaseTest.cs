using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using Sports.Repository.Context;
using Sports.Repository.Interface;
using Sports.Repository;
using Microsoft.EntityFrameworkCore;
using Sports.Logic;
using Sports.Logic.Exceptions;
using Sports.Logic.Interface;
using Sports.Repository.UnitOfWork;
using Sports.Domain;

namespace Sports.Repository.Test
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class RepositoryBaseTest
    {
        IRepositoryUnitOfWork unit;
        ITeamRepository teamRepository;
        Team team;

     [TestInitialize]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<RepositoryContext>().Options;
            RepositoryContext repository = new RepositoryContext(options);
            unit = new RepositoryUnitOfWork(repository);
            teamRepository = unit.Team;
            team = new Team()
            {
                Name = "Team"
            };
        }

        [TestMethod]
        [ExpectedException(typeof(UnknownDataException))]
        public void UnknownErrorSave()
        {
            teamRepository.Save();
        }

        [TestMethod]
        [ExpectedException(typeof(UnknownDataException))]
        public void UnknownErrorAddTeam()
        {
            teamRepository.Create(team);
        }

        [TestMethod]
        [ExpectedException(typeof(UnknownDataException))]
        public void UnknownErrorDeleteTeam()
        {
            teamRepository.Delete(team);
        }

        [TestMethod]
        [ExpectedException(typeof(UnknownDataException))]
        public void UnknownErrorUpdateTeam()
        {
            teamRepository.Update(team);
        }

        /*
        [TestMethod]
        [ExpectedException(typeof(DisconnectedDatabaseException))]
        public void AddTeamSqlError()
        {
            var options = new DbContextOptionsBuilder<RepositoryContext>().UseSqlServer("Server=fakeServer;Database=fakeDb").Options;
            RepositoryContext fakeRepository = new RepositoryContext(options);
            IRepositoryUnitOfWork newUnit = new RepositoryUnitOfWork(fakeRepository);
            ITeamRepository fakeTeamRepository = newUnit.Team;
            team = new Team()
            {
                Name = "Team"
            };
            fakeTeamRepository.Create(team);
        }*/
    }
}
