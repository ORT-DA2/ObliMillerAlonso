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
    public class TeamsControllerTest
    {
        Mock<ITeamLogic> teamLogicMock;
        Mock<ISportLogic> sportLogicMock;
        TeamsController controller;
        string token;

        [TestInitialize]
        public void SetUp()
        {

            teamLogicMock = new Mock<ITeamLogic>();
            sportLogicMock = new Mock<ISportLogic>();
            var config = new MapperConfiguration(cfg => cfg.AddProfile(new MapperProfile()));
            IMapper mapper = new Mapper(config);
            controller = new TeamsController(teamLogicMock.Object, sportLogicMock.Object);
            teamLogicMock.Setup(teamLogic => teamLogic.SetSession(It.IsAny<Guid>()));
            token = new Guid().ToString();
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

            teamLogicMock.Setup(sportLogic => sportLogic.GetFilteredTeams(It.IsAny<string>(), It.IsAny<string>())).Returns(teams);

            IActionResult result = controller.GetAllTeams(token,null,null);
            var okResult = result as OkObjectResult;
            var modelOut = okResult.Value as ICollection<TeamModelOut>;

            teamLogicMock.VerifyAll();

            Assert.AreEqual(200, okResult.StatusCode);
            Assert.IsNotNull(modelOut);
        }

        [TestMethod]
        public void ValidGetTeamById()
        {
            int teamId = 1;
            Team fakeTeam = new Team()
            {
                Id = teamId,
                Name = "Team"
            };

            teamLogicMock.Setup(teamLogic => teamLogic.GetTeamById(teamId)).Returns(fakeTeam);

            IActionResult result = controller.Get(teamId, token);
            var okResult = result as OkObjectResult;
            var modelOut = okResult.Value as TeamModelOut;

            teamLogicMock.VerifyAll();

            Assert.AreEqual(200, okResult.StatusCode);
            Assert.IsNotNull(modelOut);
        }


        [TestMethod]
        public void ValidModifyTeam()
        {

            TeamModelIn fakeTeam = new TeamModelIn()
            {
                Name = "ChangedName"
            };
            int teamId = 1;


            teamLogicMock.Setup(teamLogic => teamLogic.Modify(It.IsAny<int>(), It.IsAny<Team>()));

            IActionResult result = controller.PutTeam(teamId, fakeTeam, token);
            var createdResult = result as RedirectToRouteResult;

            sportLogicMock.VerifyAll();

            Assert.IsNotNull(createdResult);
        }

        [TestMethod]
        public void ValidDeleteTeam()
        {
            int teamId = 1;

            teamLogicMock.Setup(teamLogic => teamLogic.Delete(It.IsAny<int>()));

            IActionResult result = controller.DeleteTeam(teamId, token);
            var okResult = result as OkObjectResult;

            teamLogicMock.VerifyAll();

            Assert.AreEqual(200, okResult.StatusCode);
        }

    }
}
