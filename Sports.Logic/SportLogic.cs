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
    public class SportLogic : ISportLogic
    {
        ISportRepository _repository;
        ITeamLogic _teamLogic;

        public SportLogic(IRepositoryUnitOfWork wrapper)
        {
            _repository = wrapper.Sport;
            _teamLogic = new TeamLogic(wrapper);
        }
        public void AddSport(Sport sport)
        {
            ValidateSport(sport);
            _repository.Create(sport);
        }

        private void ValidateSport(Sport sport)
        {
            CheckNotNull(sport);
            sport.IsValid();
            CheckNotExists(sport.Name, sport.Id);
        }

        private void CheckNotExists(string name, int id = 0)
        {
            if (_repository.FindByCondition(s => s.Name == name && s.Id != id).Count != 0)
            {
                throw new InvalidSportDataException("Cannot repeat name");
            }
        }
        private void CheckNotNull(Sport sport)
        {
            if (sport == null)
            {
                throw new InvalidSportDataException("Cannot add null sport");
            }
        }

        public Sport GetSportById(int id)
        {
            ICollection<Sport> sports = _repository.FindByCondition(s => s.Id == id);
            if (sports.Count == 0)
            {
                throw new InvalidSportDataException("Id does not match any existing sports");
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
                throw new InvalidSportDataException("Id does not match any existing sports");
            }
            return sports.First();
        }

        public void UpdateSport(int id, Sport updatedSport)
        {
            Sport originalsport = GetSportById(id);
            originalsport.UpdateData(updatedSport);
            ValidateSport(originalsport);
            _repository.Update(originalsport);
        }

        public void RemoveSport(int id)
        {
            Sport sport = GetSportById(id);
            _repository.Delete(sport);
        }

        public void Modify(Sport sport)
        {
            Sport realSport = GetSportById(sport.Id);
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
                throw new InvalidSportDataException("Team already in sport");
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
    }
}
