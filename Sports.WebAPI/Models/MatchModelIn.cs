using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sports.WebAPI.Models
{
    public class MatchModelIn
    {
        public int SportId { get; set; }
        public int VisitorId { get;  set; }
        public int LocalId { get; set; }
        public string Date { get; set; }
    }
}
