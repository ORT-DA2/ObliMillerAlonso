using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Sports.Domain;
using Sports.Logic;
using Sports.Repository;
using Sports.Repository.Interface;
using Sports.Repository.Context;
using Sports.Exceptions;
using System.Diagnostics.CodeAnalysis;

namespace Sports.Logic.Test
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class TeamLogicTest
    {
        [TestMethod]
        public void AddTeam()
        {
            Team team = new Team()
            {
                Name = "name"
            };
            _teamLogic.AddUser(team);
            Assert.IsNotNull(_teamLogic.GetTeamById(team.Id));
        }
    }
}
