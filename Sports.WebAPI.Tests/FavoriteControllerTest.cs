using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Sports.Domain;
using Sports.Logic.Interface;
using System.Net.Http;
using Sports.WebAPI.Models;
using Sports.WebAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using AutoMapper;
using System;

namespace Sports.WebAPI.Tests
{
    [TestClass]
    public class FavoriteControllerTest
    {
        Mock<IFavoriteLogic> favoriteLogicMock;
        FavoritesController controller;
        IMapper mapper;
        Guid token;

        [TestInitialize]
        public void SetUp()
        {

            favoriteLogicMock = new Mock<IFavoriteLogic>();
            var config = new MapperConfiguration(cfg => cfg.AddProfile(new MapperProfile()));
            IMapper mapper = new Mapper(config);
            controller = new FavoritesController(favoriteLogicMock.Object, mapper);
            favoriteLogicMock.Setup(favoriteLogic => favoriteLogic.SetSession(It.IsAny<Guid>()));
            token = new Guid();
        }
    }
}
