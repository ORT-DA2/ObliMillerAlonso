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
    public class CompetitorsControllerTest
    {
        Mock<ICompetitorLogic> competitorLogicMock;
        Mock<ISportLogic> sportLogicMock;
        CompetitorsController controller;
        string token;

        [TestInitialize]
        public void SetUp()
        {

            competitorLogicMock = new Mock<ICompetitorLogic>();
            sportLogicMock = new Mock<ISportLogic>();
            var config = new MapperConfiguration(cfg => cfg.AddProfile(new MapperProfile()));
            IMapper mapper = new Mapper(config);
            controller = new CompetitorsController(competitorLogicMock.Object, sportLogicMock.Object);
            competitorLogicMock.Setup(competitorLogic => competitorLogic.SetSession(It.IsAny<Guid>()));
            token = new Guid().ToString();
        }

        [TestMethod]
        public void ValidGetCompetitors()
        {
            Competitor fakeCompetitor = new Competitor()
            {
                Name = "Competitor"
            };
            ICollection<Competitor> competitors = new List<Competitor>();
            competitors.Add(fakeCompetitor);

            competitorLogicMock.Setup(sportLogic => sportLogic.GetFilteredCompetitors(It.IsAny<string>(), It.IsAny<string>())).Returns(competitors);

            IActionResult result = controller.GetAllCompetitors(token,null,null);
            var okResult = result as OkObjectResult;
            var modelOut = okResult.Value as ICollection<CompetitorModelOut>;

            competitorLogicMock.VerifyAll();

            Assert.AreEqual(200, okResult.StatusCode);
            Assert.IsNotNull(modelOut);
        }

        [TestMethod]
        public void ValidGetCompetitorById()
        {
            int competitorId = 1;
            Competitor fakeCompetitor = new Competitor()
            {
                Id = competitorId,
                Name = "Competitor"
            };

            competitorLogicMock.Setup(competitorLogic => competitorLogic.GetCompetitorById(competitorId)).Returns(fakeCompetitor);

            IActionResult result = controller.Get(competitorId, token);
            var okResult = result as OkObjectResult;
            var modelOut = okResult.Value as CompetitorModelOut;

            competitorLogicMock.VerifyAll();

            Assert.AreEqual(200, okResult.StatusCode);
            Assert.IsNotNull(modelOut);
        }


        [TestMethod]
        public void ValidModifyCompetitor()
        {

            CompetitorModelIn fakeCompetitor = new CompetitorModelIn()
            {
                Name = "ChangedName"
            };
            int competitorId = 1;


            competitorLogicMock.Setup(competitorLogic => competitorLogic.Modify(It.IsAny<int>(), It.IsAny<Competitor>()));

            IActionResult result = controller.PutCompetitor(competitorId, fakeCompetitor, token);
            var createdResult = result as RedirectToRouteResult;

            sportLogicMock.VerifyAll();

            Assert.IsNotNull(createdResult);
        }

        [TestMethod]
        public void ValidDeleteCompetitor()
        {
            int competitorId = 1;

            competitorLogicMock.Setup(competitorLogic => competitorLogic.Delete(It.IsAny<int>()));

            IActionResult result = controller.DeleteCompetitor(competitorId, token);
            var okResult = result as OkObjectResult;

            competitorLogicMock.VerifyAll();

            Assert.AreEqual(200, okResult.StatusCode);
        }

    }
}
