using System;
using System.Collections.Generic;
using System.Text;
using Sports.Domain;
using Sports.Repository.Interface;
using Sports.Repository.Context;
using System.Linq;

namespace Sports.Repository
{
    public class CommentRepository : RepositoryBase<Comment>, ICommentRepository
    {
        public CommentRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }
    }
}
