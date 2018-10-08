using Sports.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace FailingFixtureImplementations
{
    public class FixtureFailing : IFixtureGeneratorStrategy
    {
        public string FixtureInfo()
        {
            throw new NotImplementedException();
        }

        public ICollection<Match> GenerateFixture(ICollection<Sport> sports, DateTime startDate)
        {
            throw new NotImplementedException();
        }
    }
}
