using System;
using System.Runtime.Serialization;
using Sports.Repsository.Interface.Exceptions;

namespace Sports.Logic.Exceptions
{
    [Serializable]
    public class DisconnectedDatabaseException : UnknownDataAccessException
    {
        public DisconnectedDatabaseException(string message) : base(message) { }
    }
        
}
