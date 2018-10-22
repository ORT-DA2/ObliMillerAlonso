﻿using System;
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
        Competitor localCompetitor;
        Competitor visitorCompetitor;
        Sport sport;
        Comment comment;

        [TestInitialize]
        public void SetUp()
        {
            localCompetitor = new Competitor()
            {
                Name = "Local competitor"
            };
            visitorCompetitor = new Competitor()
            {
                Name = "Visitor competitor"
            };
            sport = new Sport()
            {
                Name = "Tennis"
            };
            sport.AddCompetitor(localCompetitor);
            sport.AddCompetitor(visitorCompetitor);
            match = new Match()
            {
                Sport = sport,
                Local = localCompetitor,
                Visitor = visitorCompetitor,
                Date = DateTime.Now.AddDays(1)
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
        [ExpectedException(typeof(InvalidCompetitorVersusException))]
        public void InvalidMatchCompetitors()
        {
            Competitor competitor = new Competitor()
            {
                Name = "Local competitor"
            };
            match.Visitor = competitor;
            match.IsValid();
            match.IsValidMatch();
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
            String expectedToString = string.Format("Sport: {0} Local Competitor: {1} Visitor Competitor: {2} Date: {3}", match.Sport, match.Local, match.Visitor, match.Date);
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

        [TestMethod]
        [ExpectedException(typeof(InvalidCommentIsEmptyException))]
        public void AddNullCommentTest()
        {
            match.AddComment(null);
        }
    }
}
