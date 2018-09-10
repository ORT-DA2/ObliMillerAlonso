using System;
using System.Collections.Generic;
using System.Text;
using Sports.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sports.Exceptions;

namespace Sports.Domain.Test
{
    public class FavoriteTest
    {
        Favorite favorite;
        [TestInitialize]
        public void SetUp()
        {
            User user = new User();
            Team team = new Team();
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
    }
}
