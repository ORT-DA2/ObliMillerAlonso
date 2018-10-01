using System;
using System.Collections.Generic;
using System.Text;
using Sports.Domain;

namespace Sports.Logic.Interface
{
    public interface ICommentLogic
    {
        void AddComment(Comment comment);
        Comment GetCommentById(int id);
        ICollection<Comment> GetAll();
        void SetSession(Guid token);
    }
}
