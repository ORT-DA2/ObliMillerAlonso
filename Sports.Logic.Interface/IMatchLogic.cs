using System;
using System.Collections.Generic;
using System.Text;
using Sports.Domain;

namespace Sports.Logic.Interface
{
    public interface IMatchLogic
    {
        void AddMatch(Match match);
        Match GetMatchById(int id);
    }
}
