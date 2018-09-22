using System;
using System.Runtime.Serialization;

namespace Sports.Domain.Exceptions
{
    [Serializable]
    public class InvalidAuthenticationException : Exception
    {
        public InvalidAuthenticationException(string message) : base(message)
        {
        }

    }
}
