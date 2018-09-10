using System;
using System.Collections.Generic;
using System.Text;
using Sports.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Linq;

namespace Sports.Domain.Test
{
    [TestClass]
    public class TeamTest
    {
        Team team;
        [TestInitialize]
        public void SetUp()
        {
            team = new Team()
            {
                Name = "Test Team",

            };
        }

        [TestMethod]
        public void NewTeam()
        {
            Assert.IsNotNull(team);
        }

        [ExpectedException(typeof(Exception))]
        [TestMethod]
        public void InvalidName()
        {
            Team invalidNameTeam = new Team()
            {
                Name = "",

            };
            invalidNameTeam.IsValid();
        }
        
        [TestMethod]
        public void ValidName()
        {
            team.IsValid();
            Assert.AreEqual<string>(team.Name, "Test Team");
        }

        [TestMethod]
        public void AddPictureFromPath()
        {
            string picturePath = "C:/Users/Rafael/Documents/Diseno2/MillerAlonso/Sports.Domain.Test/TestImage/gun.png";
            team.AddPictureFromPath(picturePath);
            byte[] file = File.ReadAllBytes(picturePath);
            Assert.IsTrue(Enumerable.SequenceEqual(file, team.Picture));
        }

        //invalidImagePath

        //invalidImageFile
    }
}
