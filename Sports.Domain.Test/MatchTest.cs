using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sports.Exceptions;


namespace Sports.Domain.Test
{
    [TestClass]
    public class MatchTest
    {
        Match match;
        Team localTeam;
        Team visitorTeam;
        Sport sport;

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
        }

        [TestMethod]
        public void NewMatch()
        {
            Assert.IsNotNull(match);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidMatchDataException))]
        public void InvalidMatch()
        {
            Team team = new Team()
            {
                Name = "Local team"
            };
            match.IsValid(localTeam, team);
        }

        [TestMethod]
        public void ToStringIsOk()
        {
            String expectedToString = string.Format("Sport: {0} Local Team: {1} Visitor Team: {2} Date: {3}", match.Sport, match.Local, match.Visitor, match.Date);
            Assert.AreEqual(expectedToString, match.ToString());
        }
    }
}
