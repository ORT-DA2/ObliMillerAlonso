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
    public class FavoritesControllerTest
    {
        Mock<IFavoriteLogic> favoriteLogicMock;
        Mock<ISessionLogic> sessionLogicMock;
        Mock<IUserLogic> userLogicMock;
        UsersController controller;
        string token;

        [TestInitialize]
        public void SetUp()
        {
            sessionLogicMock = new Mock<ISessionLogic>();
            favoriteLogicMock = new Mock<IFavoriteLogic>();
            userLogicMock = new Mock<IUserLogic>();
            var config = new MapperConfiguration(cfg => cfg.AddProfile(new MapperProfile()));
            IMapper mapper = new Mapper(config);
            controller = new UsersController(userLogicMock.Object, sessionLogicMock.Object, favoriteLogicMock.Object);
            favoriteLogicMock.Setup(favoriteLogic => favoriteLogic.SetSession(It.IsAny<Guid>()));
            userLogicMock.Setup(userLogic => userLogic.SetSession(It.IsAny<Guid>()));
            token = new Guid().ToString();
        }

        [TestMethod]
        public void ValidPostFavorite()
        {
            Competitor fakeCompetitor = new Competitor()
            {
                Name = "Competitor"
            };
            CompetitorModelIn competitorModelIn = new CompetitorModelIn()
            {
                Id = 1,
                Name = "Competitor"
            };
            favoriteLogicMock.Setup(favoriteLogic => favoriteLogic.AddFavoriteCompetitor(It.IsAny<Competitor>()));
            IActionResult result = controller.PostFavorite(competitorModelIn, token);
            var createdResult = result as RedirectToRouteResult;

            favoriteLogicMock.VerifyAll();
            
            Assert.IsNotNull(createdResult);
        }

        [TestMethod]
        public void ValidGetFavoritesFromUser()
        {
            Competitor fakeCompetitor = new Competitor()
            {
                Name = "Competitor name"
            };
            ICollection<Competitor> competitors = new List<Competitor>();
            competitors.Add(fakeCompetitor);

            favoriteLogicMock.Setup(favoriteLogic => favoriteLogic.GetFavoritesFromUser()).Returns(competitors);

            var result = controller.GetFavoriteCompetitors(token);
            var okResult = result as OkObjectResult;
            var favoriteCompetitors = okResult.Value as ICollection<CompetitorModelOut>;

            favoriteLogicMock.VerifyAll();
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.IsNotNull(favoriteCompetitors);
        }

        [TestMethod]
        public void GetFavoritesCompetitorsComments()
        {

            Domain.Match match = new Domain.Match()
            {
                Id = 1
            };
            User user = new User()
            {
                Id = 1
            };
            Comment fakeComment = new Comment()
            {
                Text = "comment",
                User = user,
                Match = match
            };
            ICollection<Comment> comments = new List<Comment>();
            comments.Add(fakeComment);

            favoriteLogicMock.Setup(favoriteLogic => favoriteLogic.GetFavoritesCompetitorsComments()).Returns(comments);

            var result = controller.GetFavoritesCompetitorsComents(token);
            var okResult = result as OkObjectResult;
            var favoriteCompetitorsComments = okResult.Value as ICollection<CommentModelOut>;
            
            favoriteLogicMock.VerifyAll();
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.IsNotNull(favoriteCompetitorsComments);
        }



    }
}
