using System;
using System.Collections.Generic;
using System.Text;
using Sports.Domain;
using Sports.Repository.Interface;

namespace Sports.Logic.Interface
{
    public interface ICompetitorLogic
    {
        void AddCompetitor(Competitor competitor);
        Competitor GetCompetitorById(int id);
        void SetPictureFromPath(int competitorId, string testImagePath);
        void Modify(int id, Competitor competitor);
        void Delete(int id);
        ICollection<Competitor> GetAll();
        void SetSession(Guid token);
        ICollection<Competitor> GetFilteredCompetitors(string name, string order);
    }
}
