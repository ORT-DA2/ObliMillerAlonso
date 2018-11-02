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
using Sports.Repository.UnitOfWork;
using Sports.Logic.Exceptions;
using Sports.Domain.Exceptions;
using System.Diagnostics.CodeAnalysis;

namespace Sports.Logic.Test
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class SportLogicTest
    {
        private IRepositoryUnitOfWork unitOfWork;
        private RepositoryContext repository;
        private ISportLogic sportLogic;
        private ICompetitorLogic competitorLogic;
        private IUserLogic userLogic;
        private ISessionLogic sessionLogic;
        Sport sport;
        
        
        [TestInitialize]
        public void SetUp()
        {
            SetUpRepositories();
            SetUpAdminSession();
            sport = new Sport()
            {
                Name = "Tennis",
                Amount = 2
            };
        }

        private void SetUpRepositories()
        {
            var options = new DbContextOptionsBuilder<RepositoryContext>()
                .UseInMemoryDatabase<RepositoryContext>(databaseName: "SportLogicTestDB")
                .Options;
            repository = new RepositoryContext(options);
            unitOfWork = new RepositoryUnitOfWork(repository);
            sportLogic = new SportLogic(unitOfWork);
            competitorLogic = new CompetitorLogic(unitOfWork);
            userLogic = new UserLogic(unitOfWork);
            sessionLogic = new SessionLogic(unitOfWork);
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
            sportLogic.SetSession(adminToken);
            competitorLogic.SetSession(adminToken);
        }

        [TestCleanup]
        public void TearDown()
        {
            repository.Users.RemoveRange(repository.Users);
            repository.Sports.RemoveRange(repository.Sports);
            repository.SaveChanges();
        }

        private User ValidUser()
        {
            return new User()
            {
                FirstName = "Itai",
                LastName = "Miller",
                Email = "itaimiller@gmail.com",
                UserName = "iMiller",
                Password = "root"
            };
        }

        [TestMethod]
        public void AddSport()
        {
            sportLogic.AddSport(sport);
            Assert.IsNotNull(sportLogic.GetSportById(sport.Id));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNullValueException))]
        public void AddNullSport()
        {
            sportLogic.AddSport(null);
        }

        [TestMethod]
        public void UpdateSportName()
        {
            sportLogic.AddSport(sport);
            Sport sportChanges = new Sport()
            {
                Name = "Basketball"
            };
            sportLogic.ModifySport(sport.Id, sportChanges);
            Assert.AreEqual<string>(sportLogic.GetSportById(sport.Id).Name, sportChanges.Name);
        }

        
        [TestMethod]
        [ExpectedException(typeof(InvalidEmptyTextFieldException))]
        public void UpdateSportNameInvalid()
        {
            sportLogic.AddSport(sport);
            sport.Name = "";
            sportLogic.ModifySport(sport.Id, sport);
        }


        [TestMethod]
        public void UpdateIgnoreEmptyFields()
        {
            sportLogic.AddSport(sport);
            Sport sportChanges = new Sport()
            {
                Name = ""
            };
            sportLogic.ModifySport(sport.Id, sportChanges);
            Assert.AreNotEqual<string>(sportLogic.GetSportById(sport.Id).Name, sportChanges.Name);
        }

        [TestMethod]
        [ExpectedException(typeof(SportAlreadyExistsException))]
        public void AddDuplicatedName()
        {
            sportLogic.AddSport(sport);
            Sport identicalSport = new Sport()
            {
                Name = "Tennis",
                Amount = 4
            };
            sportLogic.AddSport(identicalSport);
        }

        [TestMethod]
        public void GetSportByName()
        {
            sportLogic.AddSport(sport);
            Assert.AreEqual<string>(sportLogic.GetSportByName(sport.Name).Name, sport.Name);
        }

        [TestMethod]
        [ExpectedException(typeof(SportDoesNotExistException))]
        public void GetSportByInvalidName()
        {
            sportLogic.GetSportByName("fakeName");
        }

        [TestMethod]
        public void DeleteSport()
        {
            sportLogic.AddSport(sport);
            sportLogic.RemoveSport(sport.Id);
            Assert.AreEqual(sportLogic.GetAll().Count, 0);
        }

        [TestMethod]
        public void DeleteCompetitorFromSport()
        {
            sportLogic.AddSport(sport);
            Competitor competitor = new Competitor()
            {
                Name = "Barcelona"
            };
            sportLogic.AddCompetitorToSport(sport.Id, competitor);
            sportLogic.DeleteCompetitorFromSport(sport.Id, competitor.Id);
            Assert.AreEqual(sportLogic.GetSportById(sport.Id).Competitors.Count, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(SportDoesNotExistException))]
        public void DeleteNonExistingSport()
        {
            sportLogic.AddSport(sport);
            sportLogic.RemoveSport(sport.Id + 1);
        }

        [TestMethod]
        public void AddCompetitortoSport()
        {
            sportLogic.AddSport(sport);
            Competitor _competitor = new Competitor()
            {
                Name = "Barcelona"
            };
            sportLogic.AddCompetitorToSport(sport.Id, _competitor);
            Assert.AreEqual(sportLogic.GetSportById(sport.Id).Competitors.Count, 1);
        }

        
        [TestMethod]
        public void UpdateCompetitorSport()
        {
            sportLogic.AddSport(sport);
            Competitor competitor = new Competitor()
            {
                Name = "Barcelona"
            };
            Competitor competitorChanges = new Competitor()
            {
                Name = "Villareal"
            };
            sportLogic.AddCompetitorToSport(sport.Id, competitor);
            sportLogic.UpdateCompetitorSport(sport.Id, competitor.Id, competitorChanges);
            Assert.AreEqual<string>(sportLogic.GetCompetitorFromSport(sport.Id, competitor.Id).Name, competitorChanges.Name);
        }
        

        [TestMethod]
        [ExpectedException(typeof(SportDoesNotExistException))]
        public void AddCompetitorToInvalidSport()
        {
            Competitor _competitor = new Competitor()
            {
                Name = "Barcelona"
            };
            sportLogic.AddCompetitorToSport(sport.Id, _competitor);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidEmptyTextFieldException))]
        public void AddInvalidCompetitor()
        {
            sportLogic.AddSport(sport);
            Competitor _competitor = new Competitor()
            {
                Name = ""
            };
            sportLogic.AddCompetitorToSport(sport.Id, _competitor);
        }

        [TestMethod]
        [ExpectedException(typeof(CompetitorAlreadyInSportException))]
        public void AddDuplicateCompetitorToSport()
        {
            sportLogic.AddSport(sport);
            Competitor competitor = new Competitor()
            {
                Name = "Barcelona"
            };
            Competitor identicalCompetitor = new Competitor()
            {
                Name = "Barcelona"
            };
            sportLogic.AddCompetitorToSport(sport.Id, competitor);
            sportLogic.AddCompetitorToSport(sport.Id, identicalCompetitor);
        }

        [TestMethod]
        public void GetCompetitorFromSport()
        {
            sportLogic.AddSport(sport);
            Competitor competitor = new Competitor()
            {
                Name = "Barcelona"
            };
            sportLogic.AddCompetitorToSport(sport.Id, competitor);
            Competitor returnedCompetitor = sportLogic.GetCompetitorFromSport(sport.Id, competitor.Id);
            Assert.AreEqual(returnedCompetitor, competitor);
        }

        [TestMethod]
        public void GetCompetitorsFromSport()
        {
            sportLogic.AddSport(sport);
            Competitor competitor = new Competitor()
            {
                Name = "Barcelona"
            };
            sportLogic.AddCompetitorToSport(sport.Id, competitor);
            ICollection<Competitor> returnedCompetitors = sportLogic.GetCompetitorsFromSport(sport.Id);
            Assert.AreEqual(1, returnedCompetitors.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidEmptyTextFieldException))]
        public void GetInvalidCompetitorFromSport()
        {
            sportLogic.AddSport(sport);
            Competitor competitor = new Competitor()
            {
                Name = ""
            };
            sportLogic.AddCompetitorToSport(sport.Id, competitor);
            Competitor returnedCompetitor = sportLogic.GetCompetitorFromSport(sport.Id, competitor.Id);
        }

        [TestMethod]
        public void CascadeDeleteCompetitors()
        {
            sportLogic.AddSport(sport);
            Competitor competitor = new Competitor()
            {
                Name = "Competitor"
            };
            sportLogic.AddCompetitorToSport(sport.Id, competitor);
            sportLogic.RemoveSport(sport.Id);
            Assert.AreEqual(competitorLogic.GetAll().Count, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(NonAdminException))]
        public void SportSetSessionNonAdminUser()
        {
            User user = ValidUser();
            Competitor competitor = new Competitor()
            {
                Name = "Competitor"
            };
            Competitor competitorChanges = new Competitor()
            {
                Name = "CompetitorChanges"
            };
            userLogic.AddUser(user);
            Guid token = sessionLogic.LogInUser(user.UserName, user.Password);
            sessionLogic.GetUserFromToken(token);
            sportLogic.SetSession(token);
            sportLogic.AddSport(sport);
        }
        

    }
}
