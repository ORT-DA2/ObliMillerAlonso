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
        ISportRepository _repository;
        ITeamLogic _teamLogic;

        public SportLogic(IRepositoryUnitOfWork unitOfwork)
        {
            _repository = unitOfwork.Sport;
            _teamLogic = new TeamLogic(unitOfwork);
        }
        public void AddSport(Sport sport)
        {
            ValidateSport(sport);
            CheckNotExists(sport.Name, sport.Id);
            _repository.Create(sport);
        }

        private void ValidateSport(Sport sport)
        {
            CheckNotNull(sport);
            sport.IsValid();
        }

        private void CheckNotExists(string name, int id = 0)
        {
            if (_repository.FindByCondition(s => s.Name == name && s.Id != id).Count != 0)
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
            ICollection<Sport> sports = _repository.FindByCondition(s => s.Id == id);
            if (sports.Count == 0)
            {
                throw new SportDoesNotExistException(SportNotFound.SPORT_NOT_FOUND_MESSAGE);
            }
            return sports.First();
        }

        public Team GetTeamFromSport(Sport sport, Team team)
        {
            Sport realSport = GetSportById(sport.Id);
            return _teamLogic.GetTeamById(realSport.GetTeam(team).Id);
        }

        
        public Sport GetSportByName(string name)
        {
            ICollection<Sport> sports = _repository.FindByCondition(s => s.Name == name);
            if (sports.Count == 0)
            {
                throw new SportDoesNotExistException(SportNotFound.SPORT_NOT_FOUND_MESSAGE);
            }
            return sports.First();
        }
        

        public void RemoveSport(int id)
        {
            Sport sport = GetSportById(id);
            _repository.Delete(sport);
        }

        public void ModifySport(int id, Sport sport)
        {
            Sport realSport = GetSportById(id);
            realSport.UpdateData(sport);
            ValidateSport(realSport);
            _repository.Update(realSport);
        }

        public ICollection<Sport> GetAll()
        {
            return _repository.FindAll();
        }
        
        public void AddTeamToSport(Sport sport, Team team)
        {
            Sport realSport = GetSportById(sport.Id);
            CheckTeamIsNotUsed(sport, team);
            _teamLogic.AddTeam(team);
            realSport.AddTeam(team);
            _repository.Update(realSport);
        }

        private void CheckTeamIsNotUsed(Sport sport, Team team)
        {
            if (sport.Teams.Contains(team))
            {
                throw new TeamAlreadyInSportException(UniqueTeam.DUPLICATE_TEAM_IN_SPORT_MESSAGE);
            }
        }

        public void DeleteTeamFromSport(Sport sport, Team team)
        {
            Sport realSport = GetSportById(sport.Id);
            Team realTeam = _teamLogic.GetTeamById(team.Id);
            realSport.DeleteTeam(realTeam);
            _repository.Update(realSport);
            _teamLogic.Delete(realTeam);
        }

        public void UpdateTeamSport(int id, Team originalTeam, Team teamChanges)
        {
            Sport originalsport = GetSportById(id);
            Team original = GetTeamFromSport(originalsport, originalTeam);
            _teamLogic.Modify(original.Id, teamChanges);
            _repository.Update(originalsport);
        }
    }
}
