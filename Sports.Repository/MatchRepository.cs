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
    public class MatchRepository : RepositoryBase<Match>, IMatchRepository
    {
        public MatchRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }
        public override ICollection<Match> FindAll()
        {
            try
            {
                return RepositoryContext.Matches
                    .Include(m => m.Comments)
                        .ThenInclude(c => c.User)
                    .Include(m => m.Local)
                    .Include(m => m.Visitor)
                    .Include(m => m.Sport)
                    .ToList<Match>();
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

        public override ICollection<Match> FindByCondition(Expression<Func<Match, bool>> expression)
        {
            try
            {
                return RepositoryContext.Matches
                    .Where(expression)
                    .Include(m => m.Comments)
                        .ThenInclude(c => c.User)
                    .Include(m => m.Local)
                    .Include(m => m.Visitor)
                    .Include(m => m.Sport)
                    .ToList<Match>();
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
