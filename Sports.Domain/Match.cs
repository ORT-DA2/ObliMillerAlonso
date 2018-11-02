using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Sports.Domain.Exceptions;
using Sports.Domain.Constants;
using System.Linq;

namespace Sports.Domain
{
    [Table("Match")]
    public class Match
    {
        [Key]
        public int Id { get;  set; }
        public DateTime Date { get; set; }
        public Sport Sport { get; set; }
        public ICollection<Comment> Comments {  get; set; }
        public ICollection<CompetitorScore> Competitors { get; set; }

        public Match()
        {
            Comments = new List<Comment>();
            Competitors = new List<CompetitorScore>();
        }

        public void IsValid()
        {
            CheckSportNotEmpty();
            CheckCompetitorsAmount();
            IsValidDate(this.Date);
        }

        private void CheckSportNotEmpty()
        {
            if (this.Sport == null)
            {
                throw new InvalidSportIsEmptyException(EmptySport.EMPTY_SPORT_MESSAGE);
            }
        }
        private void CheckCompetitorsAmount()
        {
            if (this.Sport.Amount != this.Competitors.Count)
            {
                throw new InvalidCompetitorAmountException(InvalidCompetitorAmount.INVALID_COMPETITORS_AMOUNT_MESSAGE);
            }
        }
        
        private void CheckCommentNotEmpty(Comment comment)
        {
            if (comment == null)
            {
                throw new InvalidCommentIsEmptyException(EmptyComment.EMPTY_COMMENT);
            }
        }

        private void IsValidDate(DateTime date)
        {
            if (date.Date.CompareTo(DateTime.Now.Date)<1)
                
            {
                throw new InvalidMatchDateFormatException(MatchDateFormat.INVALID_DATE_FORMAT_MESSAGE);
            }
        }

        public void IsValidMatch()
        {
            foreach(CompetitorScore mCompetitor in Competitors)
            {
                string competitorName = mCompetitor.Competitor.Name;
                List<CompetitorScore> occurences = Competitors.Where(c => c.Competitor.Name.Equals(competitorName)).ToList();
                if (occurences.Count>1)
                {
                    throw new InvalidCompetitorVersusException(CompetitorVersus.INVALID_COMPETITOR_VERSUS_MESSAGE);
                }
            }
        }

        public override string ToString()
        {
            string tostring = "Sport: " + Sport + " Competitors: " + PrintCompetitors() + " Date: " + Date;
            return tostring;
        }

        private string PrintCompetitors()
        {
            string competitorsList = "";
            foreach(CompetitorScore mCompetitor in Competitors)
            {
                competitorsList += mCompetitor.ToString() + ", ";
            }
            return competitorsList;
        }

        public void UpdateMatch(Match updatedMatch)
        {
            this.Date = IgnoreNullDate(this.Date,updatedMatch.Date);
            this.Sport = (Sport)IgnoreNull(this.Sport, updatedMatch.Sport);
            this.Competitors = (ICollection<CompetitorScore>)IgnoreNull(this.Competitors, updatedMatch.Competitors);
        }

        private DateTime IgnoreNullDate(DateTime originalDate, DateTime updatedDate)
        {
            if (updatedDate.Equals(DateTime.MinValue))
            {
                return originalDate;
            }
            return updatedDate;
        }

        public ICollection<Comment> GetAllComments()
        {
            return this.Comments;
        }

        public void AddComment(Comment comment)
        {
            CheckCommentNotEmpty(comment);
            comment.IsValid();
            this.Comments.Add(comment);
        }

        private Object IgnoreNull(Object original, Object updated)
        {
            if (updated==null)
            {
                return original;
            }
            return updated;
        }

    }
}
