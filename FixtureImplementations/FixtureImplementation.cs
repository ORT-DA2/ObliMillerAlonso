using System;
using System.Collections.Generic;
using System.Text;
using Sports.Domain;

namespace FixtureImplementations
{
    public class FixtureImplementation : IFixtureGeneratorStrategy
    {
        public ICollection<Match> GenerateFixture(ICollection<Sport> sports)
        {
            List<Match> generatedMatches = new List<Match>();
            foreach (Sport sport in sports)
            {
                BackAndForthFixture(sport, generatedMatches);
            }
            return generatedMatches;
        }

        private void BackAndForthFixture(Sport sport, List<Match> generatedMatches)
        {
            ICollection<Team> uncoveredTeams = sport.Teams;
            int amountOfTeams = sport.Teams.Count;
            int dayBonus = 1;
            foreach (Team team in sport.Teams)
            {
                int currentTeamDayBonus = dayBonus;
                uncoveredTeams.Remove(team);
                DateTime date = DateTime.Now.AddDays(dayBonus);
                foreach(Team visitor in uncoveredTeams)
                {
                    Match match = new Match()
                    {
                        Local = team,
                        Visitor = visitor,
                        Sport = sport,
                        Date = date,
                    };
                    generatedMatches.Add(match);
                    currentTeamDayBonus += amountOfTeams;
                }
            }
        }
    }
}
