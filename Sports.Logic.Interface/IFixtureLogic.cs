using System;
using System.Collections.Generic;
using System.Text;
using Sports.Domain;

namespace Sports.Logic.Interface
{
    public interface IFixtureLogic
    {
        void AddFixtureImplementations(String path);
        ICollection<Match> GenerateFixture(ICollection<Sport> sports);
        void ChangeFixtureImplementation();
        void SetSession(Guid token);
    }
}
