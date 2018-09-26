using System;
using System.Runtime.Serialization;

namespace Sports.Domain.Exceptions
{
    [Serializable]
    public class InvalidTeamImageException : Exception
    {
        public InvalidTeamImageException(string message) : base(message)
        {
        }

    }
}
