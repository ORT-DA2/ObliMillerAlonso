using System;
using System.Runtime.Serialization;
using Sports.Logic.Interface.Exceptions;

namespace Sports.Logic.Exceptions
{
    [Serializable]
    public class SportAlreadyExistsException : LogicException
    {
        public SportAlreadyExistsException(string message) : base(message)
        {
        }

    }
}
