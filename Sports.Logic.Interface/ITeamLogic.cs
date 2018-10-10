using System;
using System.Collections.Generic;
using System.Text;
using Sports.Domain;
using Sports.Repository.Interface;

namespace Sports.Logic.Interface
{
    public interface ITeamLogic
    {
        void AddTeam(Team team);
        Team GetTeamById(int id);
        void SetPictureFromPath(int teamId, string testImagePath);
        void Modify(int id, Team team);
        void Delete(int id);
        ICollection<Team> GetAll();
        void SetSession(Guid token);
        ICollection<Team> GetFilteredTeams(string name, string order);
    }
}
