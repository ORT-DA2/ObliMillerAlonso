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
using System.Diagnostics.CodeAnalysis;
using Sports.Logic.Exceptions;

namespace Sports.Logic.Test
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class FavoriteLogicTest
    {
        private IRepositoryUnitOfWork _unitOfWork;
        private RepositoryContext _repository;
        private IFavoriteLogic _favoriteLogic;
        Favorite _favorite;

        [TestInitialize]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<RepositoryContext>()
                .UseInMemoryDatabase<RepositoryContext>(databaseName: "FavoriteLogicTestDB")
                .Options;
            _repository = new RepositoryContext(options);
            _unitOfWork = new RepositoryUnitOfWork(_repository);
            _favoriteLogic = new FavoriteLogic(_unitOfWork);
        }

        [TestCleanup]
        public void TearDown()
        {
            _repository.Favorites.RemoveRange(_repository.Favorites);
            _repository.SaveChanges();
        }
    }
}
