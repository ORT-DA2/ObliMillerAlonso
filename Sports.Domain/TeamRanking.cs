using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Sports.Domain
{
    public class TeamRanking : IRankingGenerator
    {
        CompetitorScore firstTeam;
        CompetitorScore secondTeam;
        public ICollection<CompetitorScore> GenerateScores(ICollection<CompetitorScore> competitors)
        {
            firstTeam = competitors.ElementAt(0);
            secondTeam = competitors.ElementAt(1);
            if (firstTeam.Score < secondTeam.Score)
            {
                firstTeam.Score = 0;
                secondTeam.Score = 3;
                return new List<CompetitorScore>() { firstTeam, secondTeam };
            }
            else if(firstTeam.Score > secondTeam.Score)
            {
                firstTeam.Score = 3;
                secondTeam.Score = 0;
                return new List<CompetitorScore>() { firstTeam, secondTeam };
            }
            else
            {
                firstTeam.Score = 1;
                secondTeam.Score = 1;
                return new List<CompetitorScore>() { firstTeam, secondTeam };
            }
        }
    }
}
