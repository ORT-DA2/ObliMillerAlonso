using System;
using System.Collections.Generic;
using System.Text;
using Sports.Domain;
using System.Linq;

namespace FixtureImplementations
{
    public class FixtureWeekendMatches : IFixtureGeneratorStrategy
    {
        private List<Match> generatedMatches;
        private List<Competitor> uncoveredCompetitors;
        private List<DateTime> occupiedDates;  
        private Sport currentSport;
        private int lastFreeDate;
        private DateTime initialDate;
        public ICollection<Match> GenerateFixture(ICollection<Sport> sports, DateTime startDate)
        {
            generatedMatches = new List<Match>();
            foreach (Sport sport in sports.ToList())
            {
                currentSport = sport;
                initialDate = startDate;
                uncoveredCompetitors = currentSport.Competitors.ToList();
                WeekendMatches();
            }
            return generatedMatches;
        }

        private void WeekendMatches()
        {
            foreach (Competitor competitor in currentSport.Competitors.ToList())
            {
                lastFreeDate = 1;
                uncoveredCompetitors.Remove(competitor);
                GenerateMatches(competitor);
            }
        }
      

        private void GenerateMatches(Competitor competitor)
        {
            foreach (Competitor rivalCompetitor in uncoveredCompetitors)
            {
                Match nextMatch = CreateNextMatch(competitor, rivalCompetitor);
                generatedMatches.Add(nextMatch);
            }
        }
        

        private Match CreateNextMatch(Competitor local, Competitor visitor)
        {
            DateTime nextFreeDate = GetNextFreeWeekendDate(local, visitor);
            Match nextMatch = new Match()
            {
                Sport = currentSport,
                Local = local,
                Visitor = visitor,
                Date = nextFreeDate
            };
            return nextMatch;
        }

        private DateTime GetNextFreeWeekendDate(Competitor local, Competitor visitor)
        {
            bool dateIsOcupied = true;
            DateTime validDate = initialDate;
            while (dateIsOcupied)
            {
                DateTime date = initialDate.AddDays(lastFreeDate);
                if (IsWeekend(date) )
                {
                    if(UnoccupiedDateByCompetitors(date, local, visitor))
                    {
                        dateIsOcupied = false;
                        validDate = date;
                    }
                    lastFreeDate += 5;
                }
                else
                {
                    lastFreeDate += 1;
                }
            }
            return validDate;
        }

        private bool IsWeekend(DateTime date)
        {
            return date.DayOfWeek.Equals(DayOfWeek.Sunday) || date.DayOfWeek.Equals(DayOfWeek.Saturday);
        }

        private bool UnoccupiedDateByCompetitors(DateTime date, Competitor local, Competitor visitor)
        {
            return !generatedMatches.Exists(m => m.Date.Date.Equals(date.Date) && (IsInMatch(local, m)||IsInMatch(visitor,m)));
        }

        private bool IsInMatch(Competitor competitor, Match match)
        {
            return match.Local.Equals(competitor) || match.Visitor.Equals(competitor);
        }

        public string FixtureInfo()
        {
            return "Generates all versus all matches only on weekends, no rematch.";
        }
    }
}
