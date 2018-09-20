﻿using System;
using System.Collections.Generic;
using System.Text;
using Sports.Exceptions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        public ICollection<Comment> Comments { get; set; }

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
                throw new InvalidMatchDataException("Match should include a sport.");
            }
        }
        private void CheckVisitorNotEmpty()
        {
            if (this.Visitor == null)
            {
                throw new InvalidMatchDataException("Match should include a visitor team.");
            }
        }
        private void CheckLocalNotEmpty()
        {
            if (this.Local == null)
            {
                throw new InvalidMatchDataException("Match should include a local team.");
            }
        }

        private void IsValidDate(DateTime date)
        {
            if (date.Subtract(DateTime.Now).TotalSeconds<0)
            {
                throw new InvalidMatchDataException("Cant set past matches");
            }
        }

        private void IsValidMatch(Team localTeam, Team visitorTeam)
        {
            if(localTeam.Name == visitorTeam.Name)
            {
                throw new InvalidMatchDataException("Invalid Match. Local and Visitor must be different.");
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
