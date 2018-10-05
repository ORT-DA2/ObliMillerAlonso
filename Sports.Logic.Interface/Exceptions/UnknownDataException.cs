using System;
using System.Collections.Generic;
using System.Text;

namespace Sports.Logic.Exceptions
{
    [Serializable]
    public class UnknownDataException : Exception
    {
        public UnknownDataException(string message) : base(message)
        {
        }
    }
}
