using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Sports.Domain;
using Sports.Exceptions;
using Sports.Repository.Interface;
using Sports.Logic.Interface;

namespace Sports.Logic
{
    public class MatchLogic : IMatchLogic
    {

        IMatchRepository _repository;
        ITeamLogic _teamLogic;

        public MatchLogic(IRepositoryUnitOfWork unit)
        {
            _repository = unit.Match;
            _teamLogic = new TeamLogic(unit);
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
            match.Local = GetRealTeam(match.Local);
            match.Visitor = GetRealTeam(match.Visitor);
        }

        private Team GetRealTeam(Team team)
        {
            return _teamLogic.GetTeamById(team.Id);
        }

        private void CheckNotNull(Match match)
        {
            if (match == null)
            {
                throw new InvalidMatchDataException("Cannot add null match");
            }
        }

        public Match GetMatchById(int id)
        {
            ICollection<Match> matches = _repository.FindByCondition(m => m.Id == id);
            if (matches.Count == 0)
            {
                throw new InvalidMatchDataException("Match Id does not exist");
            }
            return matches.First();
        }

        public void ModifyMatch(Match match)
        {
            Match realMatch = GetMatchById(match.Id);
            UpdateDate(realMatch,match.Date);
            _repository.Update(realMatch);
        }

        private void UpdateDate(Match realMatch, DateTime date)
        {
            realMatch.Date = date;
        }
    }
}
