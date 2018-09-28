using System;
using System.Collections.Generic;
using System.Text;
using Sports.Domain;

namespace FixtureImplementations
{
    public class FixtureBackAndForthWeekly : IFixtureGeneratorStrategy
    {
        public ICollection<Match> GenerateFixture(ICollection<Sport> sports)
        {
            List<Match> generatedMatches = new List<Match>();
            foreach (Sport sport in sports)
            {
                BackAndForthWeeklyMatch(sport, generatedMatches);
            }
            return generatedMatches;
        }

        private void BackAndForthWeeklyMatch(Sport sport, List<Match> generatedMatches)
        {
            ICollection<Team> uncoveredTeams = sport.Teams;
            int amountOfTeams = sport.Teams.Count;
            foreach (Team team in sport.Teams)
            {
                int currentTeamDayBonus = CalculateWeeklyDaySkip(uncoveredTeams);
                uncoveredTeams.Remove(team);
                foreach (Team visitor in uncoveredTeams)
                {
                    Match localMatch = CreateNextMatch(sport, currentTeamDayBonus);
                    localMatch.Local = team;
                    localMatch.Visitor = visitor;
                    currentTeamDayBonus = CreateNonDuplicatedMatch(generatedMatches, currentTeamDayBonus, localMatch);
                }
                foreach (Team local in uncoveredTeams)
                {
                    Match visitorMatch = CreateNextMatch(sport, currentTeamDayBonus);
                    visitorMatch.Local = local;
                    visitorMatch.Visitor = team;
                    currentTeamDayBonus = CreateNonDuplicatedMatch(generatedMatches, currentTeamDayBonus, visitorMatch);
                }
            }
        }

        private int CreateNonDuplicatedMatch(List<Match> generatedMatches, int currentTeamDayBonus, Match generatedMatch)
        {
            if (generatedMatches.FindAll(m => m.Local == generatedMatch.Local && m.Visitor == generatedMatch.Visitor).Count == 0)
            {
                generatedMatches.Add(generatedMatch);
                currentTeamDayBonus += currentTeamDayBonus;
            }

            return currentTeamDayBonus;
        }

        private Match CreateNextMatch(Sport sport, int currentTeamDayBonus)
        {
            return new Match()
            {
                Sport = sport,
                Date = DateTime.Now.AddDays(currentTeamDayBonus),
            };
        }

        private int CalculateWeeklyDaySkip(ICollection<Team> uncoveredTeams)
        {
            int currentTeamDayBonus = (uncoveredTeams.Count / 2) + 1;
            if (currentTeamDayBonus > 6)
            {
                currentTeamDayBonus = (uncoveredTeams.Count / 2) % 7;
            }

            return currentTeamDayBonus;
        }
    }
}
