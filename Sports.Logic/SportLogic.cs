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
        ITeamLogic teamLogic;

        public SportLogic(IRepositoryUnitOfWork unitOfwork)
        {
            repository = unitOfwork.Sport;
            teamLogic = new TeamLogic(unitOfwork);
        }
        public void AddSport(Sport sport)
        {
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
            ICollection<Sport> sports = repository.FindByCondition(s => s.Id == id);
            if (sports.Count == 0)
            {
                throw new SportDoesNotExistException(SportNotFound.SPORT_NOT_FOUND_MESSAGE);
            }
            return sports.First();
        }

        public Team GetTeamFromSport(Sport sport, Team team)
        {
            Sport realSport = GetSportById(sport.Id);
            return teamLogic.GetTeamById(realSport.GetTeam(team).Id);
        }

        
        public Sport GetSportByName(string name)
        {
            ICollection<Sport> sports = repository.FindByCondition(s => s.Name == name);
            if (sports.Count == 0)
            {
                throw new SportDoesNotExistException(SportNotFound.SPORT_NOT_FOUND_MESSAGE);
            }
            return sports.First();
        }

        public void RemoveSport(int id)
        {
            Sport sport = GetSportById(id);
            repository.Delete(sport);
            repository.Save();
        }

        public void ModifySport(int id, Sport sport)
        {
            Sport realSport = GetSportById(id);
            realSport.UpdateData(sport);
            ValidateSport(realSport);
            repository.Update(realSport);
            repository.Save();
        }

        public ICollection<Sport> GetAll()
        {
            return repository.FindAll();
        }
        
        public void AddTeamToSport(Sport sport, Team team)
        {
            Sport realSport = GetSportById(sport.Id);
            CheckTeamIsNotUsed(sport, team);
            teamLogic.AddTeam(team);
            realSport.AddTeam(team);
            repository.Update(realSport);
            repository.Save();
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
            Team realTeam = teamLogic.GetTeamById(team.Id);
            realSport.DeleteTeam(realTeam);
            repository.Update(realSport);
            repository.Save();
        }

        public void UpdateTeamSport(int id, Team originalTeam, Team teamChanges)
        {
            Sport originalsport = GetSportById(id);
            Team original = GetTeamFromSport(originalsport, originalTeam);
            teamLogic.Modify(original.Id, teamChanges);
            repository.Update(originalsport);
            repository.Save();
        }
    }
}
