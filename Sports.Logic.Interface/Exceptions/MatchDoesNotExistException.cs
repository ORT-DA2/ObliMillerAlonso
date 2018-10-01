using System;
using System.Runtime.Serialization;

namespace Sports.Logic.Exceptions
{
    [Serializable]
    public class MatchDoesNotExistException : Exception
    {
        public MatchDoesNotExistException(string message) : base(message)
        {
        }

    }
}
