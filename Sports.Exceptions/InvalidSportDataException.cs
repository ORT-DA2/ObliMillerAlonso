using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Sports.Exceptions
{
    [Serializable]
    public class InvalidSportDataException : Exception
    {

        public InvalidSportDataException(string message) : base(message)
        {
        }
        
    }
}
