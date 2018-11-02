using System;
using System.Collections.Generic;
using System.Text;
using Sports.Domain.Exceptions;

namespace Sports.Domain
{
    public class CompetitorScore
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
                throw new InvalidCompetitorScoreException("");
            }
        }

        private void CompetitorNotNull()
        {
            if (Competitor == null)
            {
                throw new InvalidCompetitorEmptyException("");
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
                return this.Competitor.Equals(((CompetitorScore)obj).Competitor);
            }
        }
    }
}
