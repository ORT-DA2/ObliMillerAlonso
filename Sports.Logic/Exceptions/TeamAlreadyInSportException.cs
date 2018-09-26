using System;
using System.Runtime.Serialization;

namespace Sports.Logic.Exceptions
{
    [Serializable]
    public class TeamAlreadyInSportException : Exception
    {
        public TeamAlreadyInSportException(string message) : base(message)
        {
        }

    }
}
