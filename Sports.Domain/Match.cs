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
        public int Id { get; private set; }
        public DateTime Date { get; set; }
        public Sport Sport { get; set; }
        public Team Local { get; set; }
        public Team Visitor { get; set; }
        public ICollection<Comment> Comments {  get; private set; }

        public Match()
        {
            Comments = new List<Comment>();
        }

        public void IsValid()
        {
            CheckSportNotEmpty();
            CheckLocalNotEmpty();
            CheckVisitorNotEmpty();
            IsValidMatch(this.Local, this.Visitor);
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
                throw new InvalidTeamIsEmptyException(EmptyTeam.EMPTY_VISITOR_TEAM_MESSAGE);
            }
        }
        private void CheckLocalNotEmpty()
        {
            if (this.Local == null)
            {
                throw new InvalidTeamIsEmptyException(EmptyTeam.EMPTY_LOCAL_TEAM_MESSAGE);
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
            if (date.Subtract(DateTime.Now).TotalSeconds<0)
            {
                throw new InvalidMatchDateFormatException(MatchDateFormat.INVALID_DATE_FORMAT_MESSAGE);
            }
        }

        private void IsValidMatch(Team localTeam, Team visitorTeam)
        {
            if(localTeam.Name == visitorTeam.Name)
            {
                throw new InvalidTeamVersusException(TeamVersus.INVALID_TEAM_VERSUS_MESSAGE);
            }
        }

        public override string ToString()
        {
            string tostring = "Sport: " + Sport + " Local Team: " + Local + " Visitor Team: " + Visitor + " Date: " + Date;
            return tostring;
        }

        public void UpdateMatch(Match updatedMatch)
        {
            this.Date = IgnoreNullDate(this.Date,updatedMatch.Date);
            this.Sport = (Sport)IgnoreNull(this.Sport, updatedMatch.Sport);
            this.Local = (Team)IgnoreNull(this.Local, updatedMatch.Local);
            this.Visitor = (Team)IgnoreNull(this.Visitor, updatedMatch.Visitor);
            
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
