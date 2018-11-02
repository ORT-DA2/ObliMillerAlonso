using System;
using System.Collections.Generic;
using System.Text;

namespace Sports.Domain.Exceptions
{

    [Serializable]
    public class InvalidCompetitorEmptyException : DomainException
    {
        public InvalidCompetitorEmptyException(string message) : base(message)
        {
        }

    }
}
