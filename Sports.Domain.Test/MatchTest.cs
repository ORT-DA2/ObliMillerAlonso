using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sports.Domain.Exceptions;

namespace Sports.Domain.Test
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class MatchTest
    {
        Match match;
        Team localTeam;
        Team visitorTeam;
        Sport sport;
        Comment comment;

        [TestInitialize]
        public void SetUp()
        {
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
                Name = "Tennis"
            };
            sport.AddTeam(localTeam);
            sport.AddTeam(visitorTeam);
            match = new Match()
            {
                Sport = sport,
                Local = localTeam,
                Visitor = visitorTeam,
                Date = DateTime.Now
            };
            comment = new Comment()
            {
                Text = "comment",
            };
        }

        [TestMethod]
        public void NewMatch()
        {
            Assert.IsNotNull(match);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidTeamVersusException))]
        public void InvalidMatchTeams()
        {
            Team team = new Team()
            {
                Name = "Local team"
            };
            match.Visitor = team;
            match.IsValid();
        }


        [TestMethod]
        [ExpectedException(typeof(InvalidMatchDateFormatException))]
        public void InvalidMatchDate()
        {
            DateTime yesterday = DateTime.Now.AddDays(-1);
            match.Date = yesterday;
            match.IsValid();
        }
        

        [TestMethod]
        public void ToStringIsOk()
        {
            String expectedToString = string.Format("Sport: {0} Local Team: {1} Visitor Team: {2} Date: {3}", match.Sport, match.Local, match.Visitor, match.Date);
            Assert.AreEqual(expectedToString, match.ToString());
        }



        [TestMethod]
        public void EqualsNull()
        {
            Assert.IsFalse(match.Equals(null));
        }

        [TestMethod]
        public void AddComment()
        {
            Comment testComment = new Comment
            {
                Text = "test comment",
                User = new User()
            };
           match.AddComment(testComment);
           Assert.IsNotNull(match.GetAllComments());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidEmptyTextFieldException))]
        public void AddCommentNoText()
        {
            Comment testComment = new Comment
            {
                Text = "",
                User = new User()
            };
            match.AddComment(testComment);
        }

    }
}
