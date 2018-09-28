using System;
using System.Collections.Generic;
using System.Text;
using Sports.Domain;

namespace Sports.Logic.Interface
{
    public interface IFavoriteLogic
    {
        void AddFavoriteTeam(User user, Team team);
    }
}
