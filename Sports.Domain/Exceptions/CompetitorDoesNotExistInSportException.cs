using System;
using System.Collections.Generic;
using System.Text;

namespace Sports.Domain.Exceptions
{

    [Serializable]
    public class CompetitorDoesNotExistInSportException : DomainException
    {
        public CompetitorDoesNotExistInSportException(string message) : base(message)
        {
        }

    }
}
