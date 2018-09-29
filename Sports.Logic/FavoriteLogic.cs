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

        public FavoriteLogic(IRepositoryUnitOfWork unitOfWork)
        {
            repository = unitOfWork.Favorite;
            userLogic = new UserLogic(unitOfWork);
            teamLogic = new TeamLogic(unitOfWork);
            matchLogic = new MatchLogic(unitOfWork);
        }
        
        
        public void AddFavoriteTeam(User user, Team team)
        {
            Favorite favorite = new Favorite()
            {
                User = user,
                Team = team
            };
            ValidateNewFavorite(user, team, favorite);
            repository.Create(favorite);
            repository.Save();
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
            return repository.FindAll();
        }
    }
}
