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
        public void CreateUser()
        {
            //Arrange
            User fakeUser = new User(true)
            {
                FirstName = "Itai",
                LastName = "Miller",
                Email = "itaimiller@gmail.com",
                UserName = "iMiller",
                Password = "root"
            };

            // Inicializar el mock a partir de IUserService
            Mock<IUserLogic> userLogicMock = new Mock<IUserLogic>();
            // Esperamos que se llame el motodo SignUp del servicio
            userLogicMock.Setup(userLogic => userLogic.AddUser(fakeUser));
            var controller = new UsersController(userLogicMock.Object);

            //Act
            IActionResult result = controller.Post(fakeUser);
            var createdResult = result as CreatedAtRouteResult;
            var modelOut = createdResult.Value as UserModel;

            //Assert
            //Verificamos los metodos del mock
            userLogicMock.VerifyAll();

            Assert.AreEqual("GetById", createdResult.RouteName);
        }
        
    }
}
