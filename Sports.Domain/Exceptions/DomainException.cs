using System;
using System.Runtime.Serialization;

namespace Sports.Domain.Exceptions
{
    [Serializable]
    public class DomainException : Exception
    {
        public DomainException(string message) : base(message)
        {
        }

    }
}
