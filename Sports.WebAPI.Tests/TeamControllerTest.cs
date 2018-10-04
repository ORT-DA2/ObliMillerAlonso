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
    public class TeamControllerTest
    {
        Mock<ITeamLogic> teamLogicMock;
        TeamController controller;
        IMapper mapper;
        Guid token;

        [TestInitialize]
        public void SetUp()
        {

            teamLogicMock = new Mock<ITeamLogic>();
            var config = new MapperConfiguration(cfg => cfg.AddProfile(new MapperProfile()));
            IMapper mapper = new Mapper(config);
            controller = new TeamController(teamLogicMock.Object);
            teamLogicMock.Setup(favoriteLogic => favoriteLogic.SetSession(It.IsAny<Guid>()));
            token = new Guid();
        }

        [TestMethod]
        public void ValidGetTeams()
        {
            Team fakeTeam = new Team()
            {
                Name = "Team"
            };
            ICollection<Team> teams = new List<Team>();
            teams.Add(fakeTeam);

            teamLogicMock.Setup(teamLogic => teamLogic.GetAll()).Returns(teams);

            IActionResult result = controller.GetTeams(token);
            var okResult = result as OkObjectResult;
            var modelOut = okResult.Value as ICollection<TeamModelOut>;

            teamLogicMock.VerifyAll();

            Assert.AreEqual(200, okResult.StatusCode);
            Assert.IsNotNull(modelOut);
        }

    }
}
