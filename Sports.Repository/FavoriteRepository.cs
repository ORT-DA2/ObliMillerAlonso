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
    public class FavoriteRepository : RepositoryBase<Favorite>, IFavoriteRepository
    {
        public FavoriteRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public override ICollection<Favorite> FindAll()
        {
            try
            {
                return RepositoryContext.Favorites
                    .Include(f => f.User)
                    .Include(f => f.Team)
                    .ToList<Favorite>();
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

        public override ICollection<Favorite> FindByCondition(Expression<Func<Favorite, bool>> expression)
        {
            try
            {
                return RepositoryContext.Favorites
                    .Where(expression)
                    .Include(f => f.User)
                    .Include(f => f.Team)
                    .ToList<Favorite>();
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
