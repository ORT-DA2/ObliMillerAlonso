using System;
using System.Collections.Generic;
using System.Text;

namespace Sports.Domain.Exceptions
{

    [Serializable]
    public class InvalidTeamIsEmptyException : Exception
    {
        public InvalidTeamIsEmptyException(string message) : base(message)
        {
        }

    }
}
