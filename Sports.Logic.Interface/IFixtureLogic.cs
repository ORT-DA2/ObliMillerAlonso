using System;
using System.Collections.Generic;
using System.Text;
using Sports.Domain;

namespace Sports.Logic.Interface
{
    public interface IFixtureLogic
    {
        ICollection<string> RefreshFixtureImplementations();
        void GenerateFixture(int pos, int sportId, DateTime startDate);
        ICollection<string> GetFixtureImplementations();
        void ResetFixtureStrategies();
        User SetSession(Guid token);
    }
}
