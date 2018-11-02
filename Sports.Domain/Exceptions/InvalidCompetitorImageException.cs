using System;
using System.Runtime.Serialization;

namespace Sports.Domain.Exceptions
{
    [Serializable]
    public class InvalidCompetitorImageException : DomainException
    {
        public InvalidCompetitorImageException(string message) : base(message)
        {
        }

    }
}
