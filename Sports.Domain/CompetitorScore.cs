using System;
using System.Collections.Generic;
using System.Text;
using Sports.Domain.Exceptions;
using Sports.Domain.Constants;

namespace Sports.Domain
{
    public class CompetitorScore : IEquatable<CompetitorScore>
    {
        public int Id { get; set; }
        public Competitor Competitor { get; set; }
        public int Score { get; set; }


        public CompetitorScore()
        {
            this.Score = 0;
        }
        public CompetitorScore(Competitor competitor)
        {
            this.Competitor = competitor;
            this.Score = 0;
        }
        public void IsValid()
        {
            CompetitorNotNull();
            ValidScore();
        }

        private void ValidScore()
        {
            if (Score < 0)
            {
                throw new InvalidCompetitorScoreException(CompetitorValidation.COMPETITOR_INVALID_SCORE);
            }
        }

        private void CompetitorNotNull()
        {
            if (Competitor == null)
            {
                throw new InvalidCompetitorEmptyException(CompetitorValidation.COMPETITOR_EMPTY);
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !obj.GetType().Equals(this.GetType()))
            {
                return false;
            }
            else
            {
                return this.Competitor.Id.Equals(((CompetitorScore)obj).Competitor.Id);
            }
        }

        public bool Equals(CompetitorScore other)
        {
            if (other.Competitor == null )
            {
                return false;
            }
            else
            {
                return this.Competitor.Id == other.Competitor.Id;
            }
        }
        public override int GetHashCode()
        {
            return Id;
        }

        public override string ToString()
        {
            return Competitor.ToString();
        }
    }
}
