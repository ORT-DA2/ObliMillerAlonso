using System;
using System.Collections.Generic;
using System.Text;

namespace Sports.Domain.Exceptions
{

    [Serializable]
    public class InvalidCompetitorScoreException : DomainException
    {
        public InvalidCompetitorScoreException(string message) : base(message)
        {
        }

    }
}
