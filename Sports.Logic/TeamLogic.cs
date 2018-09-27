using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Sports.Domain;
using Sports.Repository.Interface;
using Sports.Logic.Interface;
using Sports.Logic.Exceptions;
using Sports.Logic.Constants;

namespace Sports.Logic
{
    public class TeamLogic : ITeamLogic
    {
        ITeamRepository repository;

        public TeamLogic(IRepositoryUnitOfWork unitOfwork)
        {
            repository = unitOfwork.Team;
        }
        public void AddTeam(Team team)
        {
            ValidateTeam(team);
            repository.Create(team);
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
                throw new InvalidNullValueException(NullValue.INVALID_TEAM_NULL_VALUE_MESSAGE);
            }
        }

        public Team GetTeamById(int id)
        {
            ICollection<Team> teams = repository.FindByCondition(t => t.Id == id);
            if (teams.Count == 0)
            {
                throw new TeamDoesNotExistException(TeamNotFound.TEAM_ID_NOT_FOUND_MESSAGE);
            }
            return teams.First();

        }

        public void SetPictureFromPath(Team team, string testImagePath)
        {
            Team realTeam = GetTeamById(team.Id);
            ValidateTeam(realTeam);
            realTeam.AddPictureFromPath(testImagePath);
            repository.Update(realTeam);
        }

        public void Modify(int id, Team team)
        {
            Team realTeam = GetTeamById(id);
            realTeam.UpdateData(team);
            ValidateTeam(realTeam);
            repository.Update(realTeam);
        }

        public void Delete(Team team)
        {
            Team realTeam = GetTeamById(team.Id);
            repository.Delete(realTeam);
        }

        public ICollection<Team> GetAll()
        {
            return repository.FindAll();
        }
    }
}
