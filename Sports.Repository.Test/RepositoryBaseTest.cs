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

namespace Sports.Repository.Test
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class RepositoryBaseTest
    {
        [TestMethod]
        [ExpectedException(typeof(InvalidDatabaseAccessException))]
        public void DisconnectedDatabase()
        {
            var options = new DbContextOptionsBuilder<RepositoryContext>().Options;
            RepositoryContext repository = new RepositoryContext(options);
            IRepositoryUnitOfWork unit = new RepositoryUnitOfWork(repository);
            ITeamRepository teamRepository = unit.Team;
            teamRepository.Save();
        }
    }
}
