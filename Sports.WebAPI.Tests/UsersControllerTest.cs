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
using Sports.Domain.Exceptions;
using Sports.Logic.Interface.Exceptions;
using Sports.Repository.Interface.Exceptions;
using Sports.Logic.Exceptions;

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
            var createdResult = result as RedirectToRouteResult;

            userLogicMock.VerifyAll();

            Assert.IsNotNull(createdResult);
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

            var result = controller.GetAllUsers(token);
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

            userLogicMock.VerifyAll();

            Assert.AreEqual(200, okResult.StatusCode);
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


        [TestMethod]
        public void InvalidPostUser()
        {

            UserModelIn modelIn = new UserModelIn();

            userLogicMock.Setup(userLogic => userLogic.SetSession(It.IsAny<Guid>())).Throws(new Exception());

            IActionResult result = controller.PostUser(modelIn, token);
            var code = result as ObjectResult;

            userLogicMock.VerifyAll();

            Assert.AreEqual(500, code.StatusCode);
        }

        [TestMethod]
        public void InvalidPutUser()
        {

            UserModelIn modelIn = new UserModelIn();

            userLogicMock.Setup(userLogic => userLogic.SetSession(It.IsAny<Guid>())).Throws(new Exception());

            IActionResult result = controller.PutUser(1, modelIn, token);
            var code = result as ObjectResult;

            userLogicMock.VerifyAll();

            Assert.AreEqual(500, code.StatusCode);
        }

        [TestMethod]
        public void InvalidDeleteUser()
        {
            

            userLogicMock.Setup(userLogic => userLogic.SetSession(It.IsAny<Guid>())).Throws(new Exception());

            IActionResult result = controller.DeleteUser(1, token);
            var code = result as ObjectResult;

            userLogicMock.VerifyAll();

            Assert.AreEqual(500, code.StatusCode);
        }

        [TestMethod]
        public void InvalidGetAllUsers()
        {
            userLogicMock.Setup(userLogic => userLogic.SetSession(It.IsAny<Guid>())).Throws(new Exception());

            IActionResult result = controller.GetAllUsers(token);
            var code = result as ObjectResult;

            userLogicMock.VerifyAll();

            Assert.AreEqual(500, code.StatusCode);
        }

        [TestMethod]
        public void InvalidLoginUser()
        {

            LoginDTO modelIn = new LoginDTO()
            {
                Username = "",
                Password = ""
            };

            sessionLogicMock.Setup(sessionLogic => sessionLogic.LogInUser(It.IsAny<string>(), It.IsAny<string>())).Throws(new Exception());

            IActionResult result = controller.Login(modelIn);
            var code = result as ObjectResult;

            sessionLogicMock.VerifyAll();

            Assert.AreEqual(500, code.StatusCode);
        }

        [TestMethod]
        public void InvalidLogoutUser()
        {

            UserModelIn modelIn = new UserModelIn();

            sessionLogicMock.Setup(sessionLogic => sessionLogic.LogoutByToken(It.IsAny<Guid>())).Throws(new Exception());

            IActionResult result = controller.Logout(token);
            var code = result as ObjectResult;

            sessionLogicMock.VerifyAll();

            Assert.AreEqual(500, code.StatusCode);
        }


        [TestMethod]
        public void UnauthorizedPostUser()
        {

            UserModelIn modelIn = new UserModelIn();

            userLogicMock.Setup(userLogic => userLogic.SetSession(It.IsAny<Guid>())).Throws(new NonAdminException(""));

            IActionResult result = controller.PostUser(modelIn, token);
            var code = result as UnauthorizedResult;

            userLogicMock.VerifyAll();

            Assert.AreEqual(401, code.StatusCode);
        }

        [TestMethod]
        public void UnauthorizedPutUser()
        {

            UserModelIn modelIn = new UserModelIn();

            userLogicMock.Setup(userLogic => userLogic.SetSession(It.IsAny<Guid>())).Throws(new NonAdminException(""));

            IActionResult result = controller.PutUser(1, modelIn, token);
            var code = result as UnauthorizedResult;

            userLogicMock.VerifyAll();

            Assert.AreEqual(401, code.StatusCode);
        }

        [TestMethod]
        public void UnauthorizedDeleteUser()
        {


            userLogicMock.Setup(userLogic => userLogic.SetSession(It.IsAny<Guid>())).Throws(new NonAdminException(""));

            IActionResult result = controller.DeleteUser(1, token);
            var code = result as UnauthorizedResult;

            userLogicMock.VerifyAll();

            Assert.AreEqual(401, code.StatusCode);
        }

        [TestMethod]
        public void UnauthorizedGetAllUsers()
        {
            userLogicMock.Setup(userLogic => userLogic.SetSession(It.IsAny<Guid>())).Throws(new NonAdminException(""));

            IActionResult result = controller.GetAllUsers(token);
            var code = result as UnauthorizedResult;

            userLogicMock.VerifyAll();

            Assert.AreEqual(401, code.StatusCode);
        }

        [TestMethod]
        public void UnauthorizedLoginUser()
        {

            LoginDTO modelIn = new LoginDTO()
            {
                Username = "",
                Password = ""
            };

            sessionLogicMock.Setup(sessionLogic => sessionLogic.LogInUser(It.IsAny<string>(), It.IsAny<string>())).Throws(new NonAdminException(""));

            IActionResult result = controller.Login(modelIn);
            var code = result as UnauthorizedResult;

            sessionLogicMock.VerifyAll();

            Assert.AreEqual(401, code.StatusCode);
        }

        [TestMethod]
        public void UnauthorizedLogoutUser()
        {

            UserModelIn modelIn = new UserModelIn();

            sessionLogicMock.Setup(sessionLogic => sessionLogic.LogoutByToken(It.IsAny<Guid>())).Throws(new NonAdminException(""));

            IActionResult result = controller.Logout(token);
            var code = result as UnauthorizedResult;

            sessionLogicMock.VerifyAll();

            Assert.AreEqual(401, code.StatusCode);
        }



        [TestMethod]
        public void DomainErrorPostUser()
        {

            UserModelIn modelIn = new UserModelIn();

            userLogicMock.Setup(userLogic => userLogic.SetSession(It.IsAny<Guid>())).Throws(new DomainException(""));

            IActionResult result = controller.PostUser(modelIn, token);
            var code = result as ObjectResult;

            userLogicMock.VerifyAll();

            Assert.AreEqual(422, code.StatusCode);
        }

        [TestMethod]
        public void DomainErrorPutUser()
        {

            UserModelIn modelIn = new UserModelIn();

            userLogicMock.Setup(userLogic => userLogic.SetSession(It.IsAny<Guid>())).Throws(new DomainException(""));

            IActionResult result = controller.PutUser(1, modelIn, token);
            var code = result as ObjectResult;

            userLogicMock.VerifyAll();

            Assert.AreEqual(422, code.StatusCode);
        }

        [TestMethod]
        public void DomainErrorDeleteUser()
        {


            userLogicMock.Setup(userLogic => userLogic.SetSession(It.IsAny<Guid>())).Throws(new DomainException(""));

            IActionResult result = controller.DeleteUser(1, token);
            var code = result as ObjectResult;

            userLogicMock.VerifyAll();

            Assert.AreEqual(422, code.StatusCode);
        }

        [TestMethod]
        public void DomainErrorGetAllUsers()
        {
            userLogicMock.Setup(userLogic => userLogic.SetSession(It.IsAny<Guid>())).Throws(new DomainException(""));

            IActionResult result = controller.GetAllUsers(token);
            var code = result as ObjectResult;

            userLogicMock.VerifyAll();

            Assert.AreEqual(422, code.StatusCode);
        }

        [TestMethod]
        public void DomainErrorLoginUser()
        {

            LoginDTO modelIn = new LoginDTO()
            {
                Username = "",
                Password = ""
            };

            sessionLogicMock.Setup(sessionLogic => sessionLogic.LogInUser(It.IsAny<string>(), It.IsAny<string>())).Throws(new DomainException(""));

            IActionResult result = controller.Login(modelIn);
            var code = result as ObjectResult;

            sessionLogicMock.VerifyAll();

            Assert.AreEqual(422, code.StatusCode);
        }

        [TestMethod]
        public void DomainErrorLogoutUser()
        {

            UserModelIn modelIn = new UserModelIn();

            sessionLogicMock.Setup(sessionLogic => sessionLogic.LogoutByToken(It.IsAny<Guid>())).Throws(new DomainException(""));

            IActionResult result = controller.Logout(token);
            var code = result as ObjectResult;

            sessionLogicMock.VerifyAll();

            Assert.AreEqual(422, code.StatusCode);
        }

        [TestMethod]
        public void LogicErrorPostUser()
        {

            UserModelIn modelIn = new UserModelIn();

            userLogicMock.Setup(userLogic => userLogic.SetSession(It.IsAny<Guid>())).Throws(new LogicException(""));

            IActionResult result = controller.PostUser(modelIn, token);
            var code = result as ObjectResult;

            userLogicMock.VerifyAll();

            Assert.AreEqual(400, code.StatusCode);
        }

        [TestMethod]
        public void LogicErrorPutUser()
        {

            UserModelIn modelIn = new UserModelIn();

            userLogicMock.Setup(userLogic => userLogic.SetSession(It.IsAny<Guid>())).Throws(new LogicException(""));

            IActionResult result = controller.PutUser(1, modelIn, token);
            var code = result as ObjectResult;

            userLogicMock.VerifyAll();

            Assert.AreEqual(400, code.StatusCode);
        }

        [TestMethod]
        public void LogicErrorDeleteUser()
        {


            userLogicMock.Setup(userLogic => userLogic.SetSession(It.IsAny<Guid>())).Throws(new LogicException(""));

            IActionResult result = controller.DeleteUser(1, token);
            var code = result as ObjectResult;

            userLogicMock.VerifyAll();

            Assert.AreEqual(400, code.StatusCode);
        }

        [TestMethod]
        public void LogicErrorGetAllUsers()
        {
            userLogicMock.Setup(userLogic => userLogic.SetSession(It.IsAny<Guid>())).Throws(new LogicException(""));

            IActionResult result = controller.GetAllUsers(token);
            var code = result as ObjectResult;

            userLogicMock.VerifyAll();

            Assert.AreEqual(400, code.StatusCode);
        }

        [TestMethod]
        public void LogicErrorLoginUser()
        {

            LoginDTO modelIn = new LoginDTO()
            {
                Username = "",
                Password = ""
            };

            sessionLogicMock.Setup(sessionLogic => sessionLogic.LogInUser(It.IsAny<string>(), It.IsAny<string>())).Throws(new LogicException(""));

            IActionResult result = controller.Login(modelIn);
            var code = result as ObjectResult;

            sessionLogicMock.VerifyAll();

            Assert.AreEqual(400, code.StatusCode);
        }

        [TestMethod]
        public void LogicErrorLogoutUser()
        {

            UserModelIn modelIn = new UserModelIn();

            sessionLogicMock.Setup(sessionLogic => sessionLogic.LogoutByToken(It.IsAny<Guid>())).Throws(new LogicException(""));

            IActionResult result = controller.Logout(token);
            var code = result as ObjectResult;

            sessionLogicMock.VerifyAll();

            Assert.AreEqual(400, code.StatusCode);
        }

        [TestMethod]
        public void DBErrorPostUser()
        {

            UserModelIn modelIn = new UserModelIn();

            userLogicMock.Setup(userLogic => userLogic.SetSession(It.IsAny<Guid>())).Throws(new UnknownDataAccessException(""));

            IActionResult result = controller.PostUser(modelIn, token);
            var code = result as ObjectResult;

            userLogicMock.VerifyAll();

            Assert.AreEqual(503, code.StatusCode);
        }

        [TestMethod]
        public void DBErrorPutUser()
        {

            UserModelIn modelIn = new UserModelIn();

            userLogicMock.Setup(userLogic => userLogic.SetSession(It.IsAny<Guid>())).Throws(new UnknownDataAccessException(""));

            IActionResult result = controller.PutUser(1, modelIn, token);
            var code = result as ObjectResult;

            userLogicMock.VerifyAll();

            Assert.AreEqual(503, code.StatusCode);
        }

        [TestMethod]
        public void DBErrorDeleteUser()
        {


            userLogicMock.Setup(userLogic => userLogic.SetSession(It.IsAny<Guid>())).Throws(new UnknownDataAccessException(""));

            IActionResult result = controller.DeleteUser(1, token);
            var code = result as ObjectResult;

            userLogicMock.VerifyAll();

            Assert.AreEqual(503, code.StatusCode);
        }

        [TestMethod]
        public void DBErrorGetAllUsers()
        {
            userLogicMock.Setup(userLogic => userLogic.SetSession(It.IsAny<Guid>())).Throws(new UnknownDataAccessException(""));

            IActionResult result = controller.GetAllUsers(token);
            var code = result as ObjectResult;

            userLogicMock.VerifyAll();

            Assert.AreEqual(503, code.StatusCode);
        }

        [TestMethod]
        public void DBErrorLoginUser()
        {

            LoginDTO modelIn = new LoginDTO()
            {
                Username = "",
                Password = ""
            };

            sessionLogicMock.Setup(sessionLogic => sessionLogic.LogInUser(It.IsAny<string>(), It.IsAny<string>())).Throws(new UnknownDataAccessException(""));

            IActionResult result = controller.Login(modelIn);
            var code = result as ObjectResult;

            sessionLogicMock.VerifyAll();

            Assert.AreEqual(503, code.StatusCode);
        }

        [TestMethod]
        public void DBErrorLogoutUser()
        {

            UserModelIn modelIn = new UserModelIn();

            sessionLogicMock.Setup(sessionLogic => sessionLogic.LogoutByToken(It.IsAny<Guid>())).Throws(new UnknownDataAccessException(""));

            IActionResult result = controller.Logout(token);
            var code = result as ObjectResult;

            sessionLogicMock.VerifyAll();

            Assert.AreEqual(503, code.StatusCode);
        }
    }
}
