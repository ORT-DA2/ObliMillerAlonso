using System;
using System.Collections.Generic;
using System.Text;
using Sports.Domain;

namespace Sports.Logic.Interface
{
    public interface IFavoriteLogic
    {
        void AddFavoriteCompetitor(Competitor competitor);
        ICollection<Competitor> GetFavoritesFromUser();
        ICollection<Comment> GetFavoritesCompetitorsComments();
        ICollection<Favorite> GetAll();
        void SetSession(Guid token);
        void DeleteFavorite(int id);
    }
}
