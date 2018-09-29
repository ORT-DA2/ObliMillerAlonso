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
        User user;
        Comment comment;
        Team favoriteTeam;
        Match match;

        [TestInitialize]
        public void SetUp()
        {
            SetUpRepositories();
            AddUserToRepository();
            AddMatchWithDataToRepository();
            comment = new Comment()
            {
                Text = "text",
                User = user
            };
        }

        private void AddMatchWithDataToRepository()
        {
            Sport sport = AddSportToRepository();
            favoriteTeam = AddTeamToSport(sport, "Local team");
            Team visitorTeam = AddTeamToSport(sport, "Visitor team");
            match = new Match()
            {
                Sport = sport,
                Local = favoriteTeam,
                Visitor = visitorTeam,
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
        }

        private Team AddTeamToSport(Sport sport, string teamName)
        {
            Team team = new Team()
            {
                Name = teamName,
            };
            sportLogic.AddTeamToSport(sport, team);
            return team;
        }

        [TestCleanup]
        public void TearDown()
        {
            repository.Favorites.RemoveRange(repository.Favorites);
            repository.Users.RemoveRange(repository.Users);
            repository.Teams.RemoveRange(repository.Teams);
            repository.Matches.RemoveRange(repository.Matches);
            repository.Sports.RemoveRange(repository.Sports);
            repository.SaveChanges();
        }
        
        [TestMethod]
        public void GetFavoritesForUser()
        {
            favoriteLogic.AddFavoriteTeam(user, favoriteTeam);
            ICollection<Team> favorites = favoriteLogic.GetFavoritesFromUser(user.Id);
            Assert.AreEqual(favorites.Count, 1);
        }


        [TestMethod]
        [ExpectedException(typeof(FavoriteAlreadyExistException))]
        public void AddFavoriteTwice()
        {
            favoriteLogic.AddFavoriteTeam(user, favoriteTeam);
            favoriteLogic.AddFavoriteTeam(user, favoriteTeam);
        }

        [TestMethod]
        [ExpectedException(typeof(FavoriteDoesNotExistException))]
        public void GetFavoritesForInexistentUser()
        {
            ICollection<Team> favorites = favoriteLogic.GetFavoritesFromUser(user.Id);
            Assert.AreEqual(favorites.Count, 0);
        }


        [TestMethod]
        public void GetFavoritesTeamsComments()
        {
            
            favoriteLogic.AddFavoriteTeam(user, favoriteTeam);
            matchLogic.AddCommentToMatch(match.Id, comment);
            ICollection<Comment> favoriteComments = favoriteLogic.GetFavoritesTeamsComments(user);
            Assert.AreEqual(favoriteComments.Count, 1);
        }


        [TestMethod]
        [ExpectedException(typeof(UserDoesNotExistException))]
        public void AddInvalidUser()
        {
            User fakeUser = new User();
            favoriteLogic.AddFavoriteTeam(fakeUser, favoriteTeam);
        }


        [TestMethod]
        [ExpectedException(typeof(TeamDoesNotExistException))]
        public void AddInvalidTeam()
        {
            Team fakeTeam = new Team();
            favoriteLogic.AddFavoriteTeam(user, fakeTeam);
        }


        [TestMethod]
        [ExpectedException(typeof(InvalidEmptyUserException))]
        public void AddNullUser()
        {
            favoriteLogic.AddFavoriteTeam(null, favoriteTeam);
        }


        [TestMethod]
        [ExpectedException(typeof(InvalidTeamIsEmptyException))]
        public void AddNullTeam()
        {
            favoriteLogic.AddFavoriteTeam(user, null);
        }



        [TestMethod]
        public void CascadeDeleteFavoritesFromUser()
        {
            favoriteLogic.AddFavoriteTeam(user, favoriteTeam);
            userLogic.RemoveUser(user.Id);
            Assert.AreEqual(favorites.Count, 1);
        }

    }
}
