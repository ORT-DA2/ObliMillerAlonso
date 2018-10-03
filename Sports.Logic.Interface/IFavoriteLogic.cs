using System;
using System.Collections.Generic;
using System.Text;
using Sports.Domain;

namespace Sports.Logic.Interface
{
    public interface IFavoriteLogic
    {
        void AddFavoriteTeam(Team team);
        ICollection<Team> GetFavoritesFromUser();
        ICollection<Comment> GetFavoritesTeamsComments();
        ICollection<Favorite> GetAll();
        void SetSession(Guid token);
    }
}
