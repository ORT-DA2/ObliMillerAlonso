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
        void AddTeamToSport(Sport sport, Team team);
        void DeleteTeamFromSport(Sport sport, Team team);
        void ModifySport(int id, Sport sport);
        Team GetTeamFromSport(Sport sport, Team team);
        void UpdateTeamSport(int id, Team originalTeam, Team teamChanges);





    }
}
