using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sports.WebAPI.Models
{
    public class MatchModelOut
    {
        public int Id { get; set; }
        public int SportId { get; set; }
        public int VisitorId { get;  set; }
        public int LocalId { get; set; }
        public string Date { get; set; }
    }
}
