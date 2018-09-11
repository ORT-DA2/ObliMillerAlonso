using System;
using System.Collections.Generic;
using System.Text;
using Sports.Domain;
using Sports.Persistence.Interface;

namespace Sports.Logic
{
    public class UserLogic
    {
        IUserPersistence persistence;

        public UserLogic(IPersistenceFactory factoryImplementation)
        {
            persistence = factoryImplementation.GetUserImplementation();
        }
        public void AddUser(User user)
        {
            persistence.Add(user);
        }

        public User GetUserById(int id)
        {
            return persistence.GetById(id);
        }
    }
}
