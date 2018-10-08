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
using System.Diagnostics.CodeAnalysis;

namespace Sports.WebAPI.Tests
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class UsersControllerTest
    {
        Mock<IUserLogic> userLogicMock;
        Mock<ISessionLogic> sessionLogicMock;
        Mock<IFavoriteLogic> favoriteLogicMock;
        UsersController controller;
        IMapper mapper;
        string token;

       [TestInitialize]
        public void SetUp()
        {

            userLogicMock = new Mock<IUserLogic>();
            sessionLogicMock = new Mock<ISessionLogic>();
            favoriteLogicMock = new Mock<IFavoriteLogic>();
            var config = new MapperConfiguration(cfg => cfg.AddProfile(new MapperProfile()));
            mapper = new Mapper(config);
            controller = new UsersController(userLogicMock.Object, sessionLogicMock.Object, favoriteLogicMock.Object);
            token = new Guid().ToString();
        }

        [TestMethod]
        public void ValidPostUser()
        {
            User fakeUser = new User(true)
            {
                FirstName = "Itai",
                LastName = "Miller",
                Email = "itaimiller@gmail.com",
                UserName = "iMiller",
                Password = "root"
            };

            UserModelIn modelIn = new UserModelIn()
            {
                Id = 1,
                FirstName = "Itai",
                LastName = "Miller",
                Email = "itaimiller@gmail.com",
                UserName = "iMiller",
                Password = "root",
                IsAdmin = true
            };

            userLogicMock.Setup(userLogic => userLogic.SetSession(It.IsAny<Guid>()));
            userLogicMock.Setup(userLogic => userLogic.AddUser(fakeUser));
            
            IActionResult result = controller.PostUser(modelIn, token);
            var okResult = result as OkObjectResult;
            var modelOut = okResult.Value as UserModelOut;
            
            userLogicMock.VerifyAll();

            Assert.AreEqual(200, okResult.StatusCode);
            Assert.IsNotNull(modelOut);
        }

        [TestMethod]
        public void ValidPutUser()
        {
            User oldUser = new User(true)
            {
                FirstName = "Itai",
                LastName = "Miller",
                Email = "itaimiller@gmail.com",
                UserName = "iMiller",
                Password = "root"
            };
            UserModelIn newUser = new UserModelIn()
            {
                FirstName = "Pepe",
                LastName = "Alonso"
            };

            userLogicMock.Setup(userLogic => userLogic.SetSession(It.IsAny<Guid>()));
            userLogicMock.Setup(userLogic => userLogic.UpdateUser(It.IsAny<int>(),It.IsAny<User>()));

            IActionResult result = controller.PutUser(1, newUser, token);
            var createdResult = result as OkObjectResult;

            userLogicMock.VerifyAll();

            Assert.AreEqual(200, createdResult.StatusCode);
        }


        [TestMethod]
        public void ValidDeleteUser()
        {
            int userId = 1;

            userLogicMock.Setup(userLogic => userLogic.SetSession(It.IsAny<Guid>()));
            userLogicMock.Setup(userLogic => userLogic.RemoveUser(It.IsAny<int>()));

            IActionResult result = controller.DeleteUser(userId,token);
            var createdResult = result as OkObjectResult;

            userLogicMock.VerifyAll();

            Assert.AreEqual(200, createdResult.StatusCode);
        }


        [TestMethod]
        public void ValidGetAllUsers()
        {

            User fakeUser = new User(true)
            {
                FirstName = "Itai",
                LastName = "Miller",
                Email = "itaimiller@gmail.com",
                UserName = "iMiller",
                Password = "root"
            };
            ICollection<User> users = new List<User>
            {
                fakeUser
            };

            userLogicMock.Setup(userLogic => userLogic.SetSession(It.IsAny<Guid>()));
            userLogicMock.Setup(userLogic => userLogic.GetAll()).Returns(users);

            var result = controller.GetAll(token);
            var okResult = result as OkObjectResult;
            var modelOut = okResult.Value as ICollection<UserModelOut>;

            userLogicMock.VerifyAll();

            Assert.AreEqual(200, okResult.StatusCode);
            Assert.IsNotNull(modelOut);
        }


        [TestMethod]
        public void ValidLogin()
        {

            LoginDTO modelIn = new LoginDTO()
            {
                Username = "iMiller",
                Password = "root"
            };

            sessionLogicMock.Setup(sessionLogic => sessionLogic.LogInUser(It.IsAny<string>(), It.IsAny<string>())).Returns(new Guid());

            IActionResult result = controller.Login(modelIn);
            var okResult = result as OkObjectResult;
            var modelOut = okResult.Value as string;

            userLogicMock.VerifyAll();

            Assert.AreEqual(200, okResult.StatusCode);
            Assert.IsNotNull(modelOut);
        }

        [TestMethod]
        public void ValidLogout()
        {
            string token = new Guid().ToString();

            sessionLogicMock.Setup(sessionLogic => sessionLogic.LogoutByToken(It.IsAny<Guid>()));

            IActionResult result = controller.Logout(token);
            var okResult = result as OkObjectResult;

            userLogicMock.VerifyAll();

            Assert.AreEqual(200, okResult.StatusCode);
        }

    }
}
