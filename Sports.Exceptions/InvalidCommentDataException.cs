using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Sports.Exceptions
{
    [Serializable]
    public class InvalidCommentDataException : Exception
    {

        public InvalidCommentDataException(string message) : base(message)
        {
        }
        
    }
}
