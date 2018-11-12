using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sports.WebAPI.Models
{
    public class MatchModelOut
    {
        public int Id { get; set; }
        public SportSimpleModelOut Sport { get; set; }
        public ICollection<CompetitorScoreModelIn> Competitors { get; set; }
        public string Date { get; set; }
        public ICollection<CommentSimpleModelOut> Comments { get; set; }
    }
}
