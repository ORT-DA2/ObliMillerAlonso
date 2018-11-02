using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics.CodeAnalysis;
using Sports.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sports.Domain.Exceptions;

namespace Sports.Domain.Test
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class FavoriteTest
    {
        User user;
        Competitor competitor;
        Favorite favorite;
        [TestInitialize]
        public void SetUp()
        {
            user = new User()
            {
                UserName = "Pepe"
            };
            competitor = new Competitor()
            {
                Name = "Manya"
            };
            favorite = new Favorite()
            {
               User = user,
               Competitor = competitor

            };
        }

        [TestMethod]
        public void NewFavorite()
        {
            Assert.IsNotNull(favorite);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidEmptyUserException))]
        public void NullUserTest()
        {
            favorite.User = null;
            favorite.Validate();
        }

    }
}
