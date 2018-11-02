using System;
using System.Collections.Generic;
using System.Text;

namespace Sports.Domain.Exceptions
{

    [Serializable]
    public class InvalidCompetitorAmountException : DomainException
    {
        public InvalidCompetitorAmountException(string message) : base(message)
        {
        }

    }
}
