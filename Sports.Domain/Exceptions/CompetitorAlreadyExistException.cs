using System;
using System.Collections.Generic;
using System.Text;

namespace Sports.Domain.Exceptions
{

    [Serializable]
    public class CompetitorAlreadyExistException : DomainException
    {
        public CompetitorAlreadyExistException(string message) : base(message)
        {
        }

    }
}
