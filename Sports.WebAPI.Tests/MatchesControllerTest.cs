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
        Mock<ICompetitorLogic> competitorLogicMock;
        Mock<ISportLogic> sportLogicMock;
        Mock<IMatchLogic> matchLogicMock;
        Mock<IFixtureLogic> fixtureLogicMock;
        MatchesController controller;
        IMapper mapper;
        string token;

        [TestInitialize]
        public void SetUp()
        {
            matchLogicMock = new Mock<IMatchLogic>();
            competitorLogicMock = new Mock<ICompetitorLogic>();
            sportLogicMock = new Mock<ISportLogic>();
            fixtureLogicMock = new Mock<IFixtureLogic>();
            var config = new MapperConfiguration(cfg => cfg.AddProfile(new MapperProfile()));
            IMapper mapper = new Mapper(config);
            controller = new MatchesController(matchLogicMock.Object,sportLogicMock.Object, competitorLogicMock.Object, fixtureLogicMock.Object);
            token = new Guid().ToString();
        }
        

        [TestMethod]
        public void ValidGetMatches()
        {
            Competitor competitor = new Competitor()
            {
                Id = 1
            };
            Sport sport = new Sport()
            {
                Id = 1
            };
            Domain.Match match = new Domain.Match()
            {
                Local = competitor,
                Visitor = competitor,
                Sport = sport,
                Date = DateTime.Today,
            };
            ICollection<Domain.Match> matches = new List<Domain.Match>
            {
                match
            };

            matchLogicMock.Setup(matchLogic => matchLogic.SetSession(It.IsAny<Guid>()));
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
            Competitor competitor = new Competitor()
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
                Local = competitor,
                Visitor = competitor,
                Sport = sport,
                Date = DateTime.Today,
            };

            matchLogicMock.Setup(matchLogic => matchLogic.SetSession(It.IsAny<Guid>()));
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
            Competitor competitor = new Competitor()
            {
                Id = 1
            };
            Sport sport = new Sport()
            {
                Id = 1
            };

            MatchModelIn model = new MatchModelIn()
            {
                LocalId = competitor.Id,
                VisitorId = competitor.Id,
                SportId = sport.Id,
                Date = DateTime.Today.ToString("dd/MM/yyyy HH:mm")
            };

            matchLogicMock.Setup(matchLogic => matchLogic.SetSession(It.IsAny<Guid>()));
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
            Competitor competitor = new Competitor()
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
                LocalId = competitor.Id,
                VisitorId = competitor.Id,
                SportId = sport.Id,
                Date = DateTime.Today.ToString("dd/MM/yyyy HH:mm")
            };

            matchLogicMock.Setup(matchLogic => matchLogic.SetSession(It.IsAny<Guid>()));
            matchLogicMock.Setup(matchLogic => matchLogic.ModifyMatch(It.IsAny<int>(), It.IsAny<Domain.Match>()));

            IActionResult result = controller.Put(matchId, model, token);
            var createdResult = result as RedirectToRouteResult;

            matchLogicMock.VerifyAll();

            Assert.IsNotNull(createdResult);
        }

        [TestMethod]
        public void ValidDeleteMatch()
        {
            int matchId = 1;

            matchLogicMock.Setup(matchLogic => matchLogic.SetSession(It.IsAny<Guid>()));
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

            matchLogicMock.Setup(matchLogic => matchLogic.SetSession(It.IsAny<Guid>()));
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

            matchLogicMock.Setup(matchLogic => matchLogic.SetSession(It.IsAny<Guid>()));
            matchLogicMock.Setup(matchLogic => matchLogic.AddCommentToMatch(It.IsAny<int>(),It.IsAny<Comment>()));

            IActionResult result = controller.PostComment(matchId, modelIn, token);
            var createdResult = result as RedirectToRouteResult;

            matchLogicMock.VerifyAll();

            Assert.IsNotNull(createdResult);
        }


        [TestMethod]
        public void AddFixtures()
        {
            FixturesPathDTO pathDTO = new FixturesPathDTO()
            {
                Path = "path"
            };

            fixtureLogicMock.Setup(fixtureLogic => fixtureLogic.SetSession(It.IsAny<Guid>()));
            fixtureLogicMock.Setup(fixtureLogic => fixtureLogic.AddFixtureImplementations(It.IsAny<string>()));

            IActionResult result = controller.PostFixtureImplementation(pathDTO, token);
            var okResult = result as OkObjectResult;

            matchLogicMock.VerifyAll();

            Assert.AreEqual(200, okResult.StatusCode);
        }


        [TestMethod]
        public void GenerateFixtures()
        {
            Competitor competitor = new Competitor()
            {
                Id = 1
            };
            Sport sport = new Sport()
            {
                Id = 1,
                Name = "Deporte"
            };

            SportModelIn sportModel = new SportModelIn()
            {
                Name = "Deporte"
            };
            ICollection<SportModelIn> sportModels = new List<SportModelIn>
            {
                sportModel
            };

            FixtureDTO fixtureDTO = new FixtureDTO()
            {
                Sports = sportModels,
                Date = "11/10/2014 10:10"
            };

            fixtureLogicMock.Setup(fixtureLogic => fixtureLogic.SetSession(It.IsAny<Guid>()));
            fixtureLogicMock.Setup(fixtureLogic => fixtureLogic.GenerateFixture(It.IsAny<ICollection<Sport>>(),It.IsAny<DateTime>()));

            IActionResult result = controller.GenerateFixture(fixtureDTO, token);
            var okResult = result as OkObjectResult;

            matchLogicMock.VerifyAll();

            Assert.AreEqual(200, okResult.StatusCode);
        }

        [TestMethod]
        public void NextFixture()
        {
            string message = "message";

            fixtureLogicMock.Setup(fixtureLogic => fixtureLogic.SetSession(It.IsAny<Guid>()));
            fixtureLogicMock.Setup(fixtureLogic => fixtureLogic.ChangeFixtureImplementation()).Returns(message);

            IActionResult result = controller.NextFixture(token);
            var okResult = result as OkObjectResult;

            matchLogicMock.VerifyAll();

            Assert.AreEqual(200, okResult.StatusCode);
        }
    }
}
