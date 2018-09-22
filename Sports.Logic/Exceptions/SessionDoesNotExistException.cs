using System;
using System.Runtime.Serialization;

namespace Sports.Logic.Exceptions
{
    [Serializable]
    public class SessionDoesNotExistException : Exception
    {
        public SessionDoesNotExistException(string message) : base(message)
        {
        }

    }
}
