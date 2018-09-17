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
        private IRepositoryWrapper _wrapper;
        private RepositoryContext _repository;
        private CommentLogic _commentLogic;
        private Comment _comment;

        [TestInitialize]
        public void SetUp()
        {
            _comment = new Comment()
            {
                Text = "comment"
            };

            var options = new DbContextOptionsBuilder<RepositoryContext>()
                .UseInMemoryDatabase<RepositoryContext>(databaseName: "UserLogicTestDB")
                .Options;
            _repository = new RepositoryContext(options);
            _wrapper = new RepositoryWrapper(_repository);
            _commentLogic = new CommentLogic(_wrapper);
        }

        [TestCleanup]
        public void TearDown()
        {
            _repository.Comments.RemoveRange(_repository.Comments);
            _repository.SaveChanges();
        }

        [TestMethod]
        public void AddComment()
        {
            _commentLogic.AddComment(_comment);
            Assert.IsNotNull(_commentLogic.GetCommentById(_comment.Id));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCommentDataException))]
        public void AddNullComment()
        {
            _commentLogic.AddComment(null);
        }
    }

}
