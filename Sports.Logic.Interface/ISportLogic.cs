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
        void AddCompetitorToSport(int sportId, Competitor competitor);
        void DeleteCompetitorFromSport(int sportId, int competitorId);
        void ModifySport(int id, Sport sport);
        Competitor GetCompetitorFromSport(int sportId, int competitorId);
        ICollection<Competitor> GetCompetitorsFromSport(int sportId);
        void UpdateCompetitorSport(int id, int competitorId, Competitor competitorChanges);
        void SetSession(Guid token);
    }
}
