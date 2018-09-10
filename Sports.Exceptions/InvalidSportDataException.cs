using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Sports.Exceptions
{
    [Serializable]
    public class InvalidSportDataException : Exception
    {
        public InvalidSportDataException()
        {
        }

        public InvalidSportDataException(string message) : base(message)
        {
        }

        public InvalidSportDataException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidSportDataException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
