using System;
using System.Collections.Generic;
using System.Text;
using Sports.Domain;

namespace Sports.Logic.Interface
{
    public interface IFavoriteLogic
    {
        void AddFavoriteTeam(User user, Team team);
        ICollection<Team> GetFavoritesFromUser(int id);
        ICollection<Comment> GetFavoritesTeamsComments(User user);
        ICollection<Favorite> GetAll();
    }
}
