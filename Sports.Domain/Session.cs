using System;
using System.Collections.Generic;
using System.Text;

namespace Sports.Domain
{
    public class Session
    {
        public User User { get; set; }
        public Guid Token { get; set; }
    }
}
