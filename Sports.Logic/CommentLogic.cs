using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Sports.Domain;
using Sports.Repository.Interface;
using Sports.Logic.Interface;
using Sports.Logic.Exceptions;
using Sports.Logic.Constants;

namespace Sports.Logic
{
    public class CommentLogic : ICommentLogic
    {
        ICommentRepository repository;
        IUserLogic userLogic;
        ISessionLogic sessionLogic;
        User user;

        public CommentLogic(IRepositoryUnitOfWork unitOfWork)
        {
            repository = unitOfWork.Comment;
            userLogic = new UserLogic(unitOfWork);
            sessionLogic = new SessionLogic(unitOfWork);
        }
        public void AddComment(Comment comment)
        {
            sessionLogic.ValidateUserNotNull(user);
            ValidateComment(comment);
            repository.Create(comment);
            repository.Save();
        }

        private void ValidateComment(Comment comment)
        {
            CheckNotNull(comment);
            CheckUserNotNull(comment);
            comment.User =userLogic.GetUserById(comment.User.Id);
        }

        private void CheckUserNotNull(Comment comment)
        {
            if (comment.User == null)
            {
                throw new InvalidNullValueException(NullValue.INVALID_USER_NULL_VALUE_MESSAGE);
            }
        }

        private void CheckNotNull(Comment comment)
        {
            if (comment == null)
            {
                throw new InvalidNullValueException(NullValue.INVALID_COMMENT_NULL_VALUE_MESSAGE);
            }
        }

        public Comment GetCommentById(int id)
        {
            sessionLogic.ValidateUserNotNull(user);
            ICollection<Comment> comments = repository.FindByCondition(c => c.Id == id);
            return comments.First();
        }

        public ICollection<Comment> GetAll()
        {
            sessionLogic.ValidateUserNotNull(user);
            return repository.FindAll();
        }

        public void SetSession(Guid token)
        {
            user = sessionLogic.GetUserFromToken(token);
            userLogic.SetSession(token);
        }
    }
}
