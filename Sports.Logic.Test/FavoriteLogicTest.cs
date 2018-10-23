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
using Sports.Repository.UnitOfWork;
using Sports.Repository.Context;
using System.Diagnostics.CodeAnalysis;
using Sports.Logic.Exceptions;
using Sports.Domain.Exceptions;


namespace Sports.Logic.Test
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class FavoriteLogicTest
    {
        private IRepositoryUnitOfWork unitOfWork;
        private RepositoryContext repository;
        private IFavoriteLogic favoriteLogic;
        private IMatchLogic matchLogic;
        private ISportLogic sportLogic;
        private IUserLogic userLogic;
        private ISessionLogic sessionLogic;
        User user;
        Comment comment;
        Competitor favoriteCompetitor;
        Match match;
        Sport sport;

        [TestInitialize]
        public void SetUp()
        {
            SetUpRepositories();
            SetUpAdminSession();
            AddMatchWithDataToRepository();
            AddUserToRepository();
            comment = new Comment()
            {
                Text = "text",
                User = user
            };
            
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
            userLogic.SetSession(adminToken);
            matchLogic.SetSession(adminToken);
            sportLogic.SetSession(adminToken);
            favoriteLogic.SetSession(adminToken);
        }

        private void AddMatchWithDataToRepository()
        {
            sport = AddSportToRepository();
            favoriteCompetitor = AddCompetitorToSport(sport, "Local competitor");
            Competitor visitorCompetitor = AddCompetitorToSport(sport, "Visitor competitor");
            match = new Match()
            {
                Sport = sport,
                Local = favoriteCompetitor,
                Visitor = visitorCompetitor,
                Date = DateTime.Now.AddDays(1)
            };
            matchLogic.AddMatch(match);
        }

        private Sport AddSportToRepository()
        {
            Sport sport = new Sport()
            {
                Name = "Match Sport"
            };
            sportLogic.AddSport(sport);
            return sport;
        }
        private void AddUserToRepository()
        {
            user = new User()
            {
                FirstName = "itai",
                LastName = "miller",
                Email = "itai@gmail.com",
                UserName = "iMiller",
                Password = "root"
            };
            userLogic.AddUser(user);
        }
       
        private void SetUpRepositories()
        {
            var options = new DbContextOptionsBuilder<RepositoryContext>()
                            .UseInMemoryDatabase<RepositoryContext>(databaseName: "FavoriteLogicTestDB")
                            .Options;
            repository = new RepositoryContext(options);
            unitOfWork = new RepositoryUnitOfWork(repository);
            favoriteLogic = new FavoriteLogic(unitOfWork);
            userLogic = new UserLogic(unitOfWork);
            matchLogic = new MatchLogic(unitOfWork);
            sportLogic = new SportLogic(unitOfWork);
            sessionLogic = new SessionLogic(unitOfWork);
        }

        private Competitor AddCompetitorToSport(Sport sport, string competitorName)
        {
            Competitor competitor = new Competitor()
            {
                Name = competitorName,
            };
            sportLogic.AddCompetitorToSport(sport.Id, competitor);
            return competitor;
        }

        [TestCleanup]
        public void TearDown()
        {
            repository.Favorites.RemoveRange(repository.Favorites);
            repository.Users.RemoveRange(repository.Users);
            repository.Competitors.RemoveRange(repository.Competitors);
            repository.Matches.RemoveRange(repository.Matches);
            repository.Sports.RemoveRange(repository.Sports);
            repository.SaveChanges();
        }
        
        [TestMethod]
        public void GetFavoritesForUser()
        {
            favoriteLogic.AddFavoriteCompetitor(favoriteCompetitor);
            ICollection<Competitor> favorites = favoriteLogic.GetFavoritesFromUser();
            Assert.AreEqual(favorites.Count, 1);
        }


        [TestMethod]
        [ExpectedException(typeof(FavoriteAlreadyExistException))]
        public void AddFavoriteTwice()
        {
            favoriteLogic.AddFavoriteCompetitor( favoriteCompetitor);
            favoriteLogic.AddFavoriteCompetitor( favoriteCompetitor);
        }

        [TestMethod]
        [ExpectedException(typeof(FavoriteDoesNotExistException))]
        public void GetFavoritesForInexistentUser()
        {
            ICollection<Competitor> favorites = favoriteLogic.GetFavoritesFromUser();
            Assert.AreEqual(favorites.Count, 0);
        }


        [TestMethod]
        public void GetFavoritesCompetitorsComments()
        {
            favoriteLogic.AddFavoriteCompetitor( favoriteCompetitor);
            matchLogic.AddCommentToMatch(match.Id, comment);
            ICollection<Comment> favoriteComments = favoriteLogic.GetFavoritesCompetitorsComments();
            Assert.AreEqual(favoriteComments.Count, 1);
        }
        


        [TestMethod]
        [ExpectedException(typeof(CompetitorDoesNotExistException))]
        public void AddInvalidCompetitor()
        {
            Competitor fakeCompetitor = new Competitor();
            favoriteLogic.AddFavoriteCompetitor( fakeCompetitor);
        }

        
        [TestMethod]
        [ExpectedException(typeof(InvalidCompetitorAmountException))]
        public void AddNullCompetitor()
        {
            favoriteLogic.AddFavoriteCompetitor( null);
        }



        [TestMethod]
        public void CascadeDeleteFavoritesFromUser()
        {
            Guid userToken = sessionLogic.LogInUser(user.UserName, user.Password);
            favoriteLogic.SetSession(userToken);
            favoriteLogic.AddFavoriteCompetitor(favoriteCompetitor);
            userLogic.RemoveUser(user.Id);
            Assert.AreEqual(favoriteLogic.GetAll().Count, 0);
        }

        [TestMethod]
        public void CascadeDeleteFavoritesFromCompetitor()
        {
            favoriteLogic.AddFavoriteCompetitor(favoriteCompetitor);
            sportLogic.DeleteCompetitorFromSport(sport.Id, favoriteCompetitor.Id);
            Assert.AreEqual(favoriteLogic.GetAll().Count, 0);
        }


        [TestMethod]
        [ExpectedException(typeof(InvalidNullValueException))]
        public void NullSession()
        {
            favoriteLogic = new FavoriteLogic(unitOfWork);
            favoriteLogic.AddFavoriteCompetitor(favoriteCompetitor);
        }


    }
}
