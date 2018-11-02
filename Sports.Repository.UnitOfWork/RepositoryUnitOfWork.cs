using System;
using System.Collections.Generic;
using System.Text;
using Sports.Repository;
using Sports.Repository.Interface;
using Sports.Repository.Context;

namespace Sports.Repository.UnitOfWork
{
    public class RepositoryUnitOfWork : IRepositoryUnitOfWork
    {
        private RepositoryContext _repositoryContext;
        private IUserRepository _user;
        private ICompetitorRepository _competitor;
        private ICommentRepository _comment;
        private ISportRepository _sport;
        private IMatchRepository _match;
        private ISessionRepository _session;
        private IFavoriteRepository _favorite;

        public RepositoryUnitOfWork(RepositoryContext repositoryContext)
        {
            _repositoryContext = repositoryContext;
        }
        public IUserRepository User
        {
            get 
            {
                if (_user == null)
                    _user = new UserRepository(_repositoryContext);
                return _user;
            }
        }
        public ICompetitorRepository Competitor {
            get {
                if (_competitor == null)
                    _competitor = new CompetitorRepository(_repositoryContext);
                return _competitor;
            }
        }

        public ICommentRepository Comment
        {
            get
            {
                if (_comment == null)
                    _comment = new CommentRepository(_repositoryContext);
                return _comment;
            }
        }
        
        public ISportRepository Sport
        {
            get {
                if (_sport == null)
                    _sport = new SportRepository(_repositoryContext);
                return _sport;
            }
        }

        public IMatchRepository Match
        {
            get {
                if (_match == null)
                    _match = new MatchRepository(_repositoryContext);
                return _match;
            }
        }

        public ISessionRepository Session
        {
            get
            {
                if (_session == null)
                    _session = new SessionRepository(_repositoryContext);
                return _session;
            }
        }

        public IFavoriteRepository Favorite
        {
            get
            {
                if (_favorite == null)
                    _favorite = new FavoriteRepository(_repositoryContext);
                return _favorite;
            }
        }

    }
}
