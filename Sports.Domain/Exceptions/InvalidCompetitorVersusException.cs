using System;
using System.Runtime.Serialization;

namespace Sports.Domain.Exceptions
{
    [Serializable]
    public class InvalidCompetitorVersusException : DomainException
    {
        public InvalidCompetitorVersusException(string message) : base(message)
        {
        }
        
    }
}
