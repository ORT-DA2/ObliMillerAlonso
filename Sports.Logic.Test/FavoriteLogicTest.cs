﻿using System;
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
        private IUserLogic userLogic;
        private ITeamLogic teamLogic;
        Favorite favorite;
        User user;
        Team team;

        [TestInitialize]
        public void SetUp()
        {
            user = new User()
            {
                FirstName = "itai",
                LastName = "miller",
                Email = "itai@gmail.com",
                UserName = "iMiller",
                Password = "root"
            };
            team = new Team()
            {
                Name = "Barcelona"
            };
            var options = new DbContextOptionsBuilder<RepositoryContext>()
                .UseInMemoryDatabase<RepositoryContext>(databaseName: "FavoriteLogicTestDB")
                .Options;
            repository = new RepositoryContext(options);
            unitOfWork = new RepositoryUnitOfWork(repository);
            favoriteLogic = new FavoriteLogic(unitOfWork);
            userLogic = new UserLogic(unitOfWork);
            teamLogic = new TeamLogic(unitOfWork);
        }

        [TestCleanup]
        public void TearDown()
        {
            repository.Favorites.RemoveRange(repository.Favorites);
            repository.Users.RemoveRange(repository.Users);
            repository.Teams.RemoveRange(repository.Teams);
            repository.SaveChanges();
        }
        
        [TestMethod]
        public void GetFavoritesForUser()
        {
            userLogic.AddUser(user);
            teamLogic.AddTeam(team);
            favoriteLogic.AddFavoriteTeam(user, team);
            ICollection<Favorite> favorites = favoriteLogic.GetFavoritesFromUser(user.Id);
            Assert.AreEqual(favorites.Count, 1);
        }

        [TestMethod]
        [ExpectedException(typeof(FavoriteDoesNotExistException))]
        public void GetFavoritesForInexistentUser()
        {
            ICollection<Favorite> favorites = favoriteLogic.GetFavoritesFromUser(user.Id);
            Assert.AreEqual(favorites.Count, 0);
        }

    }
}
