using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Sports.Domain;
using Sports.Logic.Interface;
using Sports.Repository;
using Sports.Repository.Interface;
using Sports.Repository.Context;
using System.Diagnostics.CodeAnalysis;
using Sports.Domain.Exceptions;
using Sports.Logic.Exceptions;
using Sports.Repository.UnitOfWork;

namespace Sports.Logic.Test
{
    [TestClass]
    public class LogTest
    {
        User Admin { get; set; }
        User Guest { get; set; }
        ILogLogic LogLogic { get; set; }

        [TestInitialize]
        public void TestSetUp()
        {
            LogLogic = new TextLog();
            LogLogic.CleanLog();
            Admin = new User(true)
            {
                UserName = "Admin",
                Password = "",
            };

            Guest = new User()
            {
                UserName = "Guest",
                Password = ""
            };
        }

        [TestMethod]
        public void CreateLog()
        {
            LogLogic.AddEntry("Login", Guest.UserName, DateTime.Now);
            ICollection<string> logs = LogLogic.GetBetweenDates(DateTime.Now, DateTime.Now);
            Assert.AreEqual(1, logs.Count);
        }

        [TestMethod]
        public void AddEntries()
        {
            LogLogic.AddEntry("Login", Guest.UserName, DateTime.Now.AddDays(1));
            LogLogic.AddEntry("Login", Admin.UserName, DateTime.Now);
            ICollection<string> logs = LogLogic.GetBetweenDates(DateTime.Now, DateTime.Now.AddDays(3));
            Assert.AreEqual(2, logs.Count);
        }


        [TestMethod]
        public void RegistrosFuturosIgnorados()
        {
            LogLogic.AddEntry("Login", Guest.UserName, DateTime.Now.AddDays(3));
            LogLogic.AddEntry("Login", Admin.UserName, DateTime.Now);
            ICollection<string> logs = LogLogic.GetBetweenDates(DateTime.Now, DateTime.Now.AddDays(1));
            Assert.AreEqual(1, logs.Count);
        }


        [TestMethod]
        public void LogVacio()
        {
            ICollection<string> logs = LogLogic.GetBetweenDates(DateTime.Now, DateTime.Now.AddDays(1));
            Assert.AreEqual(0, logs.Count);
        }
    }
}
