using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Sports.Exceptions
{
    [Serializable]
    public class InvalidTeamDataException : Exception
    {
        public InvalidTeamDataException(string message) : base(message)
        {
        }
        
    }
}
