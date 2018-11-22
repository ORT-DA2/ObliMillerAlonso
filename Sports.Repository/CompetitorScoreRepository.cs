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

    }
}
