using System;
using System.Runtime.Serialization;
using Sports.Logic.Interface.Exceptions;

namespace Sports.Logic.Exceptions
{
    [Serializable]
    public class UserAlreadyExistException : LogicException
    {
        public UserAlreadyExistException(string message) : base(message)
        {
        }

    }
}
