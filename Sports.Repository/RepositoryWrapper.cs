using System;
using System.Collections.Generic;
using System.Text;
using Sports.Repository.Interface;
using Sports.Repository;
using Sports.Repository.Context;

namespace Sports.Repository
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private RepositoryContext _repositoryContext;
        private IUserRepository _user;

        public RepositoryWrapper(RepositoryContext repositoryContext)
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

    }
}
