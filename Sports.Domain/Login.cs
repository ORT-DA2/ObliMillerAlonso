using System;
using System.Collections.Generic;
using System.Text;

namespace Sports.Domain
{
    public class Login
    {
        public User User { get; set; }
        public Guid TokenId { get; set; }
    }
}
