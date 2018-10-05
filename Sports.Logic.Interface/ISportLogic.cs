using System;
using System.Collections.Generic;
using System.Text;
using Sports.Domain;

namespace Sports.Logic.Interface
{
    public interface ISportLogic
    {
        void AddSport(Sport sport);
        Sport GetSportById(int id);
        Sport GetSportByName(string name);
        void RemoveSport(int id);
        ICollection<Sport> GetAll();
        void AddTeamToSport(int sportId, Team team);
        void DeleteTeamFromSport(int sportId, int teamId);
        void ModifySport(int id, Sport sport);
        Team GetTeamFromSport(int sportId, int teamId);
        ICollection<Team> GetTeamsFromSport(int sportId);
        void UpdateTeamSport(int id, int teamId, Team teamChanges);
        void SetSession(Guid token);
    }
}
