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
    public class MatchLogic : IMatchLogic
    {
        IMatchRepository repository;
        ISportLogic sportLogic;
        ICommentLogic commentLogic;
        ICompetitorLogic competitorLogic;
        ISessionLogic sessionLogic;
        IUserLogic userLogic;
        User user;

        public MatchLogic(IRepositoryUnitOfWork unit)
        {
            repository = unit.Match;
            sportLogic = new SportLogic(unit);
            competitorLogic = new CompetitorLogic(unit);
            commentLogic = new CommentLogic(unit);
            sessionLogic = new SessionLogic(unit);
            userLogic = new UserLogic(unit);
        }
        public void AddMatch(Match match)
        {
            sessionLogic.ValidateUser(user);
            ValidateMatch(match);
            CheckMatchDoesntExist(match);
            repository.Create(match);
            repository.Save();
        }

        private void ValidateMatch(Match match)
        {
            CheckNotNull(match);
            match.IsValid();
            ValidateSport(match);
            match.IsValidMatch();
        }
        
        private void CheckMatchDoesntExist(Match match)
        {
            ICollection<Match> matches = repository.FindByCondition(m => ((match.Local.Id.Equals(m.Local.Id) || match.Visitor.Id.Equals(m.Local.Id)) 
            || (match.Local.Id.Equals(m.Visitor.Id) || match.Visitor.Id.Equals(m.Visitor.Id)))
            && m.Date.Date.Equals(match.Date.Date));
            if (matches.Count != 0)
            {
                throw new MatchAlreadyExistsException(MatchValidation.COMPETITOR_ALREADY_PLAYING);
            }
        }

        private void ValidateSport(Match match)
        {
            
            Sport sport = match.Sport;
            Competitor local = match.Local;
            Competitor visitor = match.Visitor;
            match.Sport = sportLogic.GetSportById(sport.Id);
            match.Local = sportLogic.GetCompetitorFromSport(sport.Id, local.Id);
            match.Visitor = sportLogic.GetCompetitorFromSport(sport.Id, visitor.Id);
        }

        private void CheckNotNull(Match match)
        {
            if (match == null)
            {
                throw new InvalidNullValueException(NullValue.INVALID_MATCH_NULL_VALUE_MESSAGE);
            }
        }

        public Match GetMatchById(int id)
        {
            sessionLogic.ValidateUserNotNull(user);
            ICollection<Match> matches = repository.FindByCondition(m => m.Id == id);
            if (matches.Count == 0)
            {
                throw new MatchDoesNotExistException(MatchId.MATCH_ID_NOT_EXIST_MESSAGE);
            }
            return matches.First();
        }


        public ICollection<Match> GetAllMatchesForCompetitor(Competitor competitor)
        {
            sessionLogic.ValidateUserNotNull(user);
            Competitor playingCompetitor = competitorLogic.GetCompetitorById(competitor.Id);
            ICollection<Match> matches = repository.FindByCondition(m => m.Local.Equals(playingCompetitor) ||m.Visitor.Equals(playingCompetitor));
            if (matches.Count == 0)
            {
                throw new MatchDoesNotExistException(MatchValidation.COMPETITOR_DOESNT_PLAY);
            }
            return matches;
        }

        public void ModifyMatch(int id, Match match)
        {
            sessionLogic.ValidateUser(user);
            Match realMatch = GetMatchById(id);
            realMatch.UpdateMatch(match);
            ValidateMatch(realMatch);
            repository.Update(realMatch);
            repository.Save();
        }

        public void DeleteMatch(int matchId)
        {
            sessionLogic.ValidateUser(user);
            Match realMatch = GetMatchById(matchId);
            repository.Delete(realMatch);
            repository.Save();
        }

        public ICollection<Match> GetAllMatches()
        {
            sessionLogic.ValidateUserNotNull(user);
            return repository.FindAll();
        }

        public void AddCommentToMatch(int id, Comment comment)
        {
            sessionLogic.ValidateUserNotNull(user);
            comment.User = user;
            commentLogic.AddComment(comment);
            Match commentedMatch = GetMatchById(id);
            ValidateMatch(commentedMatch);
            commentedMatch.AddComment(comment);
            repository.Update(commentedMatch);
            repository.Save();
        }

        public ICollection<Comment> GetAllComments(int id)
        {
            sessionLogic.ValidateUserNotNull(user);
            Match commentedMatch = GetMatchById(id);
            return commentedMatch.GetAllComments();
        }

        public void SetSession(Guid token)
        {
            user = sessionLogic.GetUserFromToken(token);
            sportLogic.SetSession(token);
            commentLogic.SetSession(token);
            competitorLogic.SetSession(token);
        }

        public void AddMatches(ICollection<Match> matches)
        {
            foreach(Match match in matches)
            {
                DateTime date = match.Date.Date;
                AddMatch(match);
            }
        }
    }
}
