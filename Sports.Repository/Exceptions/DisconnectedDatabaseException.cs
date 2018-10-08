using System;
using System.Runtime.Serialization;
using Sports.Repository.Interface.Exceptions;

namespace Sports.Repository.Exceptions
{
    [Serializable]
    public class DisconnectedDatabaseException : UnknownDataAccessException
    {
        public DisconnectedDatabaseException(string message) : base(message) { }
    }
        
}
