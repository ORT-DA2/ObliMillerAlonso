using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sports.Domain;


namespace Sports.Logic.Test
{
    [TestClass]
    public class UserLogicTest
    {
        [TestMethod]
        public void AddUser()
        {
            User user = new User()
            {
                FirstName = "Itai",
                LastName = "Miller",
                Email = "itaimiller@gmail.com",
                UserName = "iMiller",
                Password = "root"
            };
            user.Id = UserLogic.AddUser(user);
            Assert.IsNotNull(UserLogic.GetUserById(user.Id));
        }
    }
}
