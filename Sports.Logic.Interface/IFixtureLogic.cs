using System;
using System.Collections.Generic;
using System.Text;
using Sports.Domain;

namespace Sports.Logic.Interface
{
    public interface IFixtureLogic
    {
        void AddFixtureImplementations(String path);
        void GenerateFixture(ICollection<Sport> sports, DateTime startDate);
        string ChangeFixtureImplementation();
        void ResetFixtureStrategies();
        void SetSession(Guid token);
    }
}
