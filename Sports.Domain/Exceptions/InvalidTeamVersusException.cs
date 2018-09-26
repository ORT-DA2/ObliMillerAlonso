using System;
using System.Runtime.Serialization;

namespace Sports.Domain.Exceptions
{
    [Serializable]
    public class InvalidTeamVersusException : Exception
    {
        public InvalidTeamVersusException(string message) : base(message)
        {
        }
        
    }
}
