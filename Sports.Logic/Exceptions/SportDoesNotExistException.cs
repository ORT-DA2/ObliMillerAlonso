using System;
using System.Runtime.Serialization;

namespace Sports.Logic.Exceptions
{
    [Serializable]
    public class SportDoesNotExistException : Exception
    {
        public SportDoesNotExistException(string message) : base(message)
        {
        }

    }
}
