using System;
using System.Runtime.Serialization;

namespace Sports.Logic.Exceptions
{
    [Serializable]
    public class InvalidNullValueException : Exception
    {
        public InvalidNullValueException(string message) : base(message)
        {
        }

    }
}
