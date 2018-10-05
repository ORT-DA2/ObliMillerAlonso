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

            IActionResult result = controller.GetAll(token);
            var okResult = result as OkObjectResult;
            var modelOut = okResult.Value as MatchModelOut;

            sportLogicMock.VerifyAll();

            Assert.AreEqual(200, okResult.StatusCode);
            Assert.IsNotNull(modelOut);
        }

    }
}
