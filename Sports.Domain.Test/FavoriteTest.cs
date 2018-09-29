﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics.CodeAnalysis;
using Sports.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
        
    }
}
