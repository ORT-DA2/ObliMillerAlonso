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
        ISessionLogic sessionLogic;
        User user;

        public TeamLogic(IRepositoryUnitOfWork unitOfwork)
        {
            repository = unitOfwork.Team;
            sessionLogic = new SessionLogic(unitOfwork);
        }
        public void AddTeam(Team team)
        {
            sessionLogic.ValidateUser(user);
            ValidateTeam(team);
            repository.Create(team);
            repository.Save();
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
            sessionLogic.ValidateUserNotNull(user);
            ICollection<Team> teams = repository.FindByCondition(t => t.Id == id);
            if (teams.Count == 0)
            {
                throw new TeamDoesNotExistException(TeamNotFound.TEAM_ID_NOT_FOUND_MESSAGE);
            }
            return teams.First();

        }

        public void SetPictureFromPath(Team team, string testImagePath)
        {
            sessionLogic.ValidateUserNotNull(user);
            Team realTeam = GetTeamById(team.Id);
            ValidateTeam(realTeam);
            realTeam.AddPictureFromPath(testImagePath);
            repository.Update(realTeam);
            repository.Save();
        }

        public void Modify(int id, Team team)
        {
            sessionLogic.ValidateUser(user);
            Team realTeam = GetTeamById(id);
            realTeam.UpdateData(team);
            ValidateTeam(realTeam);
            repository.Update(realTeam);
        }

        public void Delete(Team team)
        {
            sessionLogic.ValidateUser(user);
            Team realTeam = GetTeamById(team.Id);
            repository.Delete(realTeam);
            repository.Save();
        }

        public ICollection<Team> GetAll()
        {
            sessionLogic.ValidateUserNotNull(user);
            return repository.FindAll();
        }

        public void SetSession(Guid token)
        {
            user = sessionLogic.GetUserFromToken(token);
        }

        public ICollection<Team> FilterOrderTeamName(string name, string order = "asc")
        {
            sessionLogic.ValidateUser(user);
            ICollection<Team> teams;
            if (String.IsNullOrWhiteSpace(name))
            {
                teams = repository.FindAll();
            }
            else
            {
                teams = repository.FindByCondition(t => t.Name.Equals(name));
            }
            if (order.Equals("asc"))
            {
                teams.OrderBy(t => t.Name);
            }
            else if(order.Equals("desc"))
            {
                teams.OrderByDescending(t => t.Name);
            }
            return teams;
        }
    }
}
