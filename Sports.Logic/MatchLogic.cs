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

        public MatchLogic(IRepositoryWrapper wrapper)
        {
            _repository = wrapper.Match;
            _teamLogic = new TeamLogic(wrapper);
        }
        public void AddMatch(Match match)
        {
            CheckNotNull(match);
            match.IsValid();
            match.Local = GetRealTeam(match.Local);
            match.Visitor = GetRealTeam(match.Local);
            _repository.Create(match);
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
            return matches.First();
        }
    }
}
