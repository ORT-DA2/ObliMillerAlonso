using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Sports.Domain;
using Sports.Logic.Interface;
using Sports.Repository;
using Sports.Repository.Interface;
using Sports.Repository.Context;
using System.Diagnostics.CodeAnalysis;
using Sports.Domain.Exceptions;
using Sports.Logic.Exceptions;
using Sports.Repository.UnitOfWork;
using Newtonsoft.Json.Linq;
using System.IO;

namespace Sports.Logic.Test
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class CompetitorLogicTest
    {
        string testImagePath;
        private IRepositoryUnitOfWork unitOfWork;
        private RepositoryContext repository;
        private ICompetitorLogic competitorLogic;
        private IUserLogic userLogic;
        private ISportLogic sportLogic;
        private ISessionLogic sessionLogic;
        private Competitor competitor;
        private Sport sport;

        [TestInitialize]
        public void SetUp()
        {
            SetUpRepositories();
            SetUpAdminSession();
            sport = new Sport()
            {
                Name = "SportName",
                Amount = 2
            };
            sportLogic.AddSport(sport);
            competitor = new Competitor()
            {
                Name = "Competitor",
                Sport = sport
            };
            JObject jsonPaths = JObject.Parse(File.ReadAllText(@"testFilesPaths.json"));
            testImagePath = jsonPaths.SelectToken("TestImagePath").ToString();
        }

        private void SetUpAdminSession()
        {
            User admin = new User(true)
            {
                FirstName = "Rafael",
                LastName = "Alonso",
                Email = "ralonso@gmail.com",
                UserName = "rAlonso",
                Password = "pass"
            };
            IUserRepository repo = unitOfWork.User;
            repo.Create(admin);
            repo.Save();
            Guid adminToken = sessionLogic.LogInUser(admin.UserName, admin.Password);
            sessionLogic.GetUserFromToken(adminToken);
            userLogic.SetSession(adminToken);
            competitorLogic.SetSession(adminToken);
            sportLogic.SetSession(adminToken);
        }

        private void SetUpRepositories()
        {
            var options = new DbContextOptionsBuilder<RepositoryContext>()
                .UseInMemoryDatabase<RepositoryContext>(databaseName: "CompetitorLogicTestDB")
                .Options;
            repository = new RepositoryContext(options);
            unitOfWork = new RepositoryUnitOfWork(repository);
            competitorLogic = new CompetitorLogic(unitOfWork);
            userLogic = new UserLogic(unitOfWork);
            sessionLogic = new SessionLogic(unitOfWork);
            sportLogic = new SportLogic(unitOfWork);
        }

        [TestCleanup]
        public void TearDown()
        {
            repository.Competitors.RemoveRange(repository.Competitors);
            repository.Sports.RemoveRange(repository.Sports);
            repository.Users.RemoveRange(repository.Users);
            repository.SaveChanges();
        }

        [TestMethod]
        public void AddCompetitor()
        {
            competitorLogic.AddCompetitor(competitor);
            Assert.IsNotNull(competitorLogic.GetCompetitorById(competitor.Id));
        }


        [TestMethod]
        [ExpectedException(typeof(InvalidNullValueException))]
        public void AddNullCompetitor()
        {
            competitorLogic.AddCompetitor(null);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidEmptyTextFieldException))]
        public void AddInvalidCompetitor()
        {
            Competitor invalidNameCompetitor = new Competitor()
            {
                Name = ""
            };
            competitorLogic.AddCompetitor(invalidNameCompetitor);
        }

        [TestMethod]
        public void AddCompetitorPicture()
        {
            competitorLogic.AddCompetitor(competitor);
            competitorLogic.SetPictureFromPath(competitor.Id,testImagePath);
            Assert.IsNotNull(competitorLogic.GetCompetitorById(competitor.Id).Picture);
        }


        [TestMethod]
        [ExpectedException(typeof(CompetitorDoesNotExistException))]
        public void AddPictureToInvalidCompetitor()
        {
            competitorLogic.SetPictureFromPath(competitor.Id, testImagePath);
            Assert.IsNotNull(competitorLogic.GetCompetitorById(competitor.Id).Picture);
        }

        private User ValidUser()
        {
            return new User(true)
            {
                FirstName = "Itai",
                LastName = "Miller",
                Email = "itaimiller@gmail.com",
                UserName = "iMiller",
                Password = "root"
            };
        }

        [TestMethod]
        public void ChangeCompetitorName()
        {
            competitorLogic.AddCompetitor(competitor);
            Competitor changeCompetitor = new Competitor()
            {
                Name = "New Name",
                Sport = sport
            };
            competitorLogic.Modify(competitor.Id, changeCompetitor);
            Assert.AreEqual<string>(competitorLogic.GetCompetitorById(competitor.Id).Name,competitor.Name);
        }

        [TestMethod]
        public void ChangeCompetitorNameNull()
        {
            competitorLogic.AddCompetitor(competitor);
            Competitor changeCompetitor = new Competitor()
            {
                Name = null,
                Sport = sport
            };
            competitorLogic.Modify(competitor.Id, changeCompetitor);
            Assert.AreNotEqual<string>(competitorLogic.GetCompetitorById(competitor.Id).Name, changeCompetitor.Name);
        }

        [TestMethod]
        [ExpectedException(typeof(CompetitorAlreadyInSportException))]
        public void ChangeCompetitorNameInvalid()
        {
            competitorLogic.AddCompetitor(competitor);
            Competitor changeCompetitor = new Competitor()
            {
                Name = "Name",
                Sport = sport
            };
            competitorLogic.AddCompetitor(changeCompetitor);
            competitorLogic.Modify(competitor.Id, changeCompetitor);
        }

        [TestMethod]
        public void DeleteCompetitor()
        {
            competitorLogic.AddCompetitor(competitor);
            competitorLogic.Delete(competitor.Id);
            Assert.AreEqual(competitorLogic.GetAll().Count, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(CompetitorDoesNotExistException))]
        public void DeleteInvalidCompetitor()
        {
            competitorLogic.Delete(competitor.Id);
        }

        [TestMethod]
        [ExpectedException(typeof(NonAdminException))]
        public void CompetitorSetSessionNonAdminUser()
        {
            User user = new User()
            {
                FirstName = "Itai",
                LastName = "Miller",
                Email = "itaimiller@gmail.com",
                UserName = "newUser",
                Password = "root"
            };
            userLogic.AddUser(user);
            Guid token = sessionLogic.LogInUser(user.UserName, user.Password);
            sessionLogic.GetUserFromToken(token);
            competitorLogic.SetSession(token);
            competitorLogic.AddCompetitor(competitor);
        }

        [TestMethod]
        public void FilterOrderCompetitorName()
        {
            Competitor otherCompetitor = new Competitor()
            {
                Name = "CompetitorName"
            };
            competitorLogic.AddCompetitor(otherCompetitor);
            ICollection<Competitor> filteredCompetitors = competitorLogic.GetFilteredCompetitors("CompetitorName","asc");
            Assert.AreEqual(filteredCompetitors.Count, 1);
        }

        [TestMethod]
        public void FilterOrderCompetitorNameDesc()
        {
            Competitor otherCompetitor = new Competitor()
            {
                Name = "CompetitorName"
            };
            string order = "desc";
            competitorLogic.AddCompetitor(otherCompetitor);
            ICollection<Competitor> filteredCompetitors = competitorLogic.GetFilteredCompetitors("CompetitorName", order);
            Assert.AreEqual(filteredCompetitors.Count, 1);
        }

        [TestMethod]
        public void InvalidFilterOrderCompetitorName()
        {
            Competitor otherCompetitor = new Competitor()
            {
                Name = "CompetitorName"
            };
            competitorLogic.AddCompetitor(otherCompetitor);
            ICollection<Competitor> filteredCompetitors = competitorLogic.GetFilteredCompetitors(null,null);
            Assert.AreEqual(filteredCompetitors.Count, 1);
        }

    }
}
