using System;
using System.Collections.Generic;
using System.Text;
using Sports.Domain;
using Sports.Repository.Interface;
using Sports.Repository.Context;
using System.Linq;
using System.Linq.Expressions;
using Sports.Logic.Constants;
using System.Data.Common;
using Sports.Repository.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Sports.Repository
{
    public class CommentRepository : RepositoryBase<Comment>, ICommentRepository
    {
        public CommentRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public override ICollection<Comment> FindAll()
        {
            try
            {
                return RepositoryContext.Comments
                    .Include(c=>c.User)
                    .Include(c=>c.Match)
                        .ThenInclude(m=>m.Local)
                    .Include(c => c.Match)
                        .ThenInclude(m => m.Visitor)
                    .Include(c => c.Match)
                        .ThenInclude(m => m.Sport)
                    .ToList<Comment>();
            }
            catch (DbException)
            {
                throw new DisconnectedDatabaseException(AccessValidation.INVALID_ACCESS_MESSAGE);
            }
            catch (Exception)
            {
                throw new UnknownDbException(AccessValidation.UNKNOWN_ERROR_MESSAGE);
            }
        }

        public override ICollection<Comment> FindByCondition(Expression<Func<Comment, bool>> expression)
        {
            try
            {
                return RepositoryContext.Comments
                    .Where(expression)
                    .Include(c => c.User)
                    .Include(c => c.Match)
                        .ThenInclude(m => m.Local)
                    .Include(c => c.Match)
                        .ThenInclude(m => m.Visitor)
                    .Include(c => c.Match)
                        .ThenInclude(m => m.Sport)
                    .ToList<Comment>();
            }
            catch (DbException)
            {
                throw new DisconnectedDatabaseException(AccessValidation.INVALID_ACCESS_MESSAGE);
            }
            catch (Exception)
            {
                throw new UnknownDbException(AccessValidation.UNKNOWN_ERROR_MESSAGE);
            }
        }
    }
}
