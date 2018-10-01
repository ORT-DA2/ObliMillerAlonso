using System;
using System.Runtime.Serialization;

namespace Sports.Logic.Exceptions
{
    [Serializable]
    public class UserAlreadyExistException : Exception
    {
        public UserAlreadyExistException(string message) : base(message)
        {
        }

    }
}
