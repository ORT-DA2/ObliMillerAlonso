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


        /*
        private void ValidateFavorite(Favorite favorite)
        {
            CheckUserNotNull(favorite);
            CheckTeamNotNull(favorite);
        }

        private void CheckUserNotNull(Favorite favorite)
        {
            if (favorite.User == null)
            {
                throw new InvalidNullValueException(NullValue.INVALID_USER_NULL_VALUE_MESSAGE);
            }
        }

        private void CheckTeamNotNull(Favorite favorite)
        {
            if (favorite.Team == null)
            {
                throw new InvalidNullValueException(NullValue.INVALID_USER_NULL_VALUE_MESSAGE);
            }
        }
        */
    }
}
