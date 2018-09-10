using System;
using System.Collections.Generic;
using System.Text;

namespace Sports.Domain
{
    public class Favorite
    {
        public int Id { get; private set; }
        public User User { get; set; }
        public Team Team { get; set; }

    }
}
