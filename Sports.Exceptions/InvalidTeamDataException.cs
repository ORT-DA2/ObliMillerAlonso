using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Sports.Exceptions
{
    [Serializable]
    public class InvalidTeamDataException : Exception
    {
        public InvalidTeamDataException()
        {
        }

        public InvalidTeamDataException(string message) : base(message)
        {
        }

        public InvalidTeamDataException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidTeamDataException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
