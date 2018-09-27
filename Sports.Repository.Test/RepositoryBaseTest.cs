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
using Sports.Repository.Exceptions;
using Sports.Logic.Interface;
using Sports.Domain;

namespace Sports.Repository.Test
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class RepositoryBaseTest
    {

        [TestMethod]
        [ExpectedException(typeof(InvalidDatabaseAccessException))]
        public void DisconnectedDatabaseSave()
        {
            var options = new DbContextOptionsBuilder<RepositoryContext>().Options;
            RepositoryContext repository = new RepositoryContext(options);
            IRepositoryUnitOfWork unit = new RepositoryUnitOfWork(repository);
            ITeamRepository teamRepository = unit.Team;
            teamRepository.Save();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidDatabaseAccessException))]
        public void AddTeam()
        {
            Team team = new Team()
            {
                Name = "Team"
            };
            var options = new DbContextOptionsBuilder<RepositoryContext>().Options;
            RepositoryContext repository = new RepositoryContext(options);
            IRepositoryUnitOfWork unit = new RepositoryUnitOfWork(repository);
            ITeamRepository teamRepository = unit.Team;
            teamRepository.Create(team);
            teamRepository.Save();
        }
    }
}
