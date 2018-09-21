using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Sports.Exceptions
{
    [Serializable]
    public class InvalidSessionDataException : Exception
    {

        public InvalidSessionDataException(string message) : base(message)
        {
        }

    }
}
