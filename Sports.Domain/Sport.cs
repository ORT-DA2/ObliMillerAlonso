using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Sports.Domain.Exceptions;
using Sports.Domain.Constants;


namespace Sports.Domain
{
    public class Sport
    {
        public int Id { get;  set; }
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
                throw new InvalidEmptyTextFieldException(EmptyField.EMPTY_NAME_MESSAGE);
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
                throw new TeamDoesNotExistInSportException(TeamValidation.TEAM_NOT_EXIST_IN_SPORT_MESSAGE);
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
                throw new TeamAlreadyExistException(UniqueTeam.DUPLICATE_TEAM_IN_SPORT_MESSAGE);
            }
        }

        public Team GetTeam(Team team)
        {
            CheckIfTeamDoesntExist(team);
            return Teams.First(t=>t.Name == team.Name|| t.Id == team.Id);
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
