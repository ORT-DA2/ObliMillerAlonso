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
        private ICommentLogic _commentLogic;
        private IUserLogic _userLogic;
        private Comment _comment;
        private User _user;

        [TestInitialize]
        public void SetUp()
        {
            _user = new User(true)
            {
                FirstName = "Itai",
                LastName = "Miller",
                Email = "itaimiller@gmail.com",
                UserName = "iMiller",
                Password = "root"
            };

            _comment = new Comment()
            {
                Text = "comment",
                User = _user
            };

            
            var options = new DbContextOptionsBuilder<RepositoryContext>()
                .UseInMemoryDatabase<RepositoryContext>(databaseName: "UserLogicTestDB")
                .Options;
            _repository = new RepositoryContext(options);
            _wrapper = new RepositoryWrapper(_repository);
            _commentLogic = new CommentLogic(_wrapper);
            _userLogic = new UserLogic(_wrapper);
            _userLogic.AddUser(_user);
        }

        [TestCleanup]
        public void TearDown()
        {
            _repository.Comments.RemoveRange(_repository.Comments);
            _repository.Users.RemoveRange(_repository.Users);
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

        [TestMethod]
        [ExpectedException(typeof(InvalidUserDataException))]
        public void VerifyCommentUser()
        {
            User testUser = new User(){ };
            Comment userComment = new Comment()
            {
                Text = "comment",
                User = testUser
            };
            _commentLogic.AddComment(userComment);
        }

        [TestMethod]
        public void CommentGetAll()
        {
            _commentLogic.AddComment(_comment);
            Assert.AreEqual(_commentLogic.GetAll().Count, 1);
        }
    }

}
