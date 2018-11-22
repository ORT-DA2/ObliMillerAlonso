using System;
using System.Collections.Generic;
using System.Text;
using Sports.Domain;
using System.Linq;

namespace FixtureImplementations
{
    public class FixtureBackAndForthDaily : IFixtureGeneratorStrategy
    {
        private List<Match> generatedMatches;
        private Sport currentSport;
        private int daysToAddToDate;
        private DateTime initialDate;
        public ICollection<Match> GenerateFixture(Sport sport, DateTime startDate)
        {
            generatedMatches = new List<Match>();
            currentSport = sport;
            daysToAddToDate = 1;
            initialDate = startDate;
            GenerateMatches(sport.Competitors.ToList(),new List<Competitor>(), 0, sport.Competitors.Count - 1);
            GenerateMatches(sport.Competitors.ToList(), new List<Competitor>(), 0, sport.Competitors.Count - 1);
            return generatedMatches;
        }



        public void GenerateMatches(ICollection<Competitor> competitors, ICollection<Competitor> currentCompetitors, int start, int end)
        {
            if (currentCompetitors.Count == currentSport.Amount)
            {
                CreateNextMatch(AdaptForMatch(currentCompetitors));
                return;
            }
            for (int i = start; i <= end && end - i + 1 >= currentSport.Amount - currentCompetitors.Count; i++)
            {
                currentCompetitors.Add(competitors.ElementAt(i));
                GenerateMatches(competitors, currentCompetitors, i + 1, end);
                currentCompetitors.Remove(competitors.ElementAt(i));
            }
        }

        private ICollection<CompetitorScore> AdaptForMatch(ICollection<Competitor> currentCompetitors)
        {
            ICollection<CompetitorScore> adapted = new List<CompetitorScore>();
            foreach (Competitor competitor in currentCompetitors)
            {
                adapted.Add(new CompetitorScore(competitor));
            }
            return adapted;
        }

        private void CreateNextMatch(ICollection<CompetitorScore> competitors)
        {
            Match nextMatch = new Match()
            {
                Sport = currentSport,
                Competitors = competitors,
                Date = initialDate.AddDays(daysToAddToDate),
            };
            daysToAddToDate++;
            generatedMatches.Add(nextMatch);
        }


        public string FixtureInfo()
        {
            return "Generates all versus all matches one each day, with rematch.";
        }
        
    }
}
