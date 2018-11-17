using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sports.WebAPI.Models
{
    public class MatchSimpleModelOut
    {
        public int Id { get; set; }
        public SportSimpleModelOut Sport { get; set; }
        public ICollection<CompetitorScoreModelOut> Competitors { get; set; }
        public string Date { get; set; }
    }
}
