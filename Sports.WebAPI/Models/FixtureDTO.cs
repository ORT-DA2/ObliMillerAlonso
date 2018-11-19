using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sports.WebAPI.Models
{
    public class FixtureDTO
    {
        public int Pos { get; set; }
        public int SportId { get; set; }
        public string Date { get; set; }
    }
}
