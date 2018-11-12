using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Sports.Domain
{
    public class AthleteRanking : IRankingGenerator
    {
        public ICollection<CompetitorScore> GenerateScores(ICollection<CompetitorScore> competitors)
        {
            ICollection<CompetitorScore> orderedResults = competitors.OrderByDescending(c => c.Score).ToList();
            int pos = 1;
            foreach (CompetitorScore individualResult in orderedResults)
            {
                if(pos == 1)
                {
                    individualResult.Score = 3;
                }
                else if (pos == 2)
                {
                    individualResult.Score = 2;
                }
                else if (pos == 3)
                {
                    individualResult.Score = 1;
                }
                else
                {
                    individualResult.Score = 0;
                }
                pos++;
            }
            return orderedResults;
        }
    }
}
