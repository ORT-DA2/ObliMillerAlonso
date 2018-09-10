using System;
using System.Collections.Generic;
using System.Text;
using Sports.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sports.Exceptions;

namespace Sports.Domain.Test
{
    [TestClass]
    public class FavoriteTest
    {
        User user;
        Team team;
        Favorite favorite;
        [TestInitialize]
        public void SetUp()
        {
            user = new User()
            {
                UserName = "Pepe"
            };
            team = new Team()
            {
                Name = "Manya"
            };
            favorite = new Favorite()
            {
               User = user,
               Team = team

            };
        }

        [TestMethod]
        public void NewFavorite()
        {
            Assert.IsNotNull(favorite);
        }


        [TestMethod]
        public void EqualsIsTrue()
        {
            Favorite secondFavorite = new Favorite()
            {
                User = user,
                Team = team
            };
            Assert.IsTrue(favorite.Equals(secondFavorite));
        }

        /*
        [TestMethod]
        public void EqualsIsFalse()
        {
            Team secondTeam = new Team()
            {
                Name = "Different Team",

            };
            Assert.IsFalse(team.Equals(secondTeam));
        }*/
    }
}
