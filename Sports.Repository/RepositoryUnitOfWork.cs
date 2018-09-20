﻿using System;
using System.Collections.Generic;
using System.Text;
using Sports.Repository.Interface;
using Sports.Repository;
using Sports.Repository.Context;

namespace Sports.Repository
{
    public class RepositoryUnitOfWork : IRepositoryUnitOfWork
    {
        private RepositoryContext _repositoryContext;
        private IUserRepository _user;
        private ITeamRepository _team;
        private ICommentRepository _comment;
        private ISportRepository _sport;
        private IMatchRepository _match;
        private ILoginRepository _login;

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
        public ITeamRepository Team {
            get {
                if (_team == null)
                    _team = new TeamRepository(_repositoryContext);
                return _team;
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

        public ILoginRepository Login
        {
            get
            {
                if (_login == null)
                    _login = new LoginRepository(_repositoryContext);
                return _login;
            }
        }

    }
}
