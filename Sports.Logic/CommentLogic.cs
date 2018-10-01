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
            ValidateUser();
            ValidateComment(comment);
            repository.Create(comment);
            repository.Save();
        }

        private void ValidateUser()
        { 
            ValidateUserNotNull();
            ValidateUserNotAdmin();
        }

        private void ValidateUserNotNull()
        {
            if (user == null)
            {
                throw new InvalidNullValueException(NullValue.INVALID_USER_NULL_VALUE_MESSAGE);
            }
        }

        private void ValidateUserNotAdmin()
        {
            if (!user.IsAdmin)
            {
                throw new NonAdminException(AdminException.NON_ADMIN_EXCEPTION_MESSAGE);
            }
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
            ICollection<Comment> comments = repository.FindByCondition(c => c.Id == id);
            return comments.First();
        }

        public ICollection<Comment> GetAll()
        {
            return repository.FindAll();
        }

        public void SetSession(Guid token)
        {
            user = sessionLogic.GetUserFromToken(token);
        }
    }
}
