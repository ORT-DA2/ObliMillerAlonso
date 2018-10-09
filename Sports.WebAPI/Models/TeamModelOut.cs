using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sports.WebAPI.Models
{
    public class TeamModelOut
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Picture { get;  set; }
        public SportSimpleModelOut Sport  { get; set; }
    }
}
