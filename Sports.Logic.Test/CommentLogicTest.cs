using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Sports.Domain;
using Sports.Logic;
using Sports.Repository;
using Sports.Repository.Interface;
using Sports.Repository.Context;
using Sports.Exceptions;
using System.Diagnostics.CodeAnalysis;

namespace Sports.Logic.Test
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class CommentLogicTest
    {

        [TestMethod]
        public void AddComment()
        {
            Comment comment = new Comment()
            {
                Text = "comment"
            };
            _commentLogic.AddComment(comment);
            Assert.IsNotNull(_commentLogic.GetCommentById(comment.Id));
        }
    }

}
