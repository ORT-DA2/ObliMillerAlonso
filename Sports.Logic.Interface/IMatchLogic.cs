using System;
using System.Collections.Generic;
using System.Text;
using Sports.Domain;

namespace Sports.Logic.Interface
{
    public interface IMatchLogic
    {
        void AddMatch(Match match);
        void AddMatches(ICollection<Match> matches);
        Match GetMatchById(int id);
        void ModifyMatch(int id, Match match);
        void DeleteMatch(int id);
        ICollection<Match> GetAllMatches();
        ICollection<Match> GetAllMatchesForCompetitor(Competitor competitor);
        void AddCommentToMatch(int id, Comment comment);
        ICollection<Comment> GetAllComments(int matchId);
        void SetSession(Guid token);
    }
}
