using System;
using System.Collections.Generic;
using System.Text;
using Sports.Domain;

namespace Sports.Logic.Interface
{
    public interface IFixtureGeneratorStrategy
    {
        ICollection<Match> GenerateFixture(ICollection<Sport> sports);
    }
}
