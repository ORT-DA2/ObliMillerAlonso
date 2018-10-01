using System;
using System.Runtime.Serialization;

namespace Sports.Logic.Exceptions
{
    [Serializable]
    public class UserDoesNotExistException : Exception
    {
        public UserDoesNotExistException(string message) : base(message)
        {
        }

    }
}
