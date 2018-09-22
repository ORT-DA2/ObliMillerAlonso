using System;
using System.Collections.Generic;
using System.Text;

namespace Sports.Domain.Exceptions
{

    [Serializable]
    public class InvalidMatchDateFormatException : Exception
    {
        public InvalidMatchDateFormatException(string message) : base(message)
        {
        }

    }
}