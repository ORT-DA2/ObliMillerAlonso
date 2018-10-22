using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Sports.Domain.Exceptions;
using Sports.Domain.Constants;

namespace Sports.Domain
{
    [Table("Match")]
    public class Match
    {
        [Key]
        public int Id { get;  set; }
        public DateTime Date { get; set; }
        public Sport Sport { get; set; }
        public Competitor Local { get; set; }
        public Competitor Visitor { get; set; }
        public ICollection<Comment> Comments {  get; set; }

        public Match()
        {
            Comments = new List<Comment>();
        }

        public void IsValid()
        {
            CheckSportNotEmpty();
            CheckLocalNotEmpty();
            CheckVisitorNotEmpty();
            IsValidDate(this.Date);
        }

        private void CheckSportNotEmpty()
        {
            if (this.Sport == null)
            {
                throw new InvalidSportIsEmptyException(EmptySport.EMPTY_SPORT_MESSAGE);
            }
        }
        private void CheckVisitorNotEmpty()
        {
            if (this.Visitor == null)
            {
                throw new InvalidCompetitorIsEmptyException(EmptyCompetitor.EMPTY_VISITOR_COMPETITOR_MESSAGE);
            }
        }
        private void CheckLocalNotEmpty()
        {
            if (this.Local == null)
            {
                throw new InvalidCompetitorIsEmptyException(EmptyCompetitor.EMPTY_LOCAL_COMPETITOR_MESSAGE);
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
            if(Local.Name == Visitor.Name)
            {
                throw new InvalidCompetitorVersusException(CompetitorVersus.INVALID_COMPETITOR_VERSUS_MESSAGE);
            }
        }

        public override string ToString()
        {
            string tostring = "Sport: " + Sport + " Local Competitor: " + Local + " Visitor Competitor: " + Visitor + " Date: " + Date;
            return tostring;
        }

        public void UpdateMatch(Match updatedMatch)
        {
            this.Date = IgnoreNullDate(this.Date,updatedMatch.Date);
            this.Sport = (Sport)IgnoreNull(this.Sport, updatedMatch.Sport);
            this.Local = (Competitor)IgnoreNull(this.Local, updatedMatch.Local);
            this.Visitor = (Competitor)IgnoreNull(this.Visitor, updatedMatch.Visitor);
            
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
