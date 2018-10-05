using System;
using System.Runtime.Serialization;
using Sports.Logic.Interface.Exceptions;

namespace Sports.Logic.Exceptions
{
    [Serializable]
    public class UserDoesNotExistException : LogicException
    {
        public UserDoesNotExistException(string message) : base(message)
        {
        }

    }
}
