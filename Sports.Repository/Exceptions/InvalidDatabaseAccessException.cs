using System;
using System.Runtime.Serialization;

namespace Sports.Repository.Exceptions
{
    [Serializable]
    public class InvalidDatabaseAccessException : Exception
    {
        public InvalidDatabaseAccessException(string message) : base(message)
        {
        }
    }
}
