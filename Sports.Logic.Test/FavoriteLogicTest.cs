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
        private IRepositoryUnitOfWork unitOfWork;
        private RepositoryContext repository;
        private IFavoriteLogic favoriteLogic;
        Favorite favorite;

        [TestInitialize]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<RepositoryContext>()
                .UseInMemoryDatabase<RepositoryContext>(databaseName: "FavoriteLogicTestDB")
                .Options;
            repository = new RepositoryContext(options);
            unitOfWork = new RepositoryUnitOfWork(repository);
            favoriteLogic = new FavoriteLogic(unitOfWork);
        }

        [TestCleanup]
        public void TearDown()
        {
            repository.Favorites.RemoveRange(repository.Favorites);
            repository.SaveChanges();
        }

        [TestMethod]
        public void FavoriteTeam()
        {
            User user = new User()
            {
                FirstName = "itai",
                LastName = "miller",
                Email = "itai@gmail.com",
                UserName = "iMiller",
                Password = "root"
            };
            Team team = new Team()
            {
                Name = "Barcelona"
            };
            favoriteLogic.AddFavoriteTeam(user, team);
        }
    }
}
