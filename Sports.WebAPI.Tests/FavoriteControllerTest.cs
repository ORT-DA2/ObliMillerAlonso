using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Sports.Domain;
using Sports.Logic.Interface;
using System.Net.Http;
using Sports.WebAPI.Models;
using Sports.WebAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using AutoMapper;
using System;

namespace Sports.WebAPI.Tests
{
    [TestClass]
    public class FavoriteControllerTest
    {
        Mock<IFavoriteLogic> favoriteLogicMock;
        Mock<IUserLogic> userLogicMock;
        FavoritesController controller;
        IMapper mapper;
        Guid token;

        [TestInitialize]
        public void SetUp()
        {

            favoriteLogicMock = new Mock<IFavoriteLogic>();
            userLogicMock = new Mock<IUserLogic>();
            var config = new MapperConfiguration(cfg => cfg.AddProfile(new MapperProfile()));
            IMapper mapper = new Mapper(config);
            controller = new FavoritesController(userLogicMock.Object, favoriteLogicMock.Object, mapper);
            favoriteLogicMock.Setup(favoriteLogic => favoriteLogic.SetSession(It.IsAny<Guid>()));
            userLogicMock.Setup(userLogic => userLogic.SetSession(It.IsAny<Guid>()));
            token = new Guid();
        }

        [TestMethod]
        public void ValidPostFavorite()
        {
            User fakeUser = new User(true)
            {
                FirstName = "Itai",
                LastName = "Miller",
                Email = "itaimiller@gmail.com",
                UserName = "iMiller",
                Password = "root"
            };
            UserModelIn userModelIn = new UserModelIn()
            {
                Id = 1,
                FirstName = "Itai",
                LastName = "Miller",
                Email = "itaimiller@gmail.com",
                UserName = "iMiller",
                Password = "root",
                IsAdmin = true
            };
            Team fakeTeam = new Team()
            {
                Name = "Team"
            };
            TeamModelIn teamModelIn = new TeamModelIn()
            {
                Id = 1,
                Name = "Team"
            };
            favoriteLogicMock.Setup(favoriteLogic => favoriteLogic.AddFavoriteTeam(It.IsAny<User>(),It.IsAny<Team>()));
            IActionResult result = controller.PostFavorite(userModelIn, teamModelIn, token);
            var okResult = result as OkObjectResult;

            favoriteLogicMock.VerifyAll();

            Assert.AreEqual(200, okResult.StatusCode);
        }

        [TestMethod]
        public void ValidGetFavoritesFromUsers()
        {
            int userId = 1;
            Team fakeTeam = new Team()
            {
                Name = "Team name"
            };
            ICollection<Team> teams = new List<Team>();
            teams.Add(fakeTeam);

            favoriteLogicMock.Setup(favoriteLogic => favoriteLogic.GetFavoritesFromUser(userId)).Returns(teams);

            var result = controller.GetFavoritesForUser(userId, token);
            var okResult = result as OkObjectResult;

            favoriteLogicMock.VerifyAll();
            Assert.AreEqual(200, okResult.StatusCode);
        }

        [TestMethod]
        public void GetFavoritesTeamsComments()
        {
            int userId = 1;
            User fakeUser = new User(true)
            {
                FirstName = "Itai",
                LastName = "Miller",
                Email = "itaimiller@gmail.com",
                UserName = "iMiller",
                Password = "root"
            };
            Comment fakeComment = new Comment()
            {
                Text = "comment",
                User = fakeUser
            };
            ICollection<Comment> comments = new List<Comment>();
            comments.Add(fakeComment);

            favoriteLogicMock.Setup(favoriteLogic => favoriteLogic.GetFavoritesTeamsComments(fakeUser)).Returns(comments);

            var result = controller.GetFavoritesTeamsComents(fakeUser, token);
            var okResult = result as OkObjectResult;

            favoriteLogicMock.VerifyAll();
            Assert.AreEqual(200, okResult.StatusCode);
        }

    }
}
