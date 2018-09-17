using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sports.Exceptions;


namespace Sports.Domain.Test
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class SportTest
    {
        Team team;
        Sport sport;

        [TestInitialize]
        public void SetUp()
        {
            sport = new Sport()
            {
                Name = "Test sport"
            };
            team = new Team()
            {
                Name = "Barcelona"
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
            sport.Teams.Add(team);
            Assert.AreEqual(1, sport.Teams.Count);
        }

        [TestMethod]
        public void ValidateRemoveTeam()
        {
            sport.Teams.Add(team);
            sport.Teams.Remove(team);
            Assert.AreEqual(0, sport.Teams.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidSportDataException))]
        public void InvalidRemoveTeam()
        {
            sport.Teams.Add(team);
            Team secondTeam = new Team()
            {
                Name = "Real"
            };
            sport.RemoveTeam(secondTeam);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidSportDataException))]
        public void InvalidAddTeam()
        {
            sport.AddTeam(team);
            sport.AddTeam(team);
        }

        [TestMethod]
        public void EqualsIsTrue()
        {
            Sport secondSport = new Sport()
            {
                Name = "Test sport",
            };
            Assert.IsTrue(sport.Equals(secondSport));
        }

        [TestMethod]
        public void EqualsIsFalse()
        {
            Sport secondSport = new Sport()
            {
                Name = "Different Team",
            };
            Assert.IsFalse(sport.Equals(secondSport));
        }

        [TestMethod]
        public void ToStringRedefined()
        {
            Assert.AreEqual<string>(sport.ToString(), sport.Name);
        }
    }
}
