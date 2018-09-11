using System;
using System.Collections.Generic;
using System.Text;
using Sports.Exceptions;

namespace Sports.Domain
{
    public class Match
    {
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

        public void IsValid(Team localTeam, Team visitorTeam)
        {
            IsValidMatch(localTeam, visitorTeam);
        }

        private void IsValidMatch(Team localTeam, Team visitorTeam)
        {
            if(localTeam.Name == visitorTeam.Name)
                throw new InvalidMatchDataException ("Invalid Match. Local and Visitor must be different.");
        }

        public override string ToString()
        {
            string tostring = "Sport: " + Sport + " Local Team: " + Local + " Visitor Team: " + Visitor + " Date: " + Date;
            return tostring;
        }
    }
}
