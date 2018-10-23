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
                GenerateMatches(sport.Competitors.ToList(), new List<Competitor>(), sport.Competitors.Count - 1, 0);
            }
            return generatedMatches;
        }
        


        public void GenerateMatches(ICollection<Competitor> competitors, ICollection<Competitor> currentCompetitors, int start, int end)
        {
            if (currentCompetitors.Count < 1)
            {
                lastFreeDate = 1;
            }
            if (currentCompetitors.Count == currentSport.Amount)
            {
                CreateNextMatch(currentCompetitors);
                return;
            }
            for (int i = start; i <= end && end - i + 1 >= currentSport.Amount - currentCompetitors.Count; i++)
            {
                currentCompetitors.Add(competitors.ElementAt(i));
                GenerateMatches(competitors, currentCompetitors, i + 1, end);
                currentCompetitors.Remove(competitors.ElementAt(i));
            }
        }


        private void CreateNextMatch(ICollection<Competitor> competitors)
        {
            DateTime nextFreeDate = GetNextFreeWeekendDate(competitors);
            Match nextMatch = new Match()
            {
                Sport = currentSport,
                Competitors = competitors,
                Date = nextFreeDate
            };
            generatedMatches.Add(nextMatch);
        }

        private DateTime GetNextFreeWeekendDate(ICollection<Competitor> competitors)
        {
            bool dateIsOcupied = true;
            DateTime validDate = initialDate;
            while (dateIsOcupied)
            {
                DateTime date = initialDate.AddDays(lastFreeDate);
                if (IsWeekend(date) )
                {
                    if(UnoccupiedDateByCompetitors(date, competitors))
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

        private bool UnoccupiedDateByCompetitors(DateTime date, ICollection<Competitor> competitors)
        {
            return !generatedMatches.Exists(m => m.Date.Date.Equals(date.Date) && AreInMatch(competitors, m));
        }

        private bool AreInMatch(ICollection<Competitor> competitors, Match match)
        {
            ICollection<Competitor> alreadyPlaying = match.Competitors.Intersect(competitors).ToList();
            return alreadyPlaying.Count>0;
        }

        public string FixtureInfo()
        {
            return "Generates all versus all matches only on weekends, no rematch.";
        }
    }
}
