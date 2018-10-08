using System;
using System.Runtime.Serialization;

namespace Sports.Domain.Exceptions
{
    [Serializable]
    public class InvalidUserDataFormatException : DomainException
    {
        public InvalidUserDataFormatException(string message) : base(message)
        {
        }

    }
}
