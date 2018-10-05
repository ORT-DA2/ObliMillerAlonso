using System;
using System.Runtime.Serialization;
using Sports.Logic.Interface.Exceptions;

namespace Sports.Logic.Exceptions
{
    [Serializable]
    public class MatchDoesNotExistException : LogicException
    {
        public MatchDoesNotExistException(string message) : base(message)
        {
        }

    }
}
