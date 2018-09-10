using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace Sports.Domain.Test
{
    [TestClass]
    public class SportTest
    {
        Sport sport;

        [TestMethod]
        public void SetUp()
        {
            sport = new Sport()
            {
                Name = "Test sport"
            };
        }
    }
}
