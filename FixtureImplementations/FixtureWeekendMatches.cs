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
                Visitor = visitor
            };
            return nextMatch;
        }

        private DateTime GetNextFreeWeekendDate(Team local, Team visitor)
        {
            bool dateIsOcupied = true;
            DateTime date = DateTime.Now.AddDays(1);
            while (dateIsOcupied)
            {
                if (IsWeekend(date) && UnoccupiedDateByTeams(date, local, visitor))
                {
                    dateIsOcupied = false;
                }
                else
                {
                    date.AddDays(1);
                }
            }
            return date;
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
