using System;
using System.Runtime.Serialization;
using Sports.Logic.Interface.Exceptions;

namespace Sports.Logic.Exceptions
{
    [Serializable]
    public class CompetitorAlreadyInSportException : LogicException
    {
        public CompetitorAlreadyInSportException(string message) : base(message)
        {
        }

    }
}
