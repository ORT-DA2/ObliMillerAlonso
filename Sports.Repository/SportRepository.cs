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
    public class SportRepository : RepositoryBase<Sport>, ISportRepository
    {
        public SportRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }
        public override ICollection<Sport> FindAll()
        {
            try
            {
                return RepositoryContext.Sports
                    .Include(s => s.Teams)
                    .ToList<Sport>();
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

        public override ICollection<Sport> FindByCondition(Expression<Func<Sport, bool>> expression)
        {
            try
            {
                return RepositoryContext.Sports
                    .Where(expression)
                    .Include(s => s.Teams)
                    .ToList<Sport>();
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
