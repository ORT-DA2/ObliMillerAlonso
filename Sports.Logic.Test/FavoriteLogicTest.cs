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

namespace Sports.Logic.Test
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class FavoriteLogicTest
    {
        private IRepositoryUnitOfWork unitOfWork;
        private RepositoryContext repository;
        private IFavoriteLogic favoriteLogic;
        private IUserLogic userLogic;
        private ITeamLogic teamLogic;
        private ICommentLogic commentLogic;
        private IMatchLogic matchLogic;
        private ISportLogic sportLogic;
        Favorite favorite;
        User user;
        Team team;
        Comment comment;
        Team localTeam;
        Team visitorTeam;
        Sport sport;
        Match match;

        [TestInitialize]
        public void SetUp()
        {
            user = new User()
            {
                FirstName = "itai",
                LastName = "miller",
                Email = "itai@gmail.com",
                UserName = "iMiller",
                Password = "root"
            };
            team = new Team()
            {
                Name = "Barcelona"
            };
            comment = new Comment()
            {
                Text = "text",
                User = user
            };
            localTeam = new Team()
            {
                Name = "Local team"
            };
            visitorTeam = new Team()
            {
                Name = "Visitor team"
            };
            sport = new Sport()
            {
                Name = "Match Sport"
            };
            match = new Match()
            {
                Sport = sport,
                Local = localTeam,
                Visitor = visitorTeam,
                Date = DateTime.Now.AddDays(1)
            };
            var options = new DbContextOptionsBuilder<RepositoryContext>()
                .UseInMemoryDatabase<RepositoryContext>(databaseName: "FavoriteLogicTestDB")
                .Options;
            repository = new RepositoryContext(options);
            unitOfWork = new RepositoryUnitOfWork(repository);
            favoriteLogic = new FavoriteLogic(unitOfWork);
            userLogic = new UserLogic(unitOfWork);
            teamLogic = new TeamLogic(unitOfWork);
            commentLogic = new CommentLogic(unitOfWork);
            matchLogic = new MatchLogic(unitOfWork);
            sportLogic = new SportLogic(unitOfWork);
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
            userLogic.AddUser(user);
            teamLogic.AddTeam(team);
            favoriteLogic.AddFavoriteTeam(user, team);
            ICollection<Team> favorites = favoriteLogic.GetFavoritesFromUser(user.Id);
            Assert.AreEqual(favorites.Count, 1);
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
            userLogic.AddUser(user);
            sportLogic.AddSport(sport);
            sportLogic.AddTeamToSport(sport, localTeam);
            sportLogic.AddTeamToSport(sport, visitorTeam);
            matchLogic.AddMatch(match);
            favoriteLogic.AddFavoriteTeam(user, localTeam);
            matchLogic.AddCommentToMatch(match.Id, comment);
            ICollection<Comment> favoriteComments = favoriteLogic.GetFavoritesTeamsComments(user);
            Assert.AreEqual(favoriteComments.Count, 1);
        }


        [TestMethod]
        [ExpectedException(typeof(UserDoesNotExistException))]
        public void AddInvalidUser()
        {
            sportLogic.AddSport(sport);
            sportLogic.AddTeamToSport(sport, localTeam);
            favoriteLogic.AddFavoriteTeam(user, localTeam);
        }

    }
}
