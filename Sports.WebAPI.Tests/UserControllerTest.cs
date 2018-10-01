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

namespace Sports.WebAPI.Tests
{
    [TestClass]
    public class UserControllerTest
    {
        Mock<IUserLogic> userLogicMock;
        UsersController controller;
        IMapper mapper;

       [TestInitialize]
        public void SetUp()
        {

            userLogicMock = new Mock<IUserLogic>();
            var config = new MapperConfiguration(cfg => cfg.AddProfile(new MapperProfile()));
            IMapper mapper = new Mapper(config);
            controller = new UsersController(userLogicMock.Object,mapper);
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
                FirstName = "Itai",
                LastName = "Miller",
                Email = "itaimiller@gmail.com",
                UserName = "iMiller",
                Password = "root",
                IsAdmin = true
            };

            userLogicMock.Setup(userLogic => userLogic.AddUser(It.IsAny<User>()));
            
            
            IActionResult result = controller.PostUser(modelIn);
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
            
            userLogicMock.Setup(userLogic => userLogic.UpdateUser(It.IsAny<int>(),It.IsAny<User>()));

            IActionResult result = controller.PutUser(1, newUser);
            var createdResult = result as OkObjectResult;

            userLogicMock.VerifyAll();

            Assert.AreEqual(200, createdResult.StatusCode);
        }


        [TestMethod]
        public void ValidDeleteUser()
        {
            int userId = 1;

            userLogicMock.Setup(userLogic => userLogic.RemoveUser(It.IsAny<int>()));

            IActionResult result = controller.DeleteUser(userId);
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
            ICollection<User> users = new List<User>();
            users.Add(fakeUser);
            
            userLogicMock.Setup(userLogic => userLogic.GetAll()).Returns(users);

            var result = controller.GetAll();
            var okResult = result as OkObjectResult;
            var modelOut = okResult.Value as ICollection<UserModelOut>;

            userLogicMock.VerifyAll();

            Assert.AreEqual(200, okResult.StatusCode);
            Assert.IsNotNull(modelOut);
        }

    }
}
