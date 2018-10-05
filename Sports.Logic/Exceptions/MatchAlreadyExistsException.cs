using System;
using System.Runtime.Serialization;
using Sports.Logic.Interface.Exceptions;

namespace Sports.Logic.Exceptions
{
    [Serializable]
    public class MatchAlreadyExistsException : LogicException
    {
        public MatchAlreadyExistsException(string message) : base(message)
        {
        }

    }
}
