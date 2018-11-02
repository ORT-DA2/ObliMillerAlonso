using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Sports.Domain.Exceptions;
using Sports.Domain.Constants;


namespace Sports.Domain
{
    public class Sport
    {
        public int Id { get;  set; }
        public string Name { get; set; }
        public int Amount { get; set; }
        public ICollection<Competitor> Competitors { get; set; }
        

        public Sport()
        {
            Competitors = new List<Competitor>();
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !obj.GetType().Equals(this.GetType()))
            {
                return false;
            }
            else
            {
                return this.Name.Equals(((Sport)obj).Name);
            }
        }

        public override string ToString()
        {
            return this.Name;
        }

        public void IsValid()
        {
            IsValidSportName();
            IsValidAmount();
        }

        private void IsValidAmount()
        {
            if (Amount < 2)
            {
                throw new InvalidCompetitorAmountException(InvalidCompetitorAmount.INVALID_ONE_COMPETITOR_MESSAGE);
            }
        }

        private void IsValidSportName()
        {
            if (string.IsNullOrWhiteSpace(Name))
                throw new InvalidEmptyTextFieldException(EmptyField.EMPTY_NAME_MESSAGE);
        }

        public void RemoveCompetitor(Competitor competitor)
        {
            CheckIfCompetitorDoesntExist(competitor);
            Competitors.Remove(competitor);
        }

        private void CheckIfCompetitorDoesntExist(Competitor competitor)
        {
            if (!Competitors.Contains(competitor))
            {
                throw new CompetitorDoesNotExistInSportException(CompetitorValidation.COMPETITOR_NOT_EXIST_IN_SPORT_MESSAGE);
            }
        }

        public void AddCompetitor(Competitor competitor)
        {
            CheckDuplicateCompetitor(competitor);
            Competitors.Add(competitor);
        }
        

        private void CheckDuplicateCompetitor(Competitor competitor)
        {
            if (Competitors.Contains(competitor))
            {
                throw new CompetitorAlreadyExistException(UniqueCompetitor.DUPLICATE_COMPETITOR_IN_SPORT_MESSAGE);
            }
        }

        public Competitor GetCompetitor(Competitor competitor)
        {
            CheckIfCompetitorDoesntExist(competitor);
            return Competitors.First(t=>t.Name == competitor.Name|| t.Id == competitor.Id);
        }

        public void UpdateData(Sport sport)
        {
            this.Name = IgnoreWhiteSpace(this.Name, sport.Name);
        }

        private string IgnoreWhiteSpace(string originalText, string updatedText)
        {
            if (string.IsNullOrWhiteSpace(updatedText))
            {
                return originalText;
            }
            return updatedText;
        }


    }
}
