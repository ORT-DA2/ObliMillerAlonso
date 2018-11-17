using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sports.WebAPI.Models
{
    public class SportMatchModelOut
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<MatchSimpleModelOut> Matches { get; set; }
    }
}
