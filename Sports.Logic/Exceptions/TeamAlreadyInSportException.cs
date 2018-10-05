using System;
using System.Runtime.Serialization;
using Sports.Logic.Interface.Exceptions;

namespace Sports.Logic.Exceptions
{
    [Serializable]
    public class TeamAlreadyInSportException : LogicException
    {
        public TeamAlreadyInSportException(string message) : base(message)
        {
        }

    }
}
