using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sports.Domain.Exceptions;

namespace Sports.Domain.Test
{
    [ExcludeFromCodeCoverage]
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
        public void NewComment()
        {
            Assert.IsNotNull(comment);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidEmptyTextFieldException))]
        public void InvalidCommentText()
        {
            Comment invalidText = new Comment()
            {
               Text = "",
                User = new User()
            };
            invalidText.IsValid();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidEmptyUserException))]
        public void InvalidCommentUser()
        {
            Comment invalidText = new Comment()
            {
                Text = "comment"
            };
            invalidText.IsValid();
        }

    }
}
