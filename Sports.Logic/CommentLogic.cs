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
        ICommentRepository _repository;
        IUserLogic _userLogic;

        public CommentLogic(IRepositoryUnitOfWork unitOfWork)
        {
            _repository = unitOfWork.Comment;
            _userLogic = new UserLogic(unitOfWork);
        }
        public void AddComment(Comment comment)
        {
            ValidateComment(comment);
            _repository.Create(comment);
        }

        private void ValidateComment(Comment comment)
        {
            CheckNotNull(comment);
            CheckUserNotNull(comment);
            comment.User =_userLogic.GetUserById(comment.User.Id);
           
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
            ICollection<Comment> comments = _repository.FindByCondition(c => c.Id == id);
            return comments.First();

        }

        public ICollection<Comment> GetAll()
        {
            return _repository.FindAll();
        }
    }
}
