using System;
using System.Collections.Generic;
using System.Text;
using Sports.Domain;
using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Linq;
using System.Reflection;
using Sports.Domain.Exceptions;

namespace Sports.Domain.Test
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class TeamTest
    {
        static string mypath = AppDomain.CurrentDomain.BaseDirectory;
        string _testImagePath = mypath + "/TestImage/gun.png";
        string _invalidFilePath = mypath + "/TestImage/example.txt";
        string _largeFilePath = mypath+"/TestImage/BigImg.jpg";
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

        [ExpectedException(typeof(InvalidEmptyTextFieldException))]
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
            team.AddPictureFromPath(_testImagePath);
            byte[] file = File.ReadAllBytes(_testImagePath);
            string fileString = System.Text.Encoding.UTF8.GetString(file);
            Assert.AreEqual<string>(fileString, team.Picture);
        }

        [ExpectedException(typeof(InvalidTeamImageException))]
        [TestMethod]
        public void InvalidPicturePath()
        {
            team.AddPictureFromPath("C:/testPicture.png");
        }


        [ExpectedException(typeof(InvalidTeamImageException))]
        [TestMethod]
        public void InvalidFile()
        {
            team.AddPictureFromPath(_invalidFilePath);
        }

        [ExpectedException(typeof(InvalidTeamImageException))]
        [TestMethod]
        public void OversizedFile()
        {
            team.AddPictureFromPath(_largeFilePath);
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
        public void EqualsNull()
        {
            Assert.IsFalse(team.Equals(null));
        }

        [TestMethod]
        public void ToStringRedefined()
        {
            Assert.AreEqual<string>(team.ToString(),team.Name);
        }
    }
}
