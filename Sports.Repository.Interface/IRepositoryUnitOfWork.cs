﻿using System;
using System.Collections.Generic;
using System.Text;
using Sports.Domain;

namespace Sports.Repository.Interface
{
    public interface IRepositoryUnitOfWork
    {
        IUserRepository User { get; }
        ICompetitorRepository Competitor { get; }
        ICommentRepository Comment { get; }
        ISportRepository Sport { get; }
        IMatchRepository Match { get; }
        ISessionRepository Session { get; }
        IFavoriteRepository Favorite {get; }
        ICompetitorScoreRepository CompetitorScore { get; }
    }
}
