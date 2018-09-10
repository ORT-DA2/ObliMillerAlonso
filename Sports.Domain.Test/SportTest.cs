using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sports.Exceptions;


namespace Sports.Domain.Test
{
    [TestClass]
    public class SportTest
    {
        Sport sport;

        [TestInitialize]
        public void SetUp()
        {
            sport = new Sport()
            {
                Name = "Test sport"
            };
        }

        [TestMethod]
        public void NewSport()
        {
            Assert.IsNotNull(sport);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidSportDataException))]
        public void InvalidSportName()
        {
            sport.Name = "";
            sport.IsValid();
        }

        [TestMethod]
        public void ValidateAddTeam()
        {
            Team team = new Team()
            {
                Name = "Barcelona"
            };
            sport.Teams.Add(team);
            Assert.AreEqual(1, sport.Teams.Count);
        }

        [TestMethod]
        public void ValidateRemoveTeam()
        {
            Team team = new Team()
            {
                Name = "Barcelona"
            };
            sport.Teams.Add(team);
            sport.Teams.Remove(team);
            Assert.AreEqual(0, sport.Teams.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidSportDataException))]
        public void InvalidRemoveTeam()
        {
            Team team = new Team()
            {
                Name = "Barcelona"
            };
            sport.Teams.Add(team);

            Team secondTeam = new Team()
            {
                Name = "Real"
            };
            sport.Teams.Remove(secondTeam);
        }
    }
}
