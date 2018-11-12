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
    public class CompetitorTest
    {
        string testImagePath;
        string invalidFilePath;
        string largeFilePath;
        Competitor competitor;
        [TestInitialize]
        public void SetUp()
        {
            competitor = new Competitor()
            {
                Name = "Test Competitor",

            };
            JObject jsonPaths = JObject.Parse(File.ReadAllText(@"testFilesPaths.json"));
            testImagePath = jsonPaths.SelectToken("testImagePath").ToString();
            invalidFilePath = jsonPaths.SelectToken("invalidFilePath").ToString();
            largeFilePath = jsonPaths.SelectToken("bigImgPath").ToString();
        }

        [TestMethod]
        public void NewCompetitor()
        {
            Assert.IsNotNull(competitor);
        }

        [ExpectedException(typeof(InvalidEmptyTextFieldException))]
        [TestMethod]
        public void InvalidName()
        {
            Competitor invalidNameCompetitor = new Competitor()
            {
                Name = "",

            };
            invalidNameCompetitor.IsValid();
        }
        
        [TestMethod]
        public void ValidName()
        {
            competitor.IsValid();
            Assert.AreEqual<string>(competitor.Name, "Test Competitor");
        }

        [TestMethod]
        public void EqualsIsTrue()
        {
            Competitor secondCompetitor = new Competitor()
            {
                Name = "Test Competitor",

            };
            Assert.IsTrue(competitor.Equals(secondCompetitor));
        }


        [TestMethod]
        public void EqualsIsFalse()
        {
            Competitor secondCompetitor = new Competitor()
            {
                Name = "Different Competitor",

            };
            Assert.IsFalse(competitor.Equals(secondCompetitor));
        }


        [TestMethod]
        public void EqualsNull()
        {
            Assert.IsFalse(competitor.Equals(null));
        }

        [TestMethod]
        public void ToStringRedefined()
        {
            Assert.AreEqual<string>(competitor.ToString(),competitor.Name);
        }
    }
}
