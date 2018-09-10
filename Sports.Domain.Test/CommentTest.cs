using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sports.Exceptions;


namespace Sports.Domain.Test
{
    [TestClass]
    public class CommentTest
    {
        Comment comment;

        [TestInitialize]
        public void SetUp()
        {
            comment = new Comment();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCommentDataException))]
        public void InvalidCommentText()
        {
            Comment invalidText = new Comment()
            {
               Text = ""
            };
            invalidText.IsValid();
        }

    }
}
