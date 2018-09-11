using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace Sports.Domain.Test
{
    [TestClass]
    public class MatchTest
    {
        Match match;

        [TestInitialize]
        public void SetUp()
        {
            match = new Match();
        }

        [TestMethod]
        public void NewMatch()
        {
            Assert.IsNotNull(match);
        }
    }
}
