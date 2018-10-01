using System;
using System.Runtime.Serialization;

namespace Sports.Logic.Exceptions
{
    [Serializable]
    public class SportAlreadyExistsException : Exception
    {
        public SportAlreadyExistsException(string message) : base(message)
        {
        }

    }
}
