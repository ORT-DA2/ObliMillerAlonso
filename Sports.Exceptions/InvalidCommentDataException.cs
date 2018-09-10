using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Sports.Exceptions
{
    [Serializable]
    public class InvalidCommentDataException : Exception
    {
        public InvalidCommentDataException()
        {
        }

        public InvalidCommentDataException(string message) : base(message)
        {
        }

        public InvalidCommentDataException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidCommentDataException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
