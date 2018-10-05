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
    public class SportControllerTest
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
        

    }
}
