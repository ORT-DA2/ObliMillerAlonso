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
        private List<Competitor> uncoveredCompetitors;
        private Sport currentSport;
        private int daysToAddToDate;
        private DateTime initialDate;
        public ICollection<Match> GenerateFixture(ICollection<Sport> sports, DateTime startDate)
        {
            generatedMatches = new List<Match>();
            foreach (Sport sport in sports.ToList())
            {
                currentSport = sport;
                daysToAddToDate = 1;
                initialDate = startDate;
                LocalWeeklyMatches();
                VisitorWeeklyMatches();
            }
            return generatedMatches;
        }

        private void LocalWeeklyMatches()
        {
            uncoveredCompetitors = currentSport.Competitors.ToList();
            foreach (Competitor competitor in currentSport.Competitors.ToList())
            {
                uncoveredCompetitors.Remove(competitor);
                GenerateLocalMatches(competitor);
            }
        }

        private void VisitorWeeklyMatches()
        {
            uncoveredCompetitors = currentSport.Competitors.ToList();
            foreach (Competitor competitor in currentSport.Competitors.ToList())
            {
                uncoveredCompetitors.Remove(competitor);
                GenerateVisitorMatches(competitor);
            }
        }

        private void GenerateVisitorMatches(Competitor competitor)
        {
            foreach (Competitor local in uncoveredCompetitors)
            {
                Match visitorMatch = CreateNextMatch(local, competitor);
                generatedMatches.Add(visitorMatch);
            }
        }

        private void GenerateLocalMatches(Competitor competitor)
        {
            foreach (Competitor visitor in uncoveredCompetitors)
            {
                Match localMatch = CreateNextMatch(competitor,visitor);
                generatedMatches.Add(localMatch);
            }
        }
        

        private Match CreateNextMatch(Competitor local, Competitor visitor)
        {
            Match nextMatch = new Match()
            {
                Sport = currentSport,
                Local = local,
                Visitor = visitor,
                Date = initialDate.AddDays(daysToAddToDate),
            };
            daysToAddToDate++;
            return nextMatch;
        }


        public string FixtureInfo()
        {
            return "Generates all versus all matches one each day, with rematch.";
        }
        
    }
}
