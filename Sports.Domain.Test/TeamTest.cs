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
using Newtonsoft.Json.Linq;

namespace Sports.Domain.Test
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class TeamTest
    {
        string testImagePath;
        string invalidFilePath;
        string largeFilePath;
        Team team;
        [TestInitialize]
        public void SetUp()
        {
            team = new Team()
            {
                Name = "Test Team",

            };
            JObject jsonPaths = JObject.Parse(File.ReadAllText(@"testFilesPaths.json"));
            testImagePath = jsonPaths.SelectToken("testImagePath").ToString();
            invalidFilePath = jsonPaths.SelectToken("invalidFilePath").ToString();
            largeFilePath = jsonPaths.SelectToken("bigImgPath").ToString();
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
            team.AddPictureFromPath(testImagePath);
            byte[] file = File.ReadAllBytes(testImagePath);
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
            team.AddPictureFromPath(invalidFilePath);
        }

        [ExpectedException(typeof(InvalidTeamImageException))]
        [TestMethod]
        public void OversizedFile()
        {
            team.AddPictureFromPath(largeFilePath);
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
