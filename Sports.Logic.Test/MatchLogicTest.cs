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
        private Match teamMatch;
        private Match athleteMatch;
        private CompetitorScore localCompetitor;
        private CompetitorScore visitorCompetitor;
        private CompetitorScore thirdCompetitor;
        private CompetitorScore fourthCompetitor;
        private User user;
        private Sport teamSport;
        private Sport athleteSport;

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
            teamSport = new Sport()
            {
                Name = "Match Sport",
                Amount = 2
            };
            sportLogic.AddSport(teamSport);
            localCompetitor = new CompetitorScore(AddCompetitorToSport(teamSport, "Local competitor"));
            visitorCompetitor = new CompetitorScore(AddCompetitorToSport(teamSport, "Visitor competitor"));
            ICollection<CompetitorScore> competitors = new List<CompetitorScore>() { localCompetitor, visitorCompetitor };
            teamMatch = new Match()
            {
                Sport = teamSport,
                Competitors = competitors,
                Date = DateTime.Now.AddHours(2)
            };
        }


        private void CreateAthleteMatch()
        {
            athleteSport = new Sport()
            {
                Name = "Athlete Sport",
                Amount = 4
            };
            sportLogic.AddSport(athleteSport);
            localCompetitor = new CompetitorScore(AddCompetitorToSport(athleteSport, "Local competitor"));
            visitorCompetitor = new CompetitorScore(AddCompetitorToSport(athleteSport, "Visitor competitor"));
            thirdCompetitor = new CompetitorScore(AddCompetitorToSport(athleteSport, "Third competitor"));
            fourthCompetitor = new CompetitorScore(AddCompetitorToSport(athleteSport, "Fourth competitor"));
            ICollection<CompetitorScore> competitors = new List<CompetitorScore>() { localCompetitor, visitorCompetitor, thirdCompetitor, fourthCompetitor };
            athleteMatch = new Match()
            {
                Sport = athleteSport,
                Competitors = competitors,
                Date = DateTime.Now.AddHours(2)
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
            matchLogic.AddMatch(teamMatch);
            Assert.IsNotNull(matchLogic.GetMatchById(teamMatch.Id));
        }


        [TestMethod]
        [ExpectedException(typeof(MatchAlreadyExistsException))]
        public void AddFullMatchTwice()
        {
            matchLogic.AddMatch(teamMatch);
            matchLogic.AddMatch(teamMatch);
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
            teamMatch.Date = DateTime.Now.AddDays(-1);
            matchLogic.AddMatch(teamMatch);
        }

        [TestMethod]
        [ExpectedException(typeof(CompetitorDoesNotExistException))]
        public void AddMatchInvalidLocalCompetitor()
        {
            Competitor invalidCompetitor = new Competitor()
            {
                Name = "Unregistered competitor"
            };
            teamMatch.Competitors = new List<CompetitorScore>() { new CompetitorScore(invalidCompetitor),visitorCompetitor };
            matchLogic.AddMatch(teamMatch);
        }
        
        [TestMethod]
        public void ModifyDate()
        {
            matchLogic.AddMatch(teamMatch);
            teamMatch.Date = DateTime.Now.AddDays(+2);
            matchLogic.ModifyMatch(teamMatch.Id, teamMatch);
            Assert.AreEqual(matchLogic.GetMatchById(teamMatch.Id).Date, teamMatch.Date);
        }

        [TestMethod]
        [ExpectedException(typeof(MatchDoesNotExistException))]
        public void ModifyInvalidMatch()
        {
            matchLogic.ModifyMatch(teamMatch.Id, teamMatch);
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
            teamMatch.Sport = newSport;
            matchLogic.AddMatch(teamMatch);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidSportIsEmptyException))]
        public void AddMatchWithoutSport()
        {
            teamMatch.Sport = null;
            matchLogic.AddMatch(teamMatch);
        }
        
        [TestMethod]
        public void ModifySportAndCompetitors()
        {
            matchLogic.AddMatch(teamMatch);
            Sport sport = new Sport()
            {
                Name = "New Sport",
                Amount = 2
            };
            sportLogic.AddSport(sport);
            Competitor newLocalCompetitor = AddCompetitorToSport(sport, "New Local competitor");
            Competitor newVisitorCompetitor = AddCompetitorToSport(sport, "New Visitor competitor");
            ICollection<CompetitorScore> competitors = new List<CompetitorScore>() { new CompetitorScore(newLocalCompetitor), new CompetitorScore(newVisitorCompetitor) };
            teamMatch.Sport = sport;
            teamMatch.Competitors = competitors;
            matchLogic.ModifyMatch(teamMatch.Id, teamMatch);
            Assert.AreEqual(matchLogic.GetMatchById(teamMatch.Id).Sport, sport);
        }
        
        [TestMethod]
        [ExpectedException(typeof(SportDoesNotExistException))]
        public void ModifyInvalidSport()
        {
            matchLogic.AddMatch(teamMatch);
            Sport sport = new Sport()
            {
                Name = "Test Sport",
                Amount = 2
            };
            
            teamMatch.Sport = sport;
            matchLogic.ModifyMatch(teamMatch.Id, teamMatch);
        }
        

        [TestMethod]
        public void ModifyCompetitor()
        {
            matchLogic.AddMatch(teamMatch);
            Competitor localCompetitor = new Competitor()
            {
                Name = "New Local competitor",

            };
            sportLogic.AddCompetitorToSport(teamSport.Id, localCompetitor);
            CompetitorScore adaptedCompetitor = new CompetitorScore(localCompetitor);
            ICollection<CompetitorScore> competitors = new List<CompetitorScore>() { adaptedCompetitor , visitorCompetitor};
            teamMatch.Competitors = competitors;
            matchLogic.ModifyMatch(teamMatch.Id, teamMatch);
            Assert.IsTrue(matchLogic.GetMatchById(teamMatch.Id).Competitors.Contains(adaptedCompetitor));
        }
        

        [TestMethod]
        [ExpectedException(typeof(CompetitorDoesNotExistException))]
        public void ModifyInvalidCompetitor()
        {
            matchLogic.AddMatch(teamMatch);
            Competitor localCompetitor = new Competitor()
            {
                Name = "New Local competitor",

            };
            CompetitorScore adaptedCompetitor = new CompetitorScore(localCompetitor);
            teamMatch.Competitors = new List<CompetitorScore>() { adaptedCompetitor , visitorCompetitor};
            matchLogic.ModifyMatch(teamMatch.Id, teamMatch);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCompetitorAmountException))]
        public void AddWithLessCompetitors()
        {
            teamMatch.Competitors = new List<CompetitorScore>() { localCompetitor };
            matchLogic.AddMatch(teamMatch);
        }


        [TestMethod]
        [ExpectedException(typeof(InvalidCompetitorAmountException))]
        public void AddWithMoreCompetitors()
        {
            Competitor newCompetitor = new Competitor()
            {
                Name = "New Local competitor",

            };
            sportLogic.AddCompetitorToSport(teamSport.Id, newCompetitor);
            CompetitorScore adaptedCompetitor = new CompetitorScore(newCompetitor);
            teamMatch.Competitors = new List<CompetitorScore>() { localCompetitor, adaptedCompetitor, visitorCompetitor };
            matchLogic.AddMatch(teamMatch);
        }

        [TestMethod]
        public void ModifyIgnoresNullFields()
        {
            matchLogic.AddMatch(teamMatch);
            Competitor original = teamMatch.Competitors.First().Competitor;
            Match updatedMatch = new Match()
            {
            };
            matchLogic.ModifyMatch(teamMatch.Id, updatedMatch);
            Assert.AreEqual(matchLogic.GetMatchById(teamMatch.Id).Competitors.First().Competitor, original);
        }
        
        [TestMethod]
        public void DeleteMatch()
        {
            matchLogic.AddMatch(teamMatch);
            matchLogic.DeleteMatch(teamMatch.Id);
            Assert.AreEqual(matchLogic.GetAllMatches().Count, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(MatchDoesNotExistException))]
        public void DeleteNonExistingMatch()
        {
            matchLogic.DeleteMatch(teamMatch.Id);
        }


        [TestMethod]
        public void AddCommentToMatch()
        {
            matchLogic.AddMatch(teamMatch);
            Comment comment = new Comment
            {
                Text = "Text",
                User = user
            };
            matchLogic.AddCommentToMatch(teamMatch.Id, comment);
            Comment commentInMatchStored = matchLogic.GetAllComments(teamMatch.Id).FirstOrDefault();
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
            matchLogic.AddCommentToMatch(teamMatch.Id, comment);
        }
        

        [TestMethod]
        public void CheckSportIsNotDuplicated()
        {
            matchLogic.AddMatch(teamMatch);
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
            matchLogic.AddMatch(teamMatch);
            sportLogic.RemoveSport(teamSport.Id);
            Assert.AreEqual(matchLogic.GetAllMatches().Count,0);
        }

        [TestMethod]
        public void CascadeDeleteMatchFromCompetitor()
        {
            matchLogic.AddMatch(teamMatch);
            sportLogic.DeleteCompetitorFromSport(teamSport.Id,teamMatch.Competitors.First().Competitor.Id);
            Assert.AreEqual(matchLogic.GetAllMatches().Count, 0);
        }
        

        [TestMethod]
        public void CascadeDeleteCommentsFromMatch()
        {
            matchLogic.AddMatch(teamMatch);
            Comment comment = new Comment
            {
                Text = "Text",
                User = user
            };
            matchLogic.AddCommentToMatch(teamMatch.Id, comment);
            matchLogic.DeleteMatch(teamMatch.Id);
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
            matchLogic.AddMatch(teamMatch);
        }



        [TestMethod]
        public void GenerateTeamRanking()
        {
            matchLogic.AddMatch(teamMatch);
            Comment comment = new Comment
            {
                Text = "Text",
                User = user
            };
            matchLogic.AddCommentToMatch(teamMatch.Id, comment);
            localCompetitor.Score = 1;
            visitorCompetitor.Score = 0;
            ICollection<CompetitorScore> competitors = new List<CompetitorScore>() { localCompetitor, visitorCompetitor };
            teamMatch.Competitors = competitors;
            matchLogic.ModifyMatch(teamMatch.Id,teamMatch);
            ICollection<CompetitorScore> ranking = matchLogic.GenerateRanking(teamSport.Id);
            Assert.AreEqual(ranking.Count, 2);
        }


        [TestMethod]
        public void GenerateAthleteRankingOrder()
        {
            CreateAthleteMatch();
            matchLogic.AddMatch(athleteMatch);
            Comment comment = new Comment
            {
                Text = "Text",
                User = user
            };
            matchLogic.AddCommentToMatch(athleteMatch.Id, comment);
            localCompetitor.Score = 1;
            visitorCompetitor.Score = 2;
            thirdCompetitor.Score = 3;
            fourthCompetitor.Score = 4;
            ICollection<CompetitorScore> competitors = new List<CompetitorScore>() { localCompetitor, visitorCompetitor, thirdCompetitor, fourthCompetitor };
            athleteMatch.Competitors = competitors;
            matchLogic.ModifyMatch(athleteMatch.Id, athleteMatch);
            ICollection<CompetitorScore> ranking = matchLogic.GenerateRanking(athleteSport.Id).OrderByDescending(c=>c.Score).ToList();
            Assert.AreEqual(ranking.ElementAt(0).Score, 3);
        }

    }
}
