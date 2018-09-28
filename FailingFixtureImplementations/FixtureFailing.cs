using Sports.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace FailingFixtureImplementations
{
    public class FixtureFailing : IFixtureGeneratorStrategy
    {
        public ICollection<Match> GenerateFixture(ICollection<Sport> sports)
        {
            throw new NotImplementedException();
        }
    }
}
