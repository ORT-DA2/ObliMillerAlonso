using System;
using System.Collections.Generic;
using System.Text;

namespace Sports.Persistence.Interface
{
    public interface IPersistenceFactory
    {
        IUserPersistence GetUserImplementation();
    }
}
