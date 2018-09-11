using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Sports.Exceptions
{
    [Serializable]
    public class InvalidMatchDataException : Exception
    {
        public InvalidMatchDataException()
        {
        }

        public InvalidMatchDataException(string message) : base(message)
        {
        }

        public InvalidMatchDataException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidMatchDataException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
