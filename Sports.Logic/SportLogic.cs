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

        public SportLogic(IRepositoryWrapper wrapper)
        {
            _repository = wrapper.Sport;
        }
        public void AddSport(Sport sport)
        {
            _repository.Create(sport);
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

    }
}
