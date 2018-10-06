﻿using System;
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
        ISessionLogic sessionLogic;
        IUserLogic userLogic;
        User user;

        public MatchLogic(IRepositoryUnitOfWork unit)
        {
            repository = unit.Match;
            sportLogic = new SportLogic(unit);
            teamLogic = new TeamLogic(unit);
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
        }

        private void CheckMatchDoesntExist(Match match)
        {
            ICollection<Match> matches = repository.FindByCondition(m => (IsInMatch(match.Local,m) || IsInMatch(match.Visitor,m))&&m.Date.DayOfYear==match.Date.DayOfYear);
            if (matches.Count != 0)
            {
                throw new MatchAlreadyExistsException("Team already plays that day.");
            }
        }


        private bool IsInMatch(Team team, Match match)
        {
            return match.Local.Equals(team) || match.Visitor.Equals(team);
        }

        private void ValidateSport(Match match)
        {
            
            Sport sport = match.Sport;
            Team local = match.Local;
            Team visitor = match.Visitor;
            match.Sport = sportLogic.GetSportById(sport.Id);
            match.Local = sportLogic.GetTeamFromSport(sport.Id, local.Id);
            match.Visitor = sportLogic.GetTeamFromSport(sport.Id, visitor.Id);
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


        public ICollection<Match> GetAllMatchesForTeam(Team team)
        {
            sessionLogic.ValidateUserNotNull(user);
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
            teamLogic.SetSession(token);
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
