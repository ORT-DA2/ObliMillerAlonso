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
        public ICollection<Match> GenerateFixture(ICollection<Sport> sports)
        {
            generatedMatches = new List<Match>();
            foreach (Sport sport in sports.ToList())
            {
                currentSport = sport;
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
            DateTime validDate = DateTime.Now;
            while (dateIsOcupied)
            {
                DateTime date = DateTime.Now.AddDays(lastFreeDate);
                if (IsWeekend(date) && UnoccupiedDateByTeams(date, local, visitor))
                {
                    dateIsOcupied = false;
                    validDate = date;
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

        private static bool IsInMatch(Team team, Match match)
        {
            return match.Local.Equals(team) || match.Visitor.Equals(team);
        }
    }
}
