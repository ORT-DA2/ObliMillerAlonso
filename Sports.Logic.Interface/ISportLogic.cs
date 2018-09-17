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
        void UpdateSport(int id, Sport updatedSport);
        void RemoveSport(int id);
        ICollection<Sport> GetAll();




    }
}
