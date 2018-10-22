using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using Sports.Repository.Context;
using Sports.Repository.Interface;
using Sports.Repository.Exceptions;
using Microsoft.EntityFrameworkCore;
using Sports.Logic;
using Sports.Logic.Exceptions;
using Sports.Logic.Interface.Exceptions;
using Sports.Repository.UnitOfWork;
using Sports.Domain;

namespace Sports.Repository.Test
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class RepositoryBaseTest
    {
        IRepositoryUnitOfWork unit;
        ICompetitorRepository competitorRepository;
        Competitor competitor;

     [TestInitialize]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<RepositoryContext>().Options;
            RepositoryContext repository = new RepositoryContext(options);
            unit = new RepositoryUnitOfWork(repository);
            competitorRepository = unit.Competitor;
            competitor = new Competitor()
            {
                Name = "Competitor"
            };
        }

        [TestMethod]
        [ExpectedException(typeof(UnknownDbException))]
        public void UnknownErrorSave()
        {
            competitorRepository.Save();
        }

        [TestMethod]
        [ExpectedException(typeof(UnknownDbException))]
        public void UnknownErrorAddCompetitor()
        {
            competitorRepository.Create(competitor);
        }

        [TestMethod]
        [ExpectedException(typeof(UnknownDbException))]
        public void UnknownErrorDeleteCompetitor()
        {
            competitorRepository.Delete(competitor);
        }

        [TestMethod]
        [ExpectedException(typeof(UnknownDbException))]
        public void UnknownErrorUpdateCompetitor()
        {
            competitorRepository.Update(competitor);
        }
        
    }
}
