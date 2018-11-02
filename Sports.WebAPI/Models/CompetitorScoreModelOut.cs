using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sports.WebAPI.Models
{
    public class CompetitorScoreModelOut
    {
        public CompetitorSimpleModelOut Competitor { get; set; }
        public int Score { get; set; }
    }
}
