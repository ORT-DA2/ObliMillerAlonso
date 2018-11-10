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
    public class CompetitorLogic : ICompetitorLogic
    {
        IMatchRepository matchRepository;
        ISportRepository sportRepository;
        ICompetitorRepository repository;
        ISessionLogic sessionLogic;
        const string ASCENDING = "asc";
        const string DESCENDING = "desc";
        User user;

        public CompetitorLogic(IRepositoryUnitOfWork unit)
        {
            repository = unit.Competitor;
            matchRepository = unit.Match;
            sportRepository = unit.Sport;
            sessionLogic = new SessionLogic(unit);

        }
        public void AddCompetitor(Competitor Competitor)
        {
            sessionLogic.ValidateUser(user);
            ValidateCompetitor(Competitor);
            repository.Create(Competitor);
            repository.Save();
        }

        private void ValidateCompetitor(Competitor Competitor)
        {
            CheckNotNull(Competitor);
            Competitor.IsValid();
        }

        private void CheckNotNull(Competitor Competitor)
        {
            if (Competitor == null)
            {
                throw new InvalidNullValueException(NullValue.INVALID_COMPETITOR_NULL_VALUE_MESSAGE);
            }
        }

        public Competitor GetCompetitorById(int id)
        {
            sessionLogic.ValidateUserNotNull(user);
            ICollection<Competitor> Competitors = repository.FindByCondition(t => t.Id == id);
            if (Competitors.Count == 0)
            {
                throw new CompetitorDoesNotExistException(CompetitorNotFound.COMPETITOR_ID_NOT_FOUND_MESSAGE);
            }
            return Competitors.First();

        }

        public void Modify(int id, Competitor Competitor)
        {
            sessionLogic.ValidateUser(user);
            Competitor realCompetitor = GetCompetitorById(id);
            realCompetitor.UpdateData(Competitor);
            ValidateCompetitor(realCompetitor);
            ValidateNameInSport(realCompetitor);
            repository.Update(realCompetitor);
        }

        private void ValidateNameInSport(Competitor Competitor)
        {
            Sport sport = sportRepository.FindByCondition(s => s.Id == Competitor.Sport.Id).FirstOrDefault();
            if (sport.Competitors.Where(t=>t.Id!=Competitor.Id&&t.Equals(Competitor)).Count() != 0)
            {
                throw new CompetitorAlreadyInSportException(UniqueCompetitor.DUPLICATE_COMPETITOR_IN_SPORT_MESSAGE);
            }
        }

        public void Delete(int CompetitorId)
        {
            sessionLogic.ValidateUser(user);
            Competitor realCompetitor = GetCompetitorById(CompetitorId);
            DeleteAllRelatedMatches(realCompetitor);
            repository.Delete(realCompetitor);
            repository.Save();
        }

        private void DeleteAllRelatedMatches(Competitor realCompetitor)
        {
            CompetitorScore adaptedCompetitor = new CompetitorScore(realCompetitor);
            List<Match> allmatches = matchRepository.FindAll().ToList();
            List<Match> relatedMatches = matchRepository.FindByCondition(m => m.Competitors.Where(c => c.Competitor.Equals(realCompetitor)).Count() > 0).ToList();
            foreach(Match match in relatedMatches)
            {
                matchRepository.Delete(match);
            }
            matchRepository.Save();
        }

        public ICollection<Competitor> GetAll()
        {
            sessionLogic.ValidateUserNotNull(user);
            return repository.FindAll();
        }

        public void SetSession(Guid token)
        {
            user = sessionLogic.GetUserFromToken(token);
        }

        public ICollection<Competitor> GetFilteredCompetitors(string name, string order)
        {
            sessionLogic.ValidateUserNotNull(user);
            ICollection<Competitor> Competitors = new List<Competitor>();
            Competitors = FilterByName(name);
            OrderCompetitors(order, ref Competitors);
            return Competitors;
        }

        private void OrderCompetitors(string order, ref ICollection<Competitor> Competitors)
        {
            if (String.IsNullOrWhiteSpace(order) || order.ToLower().Equals(ASCENDING))
            {
                Competitors = Competitors.OrderBy(t => t.Name).ToList();
            }
            else if (order.ToLower().Equals(DESCENDING))
            {
                Competitors = Competitors.OrderByDescending(t => t.Name).ToList();
            }
        }

        private ICollection<Competitor> FilterByName(string name)
        {
            ICollection<Competitor> Competitors;
            if (String.IsNullOrWhiteSpace(name))
            {
                Competitors = repository.FindAll();
            }
            else
            {
                Competitors = repository.FindByCondition(t => t.Name.Equals(name));
            }
            return Competitors;
        }
    }
}
