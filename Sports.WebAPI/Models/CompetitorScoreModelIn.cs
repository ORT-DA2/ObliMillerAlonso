using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sports.WebAPI.Models
{
    public class CompetitorScoreModelIn
    {
        public int CompetitorId { get; set; }
        public int Score { get; set; }
    }
}
