using System;
using System.Collections.Generic;
using System.Text;
using Sports.Domain;
using System.Linq;

namespace FixtureImplementations
{
    public class FixtureBackAndForthWeekly : IFixtureGeneratorStrategy
    {
        private List<Match> generatedMatches;
        private List<Team> uncoveredTeams;
        private Sport currentSport;
        public ICollection<Match> GenerateFixture(ICollection<Sport> sports)
        {
            generatedMatches = new List<Match>();
            foreach (Sport sport in sports.ToList())
            {
                currentSport = sport;
                uncoveredTeams = currentSport.Teams.ToList();
                BackAndForthWeeklyMatch();
            }
            return generatedMatches;
        }

        private void BackAndForthWeeklyMatch()
        {
            int amountOfTeams = uncoveredTeams.Count();
            foreach (Team team in currentSport.Teams.ToList())
            {
                int currentTeamDayBonus = CalculateWeeklyDaySkip(amountOfTeams);
                uncoveredTeams.Remove(team);
                currentTeamDayBonus = GenerateLocalMatches(team, currentTeamDayBonus);
                currentTeamDayBonus = GenerateVisitorMatches(team, currentTeamDayBonus);
            }
        }

        private int GenerateVisitorMatches(Team team, int currentTeamDayBonus)
        {
            foreach (Team local in uncoveredTeams)
            {
                Match visitorMatch = CreateNextMatch(currentTeamDayBonus);
                visitorMatch.Local = local;
                visitorMatch.Visitor = team;
                currentTeamDayBonus = CreateNonDuplicatedMatch(currentTeamDayBonus, visitorMatch);
            }

            return currentTeamDayBonus;
        }

        private int GenerateLocalMatches(Team team, int currentTeamDayBonus)
        {
            foreach (Team visitor in uncoveredTeams)
            {
                Match localMatch = CreateNextMatch(currentTeamDayBonus);
                localMatch.Local = team;
                localMatch.Visitor = visitor;
                currentTeamDayBonus = CreateNonDuplicatedMatch(currentTeamDayBonus, localMatch);
            }

            return currentTeamDayBonus;
        }

        private int CreateNonDuplicatedMatch(int currentTeamDayBonus, Match generatedMatch)
        {
            if (generatedMatches.FindAll(m => m.Local == generatedMatch.Local && m.Visitor == generatedMatch.Visitor).Count == 0)
            {
                generatedMatches.Add(generatedMatch);
                currentTeamDayBonus += currentTeamDayBonus;
            }

            return currentTeamDayBonus;
        }

        private Match CreateNextMatch(int currentTeamDayBonus)
        {
            return new Match()
            {
                Sport = currentSport,
                Date = DateTime.Now.AddDays(currentTeamDayBonus),
            };
        }

        private int CalculateWeeklyDaySkip(int amountOfTeams)
        {
            int currentTeamDayBonus = (amountOfTeams / 2) + 1;
            if (currentTeamDayBonus > 6)
            {
                currentTeamDayBonus = (amountOfTeams / 2) % 7;
            }

            return currentTeamDayBonus;
        }
    }
}
