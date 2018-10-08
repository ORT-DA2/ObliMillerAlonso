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
    public class TeamRepository : RepositoryBase<Team>, ITeamRepository
    {
        public TeamRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public override ICollection<Team> FindAll()
        {
            try
            {
                return RepositoryContext.Teams
                    .Include(t=>t.Sport)
                    .ToList<Team>();
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

        public override ICollection<Team> FindByCondition(Expression<Func<Team, bool>> expression)
        {
            try
            {
                return RepositoryContext.Teams
                    .Where(expression)
                    .Include(t => t.Sport)
                    .ToList<Team>();
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
