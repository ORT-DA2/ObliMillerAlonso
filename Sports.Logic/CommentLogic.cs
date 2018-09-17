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
            _repository.Create(comment);
        }

        public Comment GetCommentById(int id)
        {
            ICollection<Comment> comments = _repository.FindByCondition(c => c.Id == id);
            return comments.First();

        }
    }
}
