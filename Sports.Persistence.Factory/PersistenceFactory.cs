using System;
using System.Collections.Generic;
using System.Text;
using Sports.Repository.Interface;
using Sports.Repository;

namespace Sports.Persistence.Factory
{
    public class PersistenceFactory : IPersistenceFactory
    {
        public IUserRepository GetUserImplementation()
        {
            return new UserRepository();
        }
        
    }
}
