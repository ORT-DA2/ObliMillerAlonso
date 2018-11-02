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
    public class CompetitorRepository : RepositoryBase<Competitor>, ICompetitorRepository
    {
        public CompetitorRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public override ICollection<Competitor> FindAll()
        {
            try
            {
                return RepositoryContext.Competitors
                    .Include(t=>t.Sport)
                    .ToList<Competitor>();
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

        public override ICollection<Competitor> FindByCondition(Expression<Func<Competitor, bool>> expression)
        {
            try
            {
                return RepositoryContext.Competitors
                    .Where(expression)
                    .Include(t => t.Sport)
                    .ToList<Competitor>();
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
