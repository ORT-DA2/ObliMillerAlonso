using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Sports.Domain;
using Sports.Repository.Interface;
using Sports.Logic.Interface;
using Sports.Logic.Exceptions;

namespace Sports.Logic
{
    public class TeamLogic : ITeamLogic
    {
        ITeamRepository _repository;

        public TeamLogic(IRepositoryUnitOfWork unitOfwork)
        {
            _repository = unitOfwork.Team;
        }
        public void AddTeam(Team team)
        {
            ValidateTeam(team);
            _repository.Create(team);
        }

        private void ValidateTeam(Team team)
        {
            CheckNotNull(team);
            team.IsValid();
        }

        private void CheckNotNull(Team team)
        {
            if (team == null)
            {
                throw new InvalidNullValueException("Cannot add null team");
            }
        }

        public Team GetTeamById(int id)
        {
            ICollection<Team> teams = _repository.FindByCondition(t => t.Id == id);
            if (teams.Count == 0)
            {
                throw new TeamDoesNotExistException("Id does not match any existing teams");
            }
            return teams.First();

        }

        public void SetPictureFromPath(Team team, string testImagePath)
        {
            Team realTeam = GetTeamById(team.Id);
            ValidateTeam(realTeam);
            realTeam.AddPictureFromPath(testImagePath);
            _repository.Update(realTeam);
        }

        public void Modify(int id, Team team)
        {
            Team realTeam = GetTeamById(id);
            realTeam.UpdateData(team);
            ValidateTeam(realTeam);
            _repository.Update(realTeam);
        }

        public void Delete(Team team)
        {
            Team realTeam = GetTeamById(team.Id);
            _repository.Delete(realTeam);
        }

        public ICollection<Team> GetAll()
        {
            return _repository.FindAll();
        }
    }
}
