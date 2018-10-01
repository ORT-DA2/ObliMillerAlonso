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
        IFavoriteRepository repository;
        IUserLogic userLogic;
        ITeamLogic teamLogic;
        IMatchLogic matchLogic;
        ISessionLogic sessionLogic;
        User user;

        public FavoriteLogic(IRepositoryUnitOfWork unitOfWork)
        {
            repository = unitOfWork.Favorite;
            userLogic = new UserLogic(unitOfWork);
            teamLogic = new TeamLogic(unitOfWork);
            matchLogic = new MatchLogic(unitOfWork);
            sessionLogic = new SessionLogic(unitOfWork);
        }
        
        
        public void AddFavoriteTeam(User user, Team team)
        {
            ValidateUser();
            Favorite favorite = new Favorite()
            {
                User = user,
                Team = team
            };
            ValidateNewFavorite(user, team, favorite);
            repository.Create(favorite);
            repository.Save();
        }

        private void ValidateUser()
        {
            ValidateUserNotNull();
            ValidateUserNotAdmin();
        }

        private void ValidateUserNotNull()
        {
            if (user == null)
            {
                throw new InvalidNullValueException(NullValue.INVALID_USER_NULL_VALUE_MESSAGE);
            }
        }

        private void ValidateUserNotAdmin()
        {
            if (!user.IsAdmin)
            {
                throw new NonAdminException(AdminException.NON_ADMIN_EXCEPTION_MESSAGE);
            }
        }

        private void ValidateNewFavorite(User user, Team team, Favorite favorite)
        {
            favorite.Validate();
            ValidateUserAndTeam(favorite);
            ValidateFavoriteDoesntExist(user, team);
        }

        private void ValidateUserAndTeam(Favorite favorite)
        {
            favorite.User = userLogic.GetUserById(favorite.User.Id);
            favorite.Team = teamLogic.GetTeamById(favorite.Team.Id);
        }

        private void ValidateFavoriteDoesntExist(User user, Team team)
        {
            ICollection<Favorite> favorites =repository.FindByCondition(f => f.Team.Equals(team) && f.User.Equals(user));
            if (favorites.Count != 0)
            {
                throw new FavoriteAlreadyExistException(UniqueFavorite.UNIQUE_FAVORITE_MESSAGE);
            }
        }

        public ICollection<Team> GetFavoritesFromUser(int id)
        {
            ValidateUserNotNull();
            ICollection<Favorite> favorites = repository.FindByCondition(f => f.User.Id == id);
            ValidateFavoritesExist(favorites);
            ICollection<Team> teams = GetTeamsFromFavorites(favorites);
            return teams;
        }

        private static void ValidateFavoritesExist(ICollection<Favorite> favorites)
        {
            if (favorites.Count == 0)
            {
                throw new FavoriteDoesNotExistException(FavoriteNotFound.FAVORITE_NOT_FOUND_MESSAGE);
            }
        }

        private static ICollection<Team> GetTeamsFromFavorites(ICollection<Favorite> favorites)
        {
            ICollection<Team> teams = new List<Team>();
            foreach (Favorite favorite in favorites)
            {
                teams.Add(favorite.Team);
            }
            return teams;
        }

        public ICollection<Comment> GetFavoritesTeamsComments(User user)
        {
            ValidateUserNotNull();
            ICollection<Team> favoriteTeams = GetFavoritesFromUser(user.Id);
            ICollection<Match> favoriteMatches = GetMatchesForTeams(favoriteTeams);
            List<Comment> favoriteComments = new List<Comment>();
            foreach (Match match in favoriteMatches)
            {
                favoriteComments.AddRange(match.Comments);
            }
            return favoriteComments;
        }

        private ICollection<Match> GetMatchesForTeams(ICollection<Team> favoriteTeams)
        {
            List<Match> favoriteMatches = new List<Match>();
            foreach (Team favoriteTeam in favoriteTeams)
            {
                ICollection<Match> favoriteTeamMatches = matchLogic.GetAllMatchesForTeam(favoriteTeam);
                favoriteMatches.AddRange(favoriteTeamMatches);
            }
            favoriteMatches.OrderBy(m => m.Date).ToList();
            return favoriteMatches;
        }

        public ICollection<Favorite> GetAll()
        {
            ValidateUserNotNull();
            return repository.FindAll();
        }

        public void SetSession(Guid token)
        {
            user = sessionLogic.GetUserFromToken(token);
            teamLogic.SetSession(token);
            matchLogic.SetSession(token);
        }
    }
}
