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

        [TestInitialize]
        public void SetUp()
        {
            match = new Match();
            localTeam = new Team()
            {
                Name = "Local team"
            };
            visitorTeam = new Team()
            {
                Name = "Visitor team"
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
            match.IsValidMatch(localTeam, team);

        }
    }
}
