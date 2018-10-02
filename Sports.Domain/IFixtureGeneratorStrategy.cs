using System;
using System.Collections.Generic;
using System.Text;
using Sports.Domain;

namespace Sports.Domain
{
    public interface IFixtureGeneratorStrategy
    {
        ICollection<Match> GenerateFixture(ICollection<Sport> sports, DateTime startDate);
    }
}
