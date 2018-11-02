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
    public class SportLogic : ISportLogic
    {
        ISportRepository repository;
        ICompetitorLogic competitorLogic;
        ISessionLogic sessionLogic;
        User user;

        public SportLogic(IRepositoryUnitOfWork unitOfwork)
        {
            repository = unitOfwork.Sport;
            competitorLogic = new CompetitorLogic(unitOfwork);
            sessionLogic = new SessionLogic(unitOfwork);
        }

        public void AddSport(Sport sport)
        {
            sessionLogic.ValidateUser(user);
            ValidateSport(sport);
            CheckNotExists(sport.Name, sport.Id);
            repository.Create(sport);
            repository.Save();
        }

        private void ValidateSport(Sport sport)
        {
            CheckNotNull(sport);
            sport.IsValid();
        }

        private void CheckNotExists(string name, int id = 0)
        {
            if (repository.FindByCondition(s => s.Name == name && s.Id != id).Count != 0)
            {
                throw new SportAlreadyExistsException(UniqueSport.DUPLICATE_SPORT_MESSAGE);
            }
        }
        private void CheckNotNull(Sport sport)
        {
            if (sport == null)
            {
                throw new InvalidNullValueException(NullValue.INVALID_SPORT_NULL_VALUE_MESSAGE);
            }
        }

        public Sport GetSportById(int id)
        {
            sessionLogic.ValidateUserNotNull(user);
            ICollection<Sport> sports = repository.FindByCondition(s => s.Id == id);
            if (sports.Count == 0)
            {
                throw new SportDoesNotExistException(SportNotFound.SPORT_NOT_FOUND_MESSAGE);
            }
            return sports.First();
        }

        public Competitor GetCompetitorFromSport(int sportId, int competitorId)
        {
            sessionLogic.ValidateUserNotNull(user);
            Sport realSport = GetSportById(sportId);
            Competitor competitor = competitorLogic.GetCompetitorById(competitorId);
            return competitorLogic.GetCompetitorById(realSport.GetCompetitor(competitor).Id);
        }

        
        public Sport GetSportByName(string name)
        {
            sessionLogic.ValidateUserNotNull(user);
            ICollection<Sport> sports = repository.FindByCondition(s => s.Name == name);
            if (sports.Count == 0)
            {
                throw new SportDoesNotExistException(SportNotFound.SPORT_NOT_FOUND_MESSAGE);
            }
            return sports.First();
        }

        public void RemoveSport(int id)
        {
            sessionLogic.ValidateUser(user);
            Sport sport = GetSportById(id);
            repository.Delete(sport);
            repository.Save();
        }

        public void ModifySport(int id, Sport sport)
        {
            sessionLogic.ValidateUser(user);
            Sport realSport = GetSportById(id);
            realSport.UpdateData(sport);
            ValidateSport(realSport);
            repository.Update(realSport);
            repository.Save();
        }

        public ICollection<Sport> GetAll()
        {
            sessionLogic.ValidateUserNotNull(user);
            return repository.FindAll();
        }
        
        public void AddCompetitorToSport(int sportId, Competitor competitor)
        {
            sessionLogic.ValidateUser(user);
            Sport realSport = GetSportById(sportId);
            CheckCompetitorIsNotUsed(realSport, competitor);
            competitorLogic.AddCompetitor(competitor);
            realSport.AddCompetitor(competitor);
            repository.Update(realSport);
            repository.Save();
        }

        private void CheckCompetitorIsNotUsed(Sport sport, Competitor competitor)
        {
            if (sport.Competitors.Contains(competitor))
            {
                throw new CompetitorAlreadyInSportException(UniqueCompetitor.DUPLICATE_COMPETITOR_IN_SPORT_MESSAGE);
            }
        }

        public void DeleteCompetitorFromSport(int sportId, int competitorId)
        {
            sessionLogic.ValidateUser(user);
            Sport realSport = GetSportById(sportId);
            competitorLogic.Delete(competitorId);
            repository.Update(realSport);
            repository.Save();
        }

        public void UpdateCompetitorSport(int sportId, int competitorId, Competitor competitorChanges)
        {
            sessionLogic.ValidateUser(user);
            Competitor original = GetCompetitorFromSport(sportId, competitorId);
            competitorLogic.Modify(original.Id, competitorChanges);
            Sport originalSport = GetSportById(sportId);
            repository.Update(originalSport);
            repository.Save();
        }

        public ICollection<Competitor> GetCompetitorsFromSport(int sportId)
        {
            Sport sport = GetSportById(sportId);
            return sport.Competitors;
        }

        public void SetSession(Guid token)
        {
            user = sessionLogic.GetUserFromToken(token);
            competitorLogic.SetSession(token);
        }
    }
}
