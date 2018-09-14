using System;
using System.Collections.Generic;
using System.Text;
using Sports.Domain;

namespace Sports.Persistence.Interface
{
    public interface IUserPersistence
    {
        void Add(User user);
        User GetById(int id);


        
    }
}
