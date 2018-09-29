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
using System.Diagnostics.CodeAnalysis;
using Sports.Logic.Exceptions;


namespace Sports.Logic.Test
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class CommentLogicTest
    {
        private IRepositoryUnitOfWork unitOfWork;
        private RepositoryContext repository;
        private ICommentLogic commentLogic;
        private IUserLogic userLogic;
        private Comment comment;
        private User user;

        [TestInitialize]
        public void SetUp()
        {
            SetUpRepositories();
            SetUpTestData();

        }

        private void SetUpTestData()
        {
            user = new User(true)
            {
                FirstName = "Itai",
                LastName = "Miller",
                Email = "itaimiller@gmail.com",
                UserName = "iMiller",
                Password = "root"
            };
            userLogic.AddUser(user);
            comment = new Comment()
            {
                Text = "comment",
                User = user
            };
        }

        private void SetUpRepositories()
        {
            var options = new DbContextOptionsBuilder<RepositoryContext>()
                .UseInMemoryDatabase<RepositoryContext>(databaseName: "CommentLogicTestDB")
                .Options;
            repository = new RepositoryContext(options);
            unitOfWork = new RepositoryUnitOfWork(repository);
            commentLogic = new CommentLogic(unitOfWork);
            userLogic = new UserLogic(unitOfWork);;
        }

        [TestCleanup]
        public void TearDown()
        {
            repository.Comments.RemoveRange(repository.Comments);
            repository.Users.RemoveRange(repository.Users);
            repository.SaveChanges();
        }

        [TestMethod]
        public void AddComment()
        {
            commentLogic.AddComment(comment);
            Assert.IsNotNull(commentLogic.GetCommentById(comment.Id));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNullValueException))]
        public void AddNullComment()
        {
            commentLogic.AddComment(null);
        }

        [TestMethod]
        [ExpectedException(typeof(UserDoesNotExistException))]
        public void VerifyCommentUser()
        {
            User testUser = new User(){ };
            Comment userComment = new Comment()
            {
                Text = "comment",
                User = testUser
            };
            commentLogic.AddComment(userComment);
        }

        [TestMethod]
        public void CommentGetAll()
        {
            commentLogic.AddComment(comment);
            Assert.AreEqual(commentLogic.GetAll().Count, 1);
        }


        [TestMethod]
        public void CascadeDeleteCommentsFromUser()
        {
            commentLogic.AddComment(comment);
            userLogic.RemoveUser(user.Id);
            Assert.AreEqual(commentLogic.GetAll().Count, 0);
        }
    }

}
