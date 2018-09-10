using System;
using System.Collections.Generic;
using System.Text;
using Sports.Exceptions;

namespace Sports.Domain
{
    public class Sport
    {
        public int Id { get; private set; }
        public string Name { get; set; }
        public ICollection<Team> Teams { get; set; }

        public Sport()
        {
            Teams = new List<Team>();
        }

        public void IsValid()
        {
            IsValidSportName();
        }

        private void IsValidSportName()
        {
            if (string.IsNullOrWhiteSpace(Name))
                throw new InvalidSportDataException("Invalid Name");
        }

        public void RemoveTeam(Team team)
        {
            CheckIfTeamExists(team);
            Teams.Remove(team);
        }

        private void CheckIfTeamExists(Team team)
        {
            if (!Teams.Contains(team))
            {
                throw new InvalidSportDataException("Team does not exist in Sport");
            }
        }

    }
}
