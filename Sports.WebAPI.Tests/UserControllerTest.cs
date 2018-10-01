using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Sports.Domain;
using Sports.Logic.Interface;
using System.Net.Http;
using Sports.WebAPI.Models;
using Sports.WebAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Sports.WebAPI.Tests
{
    [TestClass]
    public class UserControllerTest
    {
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
            
            Mock<IUserLogic> userLogicMock = new Mock<IUserLogic>();
            userLogicMock.Setup(userLogic => userLogic.AddUser(It.IsAny<User>()));
            var controller = new UsersController(userLogicMock.Object);
            
            IActionResult result = controller.PostUser(fakeUser);
            var okResult = result as OkObjectResult;
            var modelOut = okResult.Value as UserModel;
            
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
            User newUser = new User(true)
            {
                FirstName = "Pepe",
                LastName = "Alonso"
            };

            Mock<IUserLogic> userLogicMock = new Mock<IUserLogic>();
            userLogicMock.Setup(userLogic => userLogic.UpdateUser(It.IsAny<int>(),It.IsAny<User>()));
            var controller = new UsersController(userLogicMock.Object);

            IActionResult result = controller.PutUser(1, newUser);
            var createdResult = result as OkObjectResult;

            userLogicMock.VerifyAll();

            Assert.AreEqual(200, createdResult.StatusCode);
        }


        [TestMethod]
        public void ValidDeleteUser()
        {
            int userId = 1;
            Mock<IUserLogic> userLogicMock = new Mock<IUserLogic>();
            userLogicMock.Setup(userLogic => userLogic.RemoveUser(It.IsAny<int>()));
            var controller = new UsersController(userLogicMock.Object);

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

            Mock<IUserLogic> userLogicMock = new Mock<IUserLogic>();
            userLogicMock.Setup(userLogic => userLogic.GetAll()).Returns(users);
            var controller = new UsersController(userLogicMock.Object);

            var result = controller.GetAll();
            var okResult = result as OkObjectResult;
            var modelOut = okResult.Value as ICollection<UserModel>;

            userLogicMock.VerifyAll();

            Assert.AreEqual(200, okResult.StatusCode);
            Assert.IsNotNull(modelOut);
        }

    }
}
