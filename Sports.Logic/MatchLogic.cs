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

        IMatchRepository _repository;
        ISportLogic _sportLogic;
        ICommentLogic _commentLogic;

        public MatchLogic(IRepositoryUnitOfWork unit)
        {
            _repository = unit.Match;
            _sportLogic = new SportLogic(unit);
            _commentLogic = new CommentLogic(unit);
        }
        public void AddMatch(Match match)
        {
            ValidateMatch(match);
            _repository.Create(match);
        }

        private void ValidateMatch(Match match)
        {
            CheckNotNull(match);
            match.IsValid();
            ValidateSport(match);
        }

        private void ValidateSport(Match match)
        {
            Sport sport = match.Sport;

            Team local = match.Local;
            Team visitor = match.Visitor;
            match.Sport = _sportLogic.GetSportById(sport.Id);
            match.Local = _sportLogic.GetTeamFromSport(sport, local);
            match.Visitor = _sportLogic.GetTeamFromSport(sport, visitor);
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
            ICollection<Match> matches = _repository.FindByCondition(m => m.Id == id);
            if (matches.Count == 0)
            {
                throw new MatchDoesNotExistException(MatchId.MATCH_ID_NOT_EXIST_MESSAGE);
            }
            return matches.First();
        }

        public void ModifyMatch(int id, Match match)
        {
            Match realMatch = GetMatchById(id);
            realMatch.UpdateMatch(match);
            ValidateMatch(realMatch);
            _repository.Update(realMatch);
        }

        public void DeleteMatch(Match match)
        {
            Match realMatch = GetMatchById(match.Id);
            _repository.Delete(realMatch);
        }

        public ICollection<Match> GetAllMatches()
        {
            return _repository.FindAll();
        }

        public void AddCommentToMatch(int id, Comment comment)
        {
            _commentLogic.AddComment(comment);
            Match commentedMatch = GetMatchById(id);
            ValidateMatch(commentedMatch);
            commentedMatch.AddComment(comment);
            _repository.Update(commentedMatch);
        }

        public ICollection<Comment> GetAllComments(int id)
        {
            Match commentedMatch = GetMatchById(id);
            return commentedMatch.GetAllComments();
        }
    }
}
