using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Sports.Domain;
using Sports.Exceptions;
using Sports.Repository.Interface;
using Sports.Logic.Interface;

namespace Sports.Logic
{
    public class TeamLogic : ITeamLogic
    {
        ITeamRepository _repository;

        public TeamLogic(IRepositoryWrapper wrapper)
        {
            _repository = wrapper.Team;
        }
        public void AddTeam(Team team)
        {
            ValidateTeam(team);
            _repository.Create(team);
        }

        private void ValidateTeam(Team team)
        {
            CheckNotNull(team);
        }

        private void CheckNotNull(Team team)
        {
            if (team == null)
            {
                throw new InvalidTeamDataException("Cannot add null team");
            }
        }

        public Team GetTeamById(int id)
        {
            ICollection<Team> teams = _repository.FindByCondition(t => t.Id == id);
            return teams.First();

        }
    }
}
