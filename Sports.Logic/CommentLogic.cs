using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Sports.Domain;
using Sports.Exceptions;
using Sports.Repository.Interface;

namespace Sports.Logic
{
    public class CommentLogic
    {
        ICommentRepository _repository;

        public CommentLogic(IRepositoryWrapper wrapper)
        {
            _repository = wrapper.Comment;
        }
        public void AddComment(Comment comment)
        {
            ValidateComment(comment);
            _repository.Create(comment);
        }

        private void ValidateComment(Comment comment)
        {
            CheckNotNull(comment);
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
    }
}
