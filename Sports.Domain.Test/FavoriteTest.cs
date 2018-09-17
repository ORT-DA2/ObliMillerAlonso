using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics.CodeAnalysis;
using Sports.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sports.Exceptions;

namespace Sports.Domain.Test
{
    [ExcludeFromCodeCoverage]
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
        
        [TestMethod]
        public void EqualsIsFalseUser()
        {
            User differentUser = new User()
            {
                UserName = "Different",
            };
            Favorite secondFavorite = new Favorite()
            {
                User = differentUser,
                Team = team
            };
            Assert.IsFalse(favorite.Equals(secondFavorite));
        }

        [TestMethod]
        public void EqualsIsFalseTeam()
        {
            Team differentTeam = new Team()
            {
                Name = "Different",
            };
            Favorite secondFavorite = new Favorite()
            {
                Team = differentTeam,
                User = user
            };
            Assert.IsFalse(favorite.Equals(secondFavorite));
        }



        [TestMethod]
        public void EqualsNull()
        {
            Assert.IsFalse(favorite.Equals(null));
        }
    }
}
