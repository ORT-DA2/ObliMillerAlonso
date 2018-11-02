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
        Competitor competitor;
        Sport sport;

        [TestInitialize]
        public void SetUp()
        {
            sport = new Sport()
            {
                Name = "Test sport"
            };
            competitor = new Competitor()
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
        public void ValidateAddCompetitor()
        {
            sport.AddCompetitor(competitor);
            Assert.AreEqual(1, sport.Competitors.Count);
        }

        [TestMethod]
        public void ValidateRemoveCompetitor()
        {
            sport.AddCompetitor(competitor);
            sport.RemoveCompetitor(competitor);
            Assert.AreEqual(0, sport.Competitors.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(CompetitorDoesNotExistInSportException))]
        public void InvalidRemoveCompetitor()
        {
            sport.AddCompetitor(competitor);
            Competitor secondCompetitor = new Competitor()
            {
                Name = "Real"
            };
            sport.RemoveCompetitor(secondCompetitor);
        }

        [TestMethod]
        [ExpectedException(typeof(CompetitorAlreadyExistException))]
        public void InvalidAddCompetitor()
        {
            sport.AddCompetitor(competitor);
            sport.AddCompetitor(competitor);
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
                Name = "Different Competitor",
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
