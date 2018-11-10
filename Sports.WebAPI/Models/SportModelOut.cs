using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sports.WebAPI.Models
{
    public class SportModelOut
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Amount { get; set; }
        public ICollection<CompetitorSimpleModelOut> Competitors { get; set; }
    }
}
