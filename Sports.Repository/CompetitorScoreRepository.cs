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
    public class CompetitorScoreRepository : RepositoryBase<CompetitorScore>, ICompetitorScoreRepository
    {
        public CompetitorScoreRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }
/*
        public override ICollection<CompetitorScore> FindAll()
        {
            try
            {
                return RepositoryContext.CompetitorScores
                    .Include(t=>t.Competitor)
                        .ThenInclude(c=>c.Sport)
                    .ToList<CompetitorScore>();
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

        public override ICollection<CompetitorScore> FindByCondition(Expression<Func<CompetitorScore, bool>> expression)
        {
            try
            {
                return RepositoryContext.CompetitorScores
                    .Where(expression)
                    .Include(t => t.Competitor)
                        .ThenInclude(c => c.Sport)
                    .ToList<CompetitorScore>();
            }
            catch (DbException)
            {
                throw new DisconnectedDatabaseException(AccessValidation.INVALID_ACCESS_MESSAGE);
            }
            catch (Exception)
            {
                throw new UnknownDbException(AccessValidation.UNKNOWN_ERROR_MESSAGE);
            }
        }*/
    }
}
