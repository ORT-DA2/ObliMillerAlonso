using System;
using System.Runtime.Serialization;
using Sports.Logic.Interface.Exceptions;

namespace Sports.Logic.Exceptions
{
    [Serializable]
    public class SessionDoesNotExistException : UnauthorizedException
    {
        public SessionDoesNotExistException(string message) : base(message)
        {
        }

    }
}
