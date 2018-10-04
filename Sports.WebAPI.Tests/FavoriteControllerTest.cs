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
using System.Diagnostics.CodeAnalysis;

namespace Sports.WebAPI.Tests
{
    [ExcludeFromCodeCoverage]
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
            controller = new FavoritesController(userLogicMock.Object, favoriteLogicMock.Object);
            favoriteLogicMock.Setup(favoriteLogic => favoriteLogic.SetSession(It.IsAny<Guid>()));
            userLogicMock.Setup(userLogic => userLogic.SetSession(It.IsAny<Guid>()));
            token = new Guid();
        }

        [TestMethod]
        public void ValidPostFavorite()
        {
            Team fakeTeam = new Team()
            {
                Name = "Team"
            };
            TeamModelIn teamModelIn = new TeamModelIn()
            {
                Id = 1,
                Name = "Team"
            };
            favoriteLogicMock.Setup(favoriteLogic => favoriteLogic.AddFavoriteTeam(It.IsAny<Team>()));
            IActionResult result = controller.PostFavorite(teamModelIn, token);
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

            favoriteLogicMock.Setup(favoriteLogic => favoriteLogic.GetFavoritesFromUser()).Returns(teams);

            var result = controller.GetFavoritesForUser(token);
            var okResult = result as OkObjectResult;
            var favoriteTeams = okResult.Value as ICollection<Team>;

            favoriteLogicMock.VerifyAll();
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.IsNotNull(favoriteTeams);
        }

        [TestMethod]
        public void GetFavoritesTeamsComments()
        {
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

            favoriteLogicMock.Setup(favoriteLogic => favoriteLogic.GetFavoritesTeamsComments()).Returns(comments);

            var result = controller.GetFavoritesTeamsComents(token);
            var okResult = result as OkObjectResult;
            var favoriteTeamsComments = okResult.Value as ICollection<Comment>;
            
            favoriteLogicMock.VerifyAll();
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.IsNotNull(favoriteTeamsComments);
        }

    }
}
