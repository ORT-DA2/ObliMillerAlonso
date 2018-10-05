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
    public class SportsControllerTest
    {
        Mock<ISportLogic> sportLogicMock;
        Mock<ITeamLogic> teamLogicMock;
        SportsController controller;
        IMapper mapper;
        Guid token;

        [TestInitialize]
        public void SetUp()
        {
            teamLogicMock = new Mock<ITeamLogic>();
            sportLogicMock = new Mock<ISportLogic>();
            var config = new MapperConfiguration(cfg => cfg.AddProfile(new MapperProfile()));
            IMapper mapper = new Mapper(config);
            controller = new SportsController(teamLogicMock.Object,sportLogicMock.Object);
            sportLogicMock.Setup(sportLogic => sportLogic.SetSession(It.IsAny<Guid>()));
            token = new Guid();
        }

        [TestMethod]
        public void ValidGetSports()
        {
            Sport fakeSport = new Sport()
            {
                Name = "Rugby"
            };
            ICollection<Sport> sports = new List<Sport>();
            sports.Add(fakeSport);

            sportLogicMock.Setup(sportLogic => sportLogic.GetAll()).Returns(sports);

            IActionResult result = controller.GetAll(token);
            var okResult = result as OkObjectResult;
            var modelOut = okResult.Value as ICollection<SportModelOut>;

            sportLogicMock.VerifyAll();

            Assert.AreEqual(200, okResult.StatusCode);
            Assert.IsNotNull(modelOut);
        }

        [TestMethod]
        public void ValidGetSportById()
        {
            int sportId = 1;
            Sport fakeSport = new Sport()
            {
                Id = sportId,
                Name = "Rugby"
            };

            sportLogicMock.Setup(sportLogic => sportLogic.GetSportById(It.IsAny<int>())).Returns(fakeSport);

            IActionResult result = controller.Get(sportId, token);
            var okResult = result as OkObjectResult;
            var modelOut = okResult.Value as SportModelOut;

            sportLogicMock.VerifyAll();

            Assert.AreEqual(200, okResult.StatusCode);
            Assert.IsNotNull(modelOut);
        }

        [TestMethod]
        public void ValidAddSport()
        {
            int sportId = 1;
            Sport fakeSport = new Sport()
            {
                Id = sportId,
                Name = "Rugby"
            };

            sportLogicMock.Setup(sportLogic => sportLogic.AddSport(It.IsAny<Sport>()));

            IActionResult result = controller.PostSport(fakeSport, token);
            var okResult = result as OkObjectResult;

            sportLogicMock.VerifyAll();

            Assert.AreEqual(200, okResult.StatusCode);
        }


    }
}
