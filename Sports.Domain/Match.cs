using System;
using System.Collections.Generic;
using System.Text;

namespace Sports.Domain
{
    public class Match
    {
        public DateTime Date { get; set; }
        public Sport Sport { get; set; }
        public Team Local { get; set; }
        public Team Visitor { get; set; }
        public ICollection<Comment> Comments { get; set; }
    }
}
