using System;
using System.Collections.Generic;
using System.Text;
using Sports.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Linq;
using Sports.Exceptions;

namespace Sports.Domain.Test
{
    [TestClass]
    public class TeamTest
    {
        const string TEST_IMAGE_PATH = "C:/Users/Rafael/Documents/Diseno2/MillerAlonso/Sports.Domain.Test/TestImage/gun.png";
        const string INVALID_FILE_PATH = "C:/Users/Rafael/Documents/Diseno2/MillerAlonso/Sports.Domain.Test/TestImage/example.txt";
        const string LARGE_IMAGE_PATH = "C:/Users/Rafael/Documents/Diseno2/MillerAlonso/Sports.Domain.Test/TestImage/BigImg.jpg";
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

        [ExpectedException(typeof(InvalidTeamDataException))]
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
            team.AddPictureFromPath(TEST_IMAGE_PATH);
            byte[] file = File.ReadAllBytes(TEST_IMAGE_PATH);
            string fileString = System.Text.Encoding.UTF8.GetString(file);
            Assert.AreEqual<string>(fileString, team.Picture);
        }

        [ExpectedException(typeof(InvalidTeamDataException))]
        [TestMethod]
        public void InvalidPicturePath()
        {
            team.AddPictureFromPath("C:/testPicture.png");
        }


        [ExpectedException(typeof(InvalidTeamDataException))]
        [TestMethod]
        public void InvalidFile()
        {
            team.AddPictureFromPath(INVALID_FILE_PATH);
        }

        [ExpectedException(typeof(InvalidTeamDataException))]
        [TestMethod]
        public void OversizedFile()
        {
            team.AddPictureFromPath(LARGE_IMAGE_PATH);
        }

        [TestMethod]
        public void EqualsIsTrue()
        {
            Team secondTeam = new Team()
            {
                Name = "Test Team",

            };
            Assert.IsTrue(team.Equals(secondTeam));
        }


        [TestMethod]
        public void EqualsIsFalse()
        {
            Team secondTeam = new Team()
            {
                Name = "Different Team",

            };
            Assert.IsFalse(team.Equals(secondTeam));
        }

        [TestMethod]
        public void ToStringRedefined()
        {
            Assert.AreEqual<string>(team.ToString(),team.Name);
        }
    }
}
