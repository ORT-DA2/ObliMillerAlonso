using System;
using System.Runtime.Serialization;

namespace Sports.Repository.Exceptions
{
    [Serializable]
    public class InvalidSaveException : Exception
    {
        public InvalidSaveException(string message) : base(message)
        {
        }
    }
}
