using System;
using System.Collections.Generic;
using System.Text;

namespace Sports.Domain.Exceptions
{

    [Serializable]
    public class InvalidCompetitorIsEmptyException : DomainException
    {
        public InvalidCompetitorIsEmptyException(string message) : base(message)
        {
        }

    }
}
