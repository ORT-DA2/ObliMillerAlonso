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
        private List<Team> uncoveredTeams;
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
                uncoveredTeams = currentSport.Teams.ToList();
                WeekendMatches();
            }
            return generatedMatches;
        }

        private void WeekendMatches()
        {
            foreach (Team team in currentSport.Teams.ToList())
            {
                lastFreeDate = 1;
                uncoveredTeams.Remove(team);
                GenerateMatches(team);
            }
        }
      

        private void GenerateMatches(Team team)
        {
            foreach (Team rivalTeam in uncoveredTeams)
            {
                Match nextMatch = CreateNextMatch(team, rivalTeam);
                generatedMatches.Add(nextMatch);
            }
        }
        

        private Match CreateNextMatch(Team local, Team visitor)
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

        private DateTime GetNextFreeWeekendDate(Team local, Team visitor)
        {
            bool dateIsOcupied = true;
            DateTime validDate = initialDate;
            while (dateIsOcupied)
            {
                DateTime date = initialDate.AddDays(lastFreeDate);
                if (IsWeekend(date) )
                {
                    if(UnoccupiedDateByTeams(date, local, visitor))
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

        private bool UnoccupiedDateByTeams(DateTime date, Team local, Team visitor)
        {
            return !generatedMatches.Exists(m => m.Date.Date.Equals(date.Date) && (IsInMatch(local, m)||IsInMatch(visitor,m)));
        }

        private bool IsInMatch(Team team, Match match)
        {
            return match.Local.Equals(team) || match.Visitor.Equals(team);
        }

        public string FixtureInfo()
        {
            return "Generates all versus all matches only on weekends, no rematch.";
        }
    }
}
