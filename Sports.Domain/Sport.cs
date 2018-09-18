using System;
using System.Collections.Generic;
using System.Text;
using Sports.Exceptions;
using System.Linq;

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

        public override bool Equals(object obj)
        {
            if (obj == null || !obj.GetType().Equals(this.GetType()))
            {
                return false;
            }
            else
            {
                return this.Name.Equals(((Sport)obj).Name);
            }
        }

        public override string ToString()
        {
            return this.Name;
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
            CheckIfTeamDoesntExist(team);
            Teams.Remove(team);
        }

        private void CheckIfTeamDoesntExist(Team team)
        {
            if (!Teams.Contains(team))
            {
                throw new InvalidSportDataException("Team does not exist in Sport");
            }
        }

        public void AddTeam(Team team)
        {
            CheckDuplicateTeam(team);
            Teams.Add(team);
        }

        public void DeleteTeam(Team team)
        {
            CheckIfTeamDoesntExist(team);
            Teams.Remove(team);
        }

        private void CheckDuplicateTeam(Team team)
        {
            if (Teams.Contains(team))
            {
                throw new InvalidSportDataException("Team already exists in Sport");
            }
        }

        public Team GetTeam(Team team)
        {
            CheckIfTeamDoesntExist(team);
            return Teams.First(team);
        }

        public void UpdateData(Sport sport)
        {
            this.Name = IgnoreWhiteSpace(this.Name, sport.Name);
        }

        private string IgnoreWhiteSpace(string originalText, string updatedText)
        {
            if (string.IsNullOrWhiteSpace(updatedText))
            {
                return originalText;
            }
            return updatedText;
        }


    }
}
