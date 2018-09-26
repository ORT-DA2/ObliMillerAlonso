using System;
using System.Runtime.Serialization;

namespace Sports.Logic.Exceptions
{
    [Serializable]
    public class TeamDoesNotExistException : Exception
    {
        public TeamDoesNotExistException(string message) : base(message)
        {
        }

    }
}
