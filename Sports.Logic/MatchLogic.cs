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
        ITeamLogic teamLogic;

        public MatchLogic(IRepositoryUnitOfWork unit)
        {
            repository = unit.Match;
            sportLogic = new SportLogic(unit);
            teamLogic = new TeamLogic(unit);
            commentLogic = new CommentLogic(unit);
        }
        public void AddMatch(Match match)
        {
            ValidateMatch(match);
            repository.Create(match);
            repository.Save();
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
            match.Sport = sportLogic.GetSportById(sport.Id);
            match.Local = sportLogic.GetTeamFromSport(sport, local);
            match.Visitor = sportLogic.GetTeamFromSport(sport, visitor);
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
            ICollection<Match> matches = repository.FindByCondition(m => m.Id == id);
            if (matches.Count == 0)
            {
                throw new MatchDoesNotExistException(MatchId.MATCH_ID_NOT_EXIST_MESSAGE);
            }
            return matches.First();
        }


        public ICollection<Match> GetAllMatchesForTeam(Team team)
        {
            Team playingTeam = teamLogic.GetTeamById(team.Id);
            ICollection<Match> matches = repository.FindByCondition(m => m.Local.Equals(playingTeam) ||m.Visitor.Equals(playingTeam));
            if (matches.Count == 0)
            {
                throw new MatchDoesNotExistException("Team has no matches");
            }
            return matches;
        }

        public void ModifyMatch(int id, Match match)
        {
            Match realMatch = GetMatchById(id);
            realMatch.UpdateMatch(match);
            ValidateMatch(realMatch);
            repository.Update(realMatch);
            repository.Save();
        }

        public void DeleteMatch(Match match)
        {
            Match realMatch = GetMatchById(match.Id);
            repository.Delete(realMatch);
            repository.Save();
        }

        public ICollection<Match> GetAllMatches()
        {
            return repository.FindAll();
        }

        public void AddCommentToMatch(int id, Comment comment)
        {
            commentLogic.AddComment(comment);
            Match commentedMatch = GetMatchById(id);
            ValidateMatch(commentedMatch);
            commentedMatch.AddComment(comment);
            repository.Update(commentedMatch);
            repository.Save();
        }

        public ICollection<Comment> GetAllComments(int id)
        {
            Match commentedMatch = GetMatchById(id);
            return commentedMatch.GetAllComments();
        }
    }
}
