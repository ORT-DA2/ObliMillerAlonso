using System;
using System.Collections.Generic;
using System.Text;

namespace Sports.Domain
{
    public interface IRankingGenerator
    {
        ICollection<CompetitorScore> GenerateScores(ICollection<CompetitorScore> competitors);
    }
}
