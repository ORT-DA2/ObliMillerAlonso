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
    public class MatchesControllerTest
    {
        Mock<ITeamLogic> teamLogicMock;
        Mock<ISportLogic> sportLogicMock;
        Mock<IMatchLogic> matchLogicMock;
        MatchesController controller;
        IMapper mapper;
        string token;

        [TestInitialize]
        public void SetUp()
        {
            matchLogicMock = new Mock<IMatchLogic>();
            teamLogicMock = new Mock<ITeamLogic>();
            sportLogicMock = new Mock<ISportLogic>();
            var config = new MapperConfiguration(cfg => cfg.AddProfile(new MapperProfile()));
            IMapper mapper = new Mapper(config);
            controller = new MatchesController(matchLogicMock.Object,sportLogicMock.Object, teamLogicMock.Object);
            matchLogicMock.Setup(matchLogic => matchLogic.SetSession(It.IsAny<Guid>()));
            token = new Guid().ToString();
        }
        

        [TestMethod]
        public void ValidGetMatches()
        {
            Team team = new Team()
            {
                Id = 1
            };
            Sport sport = new Sport()
            {
                Id = 1
            };
            Domain.Match match = new Domain.Match()
            {
                Local = team,
                Visitor = team,
                Sport = sport,
                Date = DateTime.Today,
            };
            ICollection<Domain.Match> matches = new List<Domain.Match>();
            matches.Add(match);

            matchLogicMock.Setup(matchLogic => matchLogic.GetAllMatches()).Returns(matches);

            IActionResult result = controller.GetAll(token);
            var okResult = result as OkObjectResult;
            var modelOut = okResult.Value as ICollection<MatchModelOut>;

            sportLogicMock.VerifyAll();

            Assert.AreEqual(200, okResult.StatusCode);
            Assert.IsNotNull(modelOut);
        }

        [TestMethod]
        public void ValidGetMatchById()
        {
            int matchId = 1;
            Team team = new Team()
            {
                Id = 1
            };
            Sport sport = new Sport()
            {
                Id = 1
            };
            Domain.Match match = new Domain.Match()
            {
                Id = matchId,
                Local = team,
                Visitor = team,
                Sport = sport,
                Date = DateTime.Today,
            };
            
            matchLogicMock.Setup(matchLogic => matchLogic.GetMatchById(It.IsAny<int>())).Returns(match);

            IActionResult result = controller.Get(matchId, token);
            var okResult = result as OkObjectResult;
            var modelOut = okResult.Value as MatchModelOut;

            matchLogicMock.VerifyAll();

            Assert.AreEqual(200, okResult.StatusCode);
            Assert.IsNotNull(modelOut);
        }

        [TestMethod]
        public void ValidAddMatch()
        {
            Team team = new Team()
            {
                Id = 1
            };
            Sport sport = new Sport()
            {
                Id = 1
            };

            MatchModelIn model = new MatchModelIn()
            {
                LocalId = team.Id,
                VisitorId = team.Id,
                SportId = sport.Id,
                Date = DateTime.Today.ToString("dd/MM/yyyy HH:mm")
            };

            matchLogicMock.Setup(matchLogic => matchLogic.AddMatch(It.IsAny<Domain.Match>()));

            IActionResult result = controller.Post(model, token);
            var okResult = result as OkObjectResult;
            var modelOut = okResult.Value as MatchModelOut;

            matchLogicMock.VerifyAll();

            Assert.AreEqual(200, okResult.StatusCode);
            Assert.IsNotNull(modelOut);
        }

        [TestMethod]
        public void ValidModifyMatch()
        {
            Team team = new Team()
            {
                Id = 1
            };
            Sport sport = new Sport()
            {
                Id = 1
            };
            int matchId = 1;

            MatchModelIn model = new MatchModelIn()
            {
                LocalId = team.Id,
                VisitorId = team.Id,
                SportId = sport.Id,
                Date = DateTime.Today.ToString("dd/MM/yyyy HH:mm")
            };

            matchLogicMock.Setup(matchLogic => matchLogic.ModifyMatch(It.IsAny<int>(), It.IsAny<Domain.Match>()));

            IActionResult result = controller.Put(matchId, model, token);
            var okResult = result as OkObjectResult;

            matchLogicMock.VerifyAll();

            Assert.AreEqual(200, okResult.StatusCode);
        }

        [TestMethod]
        public void ValidDeleteMatch()
        {
            int matchId = 1;

            matchLogicMock.Setup(matchLogic => matchLogic.DeleteMatch(It.IsAny<int>()));

            IActionResult result = controller.Delete(matchId, token);
            var okResult = result as OkObjectResult;

            matchLogicMock.VerifyAll();

            Assert.AreEqual(200, okResult.StatusCode);
        }

        [TestMethod]
        public void ValidGetComments()
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
                Text = "comment text",
                User = user,
                Match = match
            };
            ICollection<Comment> comments = new List<Comment>();
            comments.Add(fakeComment);

            matchLogicMock.Setup(matchLogic => matchLogic.GetAllComments(It.IsAny<int>())).Returns(comments);

            IActionResult result = controller.GetComments(match.Id, token);
            var okResult = result as OkObjectResult;
            var modelOut = okResult.Value as ICollection<CommentModelOut>;

            matchLogicMock.VerifyAll();

            Assert.AreEqual(200, okResult.StatusCode);
            Assert.IsNotNull(modelOut);
        }

        [TestMethod]
        public void ValidAddComment()
        {
            int matchId = 1;
            CommentModelIn modelIn = new CommentModelIn()
            {
                Text = "comment text"
            };
            Comment fakeComment = new Comment()
            {
                Text = "comment text"
            };

            matchLogicMock.Setup(matchLogic => matchLogic.AddCommentToMatch(It.IsAny<int>(),It.IsAny<Comment>()));

            IActionResult result = controller.PostComment(matchId, modelIn, token);
            var okResult = result as OkObjectResult;

            matchLogicMock.VerifyAll();

            Assert.AreEqual(200, okResult.StatusCode);
        }
    }
}
