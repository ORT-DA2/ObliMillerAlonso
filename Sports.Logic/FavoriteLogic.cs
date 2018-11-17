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
        ICompetitorLogic competitorLogic;
        IMatchLogic matchLogic;
        ISessionLogic sessionLogic;
        User sessionUser;

        public FavoriteLogic(IRepositoryUnitOfWork unitOfWork)
        {
            repository = unitOfWork.Favorite;
            userLogic = new UserLogic(unitOfWork);
            competitorLogic = new CompetitorLogic(unitOfWork);
            matchLogic = new MatchLogic(unitOfWork);
            sessionLogic = new SessionLogic(unitOfWork);
        }
        
        
        public void AddFavoriteCompetitor(Competitor competitor)
        {
            sessionLogic.ValidateUserNotNull(sessionUser);
            Favorite favorite = new Favorite()
            {
                User = sessionUser,
                Competitor = competitor
            };
            ValidateNewFavorite(sessionUser, competitor, favorite);
            repository.Create(favorite);
            repository.Save();
        }

        public void DeleteFavorite(int competitorId)
        {
            sessionLogic.ValidateUserNotNull(sessionUser);
            Favorite favorite = repository.FindByCondition(f => f.Competitor.Id == competitorId 
            && f.User.Id == sessionUser.Id).First();
            repository.Delete(favorite);
            repository.Save();
        }

        private void ValidateNewFavorite(User user, Competitor competitor, Favorite favorite)
        {
            favorite.Validate();
            ValidateUserAndCompetitor(favorite);
            ValidateFavoriteDoesntExist(user, competitor);
        }

        private void ValidateUserAndCompetitor(Favorite favorite)
        {
            favorite.User = userLogic.GetUserById(favorite.User.Id);
            favorite.Competitor = competitorLogic.GetCompetitorById(favorite.Competitor.Id);
        }

        private void ValidateFavoriteDoesntExist(User user, Competitor competitor)
        {
            ICollection<Favorite> favorites =repository.FindByCondition(f => f.Competitor.Equals(competitor) && f.User.Equals(user));
            if (favorites.Count != 0)
            {
                throw new FavoriteAlreadyExistException(UniqueFavorite.UNIQUE_FAVORITE_MESSAGE);
            }
        }

        public ICollection<Competitor> GetFavoritesFromUser()
        {
            sessionLogic.ValidateUserNotNull(sessionUser);
            ICollection<Favorite> favorites = repository.FindByCondition(f => f.User.Id == sessionUser.Id);
            ValidateFavoritesExist(favorites);
            ICollection<Competitor> competitors = GetCompetitorsFromFavorites(favorites);
            return competitors;
        }

        private static void ValidateFavoritesExist(ICollection<Favorite> favorites)
        {
            if (favorites.Count == 0)
            {
                throw new FavoriteDoesNotExistException(FavoriteNotFound.FAVORITE_NOT_FOUND_MESSAGE);
            }
        }

        private static ICollection<Competitor> GetCompetitorsFromFavorites(ICollection<Favorite> favorites)
        {
            ICollection<Competitor> competitors = new List<Competitor>();
            foreach (Favorite favorite in favorites)
            {
                competitors.Add(favorite.Competitor);
            }
            return competitors;
        }

        public ICollection<Comment> GetFavoritesCompetitorsComments()
        {
            sessionLogic.ValidateUserNotNull(sessionUser);
            ICollection<Competitor> favoriteCompetitors = this.GetFavoritesFromUser();
            ICollection<Match> favoriteMatches = GetMatchesForCompetitors(favoriteCompetitors);
            List<Comment> favoriteComments = new List<Comment>();
            foreach (Match match in favoriteMatches)
            {
                favoriteComments.AddRange(match.Comments);
            }
            return favoriteComments;
        }

        private ICollection<Match> GetMatchesForCompetitors(ICollection<Competitor> favoriteCompetitors)
        {
            List<Match> favoriteMatches = new List<Match>();
            foreach (Competitor favoriteCompetitor in favoriteCompetitors)
            {
                ICollection<Match> favoriteCompetitorMatches = matchLogic.GetAllMatchesForCompetitor(favoriteCompetitor);
                favoriteMatches.AddRange(favoriteCompetitorMatches);
            }
            return favoriteMatches.OrderBy(m => m.Date).Distinct().ToList();
        }

        public ICollection<Favorite> GetAll()
        {
            sessionLogic.ValidateUserNotNull(sessionUser);
            return repository.FindAll();
        }

        public void SetSession(Guid token)
        {
            sessionUser = sessionLogic.GetUserFromToken(token);
            competitorLogic.SetSession(token);
            matchLogic.SetSession(token);
            userLogic.SetSession(token);
        }

    }
}
