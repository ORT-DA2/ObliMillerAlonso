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
using Sports.Repository.UnitOfWork;
using Sports.Logic.Exceptions;
using Sports.Domain.Exceptions;
using System.Linq;

namespace Sports.Logic.Test
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class MatchLogicTest
    {
        private IRepositoryUnitOfWork unit;
        private RepositoryContext repository;
        private IMatchLogic matchLogic;
        private ISportLogic sportLogic;
        private IUserLogic userLogic;
        private ICommentLogic commentLogic;
        private ISessionLogic sessionLogic;
        private Match match;
        private CompetitorScore localCompetitor;
        private CompetitorScore visitorCompetitor;
        private User user;
        private Sport sport;

        [TestInitialize]
        public void SetUp()
        {
            SetupRepositories();
            SetUpAdminSession();
            CreateBaseDataForTests();
            user = ValidUser();
            userLogic.AddUser(user);
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
            IUserRepository repo = unit.User;
            repo.Create(admin);
            repo.Save();
            Guid adminToken = sessionLogic.LogInUser(admin.UserName, admin.Password);
            sessionLogic.GetUserFromToken(adminToken);
            userLogic.SetSession(adminToken);
            matchLogic.SetSession(adminToken);
            sportLogic.SetSession(adminToken);
            commentLogic.SetSession(adminToken);
        }

        private void CreateBaseDataForTests()
        {
            sport = new Sport()
            {
                Name = "Match Sport",
                Amount = 2
            };
            sportLogic.AddSport(sport);
            localCompetitor = new CompetitorScore(AddCompetitorToSport(sport, "Local competitor"));
            visitorCompetitor = new CompetitorScore(AddCompetitorToSport(sport, "Visitor competitor"));
            ICollection<CompetitorScore> competitors = new List<CompetitorScore>() { localCompetitor, visitorCompetitor };
            match = new Match()
            {
                Sport = sport,
                Competitors = competitors,
                Date = DateTime.Now.AddDays(1)
            };
        }

        private void SetupRepositories()
        {
            var options = new DbContextOptionsBuilder<RepositoryContext>()
                            .UseInMemoryDatabase<RepositoryContext>(databaseName: "MatchLogicTestDB")
                            .Options;
            repository = new RepositoryContext(options);
            unit = new RepositoryUnitOfWork(repository);
            matchLogic = new MatchLogic(unit);
            sportLogic = new SportLogic(unit);
            userLogic = new UserLogic(unit);
            commentLogic = new CommentLogic(unit);
            sessionLogic = new SessionLogic(unit);
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
            repository.Matches.RemoveRange(repository.Matches);
            repository.Competitors.RemoveRange(repository.Competitors);
            repository.CompetitorScores.RemoveRange(repository.CompetitorScores);
            repository.Sports.RemoveRange(repository.Sports);
            repository.Users.RemoveRange(repository.Users);
            repository.Comments.RemoveRange(repository.Comments);
            repository.SaveChanges();
        }

        [TestMethod]
        public void AddFullMatch()
        {
            matchLogic.AddMatch(match);
            Assert.IsNotNull(matchLogic.GetMatchById(match.Id));
        }


        [TestMethod]
        [ExpectedException(typeof(MatchAlreadyExistsException))]
        public void AddFullMatchTwice()
        {
            matchLogic.AddMatch(match);
            matchLogic.AddMatch(match);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNullValueException))]
        public void AddNullMatch()
        {
            matchLogic.AddMatch(null);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidMatchDateFormatException))]
        public void AddMatchInvalidDate()
        {
            match.Date = DateTime.Now.AddDays(-1);
            matchLogic.AddMatch(match);
        }

        [TestMethod]
        [ExpectedException(typeof(CompetitorDoesNotExistException))]
        public void AddMatchInvalidLocalCompetitor()
        {
            Competitor invalidCompetitor = new Competitor()
            {
                Name = "Unregistered competitor"
            };
            match.Competitors = new List<CompetitorScore>() { new CompetitorScore(invalidCompetitor),visitorCompetitor };
            matchLogic.AddMatch(match);
        }
        
        [TestMethod]
        public void ModifyDate()
        {
            matchLogic.AddMatch(match);
            match.Date = DateTime.Now.AddDays(+2);
            matchLogic.ModifyMatch(match.Id, match);
            Assert.AreEqual(matchLogic.GetMatchById(match.Id).Date, match.Date);
        }

        [TestMethod]
        [ExpectedException(typeof(MatchDoesNotExistException))]
        public void ModifyInvalidMatch()
        {
            matchLogic.ModifyMatch(match.Id, match);
        }
        
        [TestMethod]
        [ExpectedException(typeof(SportDoesNotExistException))]
        public void AddMatchWithInvalidSport()
        {
            Sport newSport = new Sport()
            {
                Name = "Test Sport",
                Amount = 2
            };
            match.Sport = newSport;
            matchLogic.AddMatch(match);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidSportIsEmptyException))]
        public void AddMatchWithoutSport()
        {
            match.Sport = null;
            matchLogic.AddMatch(match);
        }
        
        [TestMethod]
        public void ModifySportAndCompetitors()
        {
            matchLogic.AddMatch(match);
            Sport sport = new Sport()
            {
                Name = "New Sport",
                Amount = 2
            };
            sportLogic.AddSport(sport);
            Competitor newLocalCompetitor = AddCompetitorToSport(sport, "New Local competitor");
            Competitor newVisitorCompetitor = AddCompetitorToSport(sport, "New Visitor competitor");
            ICollection<CompetitorScore> competitors = new List<CompetitorScore>() { new CompetitorScore(newLocalCompetitor), new CompetitorScore(newVisitorCompetitor) };
            match.Sport = sport;
            match.Competitors = competitors;
            matchLogic.ModifyMatch(match.Id, match);
            Assert.AreEqual(matchLogic.GetMatchById(match.Id).Sport, sport);
        }
        
        [TestMethod]
        [ExpectedException(typeof(SportDoesNotExistException))]
        public void ModifyInvalidSport()
        {
            matchLogic.AddMatch(match);
            Sport sport = new Sport()
            {
                Name = "Test Sport",
                Amount = 2
            };
            
            match.Sport = sport;
            matchLogic.ModifyMatch(match.Id, match);
        }
        

        [TestMethod]
        public void ModifyCompetitor()
        {
            matchLogic.AddMatch(match);
            Competitor localCompetitor = new Competitor()
            {
                Name = "New Local competitor",

            };
            sportLogic.AddCompetitorToSport(sport.Id, localCompetitor);
            CompetitorScore adaptedCompetitor = new CompetitorScore(localCompetitor);
            ICollection<CompetitorScore> competitors = new List<CompetitorScore>() { adaptedCompetitor , visitorCompetitor};
            match.Competitors = competitors;
            matchLogic.ModifyMatch(match.Id, match);
            Assert.IsTrue(matchLogic.GetMatchById(match.Id).Competitors.Contains(adaptedCompetitor));
        }
        

        [TestMethod]
        [ExpectedException(typeof(CompetitorDoesNotExistException))]
        public void ModifyInvalidCompetitor()
        {
            matchLogic.AddMatch(match);
            Competitor localCompetitor = new Competitor()
            {
                Name = "New Local competitor",

            };
            CompetitorScore adaptedCompetitor = new CompetitorScore(localCompetitor);
            match.Competitors = new List<CompetitorScore>() { adaptedCompetitor , visitorCompetitor};
            matchLogic.ModifyMatch(match.Id, match);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCompetitorAmountException))]
        public void AddWithLessCompetitors()
        {
            match.Competitors = new List<CompetitorScore>() { localCompetitor };
            matchLogic.AddMatch(match);
        }


        [TestMethod]
        [ExpectedException(typeof(InvalidCompetitorAmountException))]
        public void AddWithMoreCompetitors()
        {
            Competitor newCompetitor = new Competitor()
            {
                Name = "New Local competitor",

            };
            sportLogic.AddCompetitorToSport(sport.Id, newCompetitor);
            CompetitorScore adaptedCompetitor = new CompetitorScore(newCompetitor);
            match.Competitors = new List<CompetitorScore>() { localCompetitor, adaptedCompetitor, visitorCompetitor };
            matchLogic.AddMatch(match);
        }

        [TestMethod]
        public void ModifyIgnoresNullFields()
        {
            matchLogic.AddMatch(match);
            Competitor original = match.Competitors.First().Competitor;
            Match updatedMatch = new Match()
            {
            };
            matchLogic.ModifyMatch(match.Id, updatedMatch);
            Assert.AreEqual(matchLogic.GetMatchById(match.Id).Competitors.First().Competitor, original);
        }
        
        [TestMethod]
        public void DeleteMatch()
        {
            matchLogic.AddMatch(match);
            matchLogic.DeleteMatch(match.Id);
            Assert.AreEqual(matchLogic.GetAllMatches().Count, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(MatchDoesNotExistException))]
        public void DeleteNonExistingMatch()
        {
            matchLogic.DeleteMatch(match.Id);
        }


        [TestMethod]
        public void AddCommentToMatch()
        {
            matchLogic.AddMatch(match);
            Comment comment = new Comment
            {
                Text = "Text",
                User = user
            };
            matchLogic.AddCommentToMatch(match.Id, comment);
            Comment commentInMatchStored = matchLogic.GetAllComments(match.Id).FirstOrDefault();
            Assert.AreEqual(commentInMatchStored.Id, comment.Id);
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
        [ExpectedException(typeof(MatchDoesNotExistException))]
        public void AddCommentToInexistentMatch()
        {
            Comment comment = new Comment
            {
                Text = "Text",
                User = user
            };
            matchLogic.AddCommentToMatch(match.Id, comment);
        }
        

        [TestMethod]
        public void CheckSportIsNotDuplicated()
        {
            matchLogic.AddMatch(match);
            Assert.AreEqual(sportLogic.GetAll().Count,1);
        }

        [TestMethod]
        [ExpectedException(typeof(MatchDoesNotExistException))]
        public void GetMatchesForCompetitorThatDidntPlay()
        {
            Sport sport = new Sport()
            {
                Name = "Unplayed Sport",
                Amount = 2
            };
            sportLogic.AddSport(sport);

            Competitor unplayedCompetitor = new Competitor()
            {
                Name = "New Local competitor"
            };
            sportLogic.AddCompetitorToSport(sport.Id, unplayedCompetitor);
            matchLogic.GetAllMatchesForCompetitor(unplayedCompetitor);
        }


        [TestMethod]
        public void CascadeDeleteMatchFromSport()
        { 
            matchLogic.AddMatch(match);
            sportLogic.RemoveSport(sport.Id);
            Assert.AreEqual(matchLogic.GetAllMatches().Count,0);
        }

        [TestMethod]
        public void CascadeDeleteMatchFromCompetitor()
        {
            matchLogic.AddMatch(match);
            sportLogic.DeleteCompetitorFromSport(sport.Id,match.Competitors.First().Competitor.Id);
            Assert.AreEqual(matchLogic.GetAllMatches().Count, 0);
        }
        

        [TestMethod]
        public void CascadeDeleteCommentsFromMatch()
        {
            matchLogic.AddMatch(match);
            Comment comment = new Comment
            {
                Text = "Text",
                User = user
            };
            matchLogic.AddCommentToMatch(match.Id, comment);
            matchLogic.DeleteMatch(match.Id);
            Assert.AreEqual(commentLogic.GetAll().Count, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(NonAdminException))]
        public void MatchSetSessionNonAdminUser()
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
            matchLogic.SetSession(token);
            matchLogic.AddMatch(match);
        }
        

    }
}
