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
        void UpdateSport(int id, Sport updatedSport);
        void RemoveSport(int id);
        ICollection<Sport> GetAll();
        void AddTeamToSport(Sport sport, Team team);
        void DeleteTeamFromSport(Sport sport, Team team);
        void Modify(Sport sport);
        Team GetTeamFromSport(Sport sport, Team team);





    }
}
