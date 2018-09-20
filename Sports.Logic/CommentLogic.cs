using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Sports.Domain;
using Sports.Exceptions;
using Sports.Repository.Interface;
using Sports.Logic.Interface;

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
            comment.User =_userLogic.GetUserById(comment.User.Id);
           
        }

        private void CheckNotNull(Comment comment)
        {
            if (comment == null)
            {
                throw new InvalidCommentDataException("Cannot add null comment");
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
