using System;
using System.Collections.Generic;
using System.Text;
using Sports.Persistence.Interface;
using Sports.Persistence;

namespace Sports.Persistence.Factory
{
    public class PersistenceFactory : IPersistenceFactory
    {
        public IUserPersistence GetUserImplementation()
        {
            return new UserPersistence();
        }
        
    }
}
