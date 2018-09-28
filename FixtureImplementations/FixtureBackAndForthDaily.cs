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
        private List<Team> uncoveredTeams;
        private Sport currentSport;
        private int daysToAddToDate;
        public ICollection<Match> GenerateFixture(ICollection<Sport> sports)
        {
            generatedMatches = new List<Match>();
            foreach (Sport sport in sports.ToList())
            {
                currentSport = sport;
                BackAndForthWeeklyMatch();
            }
            return generatedMatches;
        }

        private void BackAndForthWeeklyMatch()
        {
            uncoveredTeams = currentSport.Teams.ToList();
            daysToAddToDate = 1;
            foreach (Team team in currentSport.Teams.ToList())
            {
                uncoveredTeams.Remove(team);
                GenerateLocalMatches(team);
            }
            uncoveredTeams = currentSport.Teams.ToList();
            foreach (Team team in currentSport.Teams.ToList())
            {
                uncoveredTeams.Remove(team);
                GenerateVisitorMatches(team);
            }
        }

        private void GenerateVisitorMatches(Team team)
        {
            foreach (Team local in uncoveredTeams)
            {
                Match visitorMatch = CreateNextMatch();
                visitorMatch.Local = local;
                visitorMatch.Visitor = team;
                generatedMatches.Add(visitorMatch);
            }
        }

        private void GenerateLocalMatches(Team team)
        {
            foreach (Team visitor in uncoveredTeams)
            {
                Match localMatch = CreateNextMatch();
                localMatch.Local = team;
                localMatch.Visitor = visitor;
                generatedMatches.Add(localMatch);
            }
        }
        

        private Match CreateNextMatch()
        {
            Match nextMatch = new Match()
            {
                Sport = currentSport,
                Date = DateTime.Now.AddDays(daysToAddToDate),
            };
            daysToAddToDate++;
            return nextMatch;
        }
        
    }
}
