using System;
using System.Collections.Generic;
using System.Text;
using Sports.Domain;

namespace Sports.Logic.Interface
{
    public interface IFixtureLogic
    {
        ICollection<string> RefreshFixtureImplementations();
        void GenerateFixture(int pos, ICollection<Sport> sports, DateTime startDate);
        ICollection<string> GetFixtureImplementations();
        void ResetFixtureStrategies();
        User SetSession(Guid token);
    }
}
