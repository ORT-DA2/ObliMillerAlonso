using System;
using System.Collections.Generic;
using System.Text;

namespace Sports.Domain.Exceptions
{

    [Serializable]
    public class InvalidTeamIsEmptyException : DomainException
    {
        public InvalidTeamIsEmptyException(string message) : base(message)
        {
        }

    }
}
