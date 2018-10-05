using System;
using System.Runtime.Serialization;
using Sports.Logic.Interface.Exceptions;

namespace Sports.Logic.Exceptions
{
    [Serializable]
    public class InvalidNullValueException : LogicException
    {
        public InvalidNullValueException(string message) : base(message)
        {
        }

    }
}
