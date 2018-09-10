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
            sport.IsValidSportName();
        }
    }
}
