using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Sports.Domain;
using Sports.Exceptions;
using Sports.Repository.Interface;
namespace Sports.Logic
{
    public class TeamLogic
    {
        ITeamRepository _repository;

        public TeamLogic(IRepositoryWrapper wrapper)
        {
            _repository = wrapper.Team;
        }
        public void AddTeam(Team team)
        {
            CheckNotNull(team);
            _repository.Create(team);
        }
        
        private void CheckNotNull(Team team)
        {
            if (team == null)
            {
                throw new InvalidTeamDataException("Cannot add null user");
            }
        }

        public Team GetTeamById(int id)
        {
            ICollection<Team> teams = _repository.FindByCondition(t => t.Id == id);
            return teams.First();

        }
    }
}
