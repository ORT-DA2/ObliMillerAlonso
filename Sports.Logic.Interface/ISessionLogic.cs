using System;
using System.Collections.Generic;
using System.Text;
using Sports.Domain;

namespace Sports.Logic.Interface
{
    public interface ISessionLogic
    {
        Guid LogInUser(string username, string password);
        User GetUserFromToken(Guid token);
        void LogoutByUser(User user);
    }
}
