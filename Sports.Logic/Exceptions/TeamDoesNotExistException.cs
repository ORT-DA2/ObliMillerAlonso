using System;
using System.Runtime.Serialization;
using Sports.Logic.Interface.Exceptions;

namespace Sports.Logic.Exceptions
{
    [Serializable]
    public class CompetitorDoesNotExistException : LogicException
    {
        public CompetitorDoesNotExistException(string message) : base(message)
        {
        }

    }
}
