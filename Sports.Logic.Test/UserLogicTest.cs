using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sports.Domain;
using Sports.Logic;
using Sports.Persistence.Factory;


namespace Sports.Logic.Test
{
    [TestClass]
    public class UserLogicTest
    {
        [TestMethod]
        public void AddUser()
        {
            User user = new User(true)
            {
                FirstName = "Itai",
                LastName = "Miller",
                Email = "itaimiller@gmail.com",
                UserName = "iMiller",
                Password = "root"
            };
            PersistenceFactory factory = new PersistenceFactory();
            UserLogic userLogic = new UserLogic(factory);
            userLogic.AddUser(user);
            Assert.IsNotNull(userLogic.GetUserById(user.Id));
        }
    }
}
