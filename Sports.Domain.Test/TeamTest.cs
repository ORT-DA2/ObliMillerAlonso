﻿using System;
using System.Collections.Generic;
using System.Text;
using Sports.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sports.Domain.Test
{
    [TestClass]
    public class TeamTest
    {
        [TestMethod]
        public void NewTeam()
        {
            Team team = new Team();
            Assert.IsNotNull(team);
        }
    }
}
