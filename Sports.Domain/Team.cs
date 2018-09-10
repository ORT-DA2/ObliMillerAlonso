using System;
using System.Collections.Generic;
using System.Text;

namespace Sports.Domain
{
    public class Team
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public byte[] Picture { get; private set; }

    }
}
