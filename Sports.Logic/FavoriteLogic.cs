using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Sports.Domain;
using Sports.Repository.Interface;
using Sports.Logic.Interface;
using Sports.Logic.Exceptions;
using Sports.Logic.Constants;

namespace Sports.Logic
{
    public class FavoriteLogic : IFavoriteLogic
    {
        IFavoriteRepository _repository;
        IUserLogic _userLogic;
        ITeamLogic _teamLogic;

        public FavoriteLogic(IRepositoryUnitOfWork unitOfWork)
        {
            _repository = unitOfWork.Favorite;
            _userLogic = new UserLogic(unitOfWork);
            _teamLogic = new TeamLogic(unitOfWork);
        }
        
        
        public void AddFavoriteTeam(User user, Team team)
        {
            Favorite favorite = new Favorite()
            {
                User = _userLogic.GetUserById(user.Id),
                Team = _teamLogic.GetTeamById(team.Id)
            };
            favorite.Validate();
            ValidateFavoriteDoesntExist(user, team);
            _repository.Create(favorite);
            _repository.Save();
        }
        

        private void ValidateFavoriteDoesntExist(User user, Team team)
        {
            ICollection<Favorite> favorites =_repository.FindByCondition(f => f.Team.Equals(team) && f.User.Equals(user));
            if (favorites.Count != 0)
            {
                throw new FavoriteAlreadyExistException(UniqueFavorite.UNIQUE_FAVORITE_MESSAGE);
            }
        }

        public ICollection<Favorite> GetFavoritesFromUser(int id)
        {
            ICollection<Favorite> favorites = _repository.FindByCondition(f => f.User.Id == id);
            if (favorites.Count == 0)
            {
                throw new FavoriteDoesNotExistException(FavoriteNotFound.FAVORITE_NOT_FOUND_MESSAGE);
            }
            return favorites;
        }
        
        
    }
}
