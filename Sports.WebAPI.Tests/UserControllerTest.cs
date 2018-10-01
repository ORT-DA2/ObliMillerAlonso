using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Sports.Domain;
using Sports.Logic.Interface;
using System.Net.Http;
using Sports.WebAPI.Models;
using Sports.WebAPI.Controllers;
using Microsoft.AspNetCore.Mvc;

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
            userLogicMock.Setup(userLogic => userLogic.AddUser(fakeUser));
            var controller = new UsersController(userLogicMock.Object);
            
            IActionResult result = controller.Post(fakeUser);
            var createdResult = result as CreatedAtRouteResult;
            var modelOut = createdResult.Value as UserModel;
            
            userLogicMock.VerifyAll();

            Assert.AreEqual(201, createdResult.StatusCode);
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
            userLogicMock.Setup(userLogic => userLogic.GetUserById(It.IsAny<int>())).Returns(oldUser);
            userLogicMock.Setup(userLogic => userLogic.UpdateUser(It.IsAny<int>(),newUser);
            var controller = new UsersController(userLogicMock.Object);

            IActionResult result = controller.PutUser(newUser);
            var createdResult = result as CreatedAtRouteResult;
            var modelOut = createdResult.Value as UserModel;

            userLogicMock.VerifyAll();

            Assert.AreEqual(201, createdResult.StatusCode);
        }

    }
}