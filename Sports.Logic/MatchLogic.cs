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
            user = new User()
            {
                FirstName = "Itai",
                LastName = "Miller",
                Email = "itaimiller@gmail.com",
                UserName = "iMiller",
                Password = "root"
            };
        }
        public void AddMatch(Match match)
        {
            ValidateUser();
            ValidateMatch(match);
            repository.Create(match);
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
            ValidateUserNotNull();
            ICollection<Match> matches = repository.FindByCondition(m => m.Id == id);
            if (matches.Count == 0)
            {
                throw new MatchDoesNotExistException(MatchId.MATCH_ID_NOT_EXIST_MESSAGE);
            }
            return matches.First();
        }


        public ICollection<Match> GetAllMatchesForTeam(Team team)
        {
            ValidateUserNotNull();
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
            ValidateUser();
            Match realMatch = GetMatchById(id);
            realMatch.UpdateMatch(match);
            ValidateMatch(realMatch);
            repository.Update(realMatch);
            repository.Save();
        }

        public void DeleteMatch(Match match)
        {
            ValidateUser();
            Match realMatch = GetMatchById(match.Id);
            repository.Delete(realMatch);
            repository.Save();
        }

        public ICollection<Match> GetAllMatches()
        {
            ValidateUserNotNull();
            return repository.FindAll();
        }

        public void AddCommentToMatch(int id, Comment comment)
        {
            ValidateUserNotNull();
            commentLogic.AddComment(comment);
            Match commentedMatch = GetMatchById(id);
            ValidateMatch(commentedMatch);
            commentedMatch.AddComment(comment);
            repository.Update(commentedMatch);
            repository.Save();
        }

        public ICollection<Comment> GetAllComments(int id)
        {
            ValidateUserNotNull();
            Match commentedMatch = GetMatchById(id);
            return commentedMatch.GetAllComments();
        }

        public void SetSession(Guid token)
        {
            User user = sessionLogic.GetUserFromToken(token);
        }
    }
}
