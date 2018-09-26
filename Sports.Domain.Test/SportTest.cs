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
        [ExpectedException(typeof(InvalidEmptyTextFieldException))]
        public void InvalidSportName()
        {
            sport.Name = "";
            sport.IsValid();
        }

        [TestMethod]
        public void ValidateAddTeam()
        {
            sport.AddTeam(team);
            Assert.AreEqual(1, sport.Teams.Count);
        }

        [TestMethod]
        public void ValidateRemoveTeam()
        {
            sport.AddTeam(team);
            sport.RemoveTeam(team);
            Assert.AreEqual(0, sport.Teams.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(TeamDoesNotExistInSportException))]
        public void InvalidRemoveTeam()
        {
            sport.AddTeam(team);
            Team secondTeam = new Team()
            {
                Name = "Real"
            };
            sport.RemoveTeam(secondTeam);
        }

        [TestMethod]
        [ExpectedException(typeof(TeamAlreadyExistException))]
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


        [TestMethod]
        public void EqualsNull()
        {
            Assert.IsFalse(sport.Equals(null));
        }
    }
}
