using System;
using System.Runtime.Serialization;

namespace Sports.Domain.Exceptions
{
    [Serializable]
    public class InvalidTeamImageException : DomainException
    {
        public InvalidTeamImageException(string message) : base(message)
        {
        }

    }
}
